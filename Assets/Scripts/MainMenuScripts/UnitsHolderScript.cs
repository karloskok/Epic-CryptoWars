using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsHolderScript : MonoBehaviour
{
    public int panelCount;
    public float baseWidth = 1200;
    public int currentPannel = 0;

    public List<SingleUnitsPanel> unitsPanels;
    SingleUnitCard singleUnitCard;
    PanelSwitchButtons panelSwitchButtons;


    public List<SingleUnitCard> allUnits;
    // Start is called before the first frame update
    void Start()
    {
        //InitUnitPanels(2);
        



    }

    public void InitUnitPanels(int unitCount, List<SingleUnitCard> units)
    {

        //unit count 7
        //if unit  > 1 && unitcount % 8 ==04 % 8 == 0
        panelCount = 0;
        panelSwitchButtons = GameObject.FindObjectOfType<PanelSwitchButtons>();

        int currentPanel = -1;
        for(int i = 0; i<unitCount; i++)
        {
            

            if (i % 8 == 0)
            {
                //move to next panel
                currentPanel++;
                panelCount++;

                
                panelSwitchButtons.AddNewButton();


                GameObject resource = Resources.Load<GameObject>("UI/SingleUnitsPanel");
                GameObject panel = Instantiate(resource, transform) as GameObject;
                panel.name = "SingleUnitsPanel" + currentPanel;
                panel.transform.SetParent(transform);
                RectTransform rectTransform = panel.GetComponent<RectTransform>();

                rectTransform.anchoredPosition = new Vector2(currentPanel * baseWidth, rectTransform.anchoredPosition.y);

                SingleUnitsPanel singleUnitsPanel = panel.GetComponent<SingleUnitsPanel>();
                //deactivate 
                singleUnitsPanel.HideAllCards();
                unitsPanels.Add(singleUnitsPanel);
            }


            //get unit from eth by id
            allUnits.Add(units[i]);





            int onPanelId = i % 8;
            unitsPanels[currentPanel].SetUnitValuesByIndex(onPanelId, units[i].unitsId, units[i]);

        }


        RectTransform rectTrans = panelSwitchButtons.gameObject.GetComponent<RectTransform>();

        int resize = panelCount * 17;

        rectTrans.offsetMin = new Vector2(rectTrans.offsetMin.x - resize, rectTrans.offsetMin.y);
        rectTrans.offsetMax = new Vector2(rectTrans.offsetMax.x + resize, rectTrans.offsetMax.y);


        panelSwitchButtons.SetGlow(true, panelSwitchButtons.buttons[0].GetComponent<UnityEngine.UI.Button>());
    }

    public bool canSwitch = true;

    public IEnumerator wait(float time)
    {
        canSwitch = false;
        yield return new WaitForSeconds(time);
        canSwitch = true;
    }




    public void SwitchPanel(int current, int next, bool ToRight)
    {
        if (current == next) return;

        if (ToRight)
        {
            unitsPanels[current].GetComponent<Animator>().enabled = true;
            unitsPanels[current].GetComponent<Animator>().Play("MainPanelToRight");

            unitsPanels[next].GetComponent<Animator>().enabled = true;
            unitsPanels[next].GetComponent<Animator>().Play("LeftToMainPanel");

        }
        else
        {
            unitsPanels[current].GetComponent<Animator>().enabled = true;
            unitsPanels[current].GetComponent<Animator>().Play("MainPanelToLeft");

            unitsPanels[next].GetComponent<Animator>().enabled = true;
            unitsPanels[next].GetComponent<Animator>().Play("RightToMainPanel");
        }

        panelSwitchButtons.SetGlow(true, panelSwitchButtons.buttons[next].GetComponent<UnityEngine.UI.Button>());
        panelSwitchButtons.SetGlow(false, panelSwitchButtons.buttons[currentPannel].GetComponent<UnityEngine.UI.Button>());

        currentPannel = next;

        StartCoroutine(wait(1.2f));
        //set current butt glow
        

        //MainPanelToLeft
        //LeftToMainPanel

        //MainPanelToRight
        //RightToMainPanel

    }


    public void MovePanelToLeft()
    {
        if (!canSwitch) return;

        int next = currentPannel - 1;
        if (next < 0)
            next = panelCount - 1;
        SwitchPanel(currentPannel, next, true);
    }

    public void MovePanelTpRight()
    {
        if (!canSwitch) return;
        int next = currentPannel + 1;
        
        next %= panelCount;
        SwitchPanel(currentPannel, next, false);
    }
}
