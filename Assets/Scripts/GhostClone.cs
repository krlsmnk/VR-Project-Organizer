using CAVS.ProjectOrganizer.Controls;
using CAVS.ProjectOrganizer.Interation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.GrabAttachMechanics;

namespace VRTK
{
    namespace CAVS.ProjectOrganizer.Interation
    {

        public class GhostClone : MonoBehaviour
        {

            private VRTK_InteractableObject InteractObjScript;
            private VRTK_ChildOfControllerGrabAttach GrabAttachScript;
            private GameObject thisClone, clonable, globalCanvasScript;
            private DICanvas DICanvasScript;
            private VRTK_ControllerEvents Hand;
            private VRTK_InteractGrab GrabScript;
            private VRTK_InteractTouch handTouch;
            private VRTK_FixedJointGrabAttach FixJointScript;
            private Transform headsetTransform;

            // Use this for initialization
            void Start()
            {
                headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
                thisClone = new GameObject();
                clonable = new GameObject();
                GrabScript = new VRTK_InteractGrab();
                Hand = GameObject.FindObjectOfType<VRTK_ControllerEvents>(); //CNG
                handTouch = GameObject.FindObjectOfType<VRTK_InteractTouch>();
                globalCanvasScript = new GameObject();
                FixJointScript = new VRTK_FixedJointGrabAttach();

                //CNG
                DICanvasScript = FindObjectOfType<DICanvas>();
                if (DICanvasScript == null)
                {
                    DICanvasScript = globalCanvasScript.AddComponent<DICanvas>();
                }
                //Debug.Log(DICanvasScript.gameObject.name);
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
                //"There can be only one."
                cleanupOldClones();

                //Instantiate the clonable at the target position
                clonable = targetClonable;
                thisClone = Instantiate(targetClonable, spawnLocation.forward, spawnLocation.rotation);
                thisClone.transform.position = new Vector3(thisClone.transform.position.x, headsetTransform.position.y, thisClone.transform.position.z);
                thisClone.transform.rotation = targetClonable.transform.rotation;
                thisClone.transform.localScale = targetClonable.transform.localScale;

                //Make the clone more transparent to distinguish it
                var trans = 0.5f;
                var col = thisClone.GetComponent<Renderer>().material.color;                
                col.a = trans;

                //set thisClone as parent so it will affect the clonable
                clonable.transform.SetParent(thisClone.transform, false);
                //clonable.transform.parent = thisClone.transform;

                //copy this script, and attach it to the clone
                thisClone.AddComponent<GhostClone>();

                //continue setup from the new script (which is ON the clone), rather than this one, which hangs out in the scene
                thisClone.GetComponent<GhostClone>().setupGC(clonable, thisClone);
            }

            private void cleanupOldClones()
            {
                GameObject[] temporary = GameObject.FindGameObjectsWithTag("temporary");
                foreach (GameObject thisTemp in temporary)
                {
                    if (thisTemp.GetComponent<GhostClone>() != null) Destroy(thisTemp);
                }
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
                GrabAttachScript = thisClone.GetComponent<VRTK_ChildOfControllerGrabAttach>();
                
                Hand = GameObject.FindObjectOfType<VRTK_ControllerEvents>(); //CNG
                //Finish script setup
                setupInteractScript(InteractObjScript, GrabAttachScript, Hand);

                

                //Give RigidBody if doesn't have
                if (thisClone.GetComponent<Rigidbody>() == null) thisClone.AddComponent<Rigidbody>();

                //Unfreeze the rigidbody so it can be modified
                thisClone.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                thisClone.GetComponent<Rigidbody>().isKinematic = true;

                //Create the canvas which will let us modify the constraints on this clone
                DICanvasScript.createDICanvas(thisClone, thisClone.transform);
            }

            internal void releaseChildren()
            {
                clonable.transform.parent = null;
            }

            private void setupInteractScript(VRTK_InteractableObject IOScript, VRTK_ChildOfControllerGrabAttach GScript, VRTK_ControllerEvents CEvents)
            {
                if (IOScript == null)
                {
                    //create new script, then continue               
                    InteractObjScript = thisClone.AddComponent<VRTK_InteractableObject>();
                }
                if (GScript == null)
                {
                    //create new script, then continue               
                    GrabAttachScript = thisClone.AddComponent<VRTK_ChildOfControllerGrabAttach>();
                }
                if (CEvents == null)
                {
                    /*
                    GameObject newHand = GameObject.FindObjectOfType<GrabControlBehavior>().gameObject;
                    //create new script, then continue                    
                    Hand = newHand.GetComponent<VRTK_ControllerEvents>();
                    if(Hand==null) Hand = newHand.AddComponent<VRTK_ControllerEvents>();
                    */
                    Debug.Log("Created new Controller Events");
                    Hand = new VRTK_ControllerEvents();
                }
                if (handTouch == null) handTouch = Hand.gameObject.GetComponent<VRTK_InteractTouch>();
                if (handTouch == null) handTouch = Hand.gameObject.AddComponent<VRTK_InteractTouch>();
                
                GrabScript = Hand.gameObject.GetComponent<VRTK_InteractGrab>();
                if (GrabScript== null) GrabScript = Hand.gameObject.AddComponent<VRTK_InteractGrab>();

                InteractObjScript.holdButtonToGrab = true;
                InteractObjScript.isGrabbable = true;
                InteractObjScript.stayGrabbedOnTeleport = true;
                InteractObjScript.touchHighlightColor = Color.yellow;
                InteractObjScript.grabOverrideButton = VRTK_ControllerEvents.ButtonAlias.GripPress;              

                GrabAttachScript.precisionGrab = true;
                if(FixJointScript == null) FixJointScript = new VRTK_FixedJointGrabAttach();
                FixJointScript.precisionGrab = true;               
                InteractObjScript.grabAttachMechanicScript = FixJointScript;

                GrabScript.grabButton = VRTK_ControllerEvents.ButtonAlias.GripPress;
                GrabScript.controllerEvents = Hand; 
            }
        }//end of class
    }//end of CAVS namespace
}//end of VRTK namespace