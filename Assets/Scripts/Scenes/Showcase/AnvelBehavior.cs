using System.Collections;
using UnityEngine;

using CAVS.Anvel;
using CAVS.Anvel.Lidar;
using CAVS.Anvel.Vehicle;


namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    public class AnvelBehavior : MonoBehaviour
    {

        enum DisplayType
        {
            Networked,
            File
        }

        [SerializeField]
        private GameObject objectForOffset;

        [SerializeField]
        private GameObject drivingController;

        [SerializeField]
        private DisplayType startup;

        [SerializeField]
        private LiveDisplayBehavior.LidarEntry[] lidarSensors;

        [SerializeField]
        private string anvelVehicleName;

        [SerializeField]
        private string anvelIP;

        [SerializeField]
        private int anvelPort;

        void InitializeNetworkMode()
        {
            
            var connectionToken = new ClientConnectionToken();

            var connnnnn = ConnectionFactory.CreateConnection(connectionToken);
            var carReference = AnvelObject.CreateObject(connnnnn, "car", AnvelAssetName.Vehicles.GENERIC_4X4);
            //CreateAnvelObjectOnCollision.Build(AnvelAssetName.Sensors.API_Camera, new Vector3(-1, 1, -5.5f), carReference, connnnnn);
            CreateAnvelObjectOnCollision.Build(AnvelAssetName.Sensors.API_3D_LIDAR, new Vector3(1, 1, -5.5f), carReference, connnnnn);

            var display = gameObject.AddComponent<LiveDisplayBehavior>();

            display.Initialize(
                    connectionToken,
                    lidarSensors == null ? new LiveDisplayBehavior.LidarEntry[0] : lidarSensors,
                    carReference.ObjectDescriptor(),
                    new Vector3(0.1f, 0.1f, 2.27f),
                    new Vector3(0, 90, 0)
                 );

            if (drivingController != null)
            {
                VehicleControl.Build(drivingController, connectionToken, carReference.ObjectDescriptor());
            }
            else
            {
                Debug.LogWarning("No Driving Controller Assigned!");
            }

            StartCoroutine(UpdateOffsetTick(display));
        }

        void InitializeFilePlayback()
        {
            gameObject.AddComponent<FileDisplayBehavior>().Initialize(
                       LidarSerialization.Load("360 Lidar-11.pcrp"),
                       VehicleLoader.LoadVehicleData("vehicle1_pos_2.vprp")
                   );
        }

        // Use this for initialization
        void Start()
        {

            switch (startup)
            {
                case DisplayType.File:
                    InitializeFilePlayback();
                    break;

                case DisplayType.Networked:
                    InitializeNetworkMode();
                    break;
            }

        }

        private void OnApplicationQuit()
        {
            AnvelObjectManager.Instance.DeleteAllObjectsWeCreatedInAnvel();
        }

        IEnumerator UpdateOffsetTick(LiveDisplayBehavior displayBehavior)
        {
            while (true)
            {
                if (objectForOffset == null)
                {
                    break;
                }
                displayBehavior.UpdateCenterOffset(objectForOffset.transform.position);
                displayBehavior.UpdateRotationOffset(objectForOffset.transform.rotation.eulerAngles);
                yield return null;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}