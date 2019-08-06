﻿using CAVS.ProjectOrganizer.Scenes.Showcase;
using System;
using UnityEngine;
using VRTK;
using VRTK.RecordAndPlay.Demo;

namespace CAVS.ProjectOrganizer.Controls
{

    public class TeleportPlayerControl : PlayerControl
    {

        public override Action Build(VRTK_ControllerEvents hand)
        {
            var renderer = hand.gameObject.AddComponent<VRTK_BezierPointerRenderer>();
            var pointer = hand.gameObject.AddComponent<VRTK_Pointer>();
            var teleport = hand.gameObject.AddComponent<VRTK_HeightAdjustTeleport>();

            pointer.targetListPolicy = hand.GetComponent<VRTK_PolicyList>();
            pointer.activateOnEnable = true;
            pointer.holdButtonToActivate = false;
            pointer.enableTeleport = true;
            pointer.activationButton = VRTK_ControllerEvents.ButtonAlias.GripPress;
            pointer.selectionButton = VRTK_ControllerEvents.ButtonAlias.TriggerPress;
            pointer.pointerRenderer = renderer;


            return delegate ()
            {
                UnityEngine.Object.Destroy(pointer);
                UnityEngine.Object.Destroy(renderer);
                UnityEngine.Object.Destroy(teleport);
            };
        }

         void Update()
        {
            if(Input.GetKey(KeyCode.R)) startRAP();
            if(Input.GetKey(KeyCode.S)) GameObject.FindObjectOfType<recordAndPlayManager>().saveRecording();
        }

        public void startRAP() { 
        //RAP

            Debug.Log("RAP Started");
            if (GameObject.FindObjectOfType<SceneManagerBehavior>().Recording)
            {
                DateTime thisDay = DateTime.Today;
                Debug.Log(thisDay.ToString());
                
                string recordingName = "Control__" + this.GetType().Name + "__Date__" +  thisDay.ToString();

                Debug.Log(recordingName);

                GameObject.FindObjectOfType<PlayerControlBehavior>().FireRAP(recordingName); // CNG
            }    
            
        }

        public override Texture2D GetIcon()
        {
            return Resources.Load<Texture2D>("PlayerControl/Teleport-icon");
        }
    }

}