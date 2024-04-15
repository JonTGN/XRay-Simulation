using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Net;
using UnityEngine.Networking;
using UnityEditor;
using Cinemachine;
using TMPro;

public class AdjustXRayFOV : MonoBehaviour
{
    public bool saveImageLocally = false;
    public TextMeshProUGUI tmp_text;
    public WriteImgWebCall writeImgWebCall;
    public int hSliderValue;
    public int vSliderValue;

    // xray image
    public RawImage imageContainer;

    // use to set val init
    public Slider hSlider;
    public Slider vSlider;

    public Light xRayLight;

    public List<int> cookieVals;

    // get from py config
    private int min = 192;
    private int max = 256;
    private int spacing = 8;


    public int cropx = 256;
    public int cropy = 256;
    public int imgHeight;
    public int imgWidth;
    public bool debug = false;

    // need to manually set each time cookies are generated :(
    public List<Texture> cookieTexs;

    // reference to the other camera that will actually take the xray pic
    public Camera skinXrayCam;
    public Camera bonesXrayCam;

    // res correlating to cookie tex
    private int x_cookie_val;
    private int y_cookie_val;
    private int upscale = 8;  // upscale * x/y cookie val = res

    public void HorizontalSliderChanged(Slider slider)
    {
        hSliderValue = Convert.ToInt16(slider.value);
        UpdateCookieTex();
    }

    public void VerticalSliderChanged(Slider slider)
    {
        vSliderValue = Convert.ToInt16(slider.value);
        UpdateCookieTex();
    }

    void Start()
    {
        Cursor.visible = true;
        hSliderValue = Convert.ToInt16(hSlider.value);
        vSliderValue = Convert.ToInt16(vSlider.value);

        // update cookie tex on start
        GenerateCookieArr();
        UpdateCookieTex();
    }

    // just do it here to avoid unecessary references all over the place 
    private static string ScreenShotName(int width, int height)
    {
        //return string.Format("screen_{0}x{1}_{2}.png",
        //                     width,
        //                     height,
        //                     System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm"));

        // localhost
        return string.Format("{0}/Screenshots/screen_{1}x{2}_{3}.png",
                             Application.dataPath,
                             width,
                             height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void TakeXRay()
    {
        int resWidth = 256 * upscale;
        int resHeight = 256 * upscale;

        //RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        //skinXrayCam.targetTexture = rt;
        //Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        //skinXrayCam.Render();
        //RenderTexture.active = rt;
        //screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

        // bones + skin rendering:
        RenderTexture rtSkin = new RenderTexture(resWidth, resHeight, 24);
        RenderTexture rtBones = new RenderTexture(resWidth, resHeight, 24);

        // Set the target textures for both cameras
        skinXrayCam.targetTexture = rtSkin;
        bonesXrayCam.targetTexture = rtBones;

        // Render both cameras
        skinXrayCam.Render();
        bonesXrayCam.Render();

        // Create textures to hold the camera renders
        Texture2D skinTexture = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Texture2D bonesTexture = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);

        // Read pixels from the RenderTextures
        RenderTexture.active = rtSkin;
        skinTexture.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        RenderTexture.active = rtBones;
        bonesTexture.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

        // Reset active RenderTexture
        RenderTexture.active = null;

        // get crop values
        if (!debug)
        {
            // initialize base crop values as if sliders are 1 x 1 to simplify math
            cropx = 780;
            cropy = 800;
            imgHeight = 600;
            imgWidth = 500;

            // at 2048x2048 res each tick on the slider represents about 3.125 pixels for crop and 6.25 for img HxW
            if (hSliderValue != 1)
            {
                cropx -= (int)(3.125 * hSliderValue);
                imgWidth += (int)(6.25 * hSliderValue);
            }

            if (vSliderValue != 1)
            {
                cropy -= (int)(3.125 * vSliderValue);
                imgHeight += (int)(6.25 * vSliderValue);
            }
        }


        //Texture2D croppedTexture = new Texture2D(imgWidth, imgHeight);
        //Texture2D originalTextureResized = screenShot;
        //croppedTexture.SetPixels(originalTextureResized.GetPixels(cropx, cropy, imgWidth, imgHeight));
        //croppedTexture.Apply();
        //screenShot = croppedTexture;

        //skinXrayCam.targetTexture = null;
        //RenderTexture.active = null;
        //Destroy(rt);
        //byte[] bytes = screenShot.EncodeToPNG();
        //string filename = ScreenShotName(resWidth, resHeight);

        // Define the crop region
        Rect cropRect = new Rect(cropx, cropy, imgWidth, imgHeight);

        // Crop textures
        skinTexture = CropTexture(skinTexture, cropRect);
        bonesTexture = CropTexture(bonesTexture, cropRect);

        // Overlay the pixels from the second camera onto the first camera's pixels
        Color[] skinPixels = skinTexture.GetPixels();
        Color[] bonesPixels = bonesTexture.GetPixels();

        for (int i = 0; i < skinPixels.Length; i++)
        {
            // Overlay the bones texture onto the skin texture
            skinPixels[i] = Color.Lerp(skinPixels[i], bonesPixels[i], 0.38f); // last float is linear blend amnt
        }

        // Apply the modified pixels back to the skinTexture
        skinTexture.SetPixels(skinPixels);
        skinTexture.Apply();

        // Encode the modified texture to PNG bytes
        byte[] bytes = skinTexture.EncodeToPNG();

        // Clean up
        skinXrayCam.targetTexture = null;
        bonesXrayCam.targetTexture = null;
        Destroy(rtSkin);
        Destroy(rtBones);

        // server
        //Debug.Log("Sending req to web...");
        //StartCoroutine(writeImgWebCall.SendPostReq(bytes, filename));

        // localhost - log to screenshot dir
        //if (saveImageLocally)
        //{
        //    tmp_text.text = filename;
        //    File.WriteAllBytes(filename, bytes);
        //    Debug.Log(string.Format("logged screenshot to: {0}", filename));
        //}

        // encode bytes into image texture to display on final screen
        LoadImageIntoCanvas(bytes);
    }

    Texture2D CropTexture(Texture2D texture, Rect cropRect)
    {
        int x = Mathf.FloorToInt(cropRect.x);
        int y = Mathf.FloorToInt(cropRect.y);
        int width = Mathf.FloorToInt(cropRect.width);
        int height = Mathf.FloorToInt(cropRect.height);

        Color[] pixels = texture.GetPixels(x, y, width, height);
        Texture2D croppedTexture = new Texture2D(width, height);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();

        return croppedTexture;
    }

    // Function to load image from raw bytes
    private void LoadImageIntoCanvas(byte[] imageData)
    {
        // Create a new texture
        Texture2D texture = new Texture2D(2, 2);

        // Load image data into the texture
        if (texture.LoadImage(imageData))
        {
            // Set texture to RawImage
            imageContainer.texture = texture;

            // Calculate aspect ratio
            float aspectRatio = (float)texture.width / texture.height;

            // Adjust size of RawImage based on aspect ratio
            RectTransform rectTransform = imageContainer.rectTransform;
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.y * aspectRatio, rectTransform.sizeDelta.y);
        }
        else
        {
            Debug.LogError("Failed to load image from bytes.");
        }
    }

    private void GenerateCookieArr()
    {
        // there is a border spacing pixels wide so that's why it's max - spacing
        for (int i = min; i <= max - spacing; i += spacing)
        {
            cookieVals.Add(i);
        }
    }

    // use the slider values to decide what the most appropriate cookie tex to use is
    private void UpdateCookieTex()
    {
        // get name of cookie tex based on slider vals
        var tex = GetCookieTex();

        // view list of texs to find matching name and set element
        Texture cookieTex = cookieTexs[0];
        foreach (var element in cookieTexs)
        {
            if (element.name == tex)
            {
                cookieTex = element;
            }
        }

        xRayLight.cookie = cookieTex;
    }

    private string GetCookieTex()
    {
        x_cookie_val = cookieVals.OrderBy(x => Math.Abs((long)x - (hSliderValue + min))).First();
        y_cookie_val = cookieVals.OrderBy(x => Math.Abs((long)x - (vSliderValue + min))).First();

        return $"{x_cookie_val}_{y_cookie_val}";
    }
}
