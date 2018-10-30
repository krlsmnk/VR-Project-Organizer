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
        private AnvelObject(AnvelControlService.Client connection)
        {
            this.client = connection;
        }

        public static AnvelObject GetReference(AnvelControlService.Client connection, string objectName)
        {
            var obj = new AnvelObject(connection);
            obj.objectDescriptor = connection.GetObjectDescriptorByName(objectName);
            return obj;
        }

        //TODO: Get ObjectKey to work for passing parent
        public static AnvelObject CreateObject(AnvelControlService.Client connection, string objectName, string assetName, ObjectDescriptor parent)
        {
            var obj = new AnvelObject(connection);

            obj.objectOrientation = new Euler();
            obj.objectOrientation.Pitch = 0;
            obj.objectOrientation.Roll = 0;
            obj.objectOrientation.Yaw = 0;

            obj.objectPosition = new Point3();
            obj.objectPosition.X = 0;
            obj.objectPosition.Y = 0;
            obj.objectPosition.Z = 0;

            obj.objectDescriptor = connection.CreateObject(assetName, objectName, parent == null ? 0 : parent.ObjectKey, obj.objectPosition, obj.objectOrientation, false);

            return obj;
        }

        public static AnvelObject CreateObject(AnvelControlService.Client connection, string objectName, string assetName)
        {
            return CreateObject(connection, objectName, assetName, null);
        }

        public void SetObjectPosition(float x, float y, float z)
        {
            objectPosition.X = z;
            objectPosition.Y = x;
            objectPosition.Z = y;

            UpdateObjectTransform();
        }

        public void SetObjectRotation(float x, float y, float z)
        {
            objectOrientation.Roll = Mathf.Deg2Rad * x;
            objectOrientation.Yaw = Mathf.Deg2Rad * y;
            objectOrientation.Pitch = Mathf.Deg2Rad * z;

            UpdateObjectTransform();
        }

        private void UpdateObjectTransform()
        {
            client.SetPoseRelE(objectDescriptor.ObjectKey, objectPosition, objectOrientation);
        }

        public void RemoveObject()
        {
            client.RemoveObject(objectDescriptor.ObjectKey);
        }

        public ObjectDescriptor GetObjectDescriptor()
        {
            return objectDescriptor;
        }
    }
}
