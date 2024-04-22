using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WriteImgWebCall : MonoBehaviour
{
    readonly string postURL = "http://170.64.74.6:5383/write_img/";

    public IEnumerator SendPostReq(byte[] byteArr, string fileName)
    {
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        form.Add(new MultipartFormDataSection("imgBytes", byteArr));
        form.Add(new MultipartFormDataSection("fileName", fileName));

        UnityWebRequest req = UnityWebRequest.Post(postURL, form);

        // Set the content type header
        req.SetRequestHeader("Content-Type", "application/octet-stream");

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
