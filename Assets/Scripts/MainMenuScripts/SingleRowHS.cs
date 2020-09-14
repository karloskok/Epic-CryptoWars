using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SingleRowHS : MonoBehaviour
{

    public TextMeshProUGUI nickname, level, score, wins, defeats, coins;

    public void SetUIRow(string name, int level, int score, int wins, int defs, int coins)
    {
        this.nickname.text = name;
        this.level.text = level.ToString();
        this.score.text = score.ToString();
        this.wins.text = wins.ToString();
        this.defeats.text = defs.ToString();
        this.coins.text = coins.ToString();
    }
    

}
