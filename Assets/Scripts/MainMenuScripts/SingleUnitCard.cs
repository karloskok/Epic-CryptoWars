using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleUnitCard : MonoBehaviour
{
    public int unitsId;
    public string id;

    public int level;
   public  int rareType;
    public int unitType;

    public int health;
   public  int attack;
    public int speed;
    public Image image;

    public bool check = true;

    public bool isChecked;

    public TMPro.TextMeshProUGUI idUI, levelUI, rareTypeUI, healthUI, speedUI, attackUI; 

    public void SetUnitValues(
        int unitsId,
        string id,
        int level,
        int rareType,
        int unitType,

        int health,
        int attack,
        int speed,
        bool topbar
        )
    {
        this.unitsId = unitsId;
        this.id = id;

        this.level = level;
        this.rareType = rareType;
        this.unitType = unitType;

        this.health = health;
        this.attack = attack;
        this.speed = speed;
        Activate();
        UpdateUI();

        //set on click
        TopPanelDeck topPanelDeck = GameObject.FindObjectOfType<TopPanelDeck>();
        if(!topbar)
            this.gameObject.GetComponent<Button>().onClick.AddListener(delegate { topPanelDeck.SetUIDeckByIndex(this); });

    }

    void UpdateUI()
    {
        idUI.text = id;
        levelUI.text = "Lvl " + level.ToString();
        rareTypeUI.text = rareType.ToString();
        healthUI.text = health.ToString();
        speedUI.text = speed.ToString();
        attackUI.text = attack.ToString();

        string heroName = UnitType.ReturnUnitTypeName(unitType);
        Sprite sprite = Resources.Load<Sprite>("Sprites/" + heroName);
        image.sprite = sprite;
    }


    public void Activate()
    {
        this.gameObject.SetActive(true);
    }
    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }

    public Image imageChecked;
    public void Checked(bool c)
    {
        isChecked = c;
        //get image
        if (check)
            imageChecked.gameObject.SetActive(c);
    }
}
