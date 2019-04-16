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

        void ToggleThis(Button thisButton) {
            ColorBlock colors = thisButton.colors;
            if (colors.normalColor == Color.red) colors.normalColor = Color.green;
            else colors.normalColor = Color.red;
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
            cloneRigidBody.constraints ^= RigidbodyConstraints.FreezePositionX;
            ToggleThis(buttons[0]);
        }
        public void posy() {
            cloneRigidBody.constraints ^= RigidbodyConstraints.FreezePositionY;
            ToggleThis(buttons[1]);
        }
        public void posz() {
            cloneRigidBody.constraints ^= RigidbodyConstraints.FreezePositionZ;
            ToggleThis(buttons[2]);
        }
        public void rotx() {
            cloneRigidBody.constraints ^= RigidbodyConstraints.FreezeRotationX;
            ToggleThis(buttons[3]);
        }
        public void roty() {
            cloneRigidBody.constraints ^= RigidbodyConstraints.FreezeRotationY;
            ToggleThis(buttons[4]);
        }
        public void rotz() {
            cloneRigidBody.constraints ^= RigidbodyConstraints.FreezeRotationZ;
            ToggleThis(buttons[5]);
        }
}
}