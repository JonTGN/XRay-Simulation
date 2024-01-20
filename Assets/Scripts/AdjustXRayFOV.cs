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

    // need to manually set each time cookies are generated :(
    public List<Texture> cookieTexs;

    // reference to the other camera that will actually take the xray pic
    public Camera cam;

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
        return string.Format("{0}/Screenshots/screen_{1}x{2}_{3}.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void TakeXRay()
    {
        int resWidth = 200;
        int resHeight = 200;

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        cam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = ScreenShotName(resWidth, resHeight);
        File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("logged screenshot to: {0}", filename));
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
        int x_cookie_val = cookieVals.OrderBy(x => Math.Abs((long)x - (hSliderValue + min))).First();
        int y_cookie_val = cookieVals.OrderBy(x => Math.Abs((long)x - (vSliderValue + min))).First();

        return $"{x_cookie_val}_{y_cookie_val}";
    }
}
