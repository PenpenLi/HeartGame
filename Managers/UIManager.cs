using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    /// <summary>
    /// Get Button
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Button GetButton(string name)
    {
        Button btn = GameObject.Find(name).GetComponent<Button>();
        if (btn != null)
        {
            return btn;
        }
        else return null;
    }
    /// <summary>
    /// Get InputField's string
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetInputStr(string name)
    {
        InputField input = GameObject.Find(name).GetComponent<InputField>();
        return input.text.Trim();
    }
    /// <summary>
    /// Get Image
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Image GetImage(string name)
    {
        Image image = GameObject.Find(name).GetComponent<Image>();
        return image;
    }
    public static void ChangeImage(string name, string path)
    {
        Image image = GetImage(name);
        Sprite sprite = Resources.Load<Sprite>(path);
        image.sprite = sprite;
    }
    public static RawImage GetRawImage(string name)
    {
        RawImage rawimage = GameObject.Find(name).GetComponent<RawImage>();
        return rawimage;
    }

    /// <summary>
    /// change image's sprite
    /// </summary>
    /// <param name="name"></param>
    /// <param name="path"></param>
    public static void ChangeRawImage(string name,string path)
    {
        RawImage image = GetRawImage(name);
        Texture2D texture = (Texture2D)GameFuncs.GetResource(path);
        image.texture = texture;
    }
    /// <summary>
    /// Get Text
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Text GetText(string name)
    {
        Text text = GameObject.Find(name).GetComponent<Text>();
        return text;
    }
    /// <summary>
    /// change text.text
    /// </summary>
    /// <param name="name"></param>
    /// <param name="s"></param>
    public static void ChangeText(string name,string s)
    {
        Text text = GetText(name);
        text.text = s;
    }
    public static Slider GetSlider(string name)
    {
        Slider slider = GameObject.Find(name).GetComponent<Slider>();
        return slider;
    }
    public static void ChangeSlider(string name,float value)
    {
        Slider slider = GetSlider(name);
        slider.value = value;
    }
}
