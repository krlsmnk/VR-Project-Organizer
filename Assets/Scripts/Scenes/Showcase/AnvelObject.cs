using UnityEngine;

using AnvelApi;
namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    public class AnvelObject
    {
        private string objectName;

        private string assetName;

        private ObjectDescriptor objectDescriptor;

        private AnvelControlService.Client client;

        private Point3 objectPosition;

        private Euler objectOrientation;

        /// <summary>
        /// Creates or fetches an object in ANVEL by object name and asset name
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="objectName"></param>
        /// <param name="assetName"></param>
        /// <param name="parentKey">If the object does not have a parent, pass in 0</param>

        public AnvelObject(AnvelControlService.Client connection, string objectName, string assetName)
        {
            this.client = connection;
            this.objectName = objectName;
            this.assetName = assetName;
            this.objectPosition = new Point3();
            this.objectOrientation = new Euler();
            try
            {
                objectDescriptor = client.GetObjectDescriptorByName(objectName);
            }
            catch (ObjectNameNotFound e)
            {
                objectOrientation.roll = 0;
                objectOrientation.pitch = 0;
                objectOrientation.yaw = 0;

                objectPosition.x = 0;
                objectPosition.y = 0;
                objectPosition.z = 0;

                objectDescriptor = client.CreateObject(assetName, objectName, 0, objectPosition, objectOrientation, false);
            }
        }

        public void SetObjectPosition(float x, float y, float z)
        {
            objectPosition.x = x;
            objectPosition.y = y;
            objectPosition.z = z;

            UpdateObjectTransform();
        }

        public void SetObjectRotation(float x, float y, float z)
        {
            objectOrientation.roll = x;
            objectOrientation.pitch = y;
            objectOrientation.yaw = z;

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
