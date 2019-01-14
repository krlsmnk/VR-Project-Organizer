using UnityEngine;
using System;
using System.IO;

namespace EliCDavis.UIGen
{
    public class View
    {
        private static AssetBundle assetBundleInstance = null;

        private static AssetBundle GetAssetBundleInstance()
        {
            if (assetBundleInstance == null)
            {
                assetBundleInstance = AssetBundle.LoadFromFile(Path.Combine("Assets/UIGen/AssetBundles", "uiparts"));

                if (assetBundleInstance == null)
                {
                    throw new Exception("Failed to load AssetBundle!");
                }
            }

            return assetBundleInstance;
        }

        private IElement root;

        public View(IElement root)
        {
            this.root = root;
        }

        public GameObject Build(Vector3 position, Quaternion rotation, Vector2 dimensions)
        {
            GameObject canvas = GameObject.Instantiate(GetAssetBundleInstance().LoadAsset<GameObject>("Canvas"));

            RectTransform rectTransform = canvas.GetComponent<RectTransform>();
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, dimensions.x);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, dimensions.y);
            rectTransform.SetPositionAndRotation(position, rotation);

            root.Build(canvas, GetAssetBundleInstance());
            return canvas;
        }

    }

}