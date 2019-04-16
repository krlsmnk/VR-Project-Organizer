using UnityEngine;
using AnvelApi;
using VRTK;
using EliCDavis.UIGen;
using CAVS.Anvel;

using CAVS.ProjectOrganizer.Interation;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    public abstract class AnvelSensorBehavior : MonoBehaviour, ISelectable
    {

        protected AnvelObject parent;

        protected AnvelControlService.Client connection;

        protected AnvelObject objectWeArecontrolling;

        protected AnvelObject objectSensorWeArecontrolling;

        private Rigidbody rb;

        private VRTK_InteractableObject interactableObject;


        private Vector3 lastPosition;

        private GameObject uiView;

        protected float lastValueSeen;

        public static AnvelSensorBehavior Build(string anvelAsset, Vector3 position, AnvelObject parent, AnvelControlService.Client connection, SensorManager sensorManager)
        {
            GameObject newObj = null;
            AnvelSensorBehavior newScript = null;

            if (anvelAsset.Equals(AssetName.Sensors.API_3D_LIDAR))
            {
                newObj = Instantiate(Resources.Load<GameObject>("Lidar Sensor"));
                newScript = newObj.AddComponent<LidarSensorBehavior>();
                sensorManager.RegisterLidar(newScript.GetComponent<LidarSensorBehavior>());
            }
            else if (anvelAsset.Equals(AssetName.Sensors.API_Camera))
            {
                newObj = Instantiate(Resources.Load<GameObject>("Camera Sensor"));
                newScript = newObj.AddComponent<CameraSensorBehavior>();
            }

            if (newScript == null)
            {
                throw new System.Exception("Do not support: " + anvelAsset);
            }

            newObj.AddComponent<TranslateBehavior>();

            newObj.transform.position = position;

            newScript.rb = newObj.AddComponent<Rigidbody>();
            newScript.rb.useGravity = false;
            newScript.rb.constraints = RigidbodyConstraints.FreezeAll;

            newScript.interactableObject = newObj.AddComponent<VRTK_InteractableObject>();
            newScript.interactableObject.InteractableObjectUngrabbed += newScript.OnUngrabbed;

            newScript.parent = parent;
            newScript.connection = connection;
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
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.name == "Big Car")
            {

                if (objectWeArecontrolling == null)
                {
                    GetComponent<Collider>().isTrigger = true;
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
            if (objectWeArecontrolling != null)
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
            interactableObject.InteractableObjectUngrabbed -= OnUngrabbed;
            if (objectWeArecontrolling != null)
            {
                objectWeArecontrolling.RemoveObject();
            }
        }


        public void SelectPress(GameObject caller)
        {
            if (uiView != null)
            {
                Destroy(uiView);
            }

            else if (objectSensorWeArecontrolling != null)
            {
                var window = new Window(PropertyKeyForModifying(), new IElement[] {
                new SliderElement(PropertyRangeForModifying().x, PropertyRangeForModifying().y, lastValueSeen, delegate(float x) {
                    lastValueSeen = x;
                    connection.SetProperty(objectSensorWeArecontrolling.ObjectDescriptor().ObjectKey, PropertyKeyForModifying(), ((int)x).ToString());
                    Debug.Log("Sent value");
                }, delegate(float x) { return x.ToString("0.00"); }) });

                Vector3 position = transform.position + ((transform.rotation * new Vector3(.8f, .2f, 0)).normalized * .5f);

                uiView = new View(window).Build(position, UnityEngine.Quaternion.identity, Vector2.one / 2f);
                uiView.transform.parent = transform;
                uiView.transform.localRotation = UnityEngine.Quaternion.Euler(0, 180, 0);

                uiView.AddComponent<VRTK_UICanvas>();

            }
        }

        public void UnSelect(GameObject caller) { }

        public void SelectUnpress(GameObject caller) { }

        public void Hover(GameObject caller) { }

        public void UnHover(GameObject caller) { }
    }

}