using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using UnityEngine;
public class MainMenuAccountHandler : MonoBehaviour
{
    string networkUrl = "";
    string playerEthereumAccount = "";
    string playerEthereumPrivateKey = "";


    private LeeContract leeContract;
    private HexBigInteger gasLimit = new HexBigInteger(4712388);

    public PlayerKeysHolder playerKeysHolder;

    SingleAccountData singleAccountData;
    public HighscoreHolder highscoreHolder;


    public GameObject quitGamePanel;

    public void QuitGame(bool quit)
    {
        if (quit)
        {
            UnityEngine.Debug.Log("Exit game");
            Application.Quit();

            //open apnel see you next time
        }
        else
        {
            quitGamePanel.SetActive(false);
        }
    }

    public void OpenQuitPanel()
    {
        quitGamePanel.SetActive(true);
    }


    public struct SingleAccount
    {
        public string name;
        public int level;
        public int score;
        public int wins;
        public int defeats;
        public int coins;
    };

    public List<SingleAccount> accountsHScore;

    public LoaderInGameManager loaderInGameManager;
    private void Start()
    {
        //StartCoroutine(GetAccountBalanceCoroutine());
        playerEthereumPrivateKey = playerKeysHolder.playerEthereumPrivateKey;
        playerEthereumAccount = playerKeysHolder.playerEthereumAccount;
        networkUrl = playerKeysHolder.networkUrl;



        leeContract = new LeeContract();

        loaderInGameManager.OpenLoading();

        StartCoroutine(GetOutputFunction_getPlayerStats());
        StartCoroutine(Get_getPlayerCount());
    }

    public void RefreshUIGetAllValues()
    {
        StartCoroutine(GetOutputFunction_getPlayerStats());
    }


    //------------getPlayerStats
    public IEnumerator GetOutputFunction_getPlayerStats()
    {
        var address = playerEthereumAccount;//Nethereum.Signer.EthECKey.GetPublicAddress(playerEthereumPrivateKey);

        var request = new EthCallUnityRequest(networkUrl);
        var callInput = leeContract.CreateCallInput_getPlayerStats(address);

        yield return request.SendRequest(callInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());

        if (request.Exception == null)
        {
            var output = leeContract.DecodeDTO_getPlayerStats(request.Result);

            UnityEngine.Debug.Log(output.Name + ", " + output.Level+ ", " + output.Wins+ ", " + output.Defeats + ", " + output.Score + ", " + output.Coins);

            //set player data playerKeysHolder

            singleAccountData = new SingleAccountData(output.Name, output.Level, output.Wins, output.Defeats, output.Score, output.Coins);

            playerKeysHolder.Name = singleAccountData.Name;
            playerKeysHolder.Level = singleAccountData.Level;
            playerKeysHolder.Wins = singleAccountData.Wins;
            playerKeysHolder.Defeats = singleAccountData.Defeats;
            playerKeysHolder.Score = singleAccountData.Score;
            playerKeysHolder.Coins = singleAccountData.Coins;


            TopPanelDeck topPanelDeck = GameObject.FindObjectOfType<TopPanelDeck>();
            topPanelDeck.nickname.text = playerKeysHolder.Name;
            topPanelDeck.level.text =  "Level " + playerKeysHolder.Level.ToString();
            string winDef = playerKeysHolder.Wins + "/" + playerKeysHolder.Defeats;
            topPanelDeck.winLose.text = winDef;
            topPanelDeck.score.text = playerKeysHolder.Score.ToString();
            topPanelDeck.coins.text = playerKeysHolder.Coins.ToString();




            StartCoroutine(Get_unitCountByOwner());
        }
        else
        {
            UnityEngine.Debug.Log("RW: Error submitted tx: " + request.Exception.Message);
        }
    }

    //------------getPlayerCount
    public IEnumerator Get_getPlayerCount()
    {

        accountsHScore = new List<SingleAccount>();

        var address = Nethereum.Signer.EthECKey.GetPublicAddress(playerEthereumPrivateKey);

        var request = new EthCallUnityRequest(networkUrl);
        var callInput = leeContract.CreateCallInput_getPlayerCount();

        yield return request.SendRequest(callInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());

        if (request.Exception == null)
        {
            var output = leeContract.DecodeGetOutput_Function_getPlayerCounts(request.Result);
            UnityEngine.Debug.Log("Players count: " + output);

            playerKeysHolder.unitCount = output;
            highscoreHolder.InitRows();

            for (int i = 0; i < output; i++)
            {
                yield return StartCoroutine(Get_getPlayerHighscore(i));
            }

            yield return new WaitForSeconds(2f);

            //TODO init panels
             

            //StartCoroutine(Get_getAllUnitIdsByOwner());
        }
        else
        {
            UnityEngine.Debug.Log("RW: Error submitted tx: " + request.Exception.Message);
        }
    }
    

    //------------getPlayerHighscore
    public IEnumerator Get_getPlayerHighscore(int index)
    {
        //var address = playerEthereumAccount;//Nethereum.Signer.EthECKey.GetPublicAddress(playerEthereumPrivateKey);

        var request = new EthCallUnityRequest(networkUrl);
        var callInput = leeContract.CreateCallInput_getPlayerHighscore(index);

        yield return request.SendRequest(callInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());

        if (request.Exception == null)
        {
            var output = leeContract.DecodeDTO_getPlayerHighscore(request.Result);

            UnityEngine.Debug.Log("HScore i: " + index + "  : " + output.Name + ", " + output.Level + ", " + output.Wins + ", " + output.Defeats + ", " + output.Score + ", " + output.Coins);
            //TODO: init row hscore
            SingleAccount singleAccount = new SingleAccount();
            singleAccount.name = output.Name;
            singleAccount.level = (int)output.Level;
            singleAccount.wins = (int)output.Wins;
            singleAccount.defeats = (int)output.Defeats;
            singleAccount.score = (int)output.Score;
            singleAccount.coins = (int)output.Coins;
            accountsHScore.Add(singleAccount);

            highscoreHolder.AddToRow(output.Name, (int)output.Level, (int)output.Score, (int)output.Wins, (int)output.Defeats, (int)output.Coins);
        }
        else
        {
            UnityEngine.Debug.Log("RW: Error submitted tx: " + request.Exception.Message);
        }
    }




    //------------unitCountByOwner
    public IEnumerator Get_unitCountByOwner()
    {
        var address = Nethereum.Signer.EthECKey.GetPublicAddress(playerEthereumPrivateKey);

        var request = new EthCallUnityRequest(networkUrl);
        var callInput = leeContract.CreateCallInput_unitCountByOwner(address);

        yield return request.SendRequest(callInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());

        if (request.Exception == null)
        {
            var output = leeContract.DecodeGetOutput_Function_unitCountByOwner(request.Result);
            UnityEngine.Debug.Log("Unit count: " + output);

            playerKeysHolder.unitCount = output;

            StartCoroutine(Get_getAllUnitIdsByOwner());
        }
        else
        {
            UnityEngine.Debug.Log("RW: Error submitted tx: " + request.Exception.Message);
        }
    }
    
    //------------getAllUnitIdsByOwner
    public IEnumerator Get_getAllUnitIdsByOwner()
    {
        var address = Nethereum.Signer.EthECKey.GetPublicAddress(playerEthereumPrivateKey);

        var request = new EthCallUnityRequest(networkUrl);
        var callInput = leeContract.CreateCallInput_getAllUnitIdsByOwner(address);

        yield return request.SendRequest(callInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());

        if (request.Exception == null)
        {
            var output = leeContract.DecodeDTO_getAllUnitIdsByOwner(request.Result);
            
            string ids = String.Join(", ", output.Ids);
            UnityEngine.Debug.Log("Unit ids: " + ids);


            //set array to new
            playerKeysHolder.playerSingleUnitCards = new List<SingleUnitCard>(playerKeysHolder.unitCount);

            foreach (int i in output.Ids)
            {
                //UnityEngine.Debug.Log(i);
                StartCoroutine(GetOutputFunction_getUnitStats(i));
            }

            yield return new WaitForSeconds(5f);
            loaderInGameManager.CloseLoading();

            //TODO: fix this loading data - errors tbd  - await
            UnitsHolderScript unitsHolderScript = GameObject.FindObjectOfType<UnitsHolderScript>();
            unitsHolderScript.InitUnitPanels(playerKeysHolder.unitCount, playerKeysHolder.playerSingleUnitCards);
        }
        else
        {
            UnityEngine.Debug.Log("RW: Error submitted tx: " + request.Exception.Message);
        }
    }

    //------------getUnitStats
    public IEnumerator GetOutputFunction_getUnitStats(int index)
    {
        //var address = playerEthereumAccount;//Nethereum.Signer.EthECKey.GetPublicAddress(playerEthereumPrivateKey);

        var request = new EthCallUnityRequest(networkUrl);
        var callInput = leeContract.CreateCallInput_getUnitStats(index);

        yield return request.SendRequest(callInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());

        if (request.Exception == null)
        {
            var output = leeContract.DecodeDTO_getUnitStats(request.Result);

            UnityEngine.Debug.Log("index: " + index +" : " + output.Id + ", " + output.Level + ", " + output.UnitType + ", " + output.RareType + ", " + output.Health + ", " + output.Speed + ", " + output.Attack);

            //TODO: init single unit
            ////UnitsHolderScript unitsHolderScript= GameObject.FindObjectOfType<UnitsHolderScript>();
            //open loadoing

            ///unitsHolderScript.GetUnitValuesFromEth(index, output.Id.ToString(), (int)output.Level, (int)output.UnitType, (int)output.RareType, (int)output.Health, (int)output.Speed, (int)output.Attack);
            ///

            SingleUnitCard singleUnit = new SingleUnitCard();
            singleUnit.unitsId = index;
            singleUnit.id = output.Id.ToString();
            singleUnit.level = (int)output.Level;
            singleUnit.unitType = (int)output.UnitType;
            singleUnit.rareType = (int)output.RareType;
            singleUnit.health = (int)output.Health;
            singleUnit.speed = (int)output.Speed;
            singleUnit.attack = (int)output.Attack;

            //int one unit and add to array
            playerKeysHolder.playerSingleUnitCards.Add(singleUnit);




        }
        else
        {
            UnityEngine.Debug.Log("RW: Error submitted tx: " + request.Exception.Message);
        }
    }

    //-----------buyUnit ()
    public IEnumerator Set_buyUnit(int chestType)
    {
        HexBigInteger price = new HexBigInteger(7700000000000000);
        switch (chestType)
        {
            case 1: 
                price = new HexBigInteger(19000000000000000);
                break;
            case 2:
                price = new HexBigInteger(77000000000000000);
                break;
            case 3:
                price = new HexBigInteger(190000000000000000);
                break;


        }

        UnityEngine.Debug.Log("test");
        // Create the transaction input with encoded values for the function      
        var transactionInput = leeContract.CreateTransactionInput_buyUnit(playerEthereumAccount, chestType, null, null, price);

        // Create Unity Request with the private key, url and user address       
        var transactionSignedRequest = new TransactionSignedUnityRequest(networkUrl, playerEthereumPrivateKey);

        yield return transactionSignedRequest.SignAndSendTransaction(transactionInput);
        if (transactionSignedRequest.Exception == null)
        {
            UnityEngine.Debug.Log("Transfered tx created: " + transactionSignedRequest.Result);

            checkTx(transactionSignedRequest.Result, (cb) => {
                UnityEngine.Debug.Log("Transaction Finished: "); 
            });

        }
        else
        {
            UnityEngine.Debug.Log("Error transfering: " + transactionSignedRequest.Exception.Message);
        }
    }

    //----------AfterBattle (bool isWin)
    public IEnumerator Set_AfterBattle(bool isWin)
    {

        // Create the transaction input with encoded values for the function      
        var transactionInput = leeContract.CreateTransactionInput_AfterBattle(playerEthereumAccount, isWin, gasLimit);

        // Create Unity Request with the private key, url and user address       
        var transactionSignedRequest = new TransactionSignedUnityRequest(networkUrl, playerEthereumPrivateKey);

        yield return transactionSignedRequest.SignAndSendTransaction(transactionInput);
        if (transactionSignedRequest.Exception == null)
        {
            UnityEngine.Debug.Log("Transfered tx created: " + transactionSignedRequest.Result);


            //TODO: if isWin
            if (isWin)
            {
                checkTx(transactionSignedRequest.Result, (cb) => {
                    UnityEngine.Debug.Log("Transaction Finished AfterBattle: ");
                });
            }

        }
        else
        {
            UnityEngine.Debug.Log("Error transfering: " + transactionSignedRequest.Exception.Message);
        }
    }


    //-----------upgradeUnit (uint unitId)
    public IEnumerator Set_upgradeUnit(int unitId)
    {
        HexBigInteger price = new HexBigInteger(7700000000000000);
        
        // Create the transaction input with encoded values for the function      
        var transactionInput = leeContract.CreateTransactionInput_upgradeUnit(playerEthereumAccount, unitId, null, null, price);

        // Create Unity Request with the private key, url and user address       
        var transactionSignedRequest = new TransactionSignedUnityRequest(networkUrl, playerEthereumPrivateKey);

        yield return transactionSignedRequest.SignAndSendTransaction(transactionInput);
        if (transactionSignedRequest.Exception == null)
        {
            UnityEngine.Debug.Log("Transfered tx created: " + transactionSignedRequest.Result);
            
            //TODO: show unit upgrade panel

        }
        else
        {
            UnityEngine.Debug.Log("Error transfering: " + transactionSignedRequest.Exception.Message);
        }
    }




    public void checkTx(string txHash, Action<bool> callback) 
    {
        StartCoroutine(CheckIsMined(
            networkUrl,
            txHash,
            (cb) => {
                //UnityEngine.Debug.Log("Transaction Last Step: ");
                callback(true);
            }
        ));
    }

    public IEnumerator CheckIsMined(string url, string txHash, System.Action<bool> callback)
    {
        var mined = false;
        var tries = 3600;
        while (!mined)
        {
            if (tries > 0)
            {
                tries = tries - 1;
            }
            else
            {
                mined = true;
                UnityEngine.Debug.Log("Performing last try..");
            }
            UnityEngine.Debug.Log("Checking receipt for: " + txHash);

            var receiptRequest = new EthGetTransactionReceiptUnityRequest(url);
            yield return receiptRequest.SendRequest(txHash);
            if (receiptRequest.Exception == null)
            {
                if (receiptRequest.Result != null)
                {

                    string txLogs = receiptRequest.Result.Logs[0]["data"].ToString();
                    var txLogsHex = txLogs.RemoveHexPrefix();
                    UnityEngine.Debug.Log("Hash:" + txLogsHex);

                    var result = int.Parse(txLogsHex, System.Globalization.NumberStyles.AllowHexSpecifier);
                    UnityEngine.Debug.Log("Unit Index: " + result);

                    //TODO: open unit panel and show new unit

                    //get unit stats
                    StartCoroutine(GetOutputFunction_getUnitStats(result));


                    var txType = "mined";
                    if (txType == "mined")
                    {
                        mined = true;
                        callback(mined);
                    }
                    else
                    {
                        mined = false;
                        callback(mined);
                    }
                }
            }
            else
            {
                UnityEngine.Debug.Log("Error checking receipt: " + receiptRequest.Exception.Message);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }


    public int chestType = 0;
    public bool isWin = true;

    public void BuyUnitOfType(int type)
    {
        StartCoroutine(Set_buyUnit(type));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(Set_buyUnit(chestType));
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartCoroutine(Set_AfterBattle(isWin));
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(Set_upgradeUnit(chestType));
        }
        
        if (Input.GetKeyDown(KeyCode.J))
        {
            chestType--;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            chestType++;
            isWin = isWin ? false : true;
        }


    }


}
