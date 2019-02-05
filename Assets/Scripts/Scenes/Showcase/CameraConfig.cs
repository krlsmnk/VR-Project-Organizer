using UnityEngine;
using System.Runtime.Serialization;
using System;
namespace CAVS.ProjectOrganizer.Scenes.Showcase
{

    public class CameraConfig
    {

        private Vector3 position;

        private Vector3 rotation;

        public Vector3 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public Vector3 Rotation
        {
            get
            {
                return rotation;
            }

            set
            {
                rotation = value;
            }
        }


        public CameraConfig()
        {

        }

        public CameraConfig(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Use the AddValue method to specify serialized values.
            info.AddValue("pos", position, typeof(Vector3));
            info.AddValue("rot", rotation, typeof(Vector3));

        }

        public CameraConfig(SerializationInfo info, StreamingContext context)
        {
            position = (Vector3)info.GetValue("pos", typeof(Vector3));
            rotation = (Vector3)info.GetValue("rot", typeof(Vector3));
        }

    }

}