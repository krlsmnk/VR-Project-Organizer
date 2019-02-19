using System.Runtime.Serialization;
using System;

using CAVS.ProjectOrganizer.Scenes.Showcase;

namespace CAVS.ProjectOrganizer.SourceControl
{
    [Serializable]
    public class Artifact : ISerializable
    {
        public int Id;

        public LidarConfig[] LidarConfigs;

        public CameraConfig[] CameraConfigs;

        public Artifact()
        {

        }

        public Artifact(int id, LidarConfig[] LidarConfigs, CameraConfig[] CameraConfigs)
        {
            this.Id = id;
            this.LidarConfigs = LidarConfigs;
            this.CameraConfigs = CameraConfigs;
        }

        //public Artifact(int id)
        //{
        //    this
        //}

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id", Id, typeof(int));
            info.AddValue("lidarConfigs", LidarConfigs, typeof(LidarConfig[]));
            info.AddValue("cameraConfigs", CameraConfigs, typeof(CameraConfig[]));
        }

        public Artifact(SerializationInfo info, StreamingContext context)
        {
            Id = (int)info.GetValue("id", typeof(int));
            LidarConfigs = (LidarConfig[])info.GetValue("lidarConfigs", typeof(LidarConfig[]));
            CameraConfigs = (CameraConfig[])info.GetValue("cameraConfigs", typeof(CameraConfig[]));
        }
    }
}
