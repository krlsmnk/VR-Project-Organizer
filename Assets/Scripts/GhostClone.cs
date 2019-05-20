using CAVS.ProjectOrganizer.Interation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRTK
{
    namespace CAVS.ProjectOrganizer.Interation
    {

        public class GhostClone : MonoBehaviour
        {

            private VRTK_InteractableObject InteractObjScript;
            private GameObject thisClone, clonable, globalCanvasScript;
            private DICanvas DICanvasScript;

            // Use this for initialization
            void Start()
            {
                thisClone = new GameObject();
                clonable = new GameObject();
                globalCanvasScript = new GameObject();

                //CNG
                DICanvasScript = FindObjectOfType<DICanvas>();
                if (DICanvasScript == null)
                {
                    DICanvasScript = globalCanvasScript.AddComponent<DICanvas>();
                }
                Debug.Log(DICanvasScript.gameObject.name);
            }

            // Update is called once per frame
            void Update()
            {

            }

            /// <summary>
            /// This function is called by the object which can be cloned.
            /// 
            /// </summary>
            /// <param name="targetClonable"></param>
            /// <param name="spawnLocation"></param>
            public void createGC(GameObject targetClonable, Transform spawnLocation)
            {
                //Instantiate the clonable at the target position
                clonable = targetClonable;
                thisClone = Instantiate(targetClonable, spawnLocation.forward, spawnLocation.rotation);

                //set thisClone as parent so it will affect the clonable
                clonable.transform.parent = thisClone.transform;

                //copy this script, and attach it to the clone
                thisClone.AddComponent<GhostClone>();

                //continue setup from the new script (which is ON the clone), rather than this one, which hangs out in the scene
                thisClone.GetComponent<GhostClone>().setupGC(clonable, thisClone);
            }

            public void setupGC(GameObject targetClonable, GameObject myClone)
            {
                //CNG
                DICanvasScript = FindObjectOfType<DICanvas>();
                if (DICanvasScript == null)
                {
                    DICanvasScript = globalCanvasScript.AddComponent<DICanvas>();
                }
                //Debug.Log(DICanvasScript.gameObject.name);

                //Assign GameObject references
                clonable = targetClonable;
                thisClone = myClone;

                //set tag and name so we can clean it up, find it later, and distinguish it in the editor
                thisClone.name = clonable.name + "(Ghost Clone)";
                thisClone.tag = "temporary";

                //Remove the script from thisClone that allows a clone to be made from it
                try { Destroy(thisClone.GetComponent<Cloneable>()); }
                catch { Debug.Log("No Clonable Script to remove."); }

                //Give grab attach if doesn't have
                InteractObjScript = thisClone.GetComponent<VRTK_InteractableObject>();
                if (InteractObjScript == null) setupInteractScript(0);
                else setupInteractScript(1);

                //Give RigidBody if doesn't have
                if (thisClone.GetComponent<Rigidbody>() == null) thisClone.AddComponent<Rigidbody>();

                //Unfreeze the rigidbody so it can be modified
                thisClone.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                //Create the canvas which will let us modify the constraints on this clone
                DICanvasScript.createDICanvas(thisClone, thisClone.transform);
            }

            private void setupInteractScript(int exists)
            {
                if (exists == 0)
                {
                    //create new script, then continue               
                    InteractObjScript = thisClone.AddComponent<VRTK_InteractableObject>();
                }

                InteractObjScript.holdButtonToGrab = true;
                InteractObjScript.isGrabbable = true;
                InteractObjScript.stayGrabbedOnTeleport = true;
                InteractObjScript.touchHighlightColor = Color.yellow;
                InteractObjScript.grabOverrideButton = VRTK_ControllerEvents.ButtonAlias.GripPress;
            }
        }//end of class
    }//end of CAVS namespace
}//end of VRTK namespace