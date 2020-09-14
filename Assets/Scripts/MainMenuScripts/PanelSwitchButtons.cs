using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSwitchButtons : MonoBehaviour
{
    public List<GameObject> buttons;
    public int buttonCount = 0;
    public int size = 12;

    private void Awake()
    {
        buttons = new List<GameObject>();
    }
    int index = 0;
    public void AddNewButton()
    {
        GameObject resource = Resources.Load<GameObject>("UI/PanelCurrentButton");
        GameObject panel = Instantiate(resource, transform) as GameObject;
        panel.transform.SetParent(this.transform.GetChild(0).transform);

        buttons.Add(panel);

        //set function
        //switch to next index of button
        //if curr and next !equals  ...
        int i = new int();
        i = index;
        panel.GetComponent<Button>().onClick.AddListener(delegate { SwitchDirectToPanel(i); });


        index++;

        buttonCount++;
    }


    public void SwitchDirectToPanel(int next)
    {
        

        UnitsHolderScript unitsHolderScript = GameObject.FindObjectOfType<UnitsHolderScript>();
        
        if (!unitsHolderScript.canSwitch) return;

        if (next != unitsHolderScript.currentPannel)
        {
            if(next > unitsHolderScript.currentPannel)
            {
                unitsHolderScript.SwitchPanel(unitsHolderScript.currentPannel, next, false);
            }
            else
            {
                unitsHolderScript.SwitchPanel(unitsHolderScript.currentPannel, next, true);
            }
        }
    }

    public void SetGlow(bool glow, Button button)
    {
        if (glow)
        {
            button.transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
        else
        {
            button.transform.GetChild(0).GetComponent<Image>().enabled = false;
        }
    }

}
