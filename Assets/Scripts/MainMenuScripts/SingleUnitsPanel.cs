using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleUnitsPanel : MonoBehaviour
{
    public List<SingleUnitCard> unitCards;
    // Start is called before the first frame update
    

    public void SetUnitValuesByIndex(int i, int ethUnitIds, SingleUnitCard singleUnitCard)
    {
        unitCards[i].SetUnitValues(ethUnitIds, singleUnitCard.id, singleUnitCard.level, singleUnitCard.rareType, singleUnitCard.unitType, singleUnitCard.health, singleUnitCard.attack, singleUnitCard.speed, false);
    }

    public void HideAllCards()
    {
        foreach (SingleUnitCard s in unitCards)
        {
            s.Deactivate();
        }
    }
}
