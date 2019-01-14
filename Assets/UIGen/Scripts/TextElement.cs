using UnityEngine;
using UnityEngine.UI;

namespace EliCDavis.UIGen
{
    public class TextElement : IElement
    {

        private string text;

        public TextElement(string text)
        {
            this.text = text;
        }

        public GameObject Build(GameObject parent, AssetBundle assetBundleInstance)
        {
            GameObject ele = GameObject.Instantiate(assetBundleInstance.LoadAsset<GameObject>("TextPlaceholder"));
            ele.GetComponent<Text>().text = text;
            ele.transform.SetParent(parent.transform);
            ele.transform.localPosition = Vector3.zero;
            ele.transform.localRotation = Quaternion.identity;
            return ele;
        }

    }
}