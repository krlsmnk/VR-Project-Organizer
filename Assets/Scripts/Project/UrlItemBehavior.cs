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

            // Get Md5 hash...
            string hashName = "something hashed";
            
            // Try Loading from memory...
            string[] chunks = url.Split('.');
            string extension = chunks[chunks.Length];


            // Make a request then to download the source
			WWW www = new WWW(url);
			yield return www;
			Renderer renderer = GetComponent<Renderer>();
			renderer.material.mainTexture = www.texture;

            // Cache the image on the machine.
            File.WriteAllBytes(hashName + "." + extension, www.bytes);

		}

	}

}