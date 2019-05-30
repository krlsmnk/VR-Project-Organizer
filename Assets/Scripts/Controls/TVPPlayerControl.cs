using System;
using UnityEngine;
using VRTK;
using KarlSmink.Teleporting;
using System.Collections.Generic;
using CAVS.ProjectOrganizer.Scenes.Showcase;

namespace CAVS.ProjectOrganizer.Controls
{
    public class TVPPlayerControl : PlayerControl
    {
        private Transform headsetTransform;

        void Awake()
        {
            VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);
        }
        void OnDestroy()
        {
            VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
        }

        void OnEnable()
        {
            headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
        }

        public override Action Build(VRTK_ControllerEvents hand)
        {
            
            var playerControlBehaviorScript = UnityEngine.Object.FindObjectOfType<PlayerControlBehavior>();
            playerControlBehaviorScript.killRadialMenu();
            //hand = new VRTK_ControllerEvents();

            if(headsetTransform==null)headsetTransform = VRTK_DeviceFinder.HeadsetTransform();

            var cameraToControl = KarlSmink.Teleporting.Util.BuildCamera(Vector3.zero, Quaternion.identity);
            //CNG if (headsetTransform == null) headsetTransform = GameObject.FindObjectOfType<SceneManagerBehavior>().headsetNullfix;
            Debug.Log(headsetTransform.gameObject.name);
            var portal = KarlSmink.Teleporting.Util.BuildPortal(cameraToControl.GetComponentInChildren<Camera>(), headsetTransform.transform.position + (headsetTransform.forward * 8), Quaternion.identity);
            var headsetCollision = UnityEngine.Object.FindObjectOfType<VRTK_HeadsetCollision>();
            var teleBehavior = TeleportBehavior.Initialize(headsetCollision, 1.7f, cameraToControl.transform, portal);

            var cameraBehavior = CameraBehavior.Initialize(cameraToControl, 2f, portal);
            cameraBehavior.transform.position = hand.transform.position + hand.transform.forward;
            cameraBehavior.transform.LookAt(hand.transform.position + (hand.transform.forward));
            var control = TVPCameraControl.Initialize(hand, cameraBehavior, this);
            
            return delegate ()
            {
                UnityEngine.Object.Destroy(cameraToControl);
                UnityEngine.Object.Destroy(portal);
                UnityEngine.Object.Destroy(control);
                UnityEngine.Object.Destroy(teleBehavior);
                playerControlBehaviorScript.rebuildMenu();
            };
        }        

        public override Texture2D GetIcon()
        {
            return Resources.Load<Texture2D>("PlayerControl/teleport-alternative-icon");
        }

        public void cleanup()
        {
            var playerControlBehaviorScript = UnityEngine.Object.FindObjectOfType<PlayerControlBehavior>();
            //playerControlBehaviorScript.SwitchToControl(playerControlBehaviorScript.currentWeaponIndex);
            playerControlBehaviorScript.SwitchToControl(-1);
        }
    }

}