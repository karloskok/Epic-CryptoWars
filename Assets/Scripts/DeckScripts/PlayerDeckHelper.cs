using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckHelper : MonoBehaviour
{
    public static PlayerDeckHelper Instance { get; private set; } = null;
    public List<string> playerHeroes;
    public List<string[]> Decks;
    public int indexActiveDeck = 0;

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        InitDeck();
    }

    private void Start()
    {
        //Decks = new List<string[]>();
        //InitDeck();

        
    }

    public void AddToDeck(string[] deck)
    {
        Decks.Add(deck);
    }

    public void RemoveFromDeck(string[] deck)
    {
        Decks.Remove(deck);
    }

    public void InitDeck()
    {
        Decks = new List<string[]>();

        //load deck

        string[] low = new string[] { "Light_Peasant", "Light_Spearman", "Light_Peasant", "Light_Spearman", "Light_Peasant" };
        string[] mid = new string[] { "Light_Peasant", "Light_Spearman", "Light_Peasant", "Light_Spearman", "Light_Spearman" };
        string[] high = new string[] { "Light_Infantry", "Light_Infantry", "Light_Peasant", "Light_Spearman", "Light_Peasant" };

        AddToDeck(low);
        AddToDeck(mid);
        AddToDeck(high);
    }

    public string[] GetActiveDeck()
    {
        return Decks[indexActiveDeck];
    }

    public string[] GetDeckByInedx(int index)
    {
        return Decks[index];
    }




}
