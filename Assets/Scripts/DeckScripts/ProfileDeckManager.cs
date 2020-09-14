using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileDeckManager : MonoBehaviour
{

    public GameObject decksHolder;
    public GameObject singleDeck;
    // Start is called before the first frame update
    public PlayerDeckHelper playerDeckHelper;

    void Start()
    {

        //if (!playerDeckHelper)
        //{
        //    playerDeckHelper = GameObject.FindObjectOfType<PlayerDeckHelper>();
        //}

        //for(int i = 0; i< playerDeckHelper.Decks.Count; i++)
        //{
        //    string[] singleDeck = playerDeckHelper.GetDeckByInedx(i);
        //    GenerateSingleDeck(singleDeck, i);
        //}

        

        //GameObject g = Instantiate(singleDeck, decksHolder.transform) as GameObject;

    }



    public void GenerateSingleDeck(string[] singleDeck, int indexOfDeck)
    {
        GameObject resource = Resources.Load<GameObject>("UI/SingleDeck");
        GameObject deck = Instantiate(resource, decksHolder.transform) as GameObject;
        deck.transform.SetParent(decksHolder.transform);

        SingleDeckHelper singleDeckHelper = deck.GetComponent<SingleDeckHelper>();
        singleDeckHelper.Init(("Deck" + (indexOfDeck+1)) , indexOfDeck, singleDeck[0], singleDeck[1], singleDeck[2], singleDeck[3], singleDeck[4]);
    }

}
