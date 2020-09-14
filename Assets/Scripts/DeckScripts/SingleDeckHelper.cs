using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleDeckHelper : MonoBehaviour
{

    public Image ImageHero1, ImageHero2, ImageHero3, ImageHero4, ImageHero5;
    public TextMeshProUGUI deckName;
    public GameObject shadow;
    public int indexOfDeck = 0;



    public void Init(string _deckName, int _index, string hero1, string hero2, string hero3, string hero4, string hero5)
    {
        deckName.text = _deckName;
        indexOfDeck = _index;
        SetImage(hero1, ImageHero1);
        SetImage(hero2, ImageHero2);
        SetImage(hero3, ImageHero3);
        SetImage(hero4, ImageHero4);
        SetImage(hero5, ImageHero5);
        ShadowSetInActive();
    }

    void SetImage(string hero, Image image)
    {
        Sprite sprite = Resources.Load<Sprite>("Sprites/" + hero);
        image.sprite = sprite;
    }


    public void ShadowSetActive()
    {
        shadow.SetActive(true);
    }
    public void ShadowSetInActive()
    {
        shadow.SetActive(false);
    }

    public void DeckOnClick()
    {
        transform.parent.GetComponent<DeckHolderResize>().DeactivateAllDecks();
        //set deck active 
        ShadowSetActive();
        if (PlayerDeckHelper.Instance)
        {
            PlayerDeckHelper.Instance.indexActiveDeck = indexOfDeck;
        }
        else
        {
            PlayerDeckHelper playerDeckHelper = GameObject.FindObjectOfType<PlayerDeckHelper>();
            playerDeckHelper.indexActiveDeck = indexOfDeck;
        }

    }

}
