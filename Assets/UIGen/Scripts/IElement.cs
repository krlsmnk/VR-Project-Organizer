using UnityEngine;

namespace EliCDavis.UIGen
{
    public interface IElement
    {
        GameObject Build(GameObject parent, AssetBundle assetBundleInstance);
    }
}