using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.iOS;
using UnityEngine.UI;


    public delegate void CreateHeroHandler(int index, SimpleVector2 position, IEnumerator wait);


public class UIManager : MonoBehaviour
{
    public CreateHeroHandler OnCreateHero;
    [SerializeField] public HeroButton[] heroButtons; //fill with pictures

    public List<SingleUnitCard> playerDeck;
    public PlayerKeysHolder playerKeysHolder;


    //[SerializeField] public HeroButton[] heroButtonsBuildings;
    //[SerializeField] public HeroButton[] heroButtonsWarMachines;
    //[SerializeField] public HeroButton[] heroButtonsArchers;
    //[SerializeField] public HeroButton[] heroButtonsHeavy;
    //[SerializeField] public HeroButton[] heroButtonsLight;
    [SerializeField] public GameManager gameManager;


    [SerializeField] private Text pausedText;
    [SerializeField] private Text winLoseText;
    [SerializeField] private LayerMask uiMask;
    [SerializeField] private Transform heroSpawnPoint;

    [SerializeField] private Transform spawnField0;
    [SerializeField] private Transform spawnField1;


    private Transform currentHeroSpawnPoint;
    private List<GameObject> heroesSpawn;
    public float HeroCreateTime { get; set; }
    private int selectedHeroIndex = -1;
    private int selectedButtonIndex = -1;
    private int groupIndex;

    public InGameAccountHandle inGameAccountHandle;

    private void Awake()
    {
        winLoseText.gameObject.SetActive(false);
        heroSpawnPoint.gameObject.SetActive(false);
    }

    public void SetGroupIndex(int index)
    {
        groupIndex = index;
    }

    private void TriggerHeroSpawnPoint(bool isOn)
    {
        if (currentHeroSpawnPoint)
        {
            currentHeroSpawnPoint.gameObject.SetActive(isOn);
        }
    }
    public void OnPaused(bool isOn)
    {
        //TODO: on paused

        pausedText.gameObject.SetActive(isOn);
    }
    public AudioSource audioSource;
    public AudioSource gameSound;
    public void OnWinOrLose(bool isWin)
    {

        //TODO: set win lose panels, and blockchain
        inGameAccountHandle.InGameAfterBattle(isWin);


        audioSource.Play();
        gameSound.Stop();
        //winLoseText.gameObject.SetActive(true);
        //winLoseText.text = isWin ? "Win" : "Lose";
    }

    private void Start()
    {
        heroesSpawn = new List<GameObject>(0);
        //for (int i = 0; i < heroButtons.Length; i++)
        //{
        //    HeroButton button = heroButtons[i];
        //    int index = button.heroIndex;
        //    //set index of hero
        //    //int index = i;
        //    button.OnDown += (HeroButton btn) =>
        //    {
        //        if (button.image.fillAmount > 0.99f)
        //        {
        //            button.image.color = Color.Lerp(Color.clear, Color.white, 0.5f);
        //            selectedHeroIndex = index;
        //            currentHeroSpawnPoint = Instantiate(heroSpawnPoint);
        //        }
        //    };
        //}
        if (playerKeysHolder) //set hero button on UI, on click search by hero name  get heroes from pkDataHolder
        {
            for (int i = 0; i < 5; i++)
            {

                string unitName = UnitType.ReturnUnitTypeName(playerKeysHolder.playerDeck[i].unitType);

                //set hero name
                heroButtons[i].heroName = unitName.ToString();
                Debug.Log("Unit" + i + " : " + unitName);

                //set hero card
                playerDeck[i].SetUnitValues(
                    playerKeysHolder.playerDeck[i].unitsId,
                    playerKeysHolder.playerDeck[i].id,
                    playerKeysHolder.playerDeck[i].level,
                    playerKeysHolder.playerDeck[i].rareType,
                    playerKeysHolder.playerDeck[i].unitType,
                    playerKeysHolder.playerDeck[i].health,
                    playerKeysHolder.playerDeck[i].attack,
                    playerKeysHolder.playerDeck[i].speed,
                    true
                    );
            }

            ButtonsOnClick(heroButtons);
        }
        else
        {
            Debug.LogError("missing player key holders!!!");
            //return to login
        }

        //ButtonsOnClick(heroButtonsBuildings);
        //ButtonsOnClick(heroButtonsWarMachines);
        //ButtonsOnClick(heroButtonsArchers);
        //ButtonsOnClick(heroButtonsHeavy);
        //ButtonsOnClick(heroButtonsLight);

        //init hero buttons name


    }


    public void ButtonsOnClick(HeroButton[] heroes)
    {
        for (int i = 0; i < heroes.Length; i++)
        {
            HeroButton button = heroes[i];
            //button.SetImage();
            //int index = button.heroIndex;
            //int index = gameManager.heroesPrefabs.(button.heroName);
            int index = Array.FindIndex(gameManager.heroesPrefabs, row => row.gameObject.name == button.heroName);
            int buttonIndex = i;

            //Debug.LogError("Hero indexs in game manager prefabs: " + index + " button: " + buttonIndex);
            button.OnDown += (HeroButton btn) =>
            {
                if (button.image.fillAmount < 0.0001f)
                {
                    //button.image.fillAmount = 0f;
                    //button.image.color = Color.Lerp(Color.clear, Color.white, 0.5f);
                    selectedHeroIndex = index;
                    selectedButtonIndex = buttonIndex;
                    currentHeroSpawnPoint = Instantiate(heroSpawnPoint);
                }
            };
        }
    }

    private void Update()
    {
        if (selectedHeroIndex != -1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(groupIndex == 0)
                spawnField0.gameObject.SetActive(true);
            else if(groupIndex == 1)
                spawnField1.gameObject.SetActive(true);

            //Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

            //if(Physics.Raycast(ray, out RaycastHit hit2, Mathf.Infinity, uiMask))
            //{
            //    Debug.Log(hit2.point);
            //}


            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, uiMask) && (groupIndex == 0 && hit.point.z < 0 || groupIndex == 1 && hit.point.z > 0))
            {
                TriggerHeroSpawnPoint(true);


                currentHeroSpawnPoint.transform.position = new Vector3((int)hit.point.x, 0.0f, (int)hit.point.z);
                if (Input.GetMouseButtonUp(0))
                {

                    if (groupIndex == 0)
                        spawnField0.gameObject.SetActive(false);
                    else if (groupIndex == 1)
                        spawnField1.gameObject.SetActive(false);


                    currentHeroSpawnPoint.gameObject.GetComponent<ParticleOnPlaceholder>().OnHolderPlaced();
                  //handle aparticle effect  currentHeroSpawnPoint.gameObject.GetComponent<>

                    Debug.Log("best");

                    //Debug.LogError("selectedHeroIndex = " + selectedHeroIndex);
                    //Debug.LogError("selectedButtonIndex = " + selectedButtonIndex);

                    //get selected button

                    HeroButton button = heroButtons[selectedButtonIndex];
                    SimpleVector2 position = new SimpleVector2((int)hit.point.x, (int)hit.point.z);
                    OnCreateHero?.Invoke(selectedHeroIndex, position, AnimateHeroButton(button, currentHeroSpawnPoint.gameObject));
                    heroesSpawn.Add(currentHeroSpawnPoint.gameObject);




                }
            }
            else
            {
                TriggerHeroSpawnPoint(false);
            }
            if (Input.GetMouseButtonUp(0))
            {
                //heroButtons[selectedButtonIndex].image.color = Color.white;
                selectedButtonIndex = -1;
                selectedHeroIndex = -1;
            }
        }
    }

    IEnumerator AnimateHeroButton(HeroButton button, GameObject heroSpown)
    {
        float time = HeroCreateTime;
        //button.image.fillAmount = 1f;
        float createTime = 0.0f;
        while (time > createTime)
        {
            time -= Time.deltaTime;
            button.image.fillAmount = (time / HeroCreateTime);
            yield return new WaitForEndOfFrame();

        }
        button.image.fillAmount = 0f;
        Destroy(heroSpown);
    }
}
