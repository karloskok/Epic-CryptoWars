using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreHolder : MonoBehaviour
{
    public List<GameObject> rows;

    public int count = 0;
    public int set = 1;



    public void InitRows()
    {
        rows = new List<GameObject>();

        //for(int i = 0; i< set; i++)
        //{
        //    AddToRow("ksk", 1,2,3,4,2);
        //}

    }

    public void AddToRow(string name, int level, int score, int wins, int defs, int coins)
    {
        
        GameObject resource = Resources.Load<GameObject>("UI/SingleRow");
        GameObject panel = Instantiate(resource, transform) as GameObject;

        panel.transform.SetParent(this.transform);

        panel.GetComponent<SingleRowHS>().SetUIRow(name, level, score, wins, defs, coins);

        //rect transfor i>5 +90
        if (count > 4)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, rectTransform.offsetMin.y - 90f);
        }


        count++;

    }


}
