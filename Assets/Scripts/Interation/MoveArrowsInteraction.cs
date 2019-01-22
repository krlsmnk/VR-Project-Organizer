using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CAVS.ProjectOrganizer.Interation
{

    public class MoveArrowsInteraction : MonoBehaviour, ISelectable
    {
        private GameObject controller;

        private GameObject objectToControl;

        private Vector3 originalArrowPosition;

        private Vector3 pointOriginallyHit;

        private float callerOriginalDistance;

        public void Select(GameObject caller)
        {
            controller = caller;
            originalArrowPosition = transform.parent.position;
            callerOriginalDistance = Vector3.Distance(transform.parent.position, controller.transform.position);
            pointOriginallyHit = controller.transform.position + (controller.transform.forward * callerOriginalDistance);
        }

        public void SetObjectToControl(GameObject objectToControl)
        {
            this.objectToControl = objectToControl;
        }

        public void UnSelect(GameObject caller)
        {
            controller = null;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(controller != null)
            {
                transform.parent.position = controller.transform.position + (controller.transform.forward * callerOriginalDistance) - pointOriginallyHit + originalArrowPosition;
                objectToControl.transform.position = transform.parent.position;
            }
        }

    }

}