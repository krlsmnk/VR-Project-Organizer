using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CAVS.ProjectOrganizer.Project
{

	public class UrlItem : Item
	{

		private static GameObject nodeGameobjectReference;
		protected override GameObject getGameobjectReference()
		{
			if (nodeGameobjectReference == null)
			{
				nodeGameobjectReference = Resources.Load<GameObject> ("Url Node");
			}
			return nodeGameobjectReference;
		}

		private string url;

		public UrlItem(string title, string url) : base(title)
		{
			this.url = url;
		}

		public string GetUrl()
		{
			return this.url;
		}

		/// <summary>
		/// Builds a graphical representation of the object inside of the scene for
		/// displaying the text content.
		/// </summary>
		/// <returns>The item just built.</returns>
		protected override ItemBehaviour BuildItem (GameObject node) 
		{
			UrlItemBehavior urlBehavior = node.AddComponent<UrlItemBehavior>();
			urlBehavior.SetImage (this.GetUrl());
			return urlBehavior;
		}

	}

}