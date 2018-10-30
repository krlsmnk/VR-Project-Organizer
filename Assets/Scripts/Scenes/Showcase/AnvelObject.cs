using UnityEngine;
using AnvelApi;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    public class AnvelObject
    {
        private ObjectDescriptor objectDescriptor;

        private AnvelControlService.Client client;

        private Point3 objectPosition;

        private Euler objectOrientation;

        /// <summary>
        /// Private so no one can directly create it, must use static methods.
        /// </summary>
        /// <param name="connection"></param>
        private AnvelObject(AnvelControlService.Client connection, ObjectDescriptor descriptor)
        {
            client = connection;
            objectDescriptor = descriptor;
            objectOrientation = new Euler
            {
                Pitch = 0,
                Roll = 0,
                Yaw = 0
            };
            objectPosition = new Point3
            {
                X = 0,
                Y = 0,
                Z = 0
            };
        }

        public static AnvelObject GetReference(AnvelControlService.Client connection, string objectName)
        {
            return new AnvelObject(connection, connection.GetObjectDescriptorByName(objectName));
        }

        //TODO: Get ObjectKey to work for passing parent
        public static AnvelObject CreateObject(AnvelControlService.Client connection, string objectName, string assetName, ObjectDescriptor parent)
        {
            return new AnvelObject(connection, connection.CreateObject(assetName, objectName, parent == null ? 0 : parent.ObjectKey, new Point3(), new Euler(), false));
        }

        public static AnvelObject CreateObject(AnvelControlService.Client connection, string objectName, string assetName)
        {
            return CreateObject(connection, objectName, assetName, null);
        }

        public void UpdateTransform(Vector3 pos, Vector3 rot)
        {
            SetObjectPosition(pos);
            SetObjectRotation(rot);
            UpdateObjectTransform();
        }

        private void SetObjectPosition(Vector3 pos)
        {
            objectPosition.X = pos.z;
            objectPosition.Y = pos.x;
            objectPosition.Z = pos.y;
        }

        private void SetObjectRotation(Vector3 rot)
        {
            var rotInRads = rot * Mathf.Deg2Rad;
            objectOrientation.Roll = rotInRads.x;
            objectOrientation.Yaw = rotInRads.y;
            objectOrientation.Pitch = rotInRads.z;
        }

        private void UpdateObjectTransform()
        {
            client.SetPoseRelE(objectDescriptor.ObjectKey, objectPosition, objectOrientation);
        }

        public void RemoveObject()
        {
            client.RemoveObject(objectDescriptor.ObjectKey);
        }

    }
}
