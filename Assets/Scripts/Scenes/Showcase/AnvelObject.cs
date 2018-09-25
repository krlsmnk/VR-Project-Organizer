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

        public AnvelObject(AnvelControlService.Client connection, string objectName, string assetName, ObjectKey parentKey)
        {
            this.client = connection;
            this.objectName = objectName;
            this.assetName = assetName;
            this.objectPosition = new Point3();
            this.objectOrientation = new Euler();
            try
            {
                objectName = client.GetObjectDescriptorByName(objectName);
            }
            catch (ObjectNameNotFound e)
            {
                objectDescriptor = client.CreateObject(assetName, objectName, parentKey);
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
            objectOrientation.x = x;
            objectOrientation.y = y;
            objectOrientation.z = z;

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
