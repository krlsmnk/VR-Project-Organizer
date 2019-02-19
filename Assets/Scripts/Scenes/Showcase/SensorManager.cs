using System.Collections.Generic;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{

    public class SensorManager 
    {

        private List<LidarSensorBehavior> lidarSensorsInScene;

        private List<CameraSensorBehavior> cameraSensorsInScene;

        public SensorManager()
        {
            lidarSensorsInScene = new List<LidarSensorBehavior>();
            cameraSensorsInScene = new List<CameraSensorBehavior>();
        }

        public void RegisterLidar(LidarSensorBehavior lidarSensorBehavior)
        {
            lidarSensorsInScene.Add(lidarSensorBehavior);
        }

        public void RegisterCamera(CameraSensorBehavior cameraSensorBehavior)
        {
            cameraSensorsInScene.Add(cameraSensorBehavior);
        }

        public LidarConfig[] GetLidarConfigs()
        {
            var configs = new LidarConfig[lidarSensorsInScene.Count];
            for (int i = 0; i < lidarSensorsInScene.Count; i++)
            {
                configs[i] = lidarSensorsInScene[i].GetConfig();
            }
            return configs;
        }

        public CameraConfig[] GetCameraConfigs()
        {
            var configs = new CameraConfig[cameraSensorsInScene.Count];
            for (int i = 0; i < cameraSensorsInScene.Count; i++)
            {
                configs[i] = cameraSensorsInScene[i].GetConfig();
            }
            return configs;
        }

        public void SetupLidar(LidarConfig[] lidarConfigs)
        {
            if (lidarConfigs.Length != lidarSensorsInScene.Count)
            {
                UnityEngine.Debug.LogError("Don't know what to do in the case of a mismatch!");
                return;
            }

            for (int i = 0; i < lidarConfigs.Length; i++)
            {
                lidarSensorsInScene[i].Set(lidarConfigs[i]);
            }
        }

        public void SetupCameras(CameraConfig[] cameraConfigs)
        {
            if (cameraConfigs.Length != cameraSensorsInScene.Count)
            {
                UnityEngine.Debug.LogError("Don't know what to do in the case of a mismatch!");
                return;
            }

            for (int i = 0; i < cameraConfigs.Length; i++)
            {
                cameraSensorsInScene[i].Set(cameraConfigs[i]);
            }
        }

    }

}