using UnityEngine;
using AnvelApi;
using VRTK;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    public abstract class CreateAnvelObjectOnCollision : MonoBehaviour
    {

        private enum ObjCreationState
        {
            NotCreated,
            Created
        };

        protected AnvelObject parent;

        protected AnvelControlService.Client connection;

        protected AnvelObject objectWeArecontrolling;

        private Rigidbody rb;

        private VRTK_InteractableObject interactableObject;

        private ObjCreationState objCreationState;

        private Vector3 lastPosition;

        public static CreateAnvelObjectOnCollision Build(string anvelAsset, Vector3 position, AnvelObject parent, AnvelControlService.Client connection)
        {
            GameObject newObj = null;
            CreateAnvelObjectOnCollision newScript = null;

            if (anvelAsset.Equals(AnvelAssetName.Sensors.API_3D_LIDAR))
            {
                newObj = Instantiate(Resources.Load<GameObject>("Lidar Sensor"));
                newScript = newObj.AddComponent<CreateLidarOnCollision>();
            } else if (anvelAsset.Equals(AnvelAssetName.Sensors.API_Camera))
            {
                newObj = Instantiate(Resources.Load<GameObject>("Camera Sensor"));
                newScript = newObj.AddComponent<CreateCameraOnCollision>();
            }

            newObj.transform.position = position;

            newScript.rb = newObj.AddComponent<Rigidbody>();
            newScript.rb.useGravity = false;

            newScript.interactableObject = newObj.AddComponent<VRTK_InteractableObject>();
            newScript.interactableObject.InteractableObjectUngrabbed += delegate (object sender, InteractableObjectEventArgs e)
            {
                newScript.rb.velocity = Vector3.zero;
                newScript.rb.angularVelocity = Vector3.zero;
            };
            newScript.parent = parent;
            newScript.connection = connection;
            newScript.objCreationState = ObjCreationState.NotCreated;
            newScript.lastPosition = newObj.transform.position;
            newScript.interactableObject.isGrabbable = true;
            newScript.objectWeArecontrolling = null;

            return newScript;
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.name == "Big Car")
            {
                
                if (objCreationState == ObjCreationState.NotCreated)
                {
                    GetComponent<Collider>().isTrigger = true;
                    objCreationState = ObjCreationState.Created;
                    CreateApprorpiateAnvelObject();
                }

                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        protected abstract void CreateApprorpiateAnvelObject();

        private void Update()
        {
            Vector3 currentPosition = transform.position;
            if (objCreationState == ObjCreationState.Created)
            {
                if ((currentPosition - lastPosition).Equals(Vector3.zero) == false)
                {
                    objectWeArecontrolling.UpdateTransform(transform.localPosition, transform.localRotation);
                }
            }
            lastPosition = currentPosition;
        }

        private void OnDestroy()
        {
            if(objectWeArecontrolling != null)
            {
                objectWeArecontrolling.RemoveObject();
            }
        }
    }

}