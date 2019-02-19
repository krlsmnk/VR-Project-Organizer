using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    public class Tracker : MonoBehaviour
    {
        int v = -1;

        // Use this for initialization
        void Start()
        {
            StartCoroutine(UpdatePos());

            Tracker_OnMainCarChange(CarManager
                .Instance()
                .GetMainCar());

            CarManager
                .Instance()
                .OnMainCarChange += Tracker_OnMainCarChange;

        }

        private void Tracker_OnMainCarChange(Project.PictureItem car)
        {
            if (car != null)
            {
                v = int.Parse(car.GetValue("id"));
            }
        }

        IEnumerator UpdatePos()
        {
            while (true)
            {
                var pos = transform.position;
                UnityWebRequest www = UnityWebRequest.Get(string.Format("http://videogamedev.club:2019/set?x={0}&y={1}&z={2}&v={3}", pos.x, pos.y, pos.z, 1));
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    if (www.downloadHandler.text != "values set")
                    {
                        Debug.LogError("Unsucsessfully set values: " + www.downloadHandler.text);
                    }
                }

            }
        }
    }

}