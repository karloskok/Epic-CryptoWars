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

public class Account : MonoBehaviour
{

    public string networkUrl = "http://127.0.0.1:7545";
    public string playerEthereumAccount = "0xa64F5EF8670a4883A62477FB5c31e5FA7a44bE66";
    public string playerEthereumPrivateKey = "dd7393ff01c97314f8ee27bafc3709b0d8d89070e9cf9b5a7a7864ec41ab7908";


    private LeeContract leeContract;
    private HexBigInteger gasLimit = new HexBigInteger(4712388);


    public PlayerKeysHolder playerKeysHolder;


    private void Start()
    {
        //StartCoroutine(GetAccountBalanceCoroutine());


        leeContract = new LeeContract();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    StartCoroutine(GetOutputFunctionGet());

        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    StartCoroutine(GetOutputFunctionGetName());

        //}
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    StartCoroutine(GetOutputFunctionGetArray());

        //}

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    StartCoroutine(SetFunctionTransactionInput());

        //}

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    StartCoroutine(SetNameFunctionTransactionInput());

        //}



        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    StartCoroutine(Get_exists());
        //}
    }


    public bool playerExist = false;
    public IEnumerator Get_exists()
    {
        var address = Nethereum.Signer.EthECKey.GetPublicAddress(playerEthereumPrivateKey);

        var request = new EthCallUnityRequest(networkUrl);
        var callInput = leeContract.CreateCallInput_exists(address);

        yield return request.SendRequest(callInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());

        if (request.Exception == null)
        {
            var output = leeContract.DecodeGetOutput_Function_exists(request.Result);
            UnityEngine.Debug.Log(output);
            playerExist = output;

            if (playerExist)
            {
                //
                UnityEngine.Debug.Log("Get acc data");

                //TODO: open connecting to server and change scene
                LoginManagerHandler loginManagerHandler = GameObject.FindObjectOfType<LoginManagerHandler>();
                //open loadoing
                loginManagerHandler.LoginComplete();

                

            }
            else
            {
                //open create new player panel
                LoginManagerHandler loginManagerHandler = GameObject.FindObjectOfType<LoginManagerHandler>();

                loginManagerHandler.OpenNewAccountPanel();


            }

        }
        else
        {
            UnityEngine.Debug.Log("RW: Error submitted tx: " + request.Exception.Message);
        }
    }


    public void CreatePlayer(string name)
    {
        try
        {
            var address = Nethereum.Signer.EthECKey.GetPublicAddress(playerEthereumPrivateKey);
            // Then we define the accountAdress private variable with the public key
            UnityEngine.Debug.Log(address);
            playerEthereumAccount = address;

            StartCoroutine(Set_CreateAccount(name));
        }
        catch (Exception e)
        {
            // If we catch some error when getting the public address, we just display the exception in the console
            UnityEngine.Debug.Log("Error importing account from PrivateKey: " + e);
        }
    }



    //create new player by (name)
    public IEnumerator Set_CreateAccount(string _name)
    {

        // Create the transaction input with encoded values for the function      
        var transactionInput = leeContract.CreateTransactionInput_CreateAccount(playerEthereumAccount, _name, gasLimit);

        // Create Unity Request with the private key, url and user address       
        var transactionSignedRequest = new TransactionSignedUnityRequest(networkUrl, playerEthereumPrivateKey);

        yield return transactionSignedRequest.SignAndSendTransaction(transactionInput);
        if (transactionSignedRequest.Exception == null)
        {
            UnityEngine.Debug.Log("Transfered tx created: " + transactionSignedRequest.Result);


            //TODO: open loading pannel and load menu
            LoginManagerHandler loginManagerHandler = GameObject.FindObjectOfType<LoginManagerHandler>();
            //open loadoing
            loginManagerHandler.LoginComplete();
        }
        else
        {
            UnityEngine.Debug.Log("Error transfering: " + transactionSignedRequest.Exception.Message);

            //TODO: set account already exists
        }
    }



    public IEnumerator GetAccountBalanceCoroutine()
    {
        var getBalanceRequest = new EthGetBalanceUnityRequest(networkUrl);
        // Send balance request with player's account, asking for balance in latest block
        yield return getBalanceRequest.SendRequest(playerEthereumAccount, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());

        if (getBalanceRequest.Exception == null)
        {
            var balance = getBalanceRequest.Result.Value;
            // Convert the balance from wei to ether and round to 8 decimals for display
            UnityEngine.Debug.Log(Nethereum.Util.UnitConversion.Convert.FromWei(balance, 18).ToString("n8"));

        }
        else
        {
            UnityEngine.Debug.Log("RW: Get Account Balance gave an exception: " + getBalanceRequest.Exception.Message);
        }
    }

    public IEnumerator GetOutputFunctionGet()
    {
        var request = new EthCallUnityRequest(networkUrl);
        var callInput = leeContract.CreateCallInputOnFunctionGet();

        yield return request.SendRequest(callInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());

        if (request.Exception == null)
        {
            var output = leeContract.DecodeGetOutput(request.Result);
            UnityEngine.Debug.Log(output);
        }
        else
        {
            UnityEngine.Debug.Log("RW: Error submitted tx: " + request.Exception.Message);
        }
    }

    //---getName
    public IEnumerator GetOutputFunctionGetName()
    {
        var request = new EthCallUnityRequest(networkUrl);
        var callInput = leeContract.CreateCallInputOnFunctionGetName();

        yield return request.SendRequest(callInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());

        if (request.Exception == null)
        {
            var output = leeContract.DecodeGetOutputName(request.Result);
            UnityEngine.Debug.Log(output);
        }
        else
        {
            UnityEngine.Debug.Log("RW: Error submitted tx: " + request.Exception.Message);
        }
    }

    //------------getArray
    public IEnumerator GetOutputFunctionGetArray()
    {
        var request = new EthCallUnityRequest(networkUrl);
        var callInput = leeContract.CreateCallInputOnFunctionGetArray();

        yield return request.SendRequest(callInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());

        if (request.Exception == null)
        {
            var output = leeContract.DecodeGetArrayDTO(request.Result);

            UnityEngine.Debug.Log(output.First + ", " + output.Second + ", " + output.Third + ", " + output.Fourth);
        }
        else
        {
            UnityEngine.Debug.Log("RW: Error submitted tx: " + request.Exception.Message);
        }
    }

    //setters


    //--------set uint
    public IEnumerator SetFunctionTransactionInput()
    {
        int setValue = 6;

        // Create the transaction input with encoded values for the function      
        var transactionInput = leeContract.CreateSetTransactionInput(playerEthereumAccount, setValue, gasLimit);

        // Create Unity Request with the private key, url and user address       
        var transactionSignedRequest = new TransactionSignedUnityRequest(networkUrl, playerEthereumPrivateKey);

        yield return transactionSignedRequest.SignAndSendTransaction(transactionInput);
        if (transactionSignedRequest.Exception == null)
        {
            UnityEngine.Debug.Log("Transfered tx created: " + transactionSignedRequest.Result);
        }
        else
        {
            UnityEngine.Debug.Log("Error transfering: " + transactionSignedRequest.Exception.Message);
        }
    }

    //--------set string
    public IEnumerator SetNameFunctionTransactionInput()
    {
        string setValue = "Karlo 13";

        // Create the transaction input with encoded values for the function      
        var transactionInput = leeContract.CreateSetNameTransactionInput(playerEthereumAccount, setValue, gasLimit);

        // Create Unity Request with the private key, url and user address       
        var transactionSignedRequest = new TransactionSignedUnityRequest(networkUrl, playerEthereumPrivateKey);

        yield return transactionSignedRequest.SignAndSendTransaction(transactionInput);
        if (transactionSignedRequest.Exception == null)
        {
            UnityEngine.Debug.Log("Transfered tx created: " + transactionSignedRequest.Result);
        }
        else
        {
            UnityEngine.Debug.Log("Error transfering: " + transactionSignedRequest.Exception.Message);
        }
    }



    public void importAccountFromPrivateKey(string _playerEthereumPrivateKey)
    {
        // Here we try to get the public address from the secretKey we defined
        playerEthereumPrivateKey = _playerEthereumPrivateKey;
        


        try
        {
            var address = Nethereum.Signer.EthECKey.GetPublicAddress(playerEthereumPrivateKey);
            // Then we define the accountAdress private variable with the public key
            UnityEngine.Debug.Log(address);
            playerEthereumAccount = address;

            //TODO: set pk and address
            playerKeysHolder.playerEthereumPrivateKey = playerEthereumPrivateKey;
            playerKeysHolder.playerEthereumAccount = playerEthereumAccount;


            StartCoroutine(GetAccountBalanceCoroutine());


            //check if account exist
            StartCoroutine(Get_exists());

        }
        catch (Exception e)
        {
            // If we catch some error when getting the public address, we just display the exception in the console
            UnityEngine.Debug.Log("Error importing account from PrivateKey: " + e);
        }




    }


    public void importAccountFromPrivateKeyRegister(string _playerEthereumPrivateKey)
    {
        // Here we try to get the public address from the secretKey we defined
        playerEthereumPrivateKey = _playerEthereumPrivateKey;

        try
        {
            var address = Nethereum.Signer.EthECKey.GetPublicAddress(playerEthereumPrivateKey);
            // Then we define the accountAdress private variable with the public key
            UnityEngine.Debug.Log(address);
            playerEthereumAccount = address;

            //TODO: set pk and address
            playerKeysHolder.playerEthereumPrivateKey = playerEthereumPrivateKey;
            playerKeysHolder.playerEthereumAccount = playerEthereumAccount;


            StartCoroutine(GetAccountBalanceCoroutine());


            //check if account exist
            //StartCoroutine(Get_exists());

        }
        catch (Exception e)
        {
            // If we catch some error when getting the public address, we just display the exception in the console
            UnityEngine.Debug.Log("Error importing account from PrivateKey: " + e);
        }




    }
}
