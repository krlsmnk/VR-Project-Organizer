using System;
using UnityEngine;
using VRTK;
using KarlSmink.Teleporting;

namespace CAVS.ProjectOrganizer.Controls
{
    public class TVPCameraControl : MonoBehaviour
    {

        private VRTK_ControllerEvents hand;

        private CameraBehavior cameraToControl;

        public static TVPCameraControl Initialize(VRTK_ControllerEvents hand, CameraBehavior cameraToControl)
        {
            if (hand == null)
            {
                throw new ArgumentException("Unable to find VRTK Controller Events");
            }

            if (cameraToControl == null)
            {
                throw new ArgumentException("We need a camera to control!");
            }

            var newScript = hand.gameObject.AddComponent<TVPCameraControl>();
            newScript.hand = hand;
            newScript.hand.ButtonTwoPressed += newScript.Hand_StartMenuPressed;
            newScript.hand.TriggerClicked += newScript.Hand_TriggerClicked;
            newScript.cameraToControl = cameraToControl;
            return newScript;
        }

        private Vector3 startingControllerLocation;

        private void Hand_TriggerClicked(object sender, ControllerInteractionEventArgs e)
        {
            startingControllerLocation = transform.position;
        }

        private void Hand_StartMenuPressed(object sender, ControllerInteractionEventArgs e)
        {
            cameraToControl.ToggleLock();
        }

        private void Update()
        {
            if (hand.triggerClicked)
            {
                cameraToControl.Move((transform.position - startingControllerLocation)/2);
            } else
            {
                cameraToControl.Move(Vector3.zero);
            }
        }

        private void OnDestroy()
        {
            hand.ButtonTwoPressed -= Hand_StartMenuPressed;
            hand.TriggerClicked -= Hand_TriggerClicked;
        }

    }

}