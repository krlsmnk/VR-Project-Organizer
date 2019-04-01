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

        private Plane orientationPlane;

        [SerializeField]
        private AxisToControl axisToControl;

        public void Select(GameObject caller)
        {
            if (controller == null)
            {
                controller = caller;
                originalArrowPosition = transform.parent.position;
                originalForward = transform.parent.forward;
                float callerOriginalDistance = Vector3.Distance(transform.parent.position, controller.transform.position);
                pointOriginallyHit = controller.transform.position + (controller.transform.forward * callerOriginalDistance);
                switch (axisToControl)
                {
                    case AxisToControl.X:
                        orientationPlane = new Plane(
                            transform.parent.position + transform.parent.TransformDirection(Vector3.left),
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
                }
            }
            else
            {
                controller = null;
            }

        }

        public void SetObjectToControl(GameObject objectToControl)
        {
            this.objectToControl = objectToControl;
        }

        public void UnSelect(GameObject caller)
        {
        }

        void FixedUpdate()
        {
            if (controller != null)
            {
                Ray ray = new Ray(controller.transform.position, controller.transform.forward);

                float enter = 0.0f;
                orientationPlane.Raycast(ray, out enter);

                if (Mathf.Abs(enter) > 0)
                {
                    Vector3 posToSet = ray.GetPoint(enter) - pointOriginallyHit + originalArrowPosition;

                    switch (axisToControl)
                    {
                        case AxisToControl.X:
                            posToSet.y = originalArrowPosition.y;
                            posToSet.z = originalArrowPosition.z;
							transform.parent.Rotate(0, 0, Vector3.SignedAngle(transform.forward, posToSet.normalized, Vector3.right));
                            break;
                        case AxisToControl.Y:
                            posToSet.x = originalArrowPosition.x;
                            posToSet.z = originalArrowPosition.z;
							transform.parent.Rotate(0, 0, Vector3.SignedAngle(transform.forward, posToSet.normalized, Vector3.up));
                            break;
                        case AxisToControl.Z:
                            posToSet.y = originalArrowPosition.y;
                            posToSet.x = originalArrowPosition.x;
							transform.parent.Rotate(0, 0, Vector3.SignedAngle(transform.forward, posToSet.normalized, Vector3.forward));
                            break;
                    }



                }
            }
        }

    }
}