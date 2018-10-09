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
        /// <param name="parent">If the object does not have a parent, pass in null</param>

        //TODO: Get ObjectKey to work for passing parent
        public AnvelObject(AnvelControlService.Client connection, string objectName, string assetName, ObjectDescriptor parent)
        {
            this.client = connection;
            this.objectName = objectName;
            this.assetName = assetName;
            this.objectPosition = new Point3();
            this.objectOrientation = new Euler();

            try
            {
                ObjectDescriptor randomObject = client.GetObjectDescriptorByName(objectName);
                objectDescriptor = client.GetObjectDescriptorByName(objectName);
            }
            catch (ObjectNameNotFound e)
            {
                objectOrientation.Pitch = 0;
                objectOrientation.Roll = 0;
                objectOrientation.Yaw = 0;

                objectPosition.X = 0;
                objectPosition.Y = 0;
                objectPosition.Z = 0;

                if(parent == null)
                {
                    objectDescriptor = client.CreateObject(assetName, objectName, 0, objectPosition, objectOrientation, false);
                }
                else
                {
                    objectDescriptor = client.CreateObject(assetName, objectName, parent.ObjectKey, objectPosition, objectOrientation, false);
                }
                
            }
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
            objectOrientation.Roll = ConvertToRadians(x);
            objectOrientation.Yaw = ConvertToRadians(y);
            objectOrientation.Pitch = ConvertToRadians(z);

            UpdateObjectTransform();
        }

        private float ConvertToRadians(float degrees)
        {
            return (Math.PI / 180) * degrees;
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
