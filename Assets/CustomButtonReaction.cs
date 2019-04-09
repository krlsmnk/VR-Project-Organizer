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
            //buttonEvents.OnPushed.AddListener(handlePush);

            TurnAll(buttons, Color.red);
       
            posButtons[0] = buttons[0];
            posButtons[1] = buttons[1];
            posButtons[2] = buttons[2];
            rotButtons[0] = buttons[3];
            rotButtons[1] = buttons[4];
            rotButtons[2] = buttons[5];
        }


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

        public void translateButton() { 
            //disable all existing constraints  
                    cloneRigidBody.constraints = RigidbodyConstraints.None;
                    TurnAll(buttons, Color.green);
                    //disallow rotations
                    cloneRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                    //TurnAll(rotButtons, Color.red); // doesn't work???
                    TurnThis(buttons[3], Color.red);
                    TurnThis(buttons[4], Color.red);
                    TurnThis(buttons[5], Color.red);    
        }
        public void rotateButton() { 
            //disable all existing constraints  
                    cloneRigidBody.constraints = RigidbodyConstraints.None;                                                                                                                                                                                           
                    TurnAll(buttons, Color.green);
                    //disallow translations
                    cloneRigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
                    TurnAll(posButtons, Color.red);            
        }
        public void posx() { 
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
        }
        public void posy() { 
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
        }
        public void posz() { 
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
        }
        public void rotx() { 
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
        }
        public void roty() { 
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
        }
        public void rotz() { 
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
        }
}
}