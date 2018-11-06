using AnvelApi;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

namespace CAVS.Anvel
{

    public class LiveDisplayBehavior : MonoBehaviour
    {
        [System.Serializable]
        public class LidarEntry
        {
            public string sensorName;

            public Color renderColor;

            private ObjectDescriptor descriptor;

            public LidarEntry(string sensorName, Color renderColor, ObjectDescriptor descriptor)
            {
                this.sensorName = sensorName;
                this.renderColor = renderColor;
                this.descriptor = descriptor;
            }

            public LidarEntry(string sensorName, Color renderColor) : this(sensorName, renderColor, null) { }

            public LidarEntry(string sensorName) : this(sensorName, Color.white, null) { }

            public ObjectDescriptor GetDescriptor(AnvelControlService.Client connection)
            {
                if (descriptor == null)
                {
                    descriptor = connection.GetObjectDescriptorByName(sensorName);
                }
                return descriptor;
            }

        }

        private ParticleSystem lidarDisplay;

        private ParticleSystem.Particle[] particles;

        private AnvelControlService.Client conn;

        private Thread pollingThread;

        private List<LidarEntry> lidarDisplays;

        private ObjectDescriptor vehicle;

        private Vector3 centerOffset;

        private Vector3 rotationOffset;

        private void Awake()
        {
            lidarDisplays = new List<LidarEntry>();
        }

        public void Initialize(ClientConnectionToken connectionToken, string lidarSensorName, ObjectDescriptor vehicle)
        {
            lidarDisplays.Add(new LidarEntry(lidarSensorName));
            this.vehicle = vehicle;
            this.conn = ConnectionFactory.CreateConnection(connectionToken);
            this.centerOffset = Vector3.zero;
            this.rotationOffset = Vector3.zero;
            particles = new ParticleSystem.Particle[0];
            lidarDisplay = gameObject.GetComponent<ParticleSystem>();
            pollingThread = new Thread(PollLidarPoints);
            pollingThread.Start();
        }

        public void Initialize(ClientConnectionToken connectionToken, LidarEntry[] lidarDisplays, ObjectDescriptor vehicle, Vector3 centerOffset, Vector3 rotationOffset)
        {
            this.lidarDisplays.AddRange(lidarDisplays);
            this.vehicle = vehicle;
            this.conn = ConnectionFactory.CreateConnection(connectionToken);
            this.centerOffset = centerOffset;
            this.rotationOffset = rotationOffset;
            particles = new ParticleSystem.Particle[0];
            lidarDisplay = gameObject.GetComponent<ParticleSystem>();
            pollingThread = new Thread(PollLidarPoints);
            pollingThread.Start();
        }

        public void UpdateCenterOffset(Vector3 newOffset)
        {
            centerOffset = newOffset;
        }

        public void UpdateRotationOffset(Vector3 newOffset)
        {
            rotationOffset = newOffset;
        }

        void Update()
        {
            var toRender = particles;
            if (toRender != null && toRender.Length > 0)
            {
                lidarDisplay.SetParticles(toRender, toRender.Length);
            }
        }

        public void AddLidar(string name, Color color, ObjectDescriptor objectDescriptor)
        {
            lidarDisplays.Add(new LidarEntry(name, color, objectDescriptor));
        }

        private void OnDestroy()
        {
            if (pollingThread != null && pollingThread.IsAlive)
            {
                pollingThread.Abort();
            }
        }

        private Vector3 ModifiedPositionFromRotationalOffset(Vector3 originalPosition, Vector3 pivot, Vector3 rotationalOffset)
        {
            return UnityEngine.Quaternion.Euler(rotationalOffset) * (originalPosition - pivot) + pivot;
        }

        /// <summary>
        /// RAN IN A SEPERATE THREAD
        /// </summary>
        private void PollLidarPoints()
        {

            int totalNumberOfPoints = 0;
            float lowestPoint = float.MaxValue;
            float highestPoint = float.MinValue;
            while (true)
            {
                try
                {
                    var displays = new List<LidarEntry>(lidarDisplays);

                    LidarPoints[] allPoints = new LidarPoints[displays.Count];
                    Vector3[] offsets = new Vector3[displays.Count];

                    Point3 vehiclePosition = conn.GetPoseAbs(vehicle.ObjectKey).Position;
                    totalNumberOfPoints = 0;

                    Vector3 lastPos = Vector3.forward * 1000000;
                    for (int i = 0; i < displays.Count; i++)
                    {
                        allPoints[i] = conn.GetLidarPoints(displays[i].GetDescriptor(conn).ObjectKey, 0);
                        totalNumberOfPoints += allPoints[i].Points.Count;
                        //if (conn.GetProperty(displays[i].GetDescriptor(conn).ObjectKey, "Lidar Global Frame") == "true")
                        //{
                        //    offsets[i] = new Vector3((float)vehiclePosition.Y, (float)vehiclePosition.Z, (float)vehiclePosition.X);
                        //}
                    }

                    var newParticles = new ParticleSystem.Particle[totalNumberOfPoints];
                    int particleIndex = 0;
                    for (int lidarIndex = 0; lidarIndex < displays.Count; lidarIndex++)
                    {
                        for (int pointIndex = 0; pointIndex < allPoints[lidarIndex].Points.Count; pointIndex++)
                        {
                            Vector3 position = ModifiedPositionFromRotationalOffset(new Vector3(
                                    -(float)allPoints[lidarIndex].Points[pointIndex].Y,
                                    (float)allPoints[lidarIndex].Points[pointIndex].Z,
                                    (float)allPoints[lidarIndex].Points[pointIndex].X
                                ) - offsets[lidarIndex], Vector3.zero, rotationOffset) + centerOffset;

                            if (position.y > highestPoint)
                            {
                                highestPoint = position.y;
                            }

                            if (position.y < lowestPoint)
                            {
                                lowestPoint = position.y;
                            }

                            var colorToRender = displays[lidarIndex].renderColor;
                            if (Mathf.Abs(highestPoint - lowestPoint) > 0.001f)
                            {
                                float H;
                                float S;
                                float V;
                                Color.RGBToHSV(colorToRender, out H, out S, out V);
                                var p = (position.y - lowestPoint) / (highestPoint - lowestPoint);
                                colorToRender = Color.HSVToRGB(H, p, p);
                                colorToRender.a = displays[lidarIndex].renderColor.a;
                            }

                            if ((position - lastPos).sqrMagnitude > .1)
                            {
                                newParticles[particleIndex] = new ParticleSystem.Particle
                                {
                                    remainingLifetime = float.MaxValue,
                                    position = position,
                                    startSize = .5f,
                                    startColor = colorToRender
                                };
                            }


                            lastPos = position;
                            particleIndex++;
                        }
                    }
                    particles = newParticles;
                }
                catch (AnvelException e)
                {
                    Debug.LogErrorFormat("Anvel Exception: {0} at {1}", e.ErrorMessage, e.Source);
                }
                catch (PropertyNameNotFound e)
                {
                    Debug.LogErrorFormat("Anvel Property \"{0}\" was not found.\n{1}", e.PropertyName, e.Message);
                }
                catch (System.Exception e)
                {
                    Debug.LogFormat("{0}:{1}", e.GetType(), e.Message);
                }
            }


        }

    }

}