using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace CAVS.ProjectOrganizer.Images
{
    /// <summary>
    /// In charge of making a request
    /// </summary>
    public class ImageLoaderBehavior : MonoBehaviour
    {

        public void LoadImage(string url, Action<string, Texture2D> sub)
        {
            StartCoroutine(LoadImageAsync(url, sub));
        }


        private IEnumerator LoadImageAsync(string url, Action<string, Texture2D> sub)
        {

            string hashName = CalculateMD5Hash(url);
            string[] chunks = url.Split('.');
            string extension = chunks[chunks.Length - 1];
            string filePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "cache"), hashName + "." + extension);

            if (File.Exists(filePath))
            {
                Texture2D textureFromCache = new Texture2D(1, 1);
                textureFromCache.LoadImage(File.ReadAllBytes(filePath));
				sub(url, textureFromCache);
            }
            else if (url != "")
            {
                // Make a request then to download the source
                Debug.Log("URL: (" + url + ")");
                WWW www = new WWW(url);
                yield return www;

                // Cache the image on the machine.
                if (www.bytes != null)
                {
                    File.WriteAllBytes(filePath, www.bytes);
                }

				sub(url, www.texture);
            } else {
                sub(url, null);
            }

        }

        /// <summary>
        /// Used for determining name for caching downloaded content to computer
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string CalculateMD5Hash(string input)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            string sb = "";
            for (int i = 0; i < hash.Length; i++)
            {
                sb += hash[i].ToString("X2");
            }

            return sb.ToString();
        }

    }

}