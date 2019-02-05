using UnityEngine;
using System.Runtime.Serialization;
using System;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    [Serializable]
    public class LidarConfig : ISerializable
    {
        public Vector3 Position;

        public Vector3 Rotation;

        public float Range;

        public LidarConfig()
        {

        }

        public LidarConfig(Vector3 position, Vector3 rotation, float range)
        {
            Position = position;
            Rotation = rotation;
            Range = range;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Use the AddValue method to specify serialized values.
            info.AddValue("pos", Position, typeof(Vector3));
            info.AddValue("rot", Rotation, typeof(Vector3));
            info.AddValue("range", Range, typeof(float));

        }

        public LidarConfig(SerializationInfo info, StreamingContext context)
        {
            Position = (Vector3)info.GetValue("pos", typeof(Vector3));
            Rotation = (Vector3)info.GetValue("rot", typeof(Vector3));
            Range = (float)info.GetValue("range", typeof(float));
        }

    }

}