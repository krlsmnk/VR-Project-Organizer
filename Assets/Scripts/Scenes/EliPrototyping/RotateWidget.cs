using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAVS.ProjectOrganizer.Interation
{
    public class RotateWidget : MonoBehaviour, ISelectable
    {

        enum AxisToControl
        {
            X,
            Y,
            Z
        };

        private GameObject controller;

        private GameObject objectToControl;

        private Vector3 originalArrowPosition;

        private Vector3 pointOriginallyHit;

        private Vector3 originalForward;

        [SerializeField]
        private AxisToControl axisToControl;

        public void SetObjectToControl(GameObject objectToControl)
        {
            this.objectToControl = objectToControl;
        }

        public void SelectPress(GameObject caller)
        {
            controller = caller;
            originalArrowPosition = transform.parent.position;
            originalForward = transform.parent.forward;
            float callerOriginalDistance = Vector3.Distance(transform.parent.position, controller.transform.position);
            pointOriginallyHit = controller.transform.position + (controller.transform.forward * callerOriginalDistance);
        }
      
        public void UnSelect(GameObject caller) { }

        public void SelectUnpress(GameObject caller)
        {
            controller = null;
        }

        public void Hover(GameObject caller) { }

        public void UnHover(GameObject caller) { }

        void FixedUpdate()
        {
            if (controller == null)
            {
                return;
            }

            Plane orientationPlane;
            switch (axisToControl)
            {
                case AxisToControl.X:
                    orientationPlane = new Plane(
                        transform.parent.position + transform.parent.TransformDirection(Vector3.right),
                        transform.parent.position + transform.parent.TransformDirection(Vector3.up),
                        transform.parent.position
                        );
                    break;
                case AxisToControl.Y:
                    orientationPlane = new Plane(
                        transform.parent.position + transform.parent.TransformDirection(Vector3.left),
                        transform.parent.position + transform.parent.TransformDirection(Vector3.up),
                        transform.parent.position
                        );
                    break;
                case AxisToControl.Z:
                    orientationPlane = new Plane(
                        transform.parent.position + transform.parent.TransformDirection(Vector3.forward),
                        transform.parent.position + transform.parent.TransformDirection(Vector3.up),
                        transform.parent.position
                        );
                    break;
                default:
                    throw new System.Exception("Why the fuck did you add a 4th axis for fucking movement");
            }


            Ray ray = new Ray(controller.transform.position, controller.transform.forward);
            Debug.DrawRay(controller.transform.position, controller.transform.forward * 10, Color.blue);

            float enter = 0.0f;
            orientationPlane.Raycast(ray, out enter);

            if (enter == 0)
            {
                return;
            }

            Vector3 posToSet = ray.GetPoint(enter);

            switch (axisToControl)
            {
                case AxisToControl.X:
                    // posToSet.y = originalArrowPosition.y;
                    // posToSet.z = originalArrowPosition.z;
                    // transform.parent.Rotate(0, 0, Vector3.SignedAngle(transform.forward, posToSet.normalized, Vector3.right));
                    break;
                case AxisToControl.Y:
                    posToSet.x = originalArrowPosition.x;
                    posToSet.z = originalArrowPosition.z;
                    // transform.parent.Rotate(0, 0, Vector3.SignedAngle(transform.forward, posToSet.normalized, Vector3.up));
                    break;
                case AxisToControl.Z:
                    posToSet.y = originalArrowPosition.y;
                    posToSet.x = originalArrowPosition.x;
                    // transform.parent.Rotate(0, 0, Vector3.SignedAngle(transform.forward, posToSet.normalized, Vector3.forward));
                    break;
            }

            transform.parent.LookAt(posToSet);

        }



    }

}