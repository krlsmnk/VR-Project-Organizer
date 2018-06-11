using UnityEngine;
using UnityEngine.UI;


namespace VRTK.Examples
{
    
    public class UI_Keyboard : MonoBehaviour
    {
        
		private InputField input;

        [SerializeField]
        private InputField[] targetInputs;

        public void ClickKey(string character)
        {
            input.text += character;
        }

        public void Backspace()
        {
            if (input.text.Length > 0)
            {
                input.text = input.text.Substring(0, input.text.Length - 1);
            }
        }

        public void Enter()
        {
            string currentText = input.text;
            bool setValue = false;
            //get reference to vending machine active field
            for (int i = 0; i < targetInputs.Length; i++)
            {
                if (targetInputs[i] == null || targetInputs[i].GetComponent<IsSelected>() == null)
                {
                    continue;
                }

                //if current field is focused / active
                if (targetInputs[i].GetComponent<IsSelected>().isSelectedBool == true)
                {

                    //set the text
                    targetInputs[i].text = currentText;
                    setValue = true;

                    targetInputs[i].GetComponent<IsSelected>().SetBool(false);
                }
            }//end of for each targetInput

            if (setValue == true)
            {
                //clear the keyboard
                input.text = "";
            }
        }

        void Start()
        {
            input = GetComponentInChildren<InputField>();
            if (targetInputs == null)
            {
                Debug.Log("Targets NOT found");
            }
        }
        

    }
}