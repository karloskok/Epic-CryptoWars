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
using UnityEngine.SceneManagement;

public class BuyUnitsHandler : MonoBehaviour
{
   public MainMenuAccountHandler mainMenuAccountHandler;

    string networkUrl = "";
    string playerEthereumAccount = "";
    string playerEthereumPrivateKey = "";


    private LeeContract leeContract;
    private HexBigInteger gasLimit = new HexBigInteger(4712388);

    public PlayerKeysHolder playerKeysHolder;

    public SingleUnitCard singleUnitCard;
    public GameObject buyUnitPanel;


    private void Start()
    {
        playerEthereumPrivateKey = playerKeysHolder.playerEthereumPrivateKey;
        playerEthereumAccount = playerKeysHolder.playerEthereumAccount;
        networkUrl = playerKeysHolder.networkUrl;



        leeContract = new LeeContract();
    }

    public void BuyUnit(int type)
    {
        StartCoroutine(Set_buyUnit(type));
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

            //int one unit and add to array
            playerKeysHolder.playerSingleUnitCards.Add(singleUnit);

            //TODO open panel and show new unit
            singleUnitCard.SetUnitValues(singleUnit.unitsId, singleUnit.id, singleUnit.level, singleUnit.rareType, singleUnit.unitType, singleUnit.health, singleUnit.attack, singleUnit.speed, true);

            mainMenuAccountHandler.RefreshUIGetAllValues();

            buyUnitPanel.SetActive(true);
            //finction to close panel



        }
        else
        {
            UnityEngine.Debug.Log("RW: Error submitted tx: " + request.Exception.Message);
        }
    }

    public void CloseBuyUnitPanel()
    {

        //buyUnitPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //buyUnitPanel.SetActive(false);
    }



}
