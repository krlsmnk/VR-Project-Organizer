using System;
using UnityEngine;
using VRTK;
using KarlSmink.Teleporting;
using System.Collections.Generic;
using CAVS.ProjectOrganizer.Scenes.Showcase;
using static VRTK.VRTK_SDKObjectAlias;
using System.Diagnostics;

namespace CAVS.ProjectOrganizer.Controls
{
    public class TouchToMoveControl : PlayerControl
    {        
        private Transform headsetTransform;
                
        public override Action Build(VRTK_ControllerEvents hand)
        {   
            var playerControlBehaviorScript = UnityEngine.Object.FindObjectOfType<PlayerControlBehavior>();
            playerControlBehaviorScript.killRadialMenu();

            if(headsetTransform==null)headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
            GameObject cameraRig = VRTK_DeviceFinder.HeadsetCamera().gameObject;
            VRTK_TouchpadWalking tpWalkScript = cameraRig.AddComponent<VRTK_TouchpadWalking>();
            tpWalkScript.maxWalkSpeed = GameObject.FindObjectOfType<SceneManagerBehavior>().CameraSpeed;
            tpWalkScript.moveOnButtonPress = VRTK_ControllerEvents.ButtonAlias.TouchpadPress;
            //VRTK_HeadsetCollisionFade fadingScript = cameraRig.AddComponent<VRTK_HeadsetCollisionFade>();           

            return delegate ()
            {
                UnityEngine.Debug.Log("TouchToMove Delegate Triggered.");
            };
        }       

        public override Texture2D GetIcon()
        {
            return Resources.Load<Texture2D>("PlayerControl/touch-to-move-icon");
        }
    }

}