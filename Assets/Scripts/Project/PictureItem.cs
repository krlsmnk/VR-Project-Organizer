using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CAVS.ProjectOrganizer.Project
{

    public class PictureItem : Item
    {

        private static GameObject nodeGameobjectReference;
        protected override GameObject getGameobjectReference()
        {
            if (nodeGameobjectReference == null)
            {
                nodeGameobjectReference = Resources.Load<GameObject>("Picture Node");
            }
            return nodeGameobjectReference;
        }

        /// <summary>
        /// Url reference to the image we want to load as our picture. If a 
        /// texture is instead used to build this object this field is ignored.
        /// </summary>
        private string url;

        private Texture2D image;

        public PictureItem(string title, string url) : base(title)
        {
            this.image = null;
            this.url = url;
        }

        public PictureItem(string title, Texture2D image, Dictionary<string, string> values) : base(title, values)
        {
            this.image = image;
            this.url = "";
        }

        public string GetUrl()
        {
            return this.url;
        }

        public Texture2D GetImage()
        {
            return this.image;
        }

        /// <summary>
        /// Builds a graphical representation of the object inside of the scene for
        /// displaying the text content.
        /// </summary>
        /// <returns>The item just built.</returns>
        protected override ItemBehaviour BuildItem(GameObject node)
        {
            PictureItemBehavior urlBehavior = node.AddComponent<PictureItemBehavior>();
            if (this.GetImage() != null)
            {
                urlBehavior.SetImage(this.GetImage());
            }
            else
            {
                urlBehavior.SetImage(this.GetUrl());
            }
            return urlBehavior;
        }

    }

}