using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRTK;

namespace CAVS.ProjectOrganizer.Controls
{    
    public class PlayerControlBehavior : MonoBehaviour
    {
        private int currentControlIndex;
        public int currentWeaponIndex;
        private List<PlayerControl> controls;

        private Action cleanupCommand;

        private VRTK_ControllerEvents hand;

        public static PlayerControlBehavior Initialize(VRTK_ControllerEvents hand, List<PlayerControl> controls)
        {
            var newScript = hand.gameObject.AddComponent<PlayerControlBehavior>();

            newScript.currentControlIndex = -1;
            newScript.hand = hand;
            newScript.controls = controls;

            VRTK_RadialMenu radialMenu = hand.gameObject.GetComponentInChildren<VRTK_RadialMenu>();
            radialMenu.buttons.Clear();
            for (int i = 0; i < controls.Count; i++)
            {
                var e = new UnityEvent();
                e.AddListener(newScript.BuildWeaponChangeCallback(i));
                var icon = controls[i].GetIcon();
                radialMenu.AddButton(new VRTK_RadialMenu.RadialMenuButton()
                {
                    ButtonIcon = Instantiate(Sprite.Create(icon, new Rect(0, 0, icon.width, icon.height), new Vector2(0.5f, 0.5f))),
                    OnClick = e
                });
            }

            newScript.SwitchToControl(0);

            return newScript;
        }

        private UnityAction BuildWeaponChangeCallback(int weaponIndex)
        {
            currentWeaponIndex = weaponIndex;
            return delegate ()
            {
                SwitchToControl(weaponIndex);
            };
        }

        public void SwitchToControl(int weaponIndex)
        {
            if (currentControlIndex > -1)
            {
                cleanupCommand();
            }
            currentControlIndex = weaponIndex;
            cleanupCommand = controls[currentControlIndex].Build(hand);
        }

        /// <summary>
        /// Destroys the radial menu on the touchpad to free up controls for individual control schemes
        /// </summary>        
        public void killRadialMenu()
        {
            var icons = FindObjectsOfType<Texture2D>();
            foreach (Texture2D thisIcon in icons)
            {
                Destroy(thisIcon);
            }
            Destroy(hand.gameObject.GetComponentInChildren<VRTK_RadialMenuController>());
            Destroy(hand.gameObject.GetComponentInChildren<VRTK_RadialMenu>());

            hand.gameObject.GetComponentInChildren<PlayerControlBehavior>().enabled = false;
            //Destroy(hand.gameObject.GetComponentInChildren<PlayerControlBehavior>());

            //Destroy(hand);
        }
/// <summary>
/// Rebuilds the radial menu to allow for control switching again
/// </summary>
        public void rebuildMenu()
        {
            hand.gameObject.GetComponentInChildren<PlayerControlBehavior>().enabled = true;
            //Rebuild control selector
            //CNG
            /*
            var config = new ControllerConfig(new List<PlayerControl>()
            {
                new GrabPlayerControl(),
                new TeleportPlayerControl(),
                new SelectPlayerControl(),
                new TVPPlayerControl()
            });
            config.Build(hand);            
            */
        }


    }

}