using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class MovieUI : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Overview;
    public RawImage Backdrop;

    public void SetData(string title, string overview, string imagePath){
        Title.text = title;
        Overview.text = overview;
        StartCoroutine(GetImage(imagePath));
    }

    IEnumerator GetImage(string url){
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url)){
            yield return request.SendWebRequest();
            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log(string.Format("SMTH went wrong {0}", request.error));
                    break;
                case UnityWebRequest.Result.Success:
                    Backdrop.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
                    CoverRawImage();
                    break;
            }
        }
    }

    Vector2 CoverRawImage(){
    float w = 0, h = 0;
        var parent = Backdrop.GetComponentInParent<RectTransform>();
        var imageTransform = Backdrop.GetComponent<RectTransform>();
        if (Backdrop.texture != null) {
            if (!parent) { 
                return imageTransform.sizeDelta;
            }
            float ratio = Backdrop.texture.width / (float)Backdrop.texture.height;
            var bounds = new Rect(0, 0, parent.rect.width, parent.rect.height);
            if (Mathf.RoundToInt(imageTransform.eulerAngles.z) % 180 == 90) {
                bounds.size = new Vector2(bounds.height, bounds.width);
            }
            h = bounds.height;
            w = h * ratio;
            if (w > bounds.width) {
                w = bounds.width;
                h = w / ratio;
            }
        }
        imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
        return imageTransform.sizeDelta;
    }
}