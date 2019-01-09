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

            var t = connection.EnumeratePropertyNames(objectWeArecontrolling.ObjectDescriptor().ObjectKey);
            foreach (var prop in t)
            {
                Debug.Log(prop.ToString());
            }
        }

        protected override string PropertyKeyForModifying()
        {
            return "Range";
        }

        protected override Vector2 PropertyRangeForModifying()
        {
            return new Vector2(1, 100);
        }

        protected override float PropertyStartingValueForModifying()
        {
            return 75f;
        }
    }

}