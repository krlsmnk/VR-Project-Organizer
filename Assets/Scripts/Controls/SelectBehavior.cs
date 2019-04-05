using UnityEngine;
using VRTK;
using CAVS.ProjectOrganizer.Interation;

namespace CAVS.ProjectOrganizer.Controls
{
    public class SelectBehavior : MonoBehaviour
    {

        private LineRenderer pointer;

        private VRTK_ControllerEvents hand;

        private ISelectable[] currentSelectable;

        private ISelectable[] currentHover;

        public static SelectBehavior Initialize(VRTK_ControllerEvents hand)
        {
            var newScript = hand.gameObject.AddComponent<SelectBehavior>();
            newScript.TurnOnPointer();
            newScript.hand = hand;
            newScript.currentSelectable = null;
            newScript.currentHover = null;
            newScript.hand.GripPressed += newScript.Hand_GripPressed;
            newScript.hand.TriggerClicked += newScript.Hand_TriggerPressed;
            newScript.hand.TriggerUnclicked += newScript.Hand_TriggerReleased;
            return newScript;
        }

        private void UpdateCurrentHovered(ISelectable[] newhover)
        {
            if (newhover == currentHover)
            {
                return;
            }

            if (currentHover != null)
            {
                foreach (var sel in currentHover)
                {
                    sel.UnHover(gameObject);
                }
            }
            currentHover = newhover;

            if (currentHover != null)
            {
                foreach (var sel in currentHover)
                {
                    sel.Hover(gameObject);
                }
            }
        }

        private void Hand_TriggerPressed(object sender, ControllerInteractionEventArgs e)
        {
            if (currentHover != currentSelectable)
            {

                if (currentSelectable != null)
                {
                    foreach (var sel in currentSelectable)
                    {
                        sel.UnSelect(gameObject);
                    }
                }
            }

            if (currentHover == null)
            {
                return;
            }

            currentSelectable = currentHover;
            foreach (var sel in currentSelectable)
            {
                sel.SelectPress(gameObject);
            }

        }

        private void Hand_TriggerReleased(object sender, ControllerInteractionEventArgs e)
        {
            if (currentSelectable != null)
            {
                foreach (var sel in currentSelectable)
                {
                    sel.SelectUnpress(gameObject);
                }
            }
        }

        private void Hand_GripPressed(object sender, ControllerInteractionEventArgs e)
        {
            if (PointsIsOn())
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
            if (pointer != null)
            {
                pointer.SetPosition(0, transform.position);

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
                {
                    UpdateCurrentHovered(hit.transform.gameObject.GetComponents<ISelectable>());
                    pointer.SetPosition(1, hit.point);
                }
                else
                {
                    UpdateCurrentHovered(null);
                    pointer.SetPosition(1, transform.position + (transform.rotation * Vector3.forward * 100));
                }
            }

        }

        private bool PointsIsOn()
        {
            return pointer != null;
        }

        private void TurnOnPointer()
        {
            if (pointer != null)
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
            if (PointsIsOn() == false)
            {
                return;
            }
            Destroy(pointer);
        }

        private void OnDestroy()
        {
            if (PointsIsOn())
            {
                TurnOffPointer();
            }
            UpdateCurrentHovered(null);
            if (currentSelectable != null)
            {
                foreach (var sel in currentSelectable)
                {
                    sel.UnSelect(gameObject);
                }
            }
            hand.GripPressed -= Hand_GripPressed;
            hand.TriggerClicked -= Hand_TriggerPressed;
            hand.TriggerUnclicked -= Hand_TriggerReleased;
        }

    }

}