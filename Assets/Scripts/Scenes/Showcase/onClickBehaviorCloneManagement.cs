namespace UnityEngine.EventSystems{
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;



public class onClickBehaviorCloneManagement : MonoBehaviour {
    public Rigidbody cloneRigidBody;
    private Button[] buttons;

	// Use this for initialization
	void Start () {
		buttons = this.GetComponentsInChildren<Button>();

        foreach(Button currentButton in buttons){ 
            currentButton.onClick = MyOnClickBehavior();
            }

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private UnityEngine.UI.Button.ButtonClickedEvent MyOnClickBehavior() { 
        string currentButtonName = EventSystem.current.currentSelectedGameObject.name;    
            
            switch (currentButtonName)
              {
                  case "translateButton":
                    //disable all existing constraints  
                    cloneRigidBody.constraints = RigidbodyConstraints.None;
                                                                                                                                                                                             
                    //disallow rotations
                    cloneRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

                    Debug.Log("Translate Button Pressed");

                      break;
                  case "rotateButton":
                      //disable all existing constraints  
                    cloneRigidBody.constraints = RigidbodyConstraints.None;
                                                                                                                                                                                             
                    //disallow translations
                    cloneRigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
                      break;
                  case "freezePosX":
                      cloneRigidBody.constraints = RigidbodyConstraints.FreezePositionX;
                    break;

                  default:
                      Debug.Log("Default case");
                      break;
              }//end of switch
            return null;
     }//end of myOnClick
        

       

        }//end of onClickBehavior

}
