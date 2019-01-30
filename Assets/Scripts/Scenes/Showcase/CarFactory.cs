using UnityEngine;
using UnityEngine.UI;
using CAVS.ProjectOrganizer.Project;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    public static class CarFactory 
    {

        private static GameObject toyReference;

        private static GameObject GetToyReference()
        {
            if(toyReference == null)
            {
                toyReference = Resources.Load<GameObject>("Toy Car");
            }
            return toyReference;
        }

        /// <summary>
        /// Loads a reference to a car model from the Resources folder to be instantiated and displayed in the scene
        /// </summary>
        /// <returns>A reference of a car to be instantiated</returns>
        public static GameObject MakeCar(Item car, CarQuality quality, Vector3 position, Quaternion rotation)
        {
            string directory = string.Format("{0} {1}", car.GetValue("Make"), car.GetValue("Model"));
            string filename = string.Format("{0} {1} {2}", directory, car.GetValue("Trim"), CarQualityToFilePath(quality));

            GameObject carModel = Resources.Load<GameObject>(string.Format("{0}/{1}", directory, filename));

            if (carModel == null)
            {
                carModel = Resources.Load<GameObject>("Low Quality Car");
            }

            return Object.Instantiate(carModel, position, rotation);
        }

        public static GameObject MakeBigCar(Item car, float color,  Vector3 position, Quaternion rotation)
        {
            GameObject carInstace = Object.Instantiate(Resources.Load<GameObject>("Big Car"));
            carInstace.transform.name = "Big Car";
            carInstace.transform.position = position;
            carInstace.transform.rotation = rotation;

            var renders = carInstace.GetComponentsInChildren<MeshRenderer>();
            foreach (var render in renders)
            {
                foreach (var mat in render.materials)
                {
                    if (mat.name == "Body (Instance)")
                    {
                        mat.color = Color.HSVToRGB(color, 1, 1);
                    }
                }
            }
            carInstace.transform.localScale = new Vector3(
                float.Parse(car.GetValue("Width (in)")) / 69.5f,
                float.Parse(car.GetValue("Height (in)")) / 56.7f,
                float.Parse(car.GetValue("Length (in)")) / 170.1f
            ) * 18;
            return carInstace;
        }

        public static GameObject MakeToyCar(Item car, string display, float color, Vector3 position, Quaternion rotation)
        {
            GameObject carInstace = Object.Instantiate(GetToyReference());
            carInstace.transform.position = position;
            carInstace.transform.rotation = rotation;

            var renders = carInstace.GetComponentsInChildren<MeshRenderer>();
            foreach(var render in renders)
            {
                foreach (var mat in render.materials)
                {
                    if(mat.name == "Body (Instance)")
                    {
                        mat.color = Color.HSVToRGB(color, 1, 1);
                    }
                }
            }
            carInstace.transform.localScale = new Vector3(
                float.Parse(car.GetValue("Width (in)")) / 69.5f,
                float.Parse(car.GetValue("Height (in)")) / 56.7f,
                float.Parse(car.GetValue("Length (in)")) / 170.1f
            );

            carInstace.GetComponentInChildren<Text>().text = display;
            carInstace.name = display;
            return carInstace;
        }

        private static string CarQualityToFilePath(CarQuality quality)
        {
            switch (quality)
            {
                default:
                    return "highdef";
            }
        }

    }

}