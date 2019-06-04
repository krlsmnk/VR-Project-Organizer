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

        public PlayerControlBehavior Initialize(VRTK_ControllerEvents hand, List<PlayerControl> controls)
        {
            PlayerControlBehavior newScript;
            if (hand.gameObject.GetComponent<PlayerControlBehavior>() == null)
            {
                newScript = hand.gameObject.AddComponent<PlayerControlBehavior>();             
            }
            else
            {
                newScript = hand.gameObject.GetComponent<PlayerControlBehavior>();
            }

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
                //CNG 6/4
                //Destroy(thisIcon);                
            }
            //CNG 6/4
            //Destroy(hand.gameObject.GetComponentInChildren<VRTK_RadialMenuController>());
            //Destroy(hand.gameObject.GetComponentInChildren<VRTK_RadialMenu>());
            hand.gameObject.GetComponentInChildren<VRTK_RadialMenuController>().enabled = false;
            hand.gameObject.GetComponentInChildren<VRTK_RadialMenu>().enabled = false;
            hand.gameObject.GetComponentInChildren<PlayerControlBehavior>().enabled = false;
        }
/// <summary>
/// Rebuilds the radial menu to allow for control switching again
/// </summary>
        public void rebuildMenu()
        {
            //CNG 6/4
            hand.gameObject.GetComponentInChildren<VRTK_RadialMenuController>().enabled = true;
            hand.gameObject.GetComponentInChildren<VRTK_RadialMenu>().enabled = true;

            hand.gameObject.GetComponentInChildren<PlayerControlBehavior>().enabled = true;         
        }


    }

}