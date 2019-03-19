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

            Transform parent;

            public ObjectState(GameObject gameObject)
            {
                rigidbody = gameObject.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbodyConstraints = rigidbody.constraints;
                }

                parent = gameObject.transform.parent;
            }

            public void Restore(GameObject gameObject, Vector3 currentControllerVelocity)
            {
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

        private ObjectState objectStateOnGrab;

        private Vector3 controllerPositionLastFrame;

        public static GrabControlBehavior Initialize(VRTK_ControllerEvents hand)
        {
            var newScript = hand.gameObject.AddComponent<GrabControlBehavior>();
            newScript.TurnOnPointer();
            newScript.hand = hand;
            newScript.interactableObject = null;
            newScript.hand.GripPressed += newScript.Hand_GripPressed;
            newScript.hand.TriggerClicked += newScript.Hand_TriggerPressed;
            newScript.hand.TriggerUnclicked += newScript.Hand_TriggerReleased;
            return newScript;
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
                RaycastHit hit;
                if (Physics.Raycast(transform.position + (transform.forward/4f), transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
                {
                    objectStateOnGrab.Restore(interactableObject.gameObject, Vector3.zero);
                    interactableObject.transform.position = hit.point + (Vector3.up / 4f);
                } else
                {
                    objectStateOnGrab.Restore(interactableObject.gameObject, (transform.position - controllerPositionLastFrame) / Time.deltaTime);
                }

                interactableObject = null;
                objectStateOnGrab = null;
            }
        }

        private void Hand_TriggerPressed(object sender, ControllerInteractionEventArgs e)
        {
            if (interactableObject != null)
            {
                objectStateOnGrab = new ObjectState(interactableObject.gameObject);

                interactableObject.transform.SetParent(transform);
                interactableObject.transform.position = transform.position;

                if (interactableObject.GetComponent<Rigidbody>() != null)
                {
                    interactableObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
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

            RaycastHit hit;
            if (Physics.Raycast(transform.position + (transform.forward / 4f), transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                if (objectStateOnGrab == null)
                {
                    UpdateInteractableObject(hit.transform.gameObject.GetComponent<VRTK_InteractableObject>());
                }
                pointer.SetPosition(1, hit.point);
            }
            else
            {
                if (objectStateOnGrab == null)
                {
                    UpdateInteractableObject(null);
                }
                pointer.SetPosition(1, transform.position + (transform.rotation * Vector3.forward * 100));
            }
            controllerPositionLastFrame = transform.position;

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
            UpdateInteractableObject(null);
            hand.GripPressed -= Hand_GripPressed;
            hand.TriggerClicked -= Hand_TriggerPressed;
            hand.TriggerUnclicked -= Hand_TriggerReleased;
        }
    }

}