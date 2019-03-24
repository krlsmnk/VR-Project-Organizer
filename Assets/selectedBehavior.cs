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
        Debug.Log("Selected");

            
            //Clone the selected object, place it with some offset to the original
            //originalItemTransform = this.gameObject.transform;
            //offsetTransform = originalItemTransform;
            

            //Clone the selected object, place it with some offset to the controller
            offsetTransform = caller.transform;
            Vector3 newpos = offsetTransform.position;
            newpos.z += -0.50f;
            offsetTransform.position = newpos;

            ghostClone = Instantiate(this.gameObject, offsetTransform.position, offsetTransform.rotation);
            //ghostClone.transform.parent = caller.transform;



    }//end of select

    public void UnSelect(UnityEngine.GameObject caller) { 
        //Debug.Log("UnSelected");
    }//end of unselect
}
}//end of namespace
