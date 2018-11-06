using UnityEngine;
using AnvelApi;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    public class AnvelObject
    {
        private ObjectDescriptor objectDescriptor;

        private AnvelControlService.Client client;

        /// <summary>
        /// Private so no one can directly create it, must use static methods.
        /// </summary>
        /// <param name="connection"></param>
        private AnvelObject(AnvelControlService.Client connection, ObjectDescriptor descriptor, bool newlyCreated)
        {
            client = connection;
            objectDescriptor = descriptor;

            if (newlyCreated)
            {
                AnvelObjectManager.Instance.RegisterCreatedObject(this);
            }
        }

        public static AnvelObject GetReferenceByName(AnvelControlService.Client connection, string objectName)
        {
            return new AnvelObject(connection, connection.GetObjectDescriptorByName(objectName), false);
        }

        //TODO: Get ObjectKey to work for passing parent
        public static AnvelObject CreateObject(AnvelControlService.Client connection, string objectName, string assetName, ObjectDescriptor parent)
        {
            return new AnvelObject(connection, connection.CreateObject(assetName, objectName, parent == null ? 0 : parent.ObjectKey, new Point3(), new Euler(), false), true);
        }

        public static AnvelObject CreateObject(AnvelControlService.Client connection, string objectName, string assetName)
        {
            return CreateObject(connection, objectName, assetName, null);
        }

        public void UpdateTransform(Vector3 pos, Vector3 rot)
        {
            var rotInRads = rot * Mathf.Deg2Rad;
            client.SetPoseRelE(objectDescriptor.ObjectKey, new Point3
            {
                X = pos.z,
                Y = pos.x,
                Z = pos.y
            }, new Euler
            {
                Pitch = rotInRads.z,
                Roll = rotInRads.x,
                Yaw = rotInRads.y
            });
        }

        public string ObjectName()
        {
            return objectDescriptor.ObjectName;
        }

        public ObjectDescriptor ObjectDescriptor()
        {
            return objectDescriptor;
        }


        public Vector3 Position()
        {
            var pose = client.GetPoseAbs(objectDescriptor.ObjectKey);
            return new Vector3((float)pose.Position.Y, (float)pose.Position.Z, (float)pose.Position.X);
        }

        public Vector3 Rotation()
        {
            var pose = client.GetPoseAbs(objectDescriptor.ObjectKey);
            return new Vector3((float)pose.Attitude.Euler.Roll, (float)pose.Attitude.Euler.Yaw, (float)pose.Attitude.Euler.Pitch);
        }

        public void RemoveObject()
        {
            client.RemoveObject(objectDescriptor.ObjectKey);
            Debug.Log($"Destroyed: {objectDescriptor.ObjectName} ({objectDescriptor.ObjectKey})");
        }

    }
}
