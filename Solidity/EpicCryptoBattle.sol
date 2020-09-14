pragma solidity ^0.5.0;

contract EpicCryptoBattle {
    //Region Events
    event NewUnit(uint unitIndex);
    //End Region
    
    //Region State 
    uint idDigits = 16;
    uint idModulus = 10 ** idDigits;
    uint public accountCount = 0;
    uint public unitCount = 0;
    
    struct Player
    {
        string name;
        uint32 level;
        uint32 wins;
        uint32 defeats;
        uint score;
        uint coins;
    }
    Player[] private players;
    
    mapping (uint => address) private accountToOwner;
    
    mapping (address => uint) public ownerToAccount;

    
    struct Unit
    {
        uint id;
        uint32 level;
        uint unitType;
        uint rareType;
        //skills
        uint32 health;
        uint32 speed;
        uint32 attack;
    }
    Unit[] private units;
    
    mapping (uint => address) public unitToOwner;
    mapping (address => uint) ownerUnitCount;
    
    //End Region
    
    //Region Internal
    function createCharacter(uint  _id, uint _rareType) private returns (uint){
        uint unitType = randUnitId();
        uint id = units.push(Unit(_id, 1, unitType, _rareType, 1, 1, 1)) - 1;

        unitToOwner[id] = msg.sender;
        ownerUnitCount[msg.sender]++;
        unitCount++;
        return id;
    }
    
    function createFirstFiveCharacter() private {
        for(int i = 0; i < 5; i++){
            uint randId = generateRandomId();
            randId = randId - randId % 100;
            createCharacter(randId, 0);
        }
    }
    //End Region
    

    
    //Region Pure
    function pow(uint base, uint32 exponent) private pure returns(uint)
    {
        return base**exponent;
    }
    
    function generateRandomId() private view returns (uint){
        uint rand = uint(keccak256(abi.encodePacked(now, msg.sender, units.length)));
        return rand % idModulus;
    }
    
    function randUnitId() view internal returns(uint) {
        return uint(keccak256(abi.encodePacked(now, msg.sender, units.length))) % 20;
    }
    
    function randRareType() view internal returns(uint) {
        return uint(keccak256(abi.encodePacked(now, msg.sender, units.length))) % 4;
    }
    //End Region
    
    //Region Views
    //End Region
    
    //Region External
    function upgradeUnit(uint unitId) public payable {
        require(msg.value >= 0.0077 ether);
        
        units[unitId].level++;
        units[unitId].health++;
        units[unitId].speed++;
        units[unitId].attack++;
    }
    
    
    function AfterBattle(bool isWin) public{
        Player storage myPlayer = players[ownerToAccount[msg.sender]];
        if(isWin){
            myPlayer.score += 10 * myPlayer.level;
            myPlayer.wins++;
            myPlayer.coins += 10;
            
            //get new character
            uint randId = generateRandomId();
            randId = randId - randId % 100;
            
            uint rareType = randRareType();
            uint unitId = createCharacter(randId, rareType);
            emit NewUnit(unitId);
        }
        else{
            myPlayer.score += 2 * myPlayer.level;
            myPlayer.defeats++;
            myPlayer.coins += 1;
        }
        uint256 levelExpMax = pow(2,myPlayer.level) * 10 * (myPlayer.level);
        if(myPlayer.score >= levelExpMax){
    		if(myPlayer.level < 100)
    			myPlayer.level++;
    	}
    }
    
    
    function buyUnit(uint rareType) public payable returns (uint){
        if(rareType == 0){
            require(msg.value >= 0.0077 ether);
        }
        else if(rareType == 1){
            require(msg.value >= 0.019 ether);
        }
        else if(rareType == 2){
            require(msg.value >= 0.077 ether);
        }
        else if(rareType == 3){
            require(msg.value >= 0.19 ether);
        }
        else{
            rareType = 0;
            require(msg.value >= 0.0077 ether);
        }

        uint randId = generateRandomId();
        randId = randId - randId % 100;
        uint buyId = createCharacter(randId, rareType);
        emit NewUnit(buyId);
        return buyId;
    }
    
    function getUnitStats(uint id) external view returns (uint, uint32, uint, uint, uint32, uint32, uint32){
        Unit storage p = units[id];
        return (p.id, p.level, p.unitType, p.rareType, p.health, p.speed, p.attack);
    }
    
    function unitCountByOwner(address _address) public view returns (uint) {
        uint res = ownerUnitCount[_address];
        return res;
    }
    
    function getAllUnitIdsByOwner(address _owner) external view returns (uint[] memory){
        uint index = 0;
        uint[] memory unitIndexes = new uint[](unitCountByOwner(_owner));
        for(uint i = 0; i < unitCount; i++){
            if(unitToOwner[i] == _owner){
                unitIndexes[index] = i;
                index++;
            }
        }
        return unitIndexes;
    }
    
    
    
    function CreateAccount(string memory _name) public {
        require(!exists(msg.sender));
        
        uint id = players.push(Player(_name, 1, 0, 0, 0, 50)) - 1;
        
        accountToOwner[id] = msg.sender;
        ownerToAccount[msg.sender] = id;
        accountCount++;
        createFirstFiveCharacter();
    }
    
    function exists(address account) public view returns (bool) {
        bool res = false;
        for (uint i = 0; i < accountCount; i++) {
            if( accountToOwner[i] == account) {
                res = true;
                break;
            }
        }
        return res;
    }
    
    function getPlayerStats(address account) external view returns (string memory, uint32, uint32, uint32, uint, uint){
        uint id = ownerToAccount[account];
        Player storage p = players[id];
        return (p.name, p.level, p.wins, p.defeats, p.score, p.coins);
    }
    
    
    function getPlayerHighscore(uint index) external view returns (string memory, uint32, uint32, uint32, uint, uint){
        Player storage p = players[index];
        return (p.name, p.level, p.wins, p.defeats, p.score, p.coins);
    }
    
    
    function getPlayerCount() external view returns (uint){
        return accountCount;
    }
    //End Region
    
    
    //Region owner
    address private _owner;
    constructor() public {
        _owner = msg.sender;
    }
    
    function withdraw() external onlyOwner(msg.sender){
        address(uint160(_owner)).transfer(address(this).balance);
    }
    
    function withdraw_ETH(uint eth_wei) external onlyOwner(msg.sender){
        address(uint160(_owner)).transfer(eth_wei);
    }
    
    function transferOwnership(address new_owner) external onlyOwner(msg.sender){
        _owner = new_owner;
    }
    //End Region
    
    //Region Modifiers
    modifier onlyOwner(address account) {
        require(msg.sender == account);
        _;
    }
    //End Region
    
}