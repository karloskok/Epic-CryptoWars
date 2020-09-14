using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroButton : MonoBehaviour, IPointerDownHandler
{
    public System.Action<HeroButton> OnDown;
    public Image image;
    public int heroIndex = 0;
    public string heroName = "Light_Peasant";
    public Image heroImage;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDown?.Invoke(this);
    }

    //public void Start()
    //{
    //    //Load a Sprite (Assets/Resources/Sprites/sprite01.png)
    //    Sprite sprite = Resources.Load<Sprite>("Sprites/" + heroName);
    //    //heroImage = GetComponentInChildren<Image>();
    //    heroImage.sprite = sprite;
    //}

    public void SetImage()
    {
        //Load a Sprite (Assets/Resources/Sprites/sprite01.png)
        Sprite sprite = Resources.Load<Sprite>("Sprites/" + heroName);
        //heroImage = GetComponentInChildren<Image>();
        heroImage.sprite = sprite;
    }
}
