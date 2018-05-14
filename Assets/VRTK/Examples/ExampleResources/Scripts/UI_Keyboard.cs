namespace VRTK.Examples
{
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_Keyboard : MonoBehaviour
    {
        
		private InputField input;
		public Canvas targetCanvas;
		public InputField[] targetInputs;

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
            //VRTK_Logger.Info("You've typed [" + input.text + "]");
            //input.text = "";
			Debug.Log("Enter()");
			muhEnter();
        }

        void Start()
        {
            input = GetComponentInChildren<InputField>();
			muhStart ();
        }
        
		public void muhEnter()
		{
			//VRTK_Logger.Info("You've typed [" + input.text + "]");
			//get current text
			string currentText = input.text;
			bool setValue = false;
			//get reference to vending machine active field
			for (int i = 0; i < targetInputs.Length; i++) {

				//if current field is focused / active
				if (targetInputs [i].GetComponent<isSelected>().isSelectedBool == true) {

					//set the text
					targetInputs[i].text = currentText;
					setValue = true;

					targetInputs [i].GetComponent<isSelected> ().setBool(false);
				}
			}//end of for each targetInput

			if (setValue == true) {
				//clear the keyboard
				input.text = "";
			} else {
				//TODO:
				//play bad beep

			}
			Debug.Log ("Set value: " + setValue);
		}

		private void muhStart()
		{
			Debug.Log("muhStart()");
			//targetInputs = targetCanvas.GetComponents<InputField> ();
			if (targetInputs != null && targetCanvas != null) {

				foreach(InputField curField in targetInputs){
					Debug.Log ("targetName: " + curField.name);
				}

			}
			else {
				Debug.Log("Targets NOT found");
			}
		}

    }
}