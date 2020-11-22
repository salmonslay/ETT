using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Download : MonoBehaviour
{
    const bool nonReadable = true;
    enum ItemType { Image, Sprite, RawImage };
    /// <summary>
    /// Finds and returns a download module if found, else creates one
    /// </summary>
    public static Download Init()
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Download");
        if (list != null && list.Length > 0) return list[0].GetComponent<Download>();
        return new GameObject().AddComponent<Download>();
    }
    private IEnumerator UpdateItem(GameObject item, string path, ItemType type)
    {
        //Debug.Log(path);
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path, nonReadable))
        {
            uwr.timeout = 2;
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
                Debug.LogError($"Download: Could not download {path}\n{uwr.error}");
            else if (!File.Exists(path.Replace("file://", "")) && !path.Contains("https://"))
                Debug.LogError($"Download/PNG: Tried to download {path} which does not exist");
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);

                if (type == ItemType.Image)
                    item.GetComponent<Image>().sprite = UnityEngine.Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                else if (type == ItemType.Sprite)
                    item.GetComponent<SpriteRenderer>().sprite = UnityEngine.Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                else if (type == ItemType.RawImage)
                    item.GetComponent<RawImage>().texture = texture;
            }
        }
    }
    private IEnumerator UpdateAndSetAlpha(RawImage item, string path)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path, nonReadable))
        {
            uwr.timeout = 4;
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
                Debug.LogError($"Download/PNGAlpha: Could not download {path}\n{uwr.error}");
            else if (File.Exists(path.Replace("file://", "")) || path.Contains("https://"))
            {
                try
                {
                    var texture = DownloadHandlerTexture.GetContent(uwr);
                    if (item) item.texture = texture;
                }
                catch (Exception e)
                {
                    Debug.LogError($"Download/PNGAlpha: Tried to download {path} which caused an error.\n{e}");
                }
            }
            if (item) item.color = Color.white;
        }
    }
    private IEnumerator Setogg(GameObject item, string path, bool play)
    {
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.OGGVORBIS))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
                Debug.LogError($"Download/Ogg: Could not download {path}\n{uwr.error}");
            else if (!File.Exists(path.Replace("file://", "")))
                Debug.LogError($"Download/Ogg: Tried to download {path} which does not exist");
            else if (item.GetComponent<AudioSource>() == null)
                Debug.LogError($"Download/Ogg: Tried to set audio {path} on gameobject {item.name} which does not have an audio source component.");
            else
            {
                item.GetComponent<AudioSource>().clip = DownloadHandlerAudioClip.GetContent(uwr);
                if (play) item.GetComponent<AudioSource>().Play();
            }
        }
    }
    public void ChangeAlpha(RawImage item, string path)
    {
        if (!path.Contains("https://") && !path.Contains("file://")) path = "file://" + path;
        StartCoroutine(UpdateAndSetAlpha(item, path));
    }
    public void Image(GameObject item, string path)
    {
        StartCoroutine(UpdateItem(item, path, ItemType.Image));
    }
    public void Sprite(GameObject item, string path)
    {
        StartCoroutine(UpdateItem(item, path, ItemType.Sprite));
    }
    public void Ogg(GameObject item, string path, bool play = false)
    {
        StartCoroutine(Setogg(item, path, play));
    }
    public void RawImage(GameObject item, string path)
    {
        StartCoroutine(UpdateItem(item, path, ItemType.RawImage));
    }
    /// <summary>
    /// Returns flag from given country code, or a placeholder if the code is invalid
    /// </summary>
    /// <param name="code">2 letter country code</param>
    public Sprite Flag(string code)
    {
        Sprite flag = Resources.Load<Sprite>($"Flags/{code.ToUpper()}@3x");
        return flag != null ? flag : Resources.Load<Sprite>($"Flags/unknown");
    }
}