using CAVS.ProjectOrganizer.Scenes.Showcase;
using UnityEngine;
using VRTK;

namespace KarlSmink.Teleporting
{

    public class CameraControl : MonoBehaviour
    {

        private VRTK_ControllerEvents hand;

        private CameraBehavior cameraToControl;
        public bool heightAdjust;

        public static CameraControl Initialize(VRTK_ControllerEvents hand, CameraBehavior cameraToControl)
        {
            if (hand == null)
            {
                throw new System.ArgumentException("Unable to find VRTK Controller Events");
            }

            if (cameraToControl == null)
            {
                throw new System.ArgumentException("We need a camera to control!");
            }

            var newScript = hand.gameObject.AddComponent<CameraControl>();
            newScript.hand = hand;
            newScript.hand.TouchpadAxisChanged += newScript.Hand_TouchpadPressed;
            newScript.hand.TouchpadTouchEnd += newScript.Hand_TouchpadTouchEnd;
            newScript.hand.ButtonTwoPressed += newScript.Hand_StartMenuPressed;
            newScript.cameraToControl = cameraToControl;
            return newScript;
        }

        private void Hand_StartMenuPressed(object sender, ControllerInteractionEventArgs e)
        {
            cameraToControl.ToggleLock();
        }

        private void Hand_TouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
        {
            cameraToControl.Move(Vector3.zero, Space.Self);
        }

        private void Hand_TouchpadPressed(object sender, ControllerInteractionEventArgs e)
        {
            if (hand.touchpadPressed)
            {
                heightAdjust = GameObject.FindObjectOfType<SceneManagerBehavior>().allowHeightAdjustTVP;
                if(!heightAdjust) cameraToControl.Move(new Vector3(e.touchpadAxis.x, 0, 0), Space.Self);
                else cameraToControl.Move(new Vector3(e.touchpadAxis.x, 0, e.touchpadAxis.y), Space.Self);
            }
        }

        private void OnDestroy()
        {
            hand.TouchpadAxisChanged -= Hand_TouchpadPressed;
        }

    }

}