using NBitcoin.BouncyCastle.Math;
using System.Collections;
using System.Collections.Generic;

public class SingleAccountData
{
    public string Name;
    public System.Numerics.BigInteger Level;
    public System.Numerics.BigInteger Wins;
    public System.Numerics.BigInteger Defeats;
    public System.Numerics.BigInteger Score;
    public System.Numerics.BigInteger Coins;

    public SingleAccountData(string name, System.Numerics.BigInteger level, System.Numerics.BigInteger wins, System.Numerics.BigInteger defeats, System.Numerics.BigInteger score, System.Numerics.BigInteger coins)
    {
        Name = name;
        Level = level;
        Wins = wins;
        Defeats = defeats;
        Score = score;
        Coins = coins;
    }

    
}


public class UnitType
{
    public static string ReturnUnitTypeName(int type)
    {
        string name = "Light_Peasant";
        switch (type)
        {
            case 0: name = "Light_Peasant";
                break;
            case 1:
                name = "Light_Spearman";
                break;
            case 2:
                name = "Light_Swordsman";
                break;
            case 3:
                name = "Light_Infantry";
                break;

            case 4:
                name = "Heavy_Infantry";
                break;
            case 5:
                name = "Heavy_Halberdier";
                break;
            case 6:
                name = "Heavy_Commander";
                break;
            case 7:
                name = "Heavy_Swordman";
                break;
            case 8:
                name = "Heavy_King";
                break;
            case 9:
                name = "Heavy_Paladin";
                break;

            case 10:
                name = "Archer_Archer";
                break;
            case 11:
                name = "Archer_Crossbowman";
                break;
            case 12:
                name = "Archer_Priest";
                break;
            case 13:
                name = "Archer_HighPriest";
                break;
            case 14:
                name = "Archer_Mage";
                break;

            case 15:
                name = "Mount_Light_Cavalry";
                break;
            case 16:
                name = "Mount_Heavy_Cavalry";
                break;
            case 17:
                name = "Mount_Knight";
                break;
            case 18:
                name = "Mount_King";
                break;
            case 19:
                name = "Mount_Paladin";
                break;
        }

        return name;
    }
}

