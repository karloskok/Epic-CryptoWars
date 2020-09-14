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
using TMPro;
using UnityEngine;

public class InGameAccountHandle : MonoBehaviour
{
    string networkUrl = "";
    string playerEthereumAccount = "";
    string playerEthereumPrivateKey = "";
    public bool win;


    private LeeContract leeContract;
    private HexBigInteger gasLimit = new HexBigInteger(4712388);

    public PlayerKeysHolder playerKeysHolder;

    public GameObject winPanel;
    public GameObject defeatPanel;
    public TextMeshProUGUI scoreOnWin;
    public TextMeshProUGUI scoreOnDefeat;

    public SingleUnitCard singleUnitCard;

    // Start is called before the first frame update
    private void Start()
    {
        //StartCoroutine(GetAccountBalanceCoroutine());
        playerEthereumPrivateKey = playerKeysHolder.playerEthereumPrivateKey;
        playerEthereumAccount = playerKeysHolder.playerEthereumAccount;
        networkUrl = playerKeysHolder.networkUrl;

        winPanel.SetActive(false);
        defeatPanel.SetActive(false);

        leeContract = new LeeContract();

        //StartCoroutine(GetOutputFunction_getPlayerStats());
    }

    public void BackToMenu()
    {
        PunNetworkManager punNetworkManager = GameObject.FindObjectOfType<PunNetworkManager>();
        punNetworkManager.BackToMenuFromRoom();
    }

    public IEnumerator GetOutputFunction_getPlayerStats()
    {
        var address = playerEthereumAccount;//Nethereum.Signer.EthECKey.GetPublicAddress(playerEthereumPrivateKey);

        var request = new EthCallUnityRequest(networkUrl);
        var callInput = leeContract.CreateCallInput_getPlayerStats(address);

        yield return request.SendRequest(callInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());

        if (request.Exception == null)
        {
            var output = leeContract.DecodeDTO_getPlayerStats(request.Result);

            UnityEngine.Debug.Log("Level" + output.Level);

            //TODO: set on ui coins and score 
            scoreOnWin.text = (output.Level * 10).ToString();
            scoreOnDefeat.text = (output.Level * 2).ToString();
            //score = level * 10

            loaderInGameManager.CloseLoading();

            //defeatPanel.SetActive(true);
            if (win)
            {
                winPanel.SetActive(true);
            }
            else
            {
                defeatPanel.SetActive(true);
            }

            
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

            UnityEngine.Debug.Log("index: " + index + " : " + output.Id + ", " + output.Level + ", " + output.UnitType + ", " + output.RareType + ", " + output.Health + ", " + output.Speed + ", " + output.Attack);

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

            //TODO: init win panel
            //UImanagere
            singleUnitCard.SetUnitValues(singleUnit.unitsId, singleUnit.id, singleUnit.level, singleUnit.rareType, singleUnit.unitType, singleUnit.health, singleUnit.attack, singleUnit.speed, true);

            //get player level
            StartCoroutine(GetOutputFunction_getPlayerStats());
            //set coins + score

            //winPanel.SetActive(true);

        }
        else
        {
            UnityEngine.Debug.Log("RW: Error submitted tx: " + request.Exception.Message);
        }
    }

    public LoaderInGameManager loaderInGameManager;
    public void InGameAfterBattle(bool isWin)
    {
        win = isWin;


        if (isWin)
        {
            StartCoroutine(Set_AfterBattle(true));


            
        }
        else
        {
            StartCoroutine(Set_AfterBattle(false));
            //TODO: open lose panel

            //defeatPanel.SetActive(true);
        }

        loaderInGameManager.OpenLoading();




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


            if (isWin)
            {
                checkTx(transactionSignedRequest.Result, (cb) => {
                    UnityEngine.Debug.Log("Transaction Finished AfterBattle: ");
                });
            }
            else
            {
                StartCoroutine(GetOutputFunction_getPlayerStats());
            }

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
}
