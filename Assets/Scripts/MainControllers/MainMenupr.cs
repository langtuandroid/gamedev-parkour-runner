using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Integration;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace MainControllers
{
    public class MainMenupr : MonoBehaviour
    {
        #region Constants
          private const string Amazon = "Amazon";
          private const string Baseball = "Baseball";
          private const string BioHaz = "BioHaz";
          private const string Boxer = "Boxer";
          private const string Clown = "Clown";
          private const string Skater = "Skater";
          private const string FlatyBoss = "FlatyBoss";
          private const string Footballer = "Footballer";
          private const string Roller = "Roller";
          private const string Goalkeeper = "Goalkeeper";
          private const string Hero = "Hero";
          private const string Hockey = "Hockey";
          private const string Karate = "Karate";
          private const string Repairer = "Repairer";
          private const string Skier = "Skier";
          private const string Tennis = "Tennis";
          private const string Volleyball = "Volleyball";
        #endregion
        
        [SerializeField]
        private UIPanel _activePanel;
        [SerializeField] 
        private List<UIPanel> _allPanels;
        [SerializeField]
        private List<Button> _backToMainMenu;
        [SerializeField]
        private Button _openLevelMenu;
        [SerializeField]
        private Button _openSelectCharactersMenu;
        [SerializeField]
        private Button _openSettingsMenu;
        [SerializeField]
        private Button _openShopMenu;
        
        public static MainMenupr Instancepr;
        public Transform spawnLocationpr;
        public GameObject[] modelsToSpawn;
        //public Animator CharacterSelectionWindowpr;
        //public Animator LevelSelectionWindowpr;
        //public Animator mainMenuWindowpr;
        private int tempIntpr;
        private bool loadingDonepr;
        //public GameObject unlockButtonpr;
        public ModelIDs currentlySelectedModelpr;
        public List<ChangeAnimator> allModels;
        public Animator acknowledgementAnimatorpr;
        public List<GameObject> acknowledgementGameObjectpr;
        public GameObject popUpMessagepr;
        public Text popUpMessageTextpr;
        public GameObject popUpMessage2pr;
        public Text popUpMessage2Textpr;
        public Text coinInfopr;
        public Text diamondInfopr;
        public Text unlockInfopr;
        
        public enum ModelIDs
        {
            repairer = 0,
            clown = 1,
            flatyBoss = 2,
            hero = 3,
            boxer = 4,
            skater = 5,
            hockey = 6,
            volley = 7,
            amazon = 8, 
            bioHazzard = 9,
            goalkeeper = 10, 
            baseball = 11,
            karate = 12,
            tennis = 13,
            skier = 14,
            roller = 15,
            footballer = 16
        }
        
        private AdMobController _adMobController;
        private RewardedAdController _rewardedAdController;
        
        [Inject]
        private void  Conctruct(AdMobController adMobController,RewardedAdController rewardedAdController)
        {
            _adMobController = adMobController;
            _rewardedAdController = rewardedAdController;
        }
        
        private void Awake()
        {
            if (!Instancepr)
            {
                Instancepr = this;
            }
            // Отключаем мультитач
            Input.multiTouchEnabled = false;
            Subscribepr();
            _adMobController.ShowBanner(true);
        }

        private void OnDestroy()
        {
            Unsubscibepr();
        }

        private void Subscribepr()
        {
            foreach (var button in _backToMainMenu)
            {
                button.onClick.AddListener(() => ActivatePanel(_allPanels[0]));
            }
            _openLevelMenu.onClick.AddListener(() => ActivatePanel(_allPanels[1]));
            _openSelectCharactersMenu.onClick.AddListener(() => ActivatePanel(_allPanels[2]));
            _openSettingsMenu.onClick.AddListener(() => ActivatePanel(_allPanels[3]));
            _openShopMenu.onClick.AddListener(() => ActivatePanel(_allPanels[4]));

            _rewardedAdController.GetRewarded += HandleRewardedAdSuccess;
        }

        
        private void Unsubscibepr()
        {
            foreach (var button in _backToMainMenu)
            {
                button.onClick.RemoveListener(() => ActivatePanel(_allPanels[0]));
            }
            _openLevelMenu.onClick.RemoveListener(() => ActivatePanel(_allPanels[1]));
            _openSelectCharactersMenu.onClick.RemoveListener(() => ActivatePanel(_allPanels[2]));
            _openSettingsMenu.onClick.RemoveListener(() => ActivatePanel(_allPanels[3]));
            _openShopMenu.onClick.RemoveListener(() => ActivatePanel(_allPanels[4]));
            
            _rewardedAdController.GetRewarded -= HandleRewardedAdSuccess;
        }
        
        private void Start()
        {
            SoundManager.Instance.PlayBGMusicOne();
            if (!PlayerPrefs.HasKey("ChoosenCharacter"))
            {
                PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.repairer);
            }
            if (!PlayerPrefs.HasKey("Repairer"))
            {
                PlayerPrefs.SetString("Repairer", "True");
            }
            if (!PlayerPrefs.HasKey("LevelProgression"))
            {
                PlayerPrefs.SetInt("LevelProgression", 1);
            }
            if(!PlayerPrefs.HasKey("Gold"))
            {
                PlayerPrefs.SetInt("Gold", 0);
            }
            if(!PlayerPrefs.HasKey("Diamond"))
            {
                PlayerPrefs.SetInt("Diamond", 0);
            }
            RefreshGoldInfo();
            RefreshDiamondInfo();
            // if (spawnLocationpr.GetChild(0).gameObject != null)
            // {
            //     Destroy(spawnLocationpr.GetChild(0).gameObject);
            //     Instantiate(modelsToSpawn[PlayerPrefs.GetInt("ChoosenCharacter")], spawnLocationpr).GetComponent<Animator>().SetTrigger("Selected");
            // }
            // else
            // {
            //     Instantiate(modelsToSpawn[PlayerPrefs.GetInt("ChoosenCharacter")], spawnLocationpr).GetComponent<Animator>().SetTrigger("Selected");
            // }
        
            StartCoroutine( InitializeUIpr());
            loadingDonepr = true;
            if (!PlayerPrefs.HasKey("OpenMainMenu"))
            {
                PlayerPrefs.SetString("OpenMainMenu", "True");
            }
            else
            {
                if (PlayerPrefs.GetString("OpenMainMenu") != "True")
                {
                   // _allPanels[1].GetComponent<Animator>().SetTrigger("MoveInInstantly");
                   ActivatefastPanel(_allPanels[1]);
                }
            }
            
        }
        
        private void ActivatePanel(UIPanel panel)
        {
            SoundManager.Instance.PlayButtonPressedSound();
            if (_activePanel != null)
                _activePanel.GetComponent<Animator>().SetTrigger("MoveOut");
        
            _activePanel = panel;
            if (_activePanel.PanelType == PanelType.CharacterSelectionMenu)
            {
                EnableCharacterSelectionWindowpr();
            }
            _activePanel.GetComponent<Animator>().SetTrigger("MoveIn");
        }
        
        private void ActivatefastPanel(UIPanel panel)
        {
            SoundManager.Instance.PlayButtonPressedSound();
            _allPanels[0].GetComponent<Animator>().SetTrigger("FastMainOut");
            _activePanel = panel;
            _activePanel.GetComponent<Animator>().SetTrigger("MoveInInstantly");
        }
        
        public void EnableCharacterSelectionWindowpr()
        {
            Destroy(spawnLocationpr.GetChild(0).gameObject);
            Instantiate(modelsToSpawn[PlayerPrefs.GetInt("ChoosenCharacter")], spawnLocationpr).GetComponent<Animator>().SetTrigger("Selected");
        }

        // public void GoBackToMainMenupr()
        // {
        //     SoundManager.Instance.PlayButtonPressedSound();
        //     mainMenuWindowpr.SetTrigger("MoveIn");
        //     CharacterSelectionWindowpr.SetTrigger("MoveOut");
        // }

        public void LoadLevelpr()
        {
            SoundManager.Instance.PlayButtonPressedSound();
            int level = PlayerPrefs.GetInt("LevelProgression", 1);
            print(level);
            SceneManager.LoadScene(level);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            PlayerPrefs.SetString("OpenMainMenu", "True");
        }

        private void OnApplicationQuit()
        {
            PlayerPrefs.SetString("OpenMainMenu", "True");
        }

        public void QuitGamepr()
        {
            SoundManager.Instance.PlayButtonPressedSound();
            PlayerPrefs.SetString("OpenMainMenu", "True");
            Application.Quit();
        }
        
        #region Annoying Region
          public void ChangeToAmazonpr()
          {
              ChangeCharacter(ModelIDs.amazon, Amazon);
          }
          public void ChangeToBaseballpr()
          {
              ChangeCharacter(ModelIDs.baseball, Baseball);
          }
          public void ChangeToBioHazpr()
          {
              ChangeCharacter(ModelIDs.bioHazzard, BioHaz);
          }
          public void ChangeToBoxerpr()
          {
              ChangeCharacter(ModelIDs.boxer, Boxer);
          }
          public void ChangeToClownpr()
          {
              ChangeCharacter(ModelIDs.clown, Clown);
          }
          public void ChangeToSkaterpr()
          {
              ChangeCharacter(ModelIDs.skater, Skater);
          }
          public void ChangeToFlatyBosspr()
          {
              ChangeCharacter(ModelIDs.flatyBoss, FlatyBoss);
          }
          public void ChangeToFootballerpr()
          {
              ChangeCharacter(ModelIDs.footballer, Footballer);
          }
          public void ChangeToRollerpr()
          {
              ChangeCharacter(ModelIDs.roller, Roller);
          }
          public void ChangeToGoalKeeperpr()
          {
              ChangeCharacter(ModelIDs.goalkeeper, Goalkeeper);
          }
          public void ChangeToHeropr()
          {
              ChangeCharacter(ModelIDs.hero, Hero);
          }
          public void ChangeToHockeypr()
          {
              ChangeCharacter(ModelIDs.hockey, Hockey);
          }
          public void ChangeToKaratepr()
          {
              ChangeCharacter(ModelIDs.karate, Karate);
          }
          public void ChangeToRepairerpr()
          {
              ChangeCharacter(ModelIDs.repairer, Repairer);
          }
          public void ChangeToSkierpr()
          {
              ChangeCharacter(ModelIDs.skier, Skier);
          }
          public void ChangeToTennispr()
          {
              ChangeCharacter(ModelIDs.tennis, Tennis);
          }
          public void ChangeToVolleyBallpr()
          {
              ChangeCharacter(ModelIDs.volley, Volleyball);
          }
          
          private void ChangeCharacter(ModelIDs modelId, string characterKey)
          {
              SoundManager.Instance.PlayButtonPressedSound(); 
              Destroy(spawnLocationpr.GetChild(0).gameObject);
              currentlySelectedModelpr = modelId;
              Instantiate(modelsToSpawn[(int)modelId], spawnLocationpr).GetComponent<Animator>().SetTrigger("Selected");
              if (!PlayerPrefs.HasKey(characterKey))
              {
                  PlayerPrefs.SetString(characterKey, "False");
                  //unlockButtonpr.SetActive(true);
              }
              else if (PlayerPrefs.GetString(characterKey) == "True")
              {
                  PlayerPrefs.SetInt("ChoosenCharacter", (int)modelId);
                  //unlockButtonpr.SetActive(false);
              }
              else
              {
                  SetOpenCharacter();
                  //unlockButtonpr.SetActive(true);
              }
          }

          private void SetOpenCharacter()
          {
               switch (currentlySelectedModelpr)
              {
                  case ModelIDs.repairer:
                  {
                      CheckUnlockpr(ModelIDs.repairer, Repairer);
                      break;
                  }
                  case ModelIDs.clown:
                  {
                      CheckUnlockpr(ModelIDs.clown, Clown);
                      break;
                  }
                  case ModelIDs.flatyBoss:
                  {
                      CheckUnlockpr(ModelIDs.flatyBoss, FlatyBoss);
                      break;
                  }
                  case ModelIDs.hero:
                  {
                      CheckUnlockpr(ModelIDs.hero, Hero);
                      break;
                  }
                  case ModelIDs.boxer:
                  {
                      CheckUnlockpr(ModelIDs.boxer, Boxer);
                      break;
                  }
                  case ModelIDs.skater:
                  {
                      CheckUnlockpr(ModelIDs.skater, Skater);
                      break;
                  }
                  
                  case ModelIDs.hockey:
                  {
                      ShowRewardedAdForCharacter();
                      break;
                  }
                  case ModelIDs.volley:
                  {
                      ShowRewardedAdForCharacter();
                      break;
                  }
                  case ModelIDs.amazon:
                  {
                      ShowRewardedAdForCharacter();
                      break;
                  }
                  case ModelIDs.bioHazzard:
                  {
                      ShowRewardedAdForCharacter();
                      break;
                  }
                  case ModelIDs.goalkeeper:
                  {
                      ShowRewardedAdForCharacter();
                      break;
                  }
                  case ModelIDs.baseball:
                  {
                      ShowRewardedAdForCharacter();
                      break;
                  }
                  
                  case ModelIDs.karate:
                  {
                      CheckUnlockDiaomodspr(ModelIDs.karate, Karate);
                      break;
                  }
                  case ModelIDs.tennis:
                  {
                      CheckUnlockDiaomodspr(ModelIDs.tennis, Tennis);
                      break;
                  }
                  case ModelIDs.skier:
                  {
                      CheckUnlockDiaomodspr(ModelIDs.skier, Skier);
                      break;
                  }
                  case ModelIDs.roller:
                  {
                      CheckUnlockDiaomodspr(ModelIDs.roller, Roller);
                      break;
                  }
                  case ModelIDs.footballer:
                  {
                      CheckUnlockDiaomodspr(ModelIDs.footballer, Footballer);
                      break;
                  }
              }
          }

        

          public void ToggleAcknowledgementpr()
          {
              SoundManager.Instance.PlayButtonPressedSound();
            
              if (!acknowledgementGameObjectpr[0].activeSelf)
              {
                  acknowledgementGameObjectpr[0].SetActive(true);
                  acknowledgementAnimatorpr.SetTrigger("In");
                  unlockInfopr.text = "Unlock " + modelsToSpawn[(int)currentlySelectedModelpr].name + "?";
              }
              else
              {
                  StartCoroutine(DisableWindowpr(acknowledgementGameObjectpr[0]));
              }
          }

          IEnumerator DisableWindowpr(GameObject window)
          {
              window.GetComponent<Animator>().SetTrigger("Out");
              yield return new WaitForSeconds(0.2f);
              window.SetActive(false);
          }
          
          IEnumerator DisableWindowpr(GameObject window,float delay)
          {
              window.GetComponent<Animator>().SetTrigger("Out");
              yield return new WaitForSeconds(delay);
              window.SetActive(false);
          }
          
          public void UnlockADspr()
          {
              SoundManager.Instance.PlayButtonPressedSound();
              StartCoroutine(DisableWindowpr(acknowledgementGameObjectpr[1],.2f));
              ShowRewardedAdForCharacter();
          }

          private void ShowRewardedAdForCharacter()
          {
              _adMobController.ShowRewardedAd();
          }

          private void HandleRewardedAdSuccess()
          {
              // Обработка успешного просмотра рекламы и разблокировка персонажа
              string modelName = "";
              switch (currentlySelectedModelpr)
              {
                  case ModelIDs.amazon:
                  {
                      modelName = Amazon;
                      break;
                  }
                  case ModelIDs.baseball:
                  {
                      modelName = Baseball;
                      break;
                  }
                  case ModelIDs.bioHazzard:
                  {
                      modelName = BioHaz;
                      break;
                  }
                  case ModelIDs.boxer:
                  {
                      modelName =Boxer;
                      break;
                  }
                  case ModelIDs.clown:
                  {
                      modelName = Clown;
                      break;
                  }
                  case ModelIDs.flatyBoss:
                  {
                      modelName =FlatyBoss;
                      break;
                  }
                  case ModelIDs.footballer:
                  {
                      modelName = Footballer;
                      break;
                  }
                  case ModelIDs.goalkeeper:
                  {
                      modelName =Goalkeeper;
                      break;
                  }
                  case ModelIDs.hero:
                  {
                      modelName =Hero;
                      break;
                  }
                  case ModelIDs.hockey:
                  {
                      modelName = Hockey;
                      break;
                  }
                  case ModelIDs.karate:
                  {
                      modelName = Karate;
                      break;
                  }
                  case ModelIDs.repairer:
                  {
                      modelName = Repairer;
                      break;
                  }
                  case ModelIDs.roller:
                  {
                      modelName =Roller;
                      break;
                  }
                  case ModelIDs.skater:
                  {
                      modelName = Skater;
                      break;
                  }
                  case ModelIDs.skier:
                  {
                      modelName = Skier;
                      break;
                  }
                  case ModelIDs.tennis:
                  {
                      modelName = Tennis;
                      break;
                  }
                  case ModelIDs.volley:
                  {
                      modelName =Volleyball;
                      break;
                  }
              }
              PlayerPrefs.SetString(modelName, "True");
              allModels[(int)currentlySelectedModelpr].UnlockThisCharacter();
              PlayerPrefs.SetInt("ChoosenCharacter", (int)currentlySelectedModelpr);
              popUpMessage2Textpr.text = $"You Have Unlocked The {modelName}";
              popUpMessage2pr.SetActive(true);
              popUpMessage2pr.GetComponent<Animator>().SetTrigger("In");
              Invoke("PopUpClosepr", 2);
          }
          
          public void Unlockpr()
          {
              SoundManager.Instance.PlayButtonPressedSound();
              StartCoroutine(DisableWindowpr(acknowledgementGameObjectpr[0],.2f));
              switch (currentlySelectedModelpr)
              {
                  case ModelIDs.amazon:
                  {
                      CheckUnlockpr(ModelIDs.amazon, Amazon);
                      break;
                  }
                  case ModelIDs.baseball:
                  {
                      CheckUnlockpr(ModelIDs.baseball, Baseball);
                      break;
                  }
                  case ModelIDs.bioHazzard:
                  {
                      CheckUnlockpr(ModelIDs.bioHazzard, BioHaz);
                      break;
                  }
                  case ModelIDs.boxer:
                  {
                      CheckUnlockpr(ModelIDs.boxer, Boxer);
                      break;
                  }
                  case ModelIDs.clown:
                  {
                      CheckUnlockpr(ModelIDs.clown, Clown);
                      break;
                  }
                  case ModelIDs.flatyBoss:
                  {
                      CheckUnlockpr(ModelIDs.flatyBoss, FlatyBoss);
                      break;
                  }
                  case ModelIDs.footballer:
                  {
                      CheckUnlockpr(ModelIDs.footballer, Footballer);
                      break;
                  }
                  case ModelIDs.goalkeeper:
                  {
                      CheckUnlockpr(ModelIDs.goalkeeper, Goalkeeper);
                      break;
                  }
                  case ModelIDs.hero:
                  {
                      CheckUnlockpr(ModelIDs.hero, Hero);
                      break;
                  }
                  case ModelIDs.hockey:
                  {
                      CheckUnlockpr(ModelIDs.hockey, Hockey);
                      break;
                  }
                  case ModelIDs.karate:
                  {
                      CheckUnlockpr(ModelIDs.karate, Karate);
                      break;
                  }
                  case ModelIDs.repairer:
                  {
                      CheckUnlockpr(ModelIDs.repairer, Repairer);
                      break;
                  }
                  case ModelIDs.roller:
                  {
                      CheckUnlockpr(ModelIDs.roller, Roller);
                      break;
                  }
                  case ModelIDs.skater:
                  {
                      CheckUnlockpr(ModelIDs.skater, Skater);
                      break;
                  }
                  case ModelIDs.skier:
                  {
                      CheckUnlockpr(ModelIDs.skier, Skier);
                      break;
                  }
                  case ModelIDs.tennis:
                  {
                      CheckUnlockpr(ModelIDs.tennis, Tennis);
                      break;
                  }
                  case ModelIDs.volley:
                  {
                      CheckUnlockpr(ModelIDs.volley, Volleyball);
                      break;
                  }
              }
          }
          
          private void CheckUnlockpr(ModelIDs modelIDs, string modelName)
          {
              if (allModels[(int)modelIDs].GetCostToUnlock() > PlayerPrefs.GetInt("Gold"))
              {
                  popUpMessagepr.SetActive(true);
                  popUpMessagepr.GetComponent<Animator>().SetTrigger("In");
                  Invoke("PopUpClosepr", 2);
              }
              else
              {
                  PlayerPrefs.SetString(modelName, "True");
                  allModels[(int)modelIDs].UnlockThisCharacter();
                  RemoveGoldpr(allModels[(int)modelIDs].GetCostToUnlock());
                  PlayerPrefs.SetInt("ChoosenCharacter", (int)modelIDs);
                  popUpMessage2Textpr.text = $"You Have Unlocked The {modelName}";
                  popUpMessage2pr.SetActive(true);
                  popUpMessage2pr.GetComponent<Animator>().SetTrigger("In");
                  Invoke("PopUpClosepr", 2);
              }
          }
          
          private void CheckUnlockDiaomodspr(ModelIDs modelIDs, string modelName)
          {
              if (allModels[(int)modelIDs].GetCostToUnlock() > PlayerPrefs.GetInt("Diamond"))
              {
                  popUpMessagepr.SetActive(true);
                  popUpMessagepr.GetComponent<Animator>().SetTrigger("In");
                  Invoke("PopUpClosepr", 2); 
              }
              else
              {
                  PlayerPrefs.SetString(modelName, "True");
                  allModels[(int)modelIDs].UnlockThisCharacter();
                  RemoveDiamondpr(allModels[(int)modelIDs].GetCostToUnlock());
                  PlayerPrefs.SetInt("ChoosenCharacter", (int)modelIDs);
                  popUpMessage2Textpr.text = $"You Have Unlocked The {modelName}";
                  popUpMessage2pr.SetActive(true);
                  popUpMessage2pr.GetComponent<Animator>().SetTrigger("In");
                  Invoke("PopUpClosepr", 2);
              }
          }
          
          public void PopUpClosepr()
          {
              SoundManager.Instance.PlayButtonPressedSound();
              if (popUpMessagepr.activeSelf)
                  StartCoroutine(DisableWindowpr(popUpMessagepr,0.2f));
              else if(popUpMessage2pr.activeSelf)
                  StartCoroutine(DisableWindowpr(popUpMessage2pr, 0.2f));
          }
          
          public IEnumerator InitializeUIpr()
          {
              _activePanel = _allPanels[0];
              coinInfopr.text = PlayerPrefs.GetInt("Gold").ToString();
             
              foreach(ChangeAnimator ca in allModels)
              {
                  if (PlayerPrefs.GetString(ca.name) == "True")
                  {
                      ca.AlreadyUnlocked();
                  }
                  else
                  {
                      ca.LockThisCharacter();
                  }
                  yield return null;
              }
          }
        
        #endregion

        public void AddGoldpr(int amount)
        {
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + amount);
            RefreshGoldInfo();
        }
        
        public void RemoveGoldpr(int amount)
        {
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") - amount);
            RefreshGoldInfo();
        }
        
        private void RefreshGoldInfo()
        {
            coinInfopr.text = PlayerPrefs.GetInt("Gold").ToString();
        }
        
        public void AddDiamondpr(int amount)
        {
            PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond") + amount);
            RefreshDiamondInfo();
        }
        
        public void RemoveDiamondpr(int amount)
        {
            PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond") - amount);
            RefreshDiamondInfo();
        }
        
        private void RefreshDiamondInfo()
        {
            diamondInfopr.text = PlayerPrefs.GetInt("Diamond").ToString();
        }
        
    }
}