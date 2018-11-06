using UnityEngine;
using CAVS.Anvel;
using AnvelApi;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    public class CreateAnvelObjectOnCollision : MonoBehaviour
    {
        AnvelObject parent;

        AnvelControlService.Client connection;

        public static CreateAnvelObjectOnCollision Build(Vector3 position, AnvelObject parent, AnvelControlService.Client connection)
        {
            // Create the object
            GameObject newObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            newObj.transform.localScale = Vector3.one * .2f;
            newObj.transform.position = position;
            newObj.name = "Create Anvel Object";

            // Assign whatever necessary components
            newObj.AddComponent<Rigidbody>();
            newObj.AddComponent<VRTK.VRTK_InteractableObject>().isGrabbable = true;

            // Setup Anvel Stuff..
            CreateAnvelObjectOnCollision newScript = newObj.AddComponent<CreateAnvelObjectOnCollision>();
            newScript.parent = parent;
            newScript.connection = connection;
            return newScript;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.transform.name == "Big Car")
            {
                string name = $"Lidar - {Random.Range(0, 1000000)}"; // Because anvel is a whore and won't allow two objects to have the same name, despite having two different keys
                var lidar = AnvelObject.CreateObject(connection, name, AnvelAssetName.Sensors.API_3D_LIDAR, parent.ObjectDescriptor());
                lidar.UpdateTransform(Vector3.up * 1.5f, parent.Rotation());
                FindObjectsOfType<LiveDisplayBehavior>()[0].AddLidar(name, Color.blue, lidar.ObjectDescriptor());
                Destroy(gameObject);
            }
        }
    }

}