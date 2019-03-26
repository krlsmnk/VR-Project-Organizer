using UnityEngine;
using VRTK;

namespace CAVS.ProjectOrganizer.Controls
{

    public class GrabControlBehavior : MonoBehaviour
    {
        private class ObjectState
        {
            Rigidbody rigidbody;

            RigidbodyConstraints rigidbodyConstraints;

            bool colliderIsEnabled;

            Transform parent;

            public ObjectState(GameObject gameObject)
            {
                var col = gameObject.GetComponent<Collider>();
                if (col != null)
                {
                    colliderIsEnabled = col.enabled;
                }

                rigidbody = gameObject.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbodyConstraints = rigidbody.constraints;
                }

                parent = gameObject.transform.parent;
            }

            public void Restore(GameObject gameObject, Vector3 currentControllerVelocity)
            {
                var col = gameObject.GetComponent<Collider>();
                if (col != null)
                {
                    col.enabled = colliderIsEnabled;
                }

                if (rigidbody != null)
                {
                    rigidbody.constraints = rigidbodyConstraints;
                    rigidbody.velocity = currentControllerVelocity;
                }

                gameObject.transform.SetParent(parent);
            }
        }

        private LineRenderer pointer;

        private VRTK_ControllerEvents hand;

        private VRTK_InteractableObject interactableObject;

        private float distanceOfObjectFromController;

        private ObjectState objectStateOnGrab;

        private Vector3 objectPositionLastFrame;

        public static GrabControlBehavior Initialize(VRTK_ControllerEvents hand)
        {
            var newScript = hand.gameObject.AddComponent<GrabControlBehavior>();
            newScript.TurnOnPointer();
            newScript.hand = hand;
            newScript.interactableObject = null;

            newScript.hand.GripPressed += newScript.Hand_GripPressed;
            newScript.hand.TriggerClicked += newScript.Hand_TriggerPressed;
            newScript.hand.TriggerUnclicked += newScript.Hand_TriggerReleased;
            newScript.hand.TouchpadAxisChanged += newScript.TouchpadAxisChanged;
            newScript.hand.TouchpadTouchEnd += newScript.Hand_TouchpadTouchEnd;

            return newScript;
        }

        private void Hand_TouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
        {
            lastTouchpadY = -666;
        }

        private int DistanceToPrecision(float distance)
        {
            if (Mathf.Abs(distance) < .1f)
            {
                return 6;
            }
            return Mathf.RoundToInt((Mathf.Log(Mathf.Abs(distance)) * -1.3f) + 2.075857104f);
        }

        private Vector3 Discritize(Vector3 pos, float distance)
        {
            var precision = DistanceToPrecision(distance);
            Debug.LogFormat("precision: {0};", precision);

            var precPow = Mathf.Pow(10, precision);
            return new Vector3(
                (Mathf.RoundToInt(pos.x * precPow) / precPow),
                (Mathf.RoundToInt(pos.y * precPow) / precPow),
                (Mathf.RoundToInt(pos.z * precPow) / precPow)
            );
        }

        float lastTouchpadY = 0;

        private void TouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
        {
            if (interactableObject != null)
            {
                if (lastTouchpadY != -666)
                {
                    distanceOfObjectFromController = Mathf.Clamp(distanceOfObjectFromController + (e.touchpadAxis.y - lastTouchpadY), 0, 1000f);
                }
                lastTouchpadY = e.touchpadAxis.y;
            }
        }

        private void UpdateInteractableObject(VRTK_InteractableObject newInteractable)
        {
            if (newInteractable == interactableObject)
            {
                return;
            }

            interactableObject = newInteractable;
        }

        private void Hand_TriggerReleased(object sender, ControllerInteractionEventArgs e)
        {
            if (interactableObject != null)
            {
                objectStateOnGrab.Restore(interactableObject.gameObject, ((interactableObject.transform.position - objectPositionLastFrame) / Time.deltaTime) * .2f);

                interactableObject = null;
                objectStateOnGrab = null;
            }
        }

        private void Hand_TriggerPressed(object sender, ControllerInteractionEventArgs e)
        {
            if (interactableObject != null && objectStateOnGrab == null)
            {
                objectStateOnGrab = new ObjectState(interactableObject.gameObject);
                distanceOfObjectFromController = Vector3.Distance(interactableObject.transform.position, transform.position);
                interactableObject.transform.SetParent(transform);
                interactableObject.transform.position = transform.position;

                var col = interactableObject.GetComponent<Collider>();
                if (col != null)
                {
                    col.enabled = false;
                }

                var rb = interactableObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }
            }
        }

        private void Hand_GripPressed(object sender, ControllerInteractionEventArgs e)
        {
            if (PointerIsOn())
            {
                TurnOffPointer();
            }
            else
            {
                TurnOnPointer();
            }
        }

        void Update()
        {
            if (pointer == null)
            {
                return;
            }

            pointer.SetPosition(0, transform.position);

            float distanceForObject = distanceOfObjectFromController;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                if (objectStateOnGrab == null)
                {
                    UpdateInteractableObject(hit.transform.gameObject.GetComponent<VRTK_InteractableObject>());
                }
                pointer.SetPosition(1, hit.point);

                distanceForObject = Mathf.Min(distanceForObject, hit.distance);
            }
            else
            {
                if (objectStateOnGrab == null)
                {
                    UpdateInteractableObject(null);
                }
                pointer.SetPosition(1, transform.position + (transform.rotation * Vector3.forward * 100));
            }


            if (objectStateOnGrab != null)
            {
                objectPositionLastFrame = interactableObject.transform.position;
                interactableObject.transform.position = Discritize((transform.forward * distanceForObject) + transform.position, distanceForObject);
            }
        }

        private bool PointerIsOn()
        {
            return pointer != null;
        }

        private void TurnOnPointer()
        {
            if (PointerIsOn())
            {
                return;
            }
            pointer = gameObject.AddComponent<LineRenderer>();
            if (pointer != null)
            {
                pointer.positionCount = 2;
                pointer.startWidth = .025f;
                pointer.endWidth = .025f;
            }

        }

        private void TurnOffPointer()
        {
            if (PointerIsOn() == false)
            {
                return;
            }
            Destroy(pointer);
        }

        private void OnDestroy()
        {
            if (PointerIsOn())
            {
                TurnOffPointer();
            }
            if (interactableObject != null)
            {
                objectStateOnGrab.Restore(interactableObject.gameObject, ((interactableObject.transform.position - objectPositionLastFrame) / Time.deltaTime) * .2f);

            }
            UpdateInteractableObject(null);
            hand.GripPressed -= Hand_GripPressed;
            hand.TriggerClicked -= Hand_TriggerPressed;
            hand.TriggerUnclicked -= Hand_TriggerReleased;
            hand.TouchpadAxisChanged -= TouchpadAxisChanged;
            hand.TouchpadTouchEnd -= Hand_TouchpadTouchEnd;
        }
    }

}