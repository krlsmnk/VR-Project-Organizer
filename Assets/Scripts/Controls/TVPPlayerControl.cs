using System;
using UnityEngine;
using VRTK;
using KarlSmink.Teleporting;

namespace CAVS.ProjectOrganizer.Controls
{
    public class TVPPlayerControl : PlayerControl
    {

        public override Action Build(VRTK_ControllerEvents hand)
        {
            var headset = VRTK_DeviceFinder.HeadsetTransform();

            var cameraToControl = KarlSmink.Teleporting.Util.BuildCamera(Vector3.zero, Quaternion.identity);
            var portal = KarlSmink.Teleporting.Util.BuildPortal(cameraToControl.GetComponentInChildren<Camera>(), headset.transform.position + (headset.forward * 2), Quaternion.identity);
            var headsetCollision = UnityEngine.Object.FindObjectOfType<VRTK_HeadsetCollision>();
            var teleBehavior = TeleportBehavior.Initialize(headsetCollision, 1.7f, cameraToControl.transform, portal);

            var cameraBehavior = CameraBehavior.Initialize(cameraToControl, 2f, portal);
            cameraBehavior.transform.position = hand.transform.position + hand.transform.forward;
            cameraBehavior.transform.LookAt(hand.transform.position + (hand.transform.forward * 2));
            var control = TVPCameraControl.Initialize(hand, cameraBehavior);

            return delegate ()
            {
                UnityEngine.Object.Destroy(cameraToControl);
                UnityEngine.Object.Destroy(portal);
                UnityEngine.Object.Destroy(control);
                UnityEngine.Object.Destroy(teleBehavior);
            };
        }

        public override Texture2D GetIcon()
        {
            return Resources.Load<Texture2D>("PlayerControl/teleport-alternative-icon");
        }
    }

}