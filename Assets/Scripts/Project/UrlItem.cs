using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CAVS.ProjectOrganizer.Project
{

	public class UrlItem : Item
	{

		private static GameObject nodeGameobjectReference;
		private static GameObject getGameobjectReference()
		{
			if (nodeGameobjectReference == null) {
				nodeGameobjectReference = Resources.Load<GameObject> ("Node");
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
			return node.AddComponent<ItemBehaviour>();
		}

	}

}