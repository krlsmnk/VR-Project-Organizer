﻿using System;
using UnityEngine;
using VRTK;
using KarlSmink.Teleporting;
using System.Collections.Generic;
using CAVS.ProjectOrganizer.Scenes.Showcase;
using static VRTK.VRTK_SDKObjectAlias;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

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

            //RAP
            if (GameObject.FindObjectOfType<SceneManagerBehavior>().Recording)
            {
                DateTime thisDay = DateTime.Today;
                Debug.Log(thisDay.ToString());
                
                string recordingName = "Control__" + this.GetType().Name + "__Date__" +  thisDay.ToString();

                Debug.Log(recordingName);

                GameObject.FindObjectOfType<PlayerControlBehavior>().FireRAP(recordingName); // CNG
            }

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