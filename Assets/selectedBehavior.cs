using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using VRTK;

namespace CAVS.ProjectOrganizer.Interation
{

public class selectedBehavior: MonoBehaviour, ISelectable {
    
     private Transform originalItemTransform, offsetTransform;
     private GameObject ghostClone = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Select(UnityEngine.GameObject caller) { 
        //Debug.Log("Selected");                 

            //see if another clone exists, if one does, remove it
            try { 
                    GameObject.Destroy(GameObject.Find("GhostClone"));                                        
                }
            catch { 
                    Debug.Log("No clone found");
                }
            
            //Clone the selected object, place it with some offset to the controller
            ghostClone = Instantiate(this.gameObject, caller.transform.position + (caller.transform.right *.5f), this.gameObject.transform.rotation);
            

            //rename object so we can define further behavior later
            ghostClone.name = "GhostClone";

            //Remove the ability to clone clones by removing the selectable script
            Destroy(ghostClone.GetComponent<selectedBehavior>());

            //Add the ability to modify the clone's transform so it will affect the original
            this.gameObject.transform.parent = ghostClone.transform;
            ghostClone.AddComponent<VRTK_InteractableObject>();
            //I'm sure there's a better way to do this...
            ghostClone.GetComponent<VRTK_InteractableObject>().isGrabbable = true;
            ghostClone.GetComponent<VRTK_InteractableObject>().holdButtonToGrab = true;

            //unfreeze the clone's rigidbody so it can be modified
            ghostClone.GetComponent<Rigidbody>().constraints =  RigidbodyConstraints.None;

    }//end of select

    public void UnSelect(UnityEngine.GameObject caller) { 
        //Debug.Log("UnSelected");
    }//end of unselect
}
}//end of namespace
