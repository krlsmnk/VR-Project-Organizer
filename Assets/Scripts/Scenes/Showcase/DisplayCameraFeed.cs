using UnityEngine;
using System;
using AnvelApi;

public class DisplayCameraFeed : MonoBehaviour
{

    private Renderer renderer;

    private Texture2D displayTexture;

    private ObjectDescriptor camera;

    private AnvelControlService.Client client;

    public void Initialize(AnvelControlService.Client connection, string cameraName)
    {

        this.client = connection;

        renderer = GetComponent<Renderer>();

        displayTexture = Instantiate(renderer.material.mainTexture) as Texture2D;

        displayTexture.Resize(640, 480, TextureFormat.ARGB32, false);

        renderer.material.mainTexture = displayTexture;

        camera = client.GetObjectDescriptorByName(cameraName);

    }

    private void Update()
    {
        Image cameraImage = client.GetCameraFrame(camera.ObjectKey, 0);
        
        const int numColors = 3;

        if (cameraImage.HasImage == true)
        {
            int size = (cameraImage.Frame.Length) / numColors;
            var colorArray = new Color32[size];

            //For each pixel is a red,blue, and green color that is added to the array
            int frameArraySize = size * numColors - numColors;

            int index = 0;

            //They come in inverted, so we start from the back of the array and work our way to the front
            for (int i = (frameArraySize); i >= 0; i += -numColors)
            {
                var color = new Color32(cameraImage.Frame[index + 0], cameraImage.Frame[index + 1], cameraImage.Frame[index + 2], 1);
                colorArray[i / numColors] = color;

                index++;
            }

            displayTexture.SetPixels32(colorArray);
            displayTexture.Apply();
        }

    }
}

