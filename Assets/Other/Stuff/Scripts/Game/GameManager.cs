using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool debug = false;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private Arena arena;
    [SerializeField] public HeroManager[] heroesPrefabs;
    //[SerializeField] private HeroManager[] AllHeroesPrefabs;
    //private int AllHeroesLastIndex = 0;

    [SerializeField] private Group[] groups;
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Material[] materials;
    [SerializeField] private Material[] materialsUnit;
    public long GameTime { get; private set; }
    private float secondsPass;
    private float myGameTime;
    private float otherGameTime;
    private List<SimpleVector2> dynamicPositions;
    private List<SimpleVector2> staticPositions;
    public bool IsMasterPlayer { get; private set; }
    public int GroupIndex { get; private set; }
    public Group MyGroup { get; private set; }
    public Group OtherGroup { get; private set; }
    public bool IsPaused { get; private set; }
    private bool gameIsEnd;

    //init player units
    public List<string> playerHeroes;


    private void Awake()
    {
        if (!debug)
        {

            if (!PunNetworkManager.NetworkManager)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("LoginScene");
                enabled = false;
                return;
            }

            IsMasterPlayer = PunNetworkManager.NetworkManager.IsMaster;
            GroupIndex = PunNetworkManager.NetworkManager.PlayerIndex;

        }
        else
        {
            IsMasterPlayer = true;
            GroupIndex = 0; 
        }

        uiManager.OnPaused(false);
        dynamicPositions = new List<SimpleVector2>(0);
        staticPositions = new List<SimpleVector2>(0);

        //IsMasterPlayer = true; //PunNetworkManager.NetworkManager.IsMaster;
        //GroupIndex = 0; // PunNetworkManager.NetworkManager.PlayerIndex;
        uiManager.SetGroupIndex(GroupIndex);
        cameraPivot.localRotation = Quaternion.Euler(GroupIndex * 180.0f  * Vector3.up);
        for (int i = 0; i < groups.Length; i++)
        {
            groups[i].Init2Group(arena, i);
            if (GroupIndex == i)
            {
                MyGroup = groups[i];
            }
            else
            {
                OtherGroup = groups[i];
            }
            List<TowerManager> simpleTowers = groups[i].Towers;
            for (int x = 0; x < simpleTowers.Count; x++)
            {
                SimpleVector2 towerPosition = groups[i].Positions[0][x * arena.sizeHalf.x];

                switch (x)
                {
                    case 0: //left tower
                        towerPosition = groups[i].Positions[3][5];
                        break;
                    case 1: //main 
                        towerPosition = groups[i].Positions[0][10];
                        break;
                    case 2: //right tower
                        towerPosition = groups[i].Positions[3][15];
                        break;
                }
                //if(x == 1)
                //{
                    
                //}
                //else
                //{
                //    towerPosition = groups[i].Positions[1][x * arena.sizeHalf.x];
                    
                //}

                simpleTowers[x].Init(i, towerPosition, null, materials[i]);
                dynamicPositions.Add(towerPosition);
            }
        }
        Debug.LogWarning("group index " + GroupIndex);
 
        for (int i = -arena.sizeHalf.x; i <= arena.sizeHalf.x; i++)
        {
            for (int j = -arena.sizeHalf.z; j <= arena.sizeHalf.z; j++)
            {
                SimpleVector2 position = new SimpleVector2(i, j);
                staticPositions.Add(position);
            }
        }

        uiManager.OnCreateHero += AddMyHeroToWaitingList;
        if (!debug)
        {
            PunNetworkManager.NetworkManager.Messenger.OnAddOtherHeroToWaitingList += AddOtherHeroToWaitingList;
            PunNetworkManager.NetworkManager.Messenger.OnStartGame += StartGame;
            PunNetworkManager.NetworkManager.Messenger.OnPing += OnPing;

            PunNetworkManager.NetworkManager.RPCToOthers("StartGame");
        }
        //PunNetworkManager.NetworkManager.Messenger.OnAddOtherHeroToWaitingList += AddOtherHeroToWaitingList;
        //PunNetworkManager.NetworkManager.Messenger.OnStartGame += StartGame;
        //PunNetworkManager.NetworkManager.Messenger.OnPing += OnPing;

        //PunNetworkManager.NetworkManager.RPCToOthers("StartGame");

        //TODO: get 5 units from pkdataholder
        playerHeroes = new List<string>();


        //if (PlayerDeckHelper.Instance)
        //{
        //    var heroes = PlayerDeckHelper.Instance.GetActiveDeck();
        //    playerHeroes = new List<string>();
        //    foreach (string h in heroes)
        //    {
        //        playerHeroes.Add(h);
        //    }

        //    //playerHeroes = PlayerDeckHelper.Instance.playerHeroes;
        //    foreach (var p in playerHeroes)
        //    {
        //        Debug.LogError(p);
        //    }
        //}
        //else
        //{
        //    Debug.LogError("There is no deck available!!");
        //}

    }




    //public void DeactivateHeroes(HeroButton[] heroes)
    //{

    //    foreach (var h in heroes)
    //    {
    //        h.gameObject.SetActive(false);
    //    }

    //    foreach (var h in heroes)
    //    {
    //        if (playerHeroes.Contains(h.gameObject.name))
    //        {
    //            h.gameObject.SetActive(true);
    //        }
    //    }


    //}

    public Animator anim;
    public InitUIOnGame initUIOnGame;
    private IEnumerator Start()
    {
        initUIOnGame.OnGameStart();

        if (anim)
        {
            //anim.SetTrigger("start");
            
            //default values

            //if (PlayerDeckHelper.Instance && PlayerDeckHelper.Instance.playerHeroes.Count == 5)
            //{
            //    playerHeroes = PlayerDeckHelper.Instance.playerHeroes;
            //    foreach (var p in playerHeroes)
            //    {
            //        Debug.LogError(p);
            //    }
            //}

            //if (PlayerDeckHelper.Instance)
            //{
            //    var heroes = PlayerDeckHelper.Instance.GetActiveDeck();
            //    playerHeroes = new List<string>();
            //    foreach (string h in heroes)
            //    {
            //        playerHeroes.Add(h);
            //    }

            //    //playerHeroes = PlayerDeckHelper.Instance.playerHeroes;
            //    foreach (var p in playerHeroes)
            //    {
            //        Debug.LogError(p);
            //    }
            //}
            //else
            //{
            //    Debug.LogError("There is no deck available!!");
            //}


            //DeactivateHeroes(uiManager.heroButtonsBuildings);
            //DeactivateHeroes(uiManager.heroButtonsWarMachines);
            //DeactivateHeroes(uiManager.heroButtonsArchers);
            //DeactivateHeroes(uiManager.heroButtonsHeavy);
            //DeactivateHeroes(uiManager.heroButtonsLight);

            //Debug.LogError("Player index: " + GroupIndex);
            //string heroes = "";
            //foreach(var h in uiManager.heroButtons)
            //{

            //    if(h.gameObject.name == "Building02")
            //    {
            //        h.gameObject.SetActive(false);
            //    }
            //    heroes += h.gameObject.name;
            //}
            //Debug.LogError("List of players " + heroes);



            yield return new WaitForSeconds(1f);
        }
        if (!debug)
        {
            while (true)
            {
                PunNetworkManager.NetworkManager.RPCToOthers("Ping", myGameTime);
                yield return new WaitForSeconds(0.25f);
            }
        }
    }

    public void OnPing(float otherGameTime)
    {
        this.otherGameTime = otherGameTime;
    }

    private void AddMyHeroToWaitingList(int index, SimpleVector2 position, IEnumerator wait)
    {
        if (!MyGroup.HeroesWaitingList.ContainsKey(GameTime + 1) && !MyGroup.HeroesWaitingList.ContainsKey(GameTime + 2))
        {
            uiManager.HeroCreateTime = 2.0f - secondsPass;
            StartCoroutine(wait);
            AddMyHeroToWaitingList(index, GameTime + 2, position);
        }
        else
        {
            long addTime = 3; 
            while(MyGroup.HeroesWaitingList.ContainsKey(GameTime + addTime))
            {
                addTime++;
            }
            uiManager.HeroCreateTime = addTime - secondsPass;
            StartCoroutine(wait);
            AddMyHeroToWaitingList(index, GameTime + addTime, position);
        }
    }

    private void AddMyHeroToWaitingList(int index, long addTime, SimpleVector2 position)
    {
        if (!MyGroup.HeroesWaitingList.ContainsKey(addTime))
        {
            MyGroup.AddHeroToWaitingList(index, addTime, position);
            PunNetworkManager.NetworkManager.RPCToOthers("AddOtherHeroToWaitingList", index, addTime, position.x, position.z);
        }
    }

    private void AddOtherHeroToWaitingList(int index, long addTime, SimpleVector2 position)
    {
        if(gameIsEnd)
        {
            return;
        }
        if (GameTime >= addTime)
        {
            Debug.LogWarning("Bed Network");
            PunNetworkManager.NetworkManager.Disconnect();
            return;
        }
        if (!OtherGroup.HeroesWaitingList.ContainsKey(addTime))
        {
            OtherGroup.AddHeroToWaitingList(index, addTime, position);
        }
    }
    public Text timerText;
    private void OnGUI()
    {
        //GUILayout.Label("Game Time: " + GameTime);
        //if (IsPaused)
        //    GUI.Label(new Rect(100, 100, 50, 30), "Game paused");
    }



    private Coroutine waitForOtherPlayer;

    public void Update()
    {
        if(IsPaused || gameIsEnd)
        {
            return;
        }

        if(myGameTime - otherGameTime > 1.0f)
        {
            if(waitForOtherPlayer != null)
            {
                StopCoroutine(waitForOtherPlayer);
            }
            waitForOtherPlayer = StartCoroutine(WaitForOtherPlayer(myGameTime - otherGameTime));
        }

        secondsPass += Time.deltaTime;
        myGameTime += Time.deltaTime;
        if (secondsPass >= 1.0f)
        {
            GameTime++;
            secondsPass = 0.0f;
            UpdateGame();
        }

        
        timerText.text = ConvertSecToMinutes(GameTime);
    }

    public string ConvertSecToMinutes(long sec)
    {
        int totalSeconds = (int)sec;
        int seconds = totalSeconds % 60;
        int minutes = totalSeconds / 60;
        string time = minutes + ":" + seconds;

        return time;
    }

    IEnumerator WaitForOtherPlayer(float waitingTime)
    {
        Pause(true);
        while (myGameTime - otherGameTime > 0.5f)
        {
            yield return null;
        }
        Pause(false);
    }

    private void Pause(bool pause)
    {
        IsPaused = pause;
        if (!pause || myGameTime - otherGameTime > 1.25f)
        {
            uiManager.OnPaused(pause);
        }
    }

    private void StartGame()
    {
        //init heroes list


        ResetTo(0);
    }

    private void UpdateGame()
    {
        if (groups[0].HeroesWaitingList.ContainsKey(GameTime))
        {
            CreateHero(groups[0].HeroesWaitingList[GameTime], 0,  groups[0], groups[1]);
        }
        if (groups[1].HeroesWaitingList.ContainsKey(GameTime))
        {
            CreateHero(groups[1].HeroesWaitingList[GameTime], 1, groups[1], groups[0]);
        }
        UpdateHeroes(groups[0], groups[1]);
        UpdateHeroes(groups[1], groups[0]);
    }

    private void CreateHero(HeroWaitingList heroWaitingList, int groupIndex, Group myGroup, Group otherGroup)
    {
        SimpleVector2 heroPosition = heroWaitingList.position;
        HeroManager newHero = CreateHero(heroWaitingList.index, groupIndex, heroPosition, otherGroup);
        myGroup.AddHero(newHero);
    }

    private HeroManager CreateHero(int index, int groupIndex, SimpleVector2 heroPosition, Group otherGroup)
    {
        HeroManager newHero = Instantiate(heroesPrefabs[index]);
        arena.AddHero(newHero);
        newHero.InitColorUnit(materialsUnit[groupIndex]);
        newHero.Init(groupIndex, heroPosition, GetTargetTowerOrHero(newHero, heroPosition, otherGroup), materials[groupIndex]);
        
        if (!newHero.IsBomb)
        {
            //add color to unit

            dynamicPositions.Add(heroPosition);
        }
        //Debug.Log("Hero created: " + heroesPrefabs[index].name);
        //add next to heroes list
        //AllHeroesLastIndex++;
        //AllHeroesLastIndex %= AllHeroesPrefabs.Length;
        //int rand = UnityEngine.Random.Range(0, AllHeroesPrefabs.Length);
        //heroesPrefabs[index] = AllHeroesPrefabs[AllHeroesLastIndex];
        //Debug.Log("Next Heroes: " + AllHeroesPrefabs[AllHeroesLastIndex].name);
        //update image
        //Image button = uiManager.heroButtons[index].GetComponentInChildren<Image>().gameObject.GetComponent<Image>();
        //Debug.Log("Sprite: " + button.sprite);
        //button.GetComponentInChildren<Image>().sprite = sprite2;

        return newHero;
    }

    private GameObjectManager GetTargetTowerOrHero(HeroManager newHero, SimpleVector2 heroPosition, Group otherGroup)
    {
        GameObjectManager target = GetTargetTower(heroPosition, otherGroup.Towers, out int distance);
        HeroManager otherHero = GetTargetHero(newHero, heroPosition, otherGroup.Heroes, out int heroDistance);
        if (otherHero && distance > heroDistance)
        {
            distance = heroDistance;
            target = otherHero;
        }
        return target;
    }

    private TowerManager GetTargetTower(SimpleVector2 heroPosition, List<TowerManager> towers, out int distance)
    {
        distance = int.MaxValue;
        if (towers.Count == 0)
        {
            return null;
        }
        TowerManager target = towers[0];
        distance = SimpleVector2.SqrDistance(heroPosition, target.Position);
        for (int i = 1; i < towers.Count; i++)
        {
            TowerManager newTarget = towers[i];
            if (newTarget != null)
            {
                int newDistance = SimpleVector2.SqrDistance(heroPosition, newTarget.Position);
                if (distance > newDistance)
                {
                    distance = newDistance;
                    target = newTarget;
                }
            }
        }
        return target;
    }

    private HeroManager GetTargetHero(TowerManager tower, List<HeroManager> heroes, out int distance)
    {
        distance = int.MaxValue;
        if (heroes.Count == 0)
        {
            return null;
        }
        HeroManager target = heroes[0].IsBomb ? null :heroes[0];
        if (target)
        {
            distance = SimpleVector2.SqrDistance(tower.Position, target.Position);
        }

        for (int i = 1; i < heroes.Count; i++)
        {
            HeroManager newTarget = heroes[i];
            if (newTarget != null && !newTarget.IsBomb)
            {
                int newDistance = SimpleVector2.SqrDistance(tower.Position, newTarget.Position);
                if (distance > newDistance)
                {
                    distance = newDistance;
                    target = newTarget;
                }
            }
        }
        return target;
    }

    private HeroManager GetTargetHero(HeroManager hero, SimpleVector2 heroPosition, List<HeroManager> heroes, out int distance)
    {
        distance = int.MaxValue;
        if (heroes.Count == 0)
        {
            return null;
        }
        HeroManager target = heroes[0].IsBomb? null : heroes[0];
        if (target)
        {
            distance = SimpleVector2.SqrDistance(heroPosition, target.Position);
        }
        for (int i = 1; i < heroes.Count; i++)
        {
            HeroManager newTarget = heroes[i];
            if (newTarget != null && !newTarget.IsBomb && hero.CanBeAsTarget(newTarget))
            {
                int newDistance = SimpleVector2.SqrDistance(heroPosition, newTarget.Position);
                if (distance > newDistance)
                {
                    distance = newDistance;
                    target = newTarget;
                }
            }
        }
        return target;
    }

    private void ResetTo(long timeToResset)
    {
        secondsPass = timeToResset;
        GameTime = timeToResset;
    }

    private void UpdateHeroes(Group group1, Group group2)
    {
        HeroManager bomb = null;
        foreach (HeroManager hero in group1.Heroes)
        {
            GameObjectManager target = GetTargetTowerOrHero(hero, hero.Position, group2);
            if(target)
            {
                hero.SetTargetObject(target);
            }
            if (hero.GetNewPosition(staticPositions, dynamicPositions, out SimpleVector2 newPosition))
            {
                dynamicPositions.Remove(hero.Position);
                dynamicPositions.Add(newPosition);
                hero.MoveTo(newPosition);
            }
            if(hero.IsBomb)
            {
                hero.OnFight();
            }
            if (hero.CanDamageTarget())
            {
                if (!hero.IsBomb)
                {
                    if (hero.CanDamageTarget())
                    {
                        hero.OnFight();
                        hero.IsAttacking = true;
                    }
                    //
                    //set post to 0
                }
                if (hero.TargetObject.Damage(hero.Force))
                {
                    dynamicPositions.Remove(hero.TargetObject.Position);
                    hero.TargetObject.RemoveFromGroup(group2);
                    hero.RemoveTargetObject();
                    if(group2.Towers.Count == 0)
                    {
                        gameIsEnd = true;
                        StartCoroutine(OnWinOrLose(group2.Index != GroupIndex));
                    }
                }
            }
            else
            {
                if (!hero.IsBomb)
                {

                    hero.IsAttacking = false;

                }
            }
            if(hero.IsBomb)
            {
                bomb = hero;
            }
        }
        if(bomb)
        {
            bomb.RemoveFromGroup(group1);
            bomb.Remove();
        }
        foreach (TowerManager tower in group1.Towers)
        {
            HeroManager target = GetTargetHero(tower, group2.Heroes, out int distance);
            tower.SetTargetObject(target);
            if (tower.CanDamageTarget())
            {
                tower.OnFight();
                if (tower.TargetObject.Damage(tower.Force))
                {
                    dynamicPositions.Remove(tower.TargetObject.Position);
                    tower.TargetObject.RemoveFromGroup(group2);
                    tower.RemoveTargetObject();
                }
            }
        }
    }
    private IEnumerator OnWinOrLose(bool isWin)
    {
        uiManager.OnWinOrLose(isWin);
        yield return null;
        //yield return new WaitForSeconds(2.5f);

        PunNetworkManager.NetworkManager.ClearActions();
    }
}

public class HeroWaitingList
{
    public int index;
    public SimpleVector2 position;
    public HeroWaitingList(int index, SimpleVector2 position)
    {
        this.index = index;
        this.position = position;
    }
}

[System.Serializable]
public class Group
{
    public List<HeroManager> Heroes { get; private set; }
    public Dictionary<long, HeroWaitingList> HeroesWaitingList { get; private set; }
    public List<TowerManager> Towers => towers;
    public SimpleVector2[][] Positions => positions;
    public int Index { get; private set; }

    [SerializeField] private List<TowerManager> towers;
    [SerializeField] private SimpleVector2[][] positions;

    public void AddHero(HeroManager hero)
    {
        Heroes.Add(hero);
    }
    public void AddTower(TowerManager tower)
    {
        Towers.Add(tower);
    }

    public void RemoveHero(HeroManager hero)
    {
        Heroes.Remove(hero);
    }
    public void RemoveTower(TowerManager tower)
    {
        Towers.Remove(tower);
    }

    public void AddHeroToWaitingList(int heroId, long addTime, SimpleVector2 position)
    {
        HeroesWaitingList.Add(addTime, new HeroWaitingList(heroId, position));
    }
    public void RemoveHeroFromWaitingList(long addTime)
    {
        HeroesWaitingList.Remove(addTime);
    }


    public void Init2Group(Arena arena, int groupIndex)
    {
        Index = groupIndex;
        Heroes = new List<HeroManager>(0);
        HeroesWaitingList = new Dictionary<long, HeroWaitingList>(0);
        positions = new SimpleVector2[arena.sizeHalf.z][];

        int z = 0;
        int x = 0;

        if (groupIndex == 0)
        {
            for (int j = -arena.sizeHalf.z; j < 0; j++)
            {
                positions[z] = new SimpleVector2[2 * arena.sizeHalf.x + 1];
                x = 0;
                for (int i = -arena.sizeHalf.x; i <= arena.sizeHalf.x; i++)
                {
                    positions[z][x] = new SimpleVector2(i, j);
                    x++;
                }
                z++;
            }
        }
        else
        {
            for (int j = arena.sizeHalf.z; j > 0; j--)
            {
                positions[z] = new SimpleVector2[2 * arena.sizeHalf.x + 1];
                x = 0;
                for (int i = arena.sizeHalf.x; i >= -arena.sizeHalf.x; i--)
                {
                    positions[z][x] = new SimpleVector2(i, j);
                    x++;
                }
                z++;
            }
        }
    }
}

