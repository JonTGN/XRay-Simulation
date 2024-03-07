using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WriteImgWebCall : MonoBehaviour
{
    readonly string postURL = "http://170.64.74.6/write_img.php";

    public IEnumerator SendPostReq(byte[] byteArr, string fileName)
    {
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        form.Add(new MultipartFormDataSection("imgBytes", byteArr));
        form.Add(new MultipartFormDataSection("fileName", fileName));

        UnityWebRequest req = UnityWebRequest.Post(postURL, form);

        yield return req.SendWebRequest();

        if (req.isNetworkError || req.isHttpError)
        {
            Debug.LogError(req.error);
        }

        else
        {
            Debug.Log("req success");
        }
    }
}
