using UnityEngine;
using AnvelApi;
using VRTK;
using EliCDavis.UIGen;
using CAVS.Anvel;

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

        protected AnvelObject objectSensorWeArecontrolling;

        private Rigidbody rb;

        private VRTK_InteractableObject interactableObject;

        private ObjCreationState objCreationState;

        private Vector3 lastPosition;

        private GameObject uiView;

        private float lastValueSeen;

        public static CreateAnvelObjectOnCollision Build(string anvelAsset, Vector3 position, AnvelObject parent, AnvelControlService.Client connection)
        {
            GameObject newObj = null;
            CreateAnvelObjectOnCollision newScript = null;

            if (anvelAsset.Equals(AssetName.Sensors.API_3D_LIDAR))
            {
                newObj = Instantiate(Resources.Load<GameObject>("Lidar Sensor"));
                newScript = newObj.AddComponent<CreateLidarOnCollision>();
            } else if (anvelAsset.Equals(AssetName.Sensors.API_Camera))
            {
                newObj = Instantiate(Resources.Load<GameObject>("Camera Sensor"));
                newScript = newObj.AddComponent<CreateCameraOnCollision>();
            }

            if(newScript == null)
            {
                throw new System.Exception("Do not support: " + anvelAsset);
            }

            newObj.transform.position = position;

            newScript.rb = newObj.AddComponent<Rigidbody>();
            newScript.rb.useGravity = false;

            newScript.interactableObject = newObj.AddComponent<VRTK_InteractableObject>();
            newScript.interactableObject.InteractableObjectUngrabbed += newScript.OnUngrabbed;
            newScript.interactableObject.InteractableObjectGrabbed += newScript.OnGrabbed;

            newScript.parent = parent;
            newScript.connection = connection;
            newScript.objCreationState = ObjCreationState.NotCreated;
            newScript.lastPosition = newObj.transform.position;
            newScript.interactableObject.isGrabbable = true;
            newScript.objectWeArecontrolling = null;
            newScript.objectSensorWeArecontrolling = null;
            newScript.lastValueSeen = newScript.PropertyStartingValueForModifying();

            return newScript;
        }

        /// <summary>
        /// Called whenever the object is "un" grabbed under a VRTK context
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUngrabbed(object sender, InteractableObjectEventArgs e)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            if (uiView != null)
            {
                Destroy(uiView);
            }
        }

        private void OnGrabbed(object sender, InteractableObjectEventArgs e)
        {
            if (objCreationState != ObjCreationState.Created)
            {
                return;
            }

            var window = new Window(PropertyKeyForModifying(), new IElement[] {
                new SliderElement(PropertyRangeForModifying().x, PropertyRangeForModifying().y, lastValueSeen, delegate(float x) {
                    lastValueSeen = x;
                    connection.SetProperty(objectSensorWeArecontrolling.ObjectDescriptor().ObjectKey, PropertyKeyForModifying(), ((int)x).ToString());
                    Debug.Log("Sent value");
                }, delegate(float x) { return x.ToString("0.00"); }) });

            Vector3 position = transform.position + ((transform.rotation * new Vector3(.8f, .2f, 0)).normalized * .5f) ;

            uiView =  new View(window).Build(position, UnityEngine.Quaternion.identity, Vector2.one / 2f);
            uiView.transform.parent = transform;
            uiView.transform.localRotation = UnityEngine.Quaternion.Euler(0, 180, 0);

            uiView.AddComponent<VRTK_UICanvas>();
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

        protected abstract string PropertyKeyForModifying();

        protected abstract Vector2 PropertyRangeForModifying();

        protected abstract float PropertyStartingValueForModifying();

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