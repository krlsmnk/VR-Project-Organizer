using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRTK;
using VRTK.RecordAndPlay.Demo;

namespace CAVS.ProjectOrganizer.Controls
{    
    public class PlayerControlBehavior : MonoBehaviour
    {
        private int currentControlIndex;
        public int currentWeaponIndex;
        private List<PlayerControl> controls;
        private Action cleanupCommand;
        private VRTK_ControllerEvents hand;   
        recordAndPlayManager RAPManagerScript;

        void Start() { 
             
         }

        private void PlayerRig_LoadedSetupChanged(VRTK_SDKManager sender, VRTK_SDKManager.LoadedSetupChangeEventArgs e)
        {
            throw new NotImplementedException();
        }

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

        public void FireRAP(string recordingName)
        {
            if(RAPManagerScript == null)
            {
               RAPManagerScript = GameObject.FindObjectOfType<recordAndPlayManager>();
               RAPManagerScript.setupRecorder(recordingName);
            }
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
            }
            //CNG 6/4
            hand.gameObject.GetComponentInChildren<VRTK_RadialMenuController>().enabled = false;
            hand.gameObject.GetComponentInChildren<VRTK_RadialMenu>().enabled = false;
            hand.gameObject.GetComponentInChildren<PlayerControlBehavior>().enabled = false;

            cleanupWheel();
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


        //this actually visually removes the wheel, the kill function breaks its functionality
        private static void cleanupWheel()
        {
            var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "RadialMenu(Clone)");
            foreach (var thisObject in objects)
            {
                UnityEngine.Object.Destroy(thisObject);
            }
            Sprite[] icons = FindObjectsOfType<Sprite>();
            foreach (Sprite thisIcon in icons)
            {
                UnityEngine.Object.Destroy(thisIcon);
            }
            Button[] buttons = FindObjectsOfType<Button>();
            foreach (Button thisIcon in buttons)
            {
                UnityEngine.Object.Destroy(thisIcon);
            }
            Texture2D[] textures = FindObjectsOfType<Texture2D>();
            foreach (Texture2D thisIcon in textures)
            {
                UnityEngine.Object.Destroy(thisIcon);
            }
        }

    }

}