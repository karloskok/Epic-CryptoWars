using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TopPanelDeck : MonoBehaviour
{
    public TextMeshProUGUI nickname, level, winLose, score, coins;
    public PlayerKeysHolder playerKeysHolder;

    public Button BattleButton;

    public List<SingleUnitCard> deck;
    int i = 0;
    private void Awake()
    {
        //deck = new List<SingleUnitCard>();
        foreach(SingleUnitCard d in deck)
        {
            d.gameObject.SetActive(false);
        }
    }

    public void SetUIDeckByIndex(SingleUnitCard unit)
    {
        if (unit.isChecked)
        {
            return;
        }

        for (int i = 0; i < 5; i++)
        {
            if (!deck[i].gameObject.activeInHierarchy)
            {


                //get from all units 

                //find unit and click checked
                unit.Checked(true);




                deck[i].SetUnitValues(unit.unitsId, unit.id, unit.level, unit.rareType, unit.unitType, unit.health, unit.attack, unit.speed, true);
                deck[i].gameObject.SetActive(true);


                break;
            }
        }

    }

    public void RemoveFromDeck(int i)
    {
        //uncheck
        deck[i].gameObject.SetActive(false);

        UnitsHolderScript unitsHolderScript = GameObject.FindObjectOfType<UnitsHolderScript>();
        SingleUnitCard[] cards = unitsHolderScript.gameObject.GetComponentsInChildren<SingleUnitCard>();

        foreach(SingleUnitCard s in cards)
        {
            if (s.unitsId == deck[i].unitsId)
            {
                s.Checked(false);
                return;
            }
        }

    }


    public void SaveToData()
    {
        int c = 0;
        foreach (SingleUnitCard d in deck)
        {
            if (d.gameObject.activeInHierarchy)
                c++;
        }
        Debug.Log("Deck count " + c);

        if (c != 5)
        {
            return;
        }

        

        playerKeysHolder.playerDeck = new List<SingleUnitCard>();
        foreach (SingleUnitCard d in deck)
        {
            playerKeysHolder.playerDeck.Add(d);
            Debug.Log("UnitsId: " + d.unitsId + ", ID: " + d.id);
        }

        PunNetworkManager punNetworkManager = GameObject.FindObjectOfType<PunNetworkManager>();
        if (!punNetworkManager)
        {
            //TODO: return to menu
            
        }

        punNetworkManager.Battle();
        SceneManager.LoadScene("LobbyScene");
    }

}
