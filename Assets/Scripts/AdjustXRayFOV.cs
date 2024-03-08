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

public class AdjustXRayFOV : MonoBehaviour
{
    public WriteImgWebCall writeImgWebCall;
    public int hSliderValue;
    public int vSliderValue;

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
    public Camera cam;

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
        return string.Format("screen_{0}x{1}_{2}.png",
                             width,
                             height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm"));

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

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        cam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

        /*
        (2048x2048)
        [cropx, cropy, imgH, imgW]
        (h=1, v=32) 730, 800, 800, 560
        (h=32, v=1) 630, 900, 600, 760
        (32, 32) 630, 800, 800, 760     
        */
        // img width/height should correlate to the collimation settings
        // if width decreases by 300, x needs to += by 150 to still be set in the middle

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

        Texture2D croppedTexture = new Texture2D(imgWidth, imgHeight);
        Texture2D originalTextureResized = screenShot;
        croppedTexture.SetPixels(originalTextureResized.GetPixels(cropx, cropy, imgWidth, imgHeight));
        croppedTexture.Apply();
        screenShot = croppedTexture;

        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = ScreenShotName(resWidth, resHeight);

        Debug.Log("BYTE ARR: " + bytes[0] + " " + bytes[1] + " " + bytes[2] + " " + bytes[3] + " " + bytes[4] + " " + bytes[5]);

        // server
        Debug.Log("Sending req to web...");
        StartCoroutine(writeImgWebCall.SendPostReq(bytes, filename));

        // localhost 
        //File.WriteAllBytes(filename, bytes);
        //Debug.Log(string.Format("logged screenshot to: {0}", filename));
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
