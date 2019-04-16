using UnityEngine;


namespace CAVS.ProjectOrganizer.Interation
{

    public class MoveArrowsInteraction : MonoBehaviour, ISelectable
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

        private Plane orientationPlane;

        [SerializeField]
        private AxisToControl axisToControl;

        public void SelectPress(GameObject caller)
        {
            controller = caller;
            originalArrowPosition = transform.parent.position;
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


        public void UnSelect(GameObject caller)
        {
        }

        public void SelectUnpress(GameObject caller)
        {
            controller = null;
        }

        public void Hover(GameObject caller)
        {
        }

        public void UnHover(GameObject caller)
        {
        }

        public void SetObjectToControl(GameObject objectToControl)
        {
            this.objectToControl = objectToControl;
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
                            break;
                        case AxisToControl.Y:
                            posToSet.x = originalArrowPosition.x;
                            posToSet.z = originalArrowPosition.z;
                            break;
                        case AxisToControl.Z:
                            posToSet.y = originalArrowPosition.y;
                            posToSet.x = originalArrowPosition.x;
                            break;
                    }

                    transform.parent.position = posToSet;
                    objectToControl.transform.position = transform.parent.position;

                }
            }
            else
            {
                transform.parent.position = objectToControl.transform.position;
            }
        }


    }

}