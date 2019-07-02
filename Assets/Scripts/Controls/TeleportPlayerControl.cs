using System;
using UnityEngine;
using VRTK;

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

            DateTime thisDay = DateTime.Today;
            string recordingName = thisDay.ToString() + " : Point & Click";
            GameObject.FindObjectOfType<PlayerControlBehavior>().FireRAP(recordingName); // CNG

            return delegate ()
            {
                UnityEngine.Object.Destroy(pointer);
                UnityEngine.Object.Destroy(renderer);
                UnityEngine.Object.Destroy(teleport);
            };
        }

        public override Texture2D GetIcon()
        {
            return Resources.Load<Texture2D>("PlayerControl/Teleport-icon");
        }
    }

}