using Nethereum.ABI.Encoders;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using System.Collections.Generic;
using System.Numerics;

public class LeeContract
{
    public static string contractABI = @"[
	{
		""constant"": false,
		""inputs"": [
			{
				""internalType"": ""bool"",
				""name"": ""isWin"",
				""type"": ""bool""
			}
		],
		""name"": ""AfterBattle"",
		""outputs"": [],
		""payable"": false,
		""stateMutability"": ""nonpayable"",
		""type"": ""function""
	},
	{
		""constant"": false,
		""inputs"": [
			{
				""internalType"": ""uint256"",
				""name"": ""rareType"",
				""type"": ""uint256""
			}
		],
		""name"": ""buyUnit"",
		""outputs"": [
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			}
		],
		""payable"": true,
		""stateMutability"": ""payable"",
		""type"": ""function""
	},
	{
		""constant"": false,
		""inputs"": [
			{
				""internalType"": ""string"",
				""name"": ""_name"",
				""type"": ""string""
			}
		],
		""name"": ""CreateAccount"",
		""outputs"": [],
		""payable"": false,
		""stateMutability"": ""nonpayable"",
		""type"": ""function""
	},
	{
		""constant"": false,
		""inputs"": [
			{
				""internalType"": ""address"",
				""name"": ""new_owner"",
				""type"": ""address""
			}
		],
		""name"": ""transferOwnership"",
		""outputs"": [],
		""payable"": false,
		""stateMutability"": ""nonpayable"",
		""type"": ""function""
	},
	{
		""inputs"": [],
		""payable"": false,
		""stateMutability"": ""nonpayable"",
		""type"": ""constructor""
	},
	{
		""anonymous"": false,
		""inputs"": [
			{
				""indexed"": false,
				""internalType"": ""uint256"",
				""name"": ""unitIndex"",
				""type"": ""uint256""
			}
		],
		""name"": ""NewUnit"",
		""type"": ""event""
	},
	{
		""constant"": false,
		""inputs"": [
			{
				""internalType"": ""uint256"",
				""name"": ""unitId"",
				""type"": ""uint256""
			}
		],
		""name"": ""upgradeUnit"",
		""outputs"": [],
		""payable"": true,
		""stateMutability"": ""payable"",
		""type"": ""function""
	},
	{
		""constant"": false,
		""inputs"": [],
		""name"": ""withdraw"",
		""outputs"": [],
		""payable"": false,
		""stateMutability"": ""nonpayable"",
		""type"": ""function""
	},
	{
		""constant"": false,
		""inputs"": [
			{
				""internalType"": ""uint256"",
				""name"": ""eth_wei"",
				""type"": ""uint256""
			}
		],
		""name"": ""withdraw_ETH"",
		""outputs"": [],
		""payable"": false,
		""stateMutability"": ""nonpayable"",
		""type"": ""function""
	},
	{
		""constant"": true,
		""inputs"": [],
		""name"": ""accountCount"",
		""outputs"": [
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			}
		],
		""payable"": false,
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""constant"": true,
		""inputs"": [
			{
				""internalType"": ""address"",
				""name"": ""account"",
				""type"": ""address""
			}
		],
		""name"": ""exists"",
		""outputs"": [
			{
				""internalType"": ""bool"",
				""name"": """",
				""type"": ""bool""
			}
		],
		""payable"": false,
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""constant"": true,
		""inputs"": [
			{
				""internalType"": ""address"",
				""name"": ""_owner"",
				""type"": ""address""
			}
		],
		""name"": ""getAllUnitIdsByOwner"",
		""outputs"": [
			{
				""internalType"": ""uint256[]"",
				""name"": """",
				""type"": ""uint256[]""
			}
		],
		""payable"": false,
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""constant"": true,
		""inputs"": [],
		""name"": ""getPlayerCount"",
		""outputs"": [
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			}
		],
		""payable"": false,
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""constant"": true,
		""inputs"": [
			{
				""internalType"": ""uint256"",
				""name"": ""index"",
				""type"": ""uint256""
			}
		],
		""name"": ""getPlayerHighscore"",
		""outputs"": [
			{
				""internalType"": ""string"",
				""name"": """",
				""type"": ""string""
			},
			{
				""internalType"": ""uint32"",
				""name"": """",
				""type"": ""uint32""
			},
			{
				""internalType"": ""uint32"",
				""name"": """",
				""type"": ""uint32""
			},
			{
				""internalType"": ""uint32"",
				""name"": """",
				""type"": ""uint32""
			},
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			},
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			}
		],
		""payable"": false,
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""constant"": true,
		""inputs"": [
			{
				""internalType"": ""address"",
				""name"": ""account"",
				""type"": ""address""
			}
		],
		""name"": ""getPlayerStats"",
		""outputs"": [
			{
				""internalType"": ""string"",
				""name"": """",
				""type"": ""string""
			},
			{
				""internalType"": ""uint32"",
				""name"": """",
				""type"": ""uint32""
			},
			{
				""internalType"": ""uint32"",
				""name"": """",
				""type"": ""uint32""
			},
			{
				""internalType"": ""uint32"",
				""name"": """",
				""type"": ""uint32""
			},
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			},
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			}
		],
		""payable"": false,
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""constant"": true,
		""inputs"": [
			{
				""internalType"": ""uint256"",
				""name"": ""id"",
				""type"": ""uint256""
			}
		],
		""name"": ""getUnitStats"",
		""outputs"": [
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			},
			{
				""internalType"": ""uint32"",
				""name"": """",
				""type"": ""uint32""
			},
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			},
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			},
			{
				""internalType"": ""uint32"",
				""name"": """",
				""type"": ""uint32""
			},
			{
				""internalType"": ""uint32"",
				""name"": """",
				""type"": ""uint32""
			},
			{
				""internalType"": ""uint32"",
				""name"": """",
				""type"": ""uint32""
			}
		],
		""payable"": false,
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""constant"": true,
		""inputs"": [
			{
				""internalType"": ""address"",
				""name"": """",
				""type"": ""address""
			}
		],
		""name"": ""ownerToAccount"",
		""outputs"": [
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			}
		],
		""payable"": false,
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""constant"": true,
		""inputs"": [],
		""name"": ""unitCount"",
		""outputs"": [
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			}
		],
		""payable"": false,
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""constant"": true,
		""inputs"": [
			{
				""internalType"": ""address"",
				""name"": ""_address"",
				""type"": ""address""
			}
		],
		""name"": ""unitCountByOwner"",
		""outputs"": [
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			}
		],
		""payable"": false,
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""constant"": true,
		""inputs"": [
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			}
		],
		""name"": ""unitToOwner"",
		""outputs"": [
			{
				""internalType"": ""address"",
				""name"": """",
				""type"": ""address""
			}
		],
		""payable"": false,
		""stateMutability"": ""view"",
		""type"": ""function""
	}
]";

    private static string contractAddress = "0x95bF49D6E5107E0dCCC8B6d693ABaF1c8271A208";
    private Contract contract;


    public LeeContract()
    {
        this.contract = new Contract(null, contractABI, contractAddress);
    }
	//calls

	//---------call function exists () returns bool
	public Function Function_exists()
	{
		return contract.GetFunction("exists");
	}
	public CallInput CreateCallInput_exists(
		string addressFrom
		)
	{
		var function = Function_exists();
		return function.CreateCallInput(addressFrom);
	}
	public bool DecodeGetOutput_Function_exists(string result)
	{
		var function = Function_exists();
		return function.DecodeSimpleTypeOutput<bool>(result);
	}

	//------getPlayerStats (account)
	public Function Function_getPlayerStats()
	{
		return contract.GetFunction("getPlayerStats");
	}
	public CallInput CreateCallInput_getPlayerStats(
		string addressFrom
		)
	{
		var function = Function_getPlayerStats();
		return function.CreateCallInput(addressFrom);
	}
	public OutputDecoder_getPlayerStats DecodeDTO_getPlayerStats(string result)
	{
		var function = Function_getPlayerStats();
		return function.DecodeDTOTypeOutput<OutputDecoder_getPlayerStats>(result);
	}

	//---------call function unitCountByOwner (address) returns uint
	public Function Function_unitCountByOwner()
	{
		return contract.GetFunction("unitCountByOwner");
	}
	public CallInput CreateCallInput_unitCountByOwner(
		string addressFrom
		)
	{
		var function = Function_unitCountByOwner();
		return function.CreateCallInput(addressFrom);
	}
	public int DecodeGetOutput_Function_unitCountByOwner(string result)
	{
		var function = Function_unitCountByOwner();
		return function.DecodeSimpleTypeOutput<int>(result);
	}

	//---------call function getAllUnitIdsByOwner (address) returns uint[]
	public Function Function_getAllUnitIdsByOwner()
	{
		return contract.GetFunction("getAllUnitIdsByOwner");
	}
	public CallInput CreateCallInput_getAllUnitIdsByOwner(
		string addressFrom
		)
	{
		var function = Function_getAllUnitIdsByOwner();
		return function.CreateCallInput(addressFrom);
	}
	public OutputDecoder_getAllUnitIdsByOwner DecodeDTO_getAllUnitIdsByOwner(string result)
	{
		var function = Function_getAllUnitIdsByOwner();
		return function.DecodeDTOTypeOutput<OutputDecoder_getAllUnitIdsByOwner>(result);
	}

	//---------call function getUnitStats (uint) returns (uint, uint32, uint, uint, uint32, uint32, uint32)
	public Function Function_getUnitStats()
	{
		return contract.GetFunction("getUnitStats");
	}
	public CallInput CreateCallInput_getUnitStats(
		int index
		)
	{
		var function = Function_getUnitStats();
		return function.CreateCallInput(index);
	}
	public OutputDecoder_getUnitStats DecodeDTO_getUnitStats(string result)
	{
		var function = Function_getUnitStats();
		return function.DecodeDTOTypeOutput<OutputDecoder_getUnitStats>(result);
	}

	//---------call function getPlayerCount () returns bool
	public Function Function_getPlayerCount()
	{
		return contract.GetFunction("getPlayerCount");
	}
	public CallInput CreateCallInput_getPlayerCount()
	{
		var function = Function_getPlayerCount();
		return function.CreateCallInput();
	}
	public int DecodeGetOutput_Function_getPlayerCounts(string result)
	{
		var function = Function_getPlayerCount();
		return function.DecodeSimpleTypeOutput<int>(result);
	}

	//------getPlayerHighscore (i)
	public Function Function_getPlayerHighscore()
	{
		return contract.GetFunction("getPlayerHighscore");
	}
	public CallInput CreateCallInput_getPlayerHighscore(
		int index
		)
	{
		var function = Function_getPlayerHighscore();
		return function.CreateCallInput(index);
	}
	public OutputDecoder_getPlayerStats DecodeDTO_getPlayerHighscore(string result)
	{
		var function = Function_getPlayerHighscore();
		return function.DecodeDTOTypeOutput<OutputDecoder_getPlayerStats>(result);
	}








	//---------transaction function CreateAccount(name)
	public Function Function_CreateAccount()
	{
		return contract.GetFunction("CreateAccount");
	}
	public TransactionInput CreateTransactionInput_CreateAccount(
		string addressFrom,
		string name,
		HexBigInteger gas = null,
		HexBigInteger valueAmount = null)
	{

		var function = Function_CreateAccount();
		return function.CreateTransactionInput(addressFrom, gas, valueAmount, name);
	}

	//---------transaction function buyUnit(uint) payable 
	public Function Function_buyUnit()
	{
		return contract.GetFunction("buyUnit");
	}
	public TransactionInput CreateTransactionInput_buyUnit(
		string addressFrom,
		int chestType,
		HexBigInteger gas = null,
		HexBigInteger gasPrice = null,
		HexBigInteger valueAmount = null)
	{

		var function = Function_buyUnit();
		return function.CreateTransactionInput(addressFrom, gas, gasPrice, valueAmount, chestType);
	}

	//---------transaction function AfterBattle(bool isWin)
	public Function Function_AfterBattle()
	{
		return contract.GetFunction("AfterBattle");
	}
	public TransactionInput CreateTransactionInput_AfterBattle(
		string addressFrom,
		bool isWin,
		HexBigInteger gas = null,
		HexBigInteger valueAmount = null)
	{

		var function = Function_AfterBattle();
		return function.CreateTransactionInput(addressFrom, gas, valueAmount, isWin);
	}


	//---------transaction function upgradeUnit(uint unitId) payable 
	public Function Function_upgradeUnit()
	{
		return contract.GetFunction("upgradeUnit");
	}
	public TransactionInput CreateTransactionInput_upgradeUnit(
		string addressFrom,
		int unitId,
		HexBigInteger gas = null,
		HexBigInteger gasPrice = null,
		HexBigInteger valueAmount = null)
	{

		var function = Function_upgradeUnit();
		return function.CreateTransactionInput(addressFrom, gas, gasPrice, valueAmount, unitId);
	}












	//----------get (uint)
	public Function FunctionGet()
	{
		return contract.GetFunction("get");
	}
	public CallInput CreateCallInputOnFunctionGet()
	{
		var function = FunctionGet();
		return function.CreateCallInput();
	}
	public int DecodeGetOutput(string result)
	{
		var function = FunctionGet();
		return function.DecodeSimpleTypeOutput<int>(result);
	}

	//---------get (string)
	public Function FunctionGetName()
	{
		return contract.GetFunction("getName");
	}
	public CallInput CreateCallInputOnFunctionGetName()
	{
		var function = FunctionGetName();
		return function.CreateCallInput();
	}
	public string DecodeGetOutputName(string result)
	{
		var function = FunctionGetName();
		return function.DecodeSimpleTypeOutput<string>(result);
	}


	//------getArray (string, uint, uint32, uint)
	public Function FunctionGetArray()
	{
		return contract.GetFunction("getArray");
	}
	public CallInput CreateCallInputOnFunctionGetArray()
	{
		var function = FunctionGetArray();
		return function.CreateCallInput();
	}
	public GetArrayOutputDecoder DecodeGetArrayDTO(string result)
	{
		var function = FunctionGetArray();
		return function.DecodeDTOTypeOutput<GetArrayOutputDecoder>(result);
	}



	//--------setFunctions

	//---------set
	public Function FunctionSet()
	{
		return contract.GetFunction("set");
	}
	public TransactionInput CreateSetTransactionInput(
		string addressFrom,
		BigInteger value,
		HexBigInteger gas = null,
		HexBigInteger valueAmount = null)
	{

		var function = FunctionSet();
		return function.CreateTransactionInput(addressFrom, gas, valueAmount, value);
	}

	//---------setName
	public Function FunctionSetName()
	{
		return contract.GetFunction("setName");
	}
	public TransactionInput CreateSetNameTransactionInput(
		string addressFrom,
		string name,
		HexBigInteger gas = null,
		HexBigInteger valueAmount = null)
	{

		var function = FunctionSetName();
		return function.CreateTransactionInput(addressFrom, gas, valueAmount, name);
	}


}

[FunctionOutput]
public class OutputDecoder_getPlayerStats
{
	[Parameter("string", "", 1)]
	public string Name { get; set; }

	[Parameter("uint32", "", 2)]
	public BigInteger Level { get; set; }

	[Parameter("uint32", "", 2)]
	public BigInteger Wins { get; set; }

	[Parameter("uint32", "", 2)]
	public BigInteger Defeats { get; set; }
	[Parameter("uint256", "", 2)]
	public BigInteger Score { get; set; }
	[Parameter("uint256", "", 2)]
	public BigInteger Coins { get; set; }
}



[FunctionOutput]
public class OutputDecoder_getUnitStats
{
	[Parameter("uint256", "", 1)]
	public BigInteger Id { get; set; }

	[Parameter("uint32", "", 2)]
	public BigInteger Level { get; set; }

	[Parameter("uint256", "", 2)]
	public BigInteger UnitType { get; set; }

	[Parameter("uint256", "", 2)]
	public BigInteger RareType { get; set; }
	
	[Parameter("uint32", "", 2)]
	public BigInteger Health { get; set; }

	[Parameter("uint32", "", 2)]
	public BigInteger Speed { get; set; }

	[Parameter("uint32", "", 2)]
	public BigInteger Attack { get; set; }
}


[FunctionOutput]
public class OutputDecoder_getAllUnitIdsByOwner
{
	[Parameter("uint256[]", "", 1)]
	public List<BigInteger> Ids { get; set; }
}




	[FunctionOutput]
public class GetArrayOutputDecoder
{
    [Parameter("string", "", 1)]
    public string First { get; set; }

    [Parameter("uint256", "", 2)]
    public BigInteger Second { get; set; }

	[Parameter("uint32", "", 2)]
	public BigInteger Third { get; set; }

	[Parameter("uint256", "", 2)]
	public BigInteger Fourth { get; set; }
}


[Event("Transfer")]
public class TransferEventDTOBase : IEventDTO
{
	[Parameter("address", "_from", 1, true)]
	public virtual string From { get; set; }

	[Parameter("address", "_to", 2, true)]
	public virtual string To { get; set; }

	[Parameter("uint256", "_value", 3, false)]
	public virtual BigInteger Value { get; set; }
}

public partial class TransferEventDTO : TransferEventDTOBase
{
	public static EventABI GetEventABI()
	{
		return EventExtensions.GetEventABI<TransferEventDTO>();
	}
}