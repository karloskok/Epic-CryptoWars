using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerKeysHolder", order = 1)]
public class PlayerKeysHolder : ScriptableObject
{

    public string networkUrl = "http://127.0.0.1:7545";
    public string playerEthereumAccount = "";
    public string playerEthereumPrivateKey = "";

    public string Name;
    public int unitCount = 0;


    public System.Numerics.BigInteger Level;
    public System.Numerics.BigInteger Wins;
    public System.Numerics.BigInteger Defeats;
    public System.Numerics.BigInteger Score;
    public System.Numerics.BigInteger Coins;


    public System.Collections.Generic.List<SingleUnitCard> playerSingleUnitCards;


    public System.Collections.Generic.List<SingleUnitCard> playerDeck;
}