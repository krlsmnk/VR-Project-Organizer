namespace VRTK.Examples
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEventHelper;


public class CustomButtonReaction : MonoBehaviour {

        public Button[] buttons = new Button[6];
        private Button[] rotButtons, posButtons = new Button[3];

        public Rigidbody cloneRigidBody;

        private VRTK_Button_UnityEvents buttonEvents;


        private void Start()
        {
            buttonEvents = GetComponent<VRTK_Button_UnityEvents>();
            if (buttonEvents == null)
            {
                buttonEvents = gameObject.AddComponent<VRTK_Button_UnityEvents>();
            }
            buttonEvents.OnPushed.AddListener(handlePush);

            TurnAll(buttons, Color.red);
       
            posButtons[0] = buttons[0];
            posButtons[1] = buttons[1];
            posButtons[2] = buttons[2];
            rotButtons[0] = buttons[3];
            rotButtons[1] = buttons[4];
            rotButtons[2] = buttons[5];
        }

        private void handlePush(object sender, Control3DEventArgs e)
        {
            VRTK_Logger.Info("Pushed");

            //actually do stuff
                Debug.Log("Button pushed: " + this.name);

            switch (this.name)
              {
                  case "translateButton":
                    //disable all existing constraints  
                    cloneRigidBody.constraints = RigidbodyConstraints.None;
                    TurnAll(buttons, Color.green);
                    //disallow rotations
                    cloneRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                    //TurnAll(rotButtons, Color.red); // doesn't work???
                    TurnThis(buttons[3], Color.red);
                    TurnThis(buttons[4], Color.red);
                    TurnThis(buttons[5], Color.red);
                  break;
                  case "rotateButton":
                    //disable all existing constraints  
                    cloneRigidBody.constraints = RigidbodyConstraints.None;                                                                                                                                                                                           
                    TurnAll(buttons, Color.green);
                    //disallow translations
                    cloneRigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
                    TurnAll(posButtons, Color.red);
                  break;
                  case "posX":
                        //check status of posX constraint
                        if((cloneRigidBody.constraints & RigidbodyConstraints.FreezePositionX) == RigidbodyConstraints.FreezePositionX) 
                         {
                            // x-position is frozen, unfreeze it
                            cloneRigidBody.constraints &= ~RigidbodyConstraints.FreezePositionX;
                            TurnThis(buttons[0], Color.green);
                         }
                        else
                        {
                            // x-position not frozen, freeze it
                            cloneRigidBody.constraints &= RigidbodyConstraints.FreezePositionX;
                            TurnThis(buttons[0], Color.red);
                        }                                                                                
                  break;
                  case "posY":
                        //check status of constraint
                        if((cloneRigidBody.constraints & RigidbodyConstraints.FreezePositionY) == RigidbodyConstraints.FreezePositionY) 
                         {
                            //position is frozen, unfreeze it
                            cloneRigidBody.constraints &= ~RigidbodyConstraints.FreezePositionY;
                            TurnThis(buttons[1], Color.green);
                         }
                        else
                        {
                            //position not frozen, freeze it
                            cloneRigidBody.constraints &= RigidbodyConstraints.FreezePositionY;
                            TurnThis(buttons[1], Color.red);
                        }                                                                                
                  break;
                  case "posZ":
                        //check status of constraint
                        if((cloneRigidBody.constraints & RigidbodyConstraints.FreezePositionZ) == RigidbodyConstraints.FreezePositionZ) 
                         {
                            //position is frozen, unfreeze it
                            cloneRigidBody.constraints &= ~RigidbodyConstraints.FreezePositionZ;
                            TurnThis(buttons[2], Color.green);
                         }
                        else
                        {
                            //position not frozen, freeze it
                            cloneRigidBody.constraints &= RigidbodyConstraints.FreezePositionZ;
                            TurnThis(buttons[2], Color.red);
                        }                                                                                
                  break;
                  case "rotX":
                        //check status of constraint
                        if((cloneRigidBody.constraints & RigidbodyConstraints.FreezeRotationX) == RigidbodyConstraints.FreezeRotationX) 
                         {
                            //rotation is frozen, unfreeze it
                            cloneRigidBody.constraints &= ~RigidbodyConstraints.FreezeRotationX;
                            TurnThis(buttons[3], Color.green);
                         }
                        else
                        {
                            //rotation not frozen, freeze it
                            cloneRigidBody.constraints &= RigidbodyConstraints.FreezeRotationX;
                            TurnThis(buttons[3], Color.red);
                        }                                                                                
                  break;
                  case "rotY":
                        //check status of constraint
                        if((cloneRigidBody.constraints & RigidbodyConstraints.FreezeRotationY) == RigidbodyConstraints.FreezeRotationY) 
                         {
                            //rotation is frozen, unfreeze it
                            cloneRigidBody.constraints &= ~RigidbodyConstraints.FreezeRotationY;
                            TurnThis(buttons[4], Color.green);
                         }
                        else
                        {
                            //rotation not frozen, freeze it
                            cloneRigidBody.constraints &= RigidbodyConstraints.FreezeRotationY;
                            TurnThis(buttons[4], Color.red);
                        } 
                  break;
                  case "rotZ":
                        //check status of constraint
                        if((cloneRigidBody.constraints & RigidbodyConstraints.FreezeRotationZ) == RigidbodyConstraints.FreezeRotationZ) 
                         {
                            //rotation is frozen, unfreeze it
                            cloneRigidBody.constraints &= ~RigidbodyConstraints.FreezeRotationZ;
                            TurnThis(buttons[5], Color.green);
                         }
                        else
                        {
                            //rotation not frozen, freeze it
                            cloneRigidBody.constraints &= RigidbodyConstraints.FreezeRotationZ;
                            TurnThis(buttons[5], Color.red);
                        } 
                  break;

                  default:
                      Debug.Log("Default case");
                      break;
              }//end of switch


        }//end of handlePush

        void TurnAll(Button[] theseButtons, Color thisColor) {             
            foreach(Button currentButton in theseButtons){ 
               ColorBlock colors = currentButton.colors;
               colors.normalColor = thisColor;
               currentButton.colors = colors;
            }
        }
        void TurnThis(Button thisButton, Color thisColor) {                          
               ColorBlock colors = thisButton.colors;
               colors.normalColor = thisColor;
               thisButton.colors = colors;            
        }
}
}