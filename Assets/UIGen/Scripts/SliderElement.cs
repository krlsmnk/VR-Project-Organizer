using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace EliCDavis.UIGen
{

    public class SliderElement : IElement
    {
        private float min;

        private float max;

        private float startingValue;

        private Action<float> onValueChanged;

        Func<float, string> formatter;

        public SliderElement(float min, float max, float startingValue, Action<float> onValueChanged) : this(min, max, startingValue, onValueChanged, null)
        {
        }

        public SliderElement(float min, float max, float startingValue, Action<float> onValueChanged, Func<float, string> formatter)
        {
            this.min = min;
            this.max = max;
            this.startingValue = startingValue;
            this.onValueChanged = onValueChanged;
            this.formatter = formatter;
        }

        public GameObject Build(GameObject parent, AssetBundle assetBundleInstance)
        {
            GameObject ele = null;
            if (formatter == null)
            {
                ele = UnityEngine.Object.Instantiate(assetBundleInstance.LoadAsset<GameObject>("Slider"));
            }
            else
            {
                ele = UnityEngine.Object.Instantiate(assetBundleInstance.LoadAsset<GameObject>("Slider With Text"));
            }
            ele.transform.SetParent(parent.transform);
            ele.transform.localPosition = Vector3.zero;
            ele.transform.localRotation = Quaternion.identity;

            Slider slider = ele.transform.Find("Slider 1").GetComponent<Slider>();
            slider.minValue = min;
            slider.maxValue = max;
            slider.value = startingValue;
            if (formatter == null)
            {
                var sliderEvent = new Slider.SliderEvent();
                sliderEvent.AddListener(new UnityAction<float>(onValueChanged));
                slider.onValueChanged = sliderEvent;
            }
            else
            {
                var textComponent = ele.transform.Find("Text").GetComponent<Text>();
                textComponent.text = formatter(slider.value);

                var formatEvent = new Slider.SliderEvent();
                formatEvent.AddListener(delegate (float input)
                {
                    textComponent.text = formatter(input);
                    onValueChanged(input);
                });
                slider.onValueChanged = formatEvent;
            }

            return ele;
        }

    }

}