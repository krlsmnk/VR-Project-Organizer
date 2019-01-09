using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace EliCDavis.UIGen
{
    public class ButtonElement : IElement
    {
        private string text;

        private UnityAction onButtonPress;

        public ButtonElement(string text, Action onButtonPress)
        {
            this.onButtonPress = new UnityAction(onButtonPress);
            this.text = text;
        }

        public GameObject Build(GameObject parent, AssetBundle assetBundleInstance)
        {
            GameObject ele = GameObject.Instantiate(assetBundleInstance.LoadAsset<GameObject>("Button"));

            var buttonEvent = new Button.ButtonClickedEvent();
            buttonEvent.AddListener(onButtonPress);
            ele.transform.Find("Button").GetComponent<Button>().onClick = buttonEvent;

            ele.transform.Find("Button/Text").GetComponent<Text>().text = text;

            ele.transform.SetParent(parent.transform);
            ele.transform.localPosition = Vector3.zero;
            ele.transform.localRotation = Quaternion.identity;
            return ele;
        }
    }

}