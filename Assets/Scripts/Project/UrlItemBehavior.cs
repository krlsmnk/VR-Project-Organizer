using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CAVS.ProjectOrganizer.Project
{

	public class UrlItemBehavior : ItemBehaviour {

		public void SetImage(string url)
		{
			StartCoroutine (LoadImage (url));
		}

		private IEnumerator LoadImage(string url)
		{
			WWW www = new WWW(url);
			yield return www;
			Renderer renderer = GetComponent<Renderer>();
			renderer.material.mainTexture = www.texture;
		}

	}

}