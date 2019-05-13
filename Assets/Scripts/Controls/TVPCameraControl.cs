using System;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using KarlSmink.Teleporting;

namespace CAVS.ProjectOrganizer.Controls
{
    public class TVPCameraControl : MonoBehaviour
    {

        private VRTK_ControllerEvents hand;
        private CameraBehavior cameraToControl;
        private SteamVR_TrackedObject trackedObj;
        private Valve.VR.EVRButtonId touchpad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
        private Valve.VR.EVRButtonId trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
        //private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

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
            //newScript.hand.TriggerAxisChanged += newScript.Hand_TriggerAxisChanged;
            newScript.hand.TriggerClicked += newScript.Hand_TriggerClicked;
            newScript.hand.GripPressed += newScript.Hand_GripPressed;
            newScript.hand.TouchpadPressed += newScript.Hand_TouchpadPressed;
            newScript.hand.TouchpadReleased += newScript.Hand_TouchpadReleased;
            newScript.hand.TriggerTouchEnd += newScript.Hand_TriggerTouchEnd;
            newScript.cameraToControl = cameraToControl;
            return newScript;
        }

        private Vector3 startingControllerLocation;
        private Vector3 startingHeadsetForwardVector;


        //Button Mapping      
        private void Hand_TouchpadReleased(object sender, ControllerInteractionEventArgs e)
        {
            cameraToControl.Move((Vector3.zero).normalized, Space.World);
        }
        private void Hand_TriggerTouchEnd(object sender, ControllerInteractionEventArgs e)
        {
            //Debug.Log("TriggerTouchEnd");
            //cameraToControl.Move((Vector3.zero).normalized, Space.World);
        }
        private void Hand_TriggerClicked(object sender, ControllerInteractionEventArgs e)
        {
            /*
            ELI's trigger-based camera movement
            startingHeadsetForwardVector = VRTK_DeviceFinder.HeadsetTransform().transform.forward;
            startingControllerLocation = transform.position;
            */
        }

        private void Hand_StartMenuPressed(object sender, ControllerInteractionEventArgs e)
        {
            cameraToControl.ToggleLock();
        }

        /// <summary>
        /// When the button is pressed, this rebuilds the radial menu to allow control scheme switching again
        /// </summary>
        private void Hand_GripPressed(object sender, ControllerInteractionEventArgs e)
        {
            OnDestroy();
        }

        private void Hand_TouchpadPressed(object sender, ControllerInteractionEventArgs e)
        {
            //Vector2 axis = controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
            Vector2 axis = hand.GetTouchpadAxis();

            if ((axis.y > 0.25f) && (-0.5f < axis.x && axis.x < 0.5f))
            {
                //Debug.Log("Pan Up");
                cameraToControl.Move((Vector3.up).normalized, Space.World);
            }
            else if ((axis.y < -0.25f) && (-0.5f < axis.x && axis.x < 0.5f))
            {
                //Debug.Log("Pan Down");
                cameraToControl.Move((Vector3.down).normalized, Space.World);
            }

            if ((axis.x > 0.25f) && (-0.5f < axis.y && axis.y < 0.5f))
            {
                //Debug.Log("Pan Right");
                cameraToControl.Move((Vector3.right).normalized, Space.Self);
            }
            else if ((axis.x < -0.25f) && (-0.5f < axis.y && axis.y < 0.5f))
            {
                //Debug.Log("Pan Left");
                cameraToControl.Move((Vector3.left).normalized, Space.Self);
            }
        }
        private void Hand_TriggerAxisChanged()
        {
            float triggerAxis = hand.GetTriggerAxis();
            cameraToControl.Move((Vector3.forward * triggerAxis).normalized, Space.Self);
        }


        private void Update()
        {
            if (hand.triggerAxisChanged == true)
            {
                Hand_TriggerAxisChanged();
            }
        }

        private void Start()
        {
            trackedObj = GameObject.FindObjectOfType<SteamVR_TrackedObject>();
            
        }

        private void OnDestroy()
        {
            hand.ButtonTwoPressed -= Hand_StartMenuPressed;
            hand.TriggerClicked -= Hand_TriggerClicked;
            //hand.TriggerAxisChanged -= Hand_TriggerAxisChanged;
            hand.GripPressed -= Hand_GripPressed;
            hand.TouchpadPressed -= Hand_TouchpadPressed;
            hand.TouchpadReleased -= Hand_TouchpadReleased;
            hand.TriggerTouchEnd -= Hand_TriggerTouchEnd;

            GameObject[] portals = GameObject.FindGameObjectsWithTag("broadcastPlane");           
            foreach (GameObject thisPortal in portals)
            {
                Destroy(thisPortal);
            }
            portals = GameObject.FindGameObjectsWithTag("Portal");
            foreach (GameObject thisPortal in portals)
            {
                Destroy(thisPortal);
            }
            Texture2D[] icons = GameObject.FindObjectsOfType<Texture2D>();
            foreach (Texture2D thisIcon in icons)
            {
                Destroy(thisIcon);
            }

            var config = new ControllerConfig(new List<PlayerControl>()
            {
                new GrabPlayerControl(),
                new TeleportPlayerControl(),
                new SelectPlayerControl(),
                new TVPPlayerControl()
            });
            config.Build(hand);
        }
    }
}