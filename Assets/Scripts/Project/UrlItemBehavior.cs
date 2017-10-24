using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace CAVS.ProjectOrganizer.Project
{

	public class UrlItemBehavior : ItemBehaviour {

		public void SetImage(string url)
		{
			StartCoroutine (LoadImage (url));
		}

		private IEnumerator LoadImage(string url)
		{

            if (!Directory.Exists("cache"))
            {
                Directory.CreateDirectory("cache");
            }
            
            // Get Md5 hash...
            string hashName = "something hashed";
            
            // Try Loading from memory...
            string[] chunks = url.Split('.');
            string extension = chunks[chunks.Length-1];

            // Make a request then to download the source
			WWW www = new WWW(url);
			yield return www;
			Renderer renderer = GetComponent<Renderer>();
			renderer.material.mainTexture = www.texture;

            string filePath = Path.Combine( Path.Combine(Directory.GetCurrentDirectory(), "cache"), hashName + "." + extension);

            Debug.Log(filePath);

            // Cache the image on the machine.
            File.WriteAllBytes(filePath, www.bytes);

		}

	}

}