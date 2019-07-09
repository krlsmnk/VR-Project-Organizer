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
    public class TVPPlayerControl : PlayerControl
    {        
        private Transform headsetTransform;
        //CNG 6/10
        public SDKObject sdkObject = SDKObject.Boundary;
        protected VRTK_SDKManager sdkManager;

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
            while(headsetTransform == null) headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
            //headsetTransform = VRTK_DeviceFinder.HeadsetTransform();

//START CNG 6/10
            sdkManager = VRTK_SDKManager.instance;
            if (sdkManager != null)
            {
                sdkManager.LoadedSetupChanged += LoadedSetupChanged;
            }
            ChildToSDKObject();

        }
        protected virtual void OnDisable()
        {
            if (sdkManager != null && !gameObject.activeSelf)
            {
                sdkManager.LoadedSetupChanged -= LoadedSetupChanged;
            }
        }
        protected virtual void LoadedSetupChanged(VRTK_SDKManager sender, VRTK_SDKManager.LoadedSetupChangeEventArgs e)
        {
            if (sdkManager != null)
            {
                ChildToSDKObject();
            }
        }

        protected virtual void ChildToSDKObject()
        {
            Vector3 currentPosition = transform.localPosition;
            Quaternion currentRotation = transform.localRotation;
            Vector3 currentScale = transform.localScale;
            Transform newParent = null;

            switch (sdkObject)
            {
                case SDKObject.Boundary:
                    newParent = VRTK_DeviceFinder.PlayAreaTransform();
                    break;
                case SDKObject.Headset:
                    newParent = VRTK_DeviceFinder.HeadsetTransform();
                    headsetTransform = newParent;
                    break;
            }

            transform.SetParent(newParent);
            transform.localPosition = currentPosition;
            transform.localRotation = currentRotation;
            transform.localScale = currentScale;
        }
//END CNG 6/10
        




        public override Action Build(VRTK_ControllerEvents hand)
        {
            var playerControlBehaviorScript = UnityEngine.Object.FindObjectOfType<PlayerControlBehavior>();
            playerControlBehaviorScript.killRadialMenu();
            //hand = new VRTK_ControllerEvents();

            if(headsetTransform==null)headsetTransform = VRTK_DeviceFinder.HeadsetTransform();

            var cameraToControl = KarlSmink.Teleporting.Util.BuildCamera(Vector3.zero, Quaternion.identity);
            cameraToControl.GetComponentInChildren<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("Roof");
            cameraToControl.GetComponentInChildren<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("Water");


            //CNG 6/11
            //getTheDamnHeadsetTransform();
            

            var portal = KarlSmink.Teleporting.Util.BuildPortal(cameraToControl.GetComponentInChildren<Camera>(), headsetTransform.transform.position + (headsetTransform.forward * 8), Quaternion.identity);
            var headsetCollision = UnityEngine.Object.FindObjectOfType<VRTK_HeadsetCollision>();
            var teleBehavior = TeleportBehavior.Initialize(headsetCollision, 1.7f, cameraToControl.transform, portal);

            var cameraBehavior = CameraBehavior.Initialize(cameraToControl, FindObjectOfType<SceneManagerBehavior>().CameraSpeed, portal);
            cameraBehavior.transform.position = hand.transform.position + hand.transform.forward;
            cameraBehavior.transform.LookAt(hand.transform.position + (hand.transform.forward));
            var control = TVPCameraControl.Initialize(hand, cameraBehavior, this);

            //CNG 6/5 - Keep the camera from changing heights
            if (!FindObjectOfType<SceneManagerBehavior>().allowHeightAdjustTVP)
            {
                GameObject camBehaviorObj = FindObjectOfType<CameraBehavior>().gameObject.transform.parent.gameObject; //footstepoffset

                //Set the camera's height to the user's height, then prevent it from moving at all
                camBehaviorObj.transform.position = new Vector3(camBehaviorObj.transform.position.x, FindObjectOfType<SceneManagerBehavior>().userHeight, camBehaviorObj.transform.position.z); 
            }

            return delegate ()
            {
                UnityEngine.Object.Destroy(cameraToControl);
                UnityEngine.Object.Destroy(portal);
                UnityEngine.Object.Destroy(control);
                UnityEngine.Object.Destroy(teleBehavior);
                playerControlBehaviorScript.rebuildMenu();
            };
        }

        private void getTheDamnHeadsetTransform()
        {
            headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
            if(headsetTransform== null) System.Threading.Thread.Sleep(5000);
            headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
            if(headsetTransform== null) System.Threading.Thread.Sleep(5000);
            headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
            if(headsetTransform== null) System.Threading.Thread.Sleep(5000);
            headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
            if(headsetTransform== null) System.Threading.Thread.Sleep(5000);
            headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
            if(headsetTransform== null) System.Threading.Thread.Sleep(5000);
            headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
            if(headsetTransform== null) System.Threading.Thread.Sleep(5000);
            headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
            /*
            Stopwatch sw = new Stopwatch(); // sw cotructor
            sw.Start(); // starts the stopwatch
             for (int i = 0; ; i++) 
              {
                if (i % 100000 == 0) // if in 100000th iteration (could be any other large number
                // depending on how often you want the time to be checked) 
                {
                  sw.Stop(); // stop the time measurement
                  if (sw.ElapsedMilliseconds > 5000) // check if desired period of time has elapsed
                  {
                    headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
                    if(headsetTransform != null) break; // if more than 5000 milliseconds have passed, and the condition has been met, stop looping and return
                    // to the existing code                    
                  }
                  else
                  {
                    sw.Start(); // if less than 5000 milliseconds have elapsed, continue looping
                    // and resume time measurement
                  }
                }
             }
             */
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