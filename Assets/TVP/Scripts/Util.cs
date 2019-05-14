using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KarlSmink.Teleporting
{

    public static class Util 
    {
        public static void PlaySoundEffect(AudioClip audioClip, Vector3 position)
        {
            if (audioClip == null)
            {
                Debug.LogWarning("Trying to play null audio clip!");
                return;
            }

            GameObject audioObj = new GameObject("Sound Effect");
            audioObj.transform.position = position;
            var aud = audioObj.AddComponent<AudioSource>();
            aud.PlayOneShot(audioClip);
            Object.Destroy(audioObj, audioClip.length + 1f);
        }

        public static GameObject BuildCamera(Vector3 position, Quaternion rotation)
        {
            GameObject existingCamera = GameObject.FindGameObjectWithTag("TVPCamera");

            if (existingCamera == null) { 
            RenderTexture tex = new RenderTexture(2160, 1200, 24);
            var camera = Object.Instantiate(Resources.Load<GameObject>("Camera"), position, rotation);
            camera.GetComponentInChildren<Camera>().targetTexture = tex;
            camera.tag = "TVPCamera";
            return camera;
            }

            return existingCamera;
        }

        public static GameObject BuildPortal(Camera camera, Vector3 position, Quaternion rotation)
        {
            var portal = Object.Instantiate(Resources.Load<GameObject>("Portal"), position, rotation);
            portal.GetComponentInChildren<MeshRenderer>().material.mainTexture = camera.targetTexture;
            portal.tag = "broadcastPlane";
            return portal;
        }

    }

}