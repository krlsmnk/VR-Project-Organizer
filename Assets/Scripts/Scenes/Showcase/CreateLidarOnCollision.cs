using UnityEngine;
using CAVS.Anvel;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    public class CreateLidarOnCollision : CreateAnvelObjectOnCollision
    {
        protected override void CreateApprorpiateAnvelObject()
        {
            string name = $"Lidar - {Random.Range(0, 1000000)}"; // Because anvel is a whore and won't allow two objects to have the same name, despite having two different keys
            objectWeArecontrolling = AnvelObject.CreateObject(connection, name, AnvelAssetName.Sensors.API_3D_LIDAR, parent.ObjectDescriptor());
            objectWeArecontrolling.UpdateTransform(transform.localPosition, transform.localRotation);
            FindObjectsOfType<LiveDisplayBehavior>()[0].AddLidar(name, Color.blue, objectWeArecontrolling.ObjectDescriptor());
        }

    }

}