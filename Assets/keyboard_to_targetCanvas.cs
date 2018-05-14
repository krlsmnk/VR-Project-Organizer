namespace VRTK.Examples
{
	using UnityEngine;
	using UnityEngine.UI;

	public class keyboard_to_targetCanvas : MonoBehaviour
	{
		private InputField input;
		public Canvas targetCanvas;
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
			//VRTK_Logger.Info("You've typed [" + input.text + "]");
			//get current text
			string currentText = input.text;
			bool setValue = false;
			//get reference to vending machine active field
			for (int i = 0; i < targetInputs.Length; i++) {

				//if current field is focused / active
				if (targetInputs [i].isFocused == true) {
				
					//set the text
					targetInputs[i].text = currentText;
					setValue = true;
				}
			}//end of for each targetInput

			if (setValue == true) {
				//clear the keyboard
				input.text = "";
			} else {
				//TODO:
				//play bad beep
			}
		}

		private void Start()
		{
			input = GetComponentInChildren<InputField>();
			targetInputs = targetCanvas.GetComponents<InputField> ();
			if (targetInputs != null && targetCanvas != null)
				Debug.Log ("Targets found");
		}
	}
}