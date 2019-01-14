using UnityEngine;
using UnityEngine.UI;

namespace EliCDavis.UIGen
{

    public class Window : IElement
    {
        private IElement[] elements;

        private string title;

        public Window(string title) : this(title, null)
        {
        }

        public Window(string title, IElement[] elements)
        {
            this.title = title;
            this.elements = elements;
        }

        public GameObject Build(GameObject parent, AssetBundle assetBundleInstance)
        {
            float headerHeight = 0.2f;

            GameObject windowInstance = GameObject.Instantiate(assetBundleInstance.LoadAsset<GameObject>("Window"));
            windowInstance.transform.SetParent(parent.transform);
            windowInstance.transform.localPosition = Vector3.zero;
            windowInstance.transform.localRotation = Quaternion.identity;

            RectTransform windowRect = windowInstance.GetComponent<RectTransform>();
            Rect pr = parent.GetComponent<RectTransform>().rect;
            windowRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, pr.width);
            windowRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, pr.height);

            RectTransform header = windowInstance.transform.Find("Header Panel").GetComponent<RectTransform>();
            header.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, windowRect.rect.width);
            header.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, headerHeight);
            header.gameObject.GetComponentInChildren<Text>().text = title;

            float remainingSpace = pr.height - headerHeight;

            if (elements != null && elements.Length > 0)
            {
                float spacePerElement = remainingSpace / elements.Length;
                for (int eleIndex = 0; eleIndex < elements.Length; eleIndex++)
                {
                    var eleInstance = elements[eleIndex].Build(windowInstance, assetBundleInstance).GetComponent<RectTransform>();
                    eleInstance.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, pr.width);
                    eleInstance.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, headerHeight + (spacePerElement * eleIndex), spacePerElement);
                }
            }

            return windowInstance;
        }

    }

}