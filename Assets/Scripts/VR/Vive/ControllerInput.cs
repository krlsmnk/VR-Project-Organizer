using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///Detects collision between controllers, and at the end of the timer, creates a node
///</summary>

public class ControllerInput : MonoBehaviour {



    //I'm aware this may not be the best way to do things. Please give feedback on how I might improve.

    public GameObject node; //node to be instantiated
    public GameObject left_controller;
    public GameObject right_controller;

    public float Timer_Limit; //Amount of time until node is created
    public float Distance_Limit; //Proximity between controllers
    private float Timer;

    //OnCollision not working
    void Update()
    {
        //Judge controller distance
        if (Vector3.Distance(left_controller.transform.position, right_controller.transform.position) < Distance_Limit)
        {
            Timer += Time.deltaTime;
            
            if (Timer>= (Timer/2)){
                VRTK.VRTK_ControllerHaptics.TriggerHapticPulse(VRTK.VRTK_ControllerReference.GetControllerReference(left_controller) , 0.5f);   //Vibrate controllers at half strength (0 < x < 1)
                VRTK.VRTK_ControllerHaptics.TriggerHapticPulse(VRTK.VRTK_ControllerReference.GetControllerReference(right_controller), 0.5f);
            }
            if (Timer >= Timer_Limit)
            {
                //------------------NODE CREATION---------------------------//

                node.transform.position = (left_controller.transform.position + right_controller.transform.position)/2; //midpoint between the two controllers

                //node created in front of controllers, in the same direction
                node.transform.Translate(Vector3.forward, left_controller.transform);
                node.transform.eulerAngles = new Vector3(0, left_controller.transform.eulerAngles.y, 0);

                Instantiate(node);

                Timer = -5; //5 second "cooldown"
            }
        }
        else
        {
            Timer = 0;  //reset timer when pulled apart
            VRTK.VRTK_ControllerHaptics.CancelHapticPulse(VRTK.VRTK_ControllerReference.GetControllerReference(left_controller));   //Cancel vibration when pulled apart
            VRTK.VRTK_ControllerHaptics.CancelHapticPulse(VRTK.VRTK_ControllerReference.GetControllerReference(right_controller));
        }
    }


}
