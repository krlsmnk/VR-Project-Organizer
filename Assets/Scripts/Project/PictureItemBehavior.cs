using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CAVS.ProjectOrganizer.Images;

namespace CAVS.ProjectOrganizer.Project
{

	public class PictureItemBehavior : ItemBehaviour {

		string uriLoadedFrom;

		void Awake()
		{
			uriLoadedFrom = "";
		}

		/// <summary>
		/// Performs an asyncronous load on the image and then sets the results as
		/// it's texture
		/// </summary>
		/// <param name="uri"></param>
		public void SetImage(string uri)
		{
			uriLoadedFrom = uri;
			ImageLoader.LoadImage (uri, this.OnImageLoad);
		}

		/// <summary>
		/// Immediately sets the image as it's texture
		/// </summary>
		/// <param name="image"></param>
		public void SetImage(Texture2D image)
		{
			GetComponentInChildren<MeshRenderer>().material.mainTexture = image;
		}

		public void OnImageLoad (string requestURI, Texture2D image)
		{
			if (uriLoadedFrom != requestURI) {
				return;
			}
			GetComponentInChildren<MeshRenderer>().material.mainTexture = image;
		}

	}

}