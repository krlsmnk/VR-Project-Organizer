using System;
using UnityEngine;
using System.IO;

namespace CAVS.ProjectOrganizer.Images
{

    /// <summary>
    /// In charge of making asyncronous requests to load an image as a texture
    /// to be used later.
    /// </summary>
    public static class ImageLoader
    {

        /// <summary>
        /// Attempts to load an image from either the filesystem or cache, or
        /// will make an http request to fetch the image for asyncronously over
        /// http
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="sub">Sub.</param>
        public static void LoadImage(string url, Action<string, Texture2D> sub)
        {
            if (!Directory.Exists("cache"))
            {
                Directory.CreateDirectory("cache");
            }
            GetRequestObject().LoadImage(url, sub);
        }

        private static ImageLoaderBehavior requestGameObjectInstance = null;

        private static ImageLoaderBehavior GetRequestObject()
        {

            if (requestGameObjectInstance == null)
            {
                GameObject obj = (GameObject)GameObject.Instantiate(new GameObject("__Image Loader Object__"));
                requestGameObjectInstance = obj.AddComponent<ImageLoaderBehavior>();
                UnityEngine.Object.DontDestroyOnLoad(obj);
            }

            return requestGameObjectInstance;

        }


    }

}