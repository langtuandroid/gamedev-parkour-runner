using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        private string _privacyPolicyURL;
        public static MainMenupr Instance;
        public Transform spawnLocation;
        public GameObject[] modelsToSpawn;
        public Animator CharacterSelectionWindow;
        public Animator mainMenuWindow;
        public int tempInt;
        public bool loadingDone;
        public GameObject unlockButton;
        public ModelIDs currentlySelectedModel;
        public List<ChangeAnimator> allModels;
        public Animator acknowledgementAnimator;
        public GameObject acknowledgementGameObject;
        public GameObject popUpMessage;
        public Text popUpMessageText;
        public GameObject popUpMessage2;
        public Text popUpMessage2Text;
        public Text coinInfo;
        public Text unlockInfo;
        
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
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
        
            PlayerPrefs.SetInt("Gold", 10000);
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
            if (spawnLocation.GetChild(0).gameObject != null)
            {
                Destroy(spawnLocation.GetChild(0).gameObject);
                Instantiate(modelsToSpawn[PlayerPrefs.GetInt("ChoosenCharacter")], spawnLocation).GetComponent<Animator>().SetTrigger("Selected");
            }
            else
            {
                Instantiate(modelsToSpawn[PlayerPrefs.GetInt("ChoosenCharacter")], spawnLocation).GetComponent<Animator>().SetTrigger("Selected");
            }
        
            StartCoroutine( InitializeUI());
            loadingDone = true;
        }
        
        public void EnableCharacterSelectionWindow()
        {
            SoundManager.Instance.PlayButtonPressedSound();
            mainMenuWindow.SetTrigger("MoveOut");
            CharacterSelectionWindow.SetTrigger("MoveIn");
            Destroy(spawnLocation.GetChild(0).gameObject);
            Instantiate(modelsToSpawn[PlayerPrefs.GetInt("ChoosenCharacter")], spawnLocation).GetComponent<Animator>().SetTrigger("Selected");
        }
        
        public void GoBackToMainMenu()
        {
            SoundManager.Instance.PlayButtonPressedSound();
            mainMenuWindow.SetTrigger("MoveIn");
            CharacterSelectionWindow.SetTrigger("MoveOut");
        }

        public void LoadLevel()
        {
            SoundManager.Instance.PlayButtonPressedSound();
            int level = PlayerPrefs.GetInt("LevelProgression", 1);
            print(level);
            SceneManager.LoadScene(level);
        }
        
        public void QuitGame()
        {
            SoundManager.Instance.PlayButtonPressedSound();
            Application.Quit();
        }
        
        #region Annoying Region
          public void ChangeToAmazon()
          {
              ChangeCharacter(ModelIDs.amazon, Amazon);
          }
          public void ChangeToBaseball()
          {
              ChangeCharacter(ModelIDs.baseball, Baseball);
          }
          public void ChangeToBioHaz()
          {
              ChangeCharacter(ModelIDs.bioHazzard, BioHaz);
          }
          public void ChangeToBoxer()
          {
              ChangeCharacter(ModelIDs.boxer, Boxer);
          }
          public void ChangeToClown()
          {
              ChangeCharacter(ModelIDs.clown, Clown);
          }
          public void ChangeToSkater()
          {
              ChangeCharacter(ModelIDs.skater, Skater);
          }
          public void ChangeToFlatyBoss()
          {
              ChangeCharacter(ModelIDs.flatyBoss, FlatyBoss);
          }
          public void ChangeToFootballer()
          {
              ChangeCharacter(ModelIDs.footballer, Footballer);
          }
          public void ChangeToRoller()
          {
              ChangeCharacter(ModelIDs.roller, Roller);
          }
          public void ChangeToGoalKeeper()
          {
              ChangeCharacter(ModelIDs.goalkeeper, Goalkeeper);
          }
          public void ChangeToHero()
          {
              ChangeCharacter(ModelIDs.hero, Hero);
          }
          public void ChangeToHockey()
          {
              ChangeCharacter(ModelIDs.hockey, Hockey);
          }
          public void ChangeToKarate()
          {
              ChangeCharacter(ModelIDs.karate, Karate);
          }
          public void ChangeToRepairer()
          {
              ChangeCharacter(ModelIDs.repairer, Repairer);
          }
          public void ChangeToSkier()
          {
              ChangeCharacter(ModelIDs.skier, Skier);
          }
          public void ChangeToTennis()
          {
              ChangeCharacter(ModelIDs.tennis, Tennis);
          }
          public void ChangeToVolleyBall()
          {
              ChangeCharacter(ModelIDs.volley, Volleyball);
          }
          
          private void ChangeCharacter(ModelIDs modelId, string characterKey)
          {
              SoundManager.Instance.PlayButtonPressedSound(); 
              Destroy(spawnLocation.GetChild(0).gameObject);
              currentlySelectedModel = modelId;
              Instantiate(modelsToSpawn[(int)modelId], spawnLocation).GetComponent<Animator>().SetTrigger("Selected");
              if (!PlayerPrefs.HasKey(characterKey))
              {
                  PlayerPrefs.SetString(characterKey, "False");
                  unlockButton.SetActive(true);
              }
              else if (PlayerPrefs.GetString(characterKey) == "True")
              {
                  PlayerPrefs.SetInt("ChoosenCharacter", (int)modelId);
                  unlockButton.SetActive(false);
              }
              else
              {
                  unlockButton.SetActive(true);
              }
          }

          public void ToggleAcknowledgement()
          {
              SoundManager.Instance.PlayButtonPressedSound();
              unlockButton.SetActive(false);
              if (!acknowledgementGameObject.activeSelf)
              {
                  acknowledgementGameObject.SetActive(true);
                  acknowledgementAnimator.SetTrigger("In");
                  unlockInfo.text = "Unlock " + modelsToSpawn[(int)currentlySelectedModel].name + "?";
              }
              else
              {
                  StartCoroutine(DisableWindow(acknowledgementGameObject));
                  unlockButton.SetActive(true);
              }
          }

          IEnumerator DisableWindow(GameObject window)
          {
              window.GetComponent<Animator>().SetTrigger("Out");
              yield return new WaitForSeconds(0.2f);
              window.SetActive(false);
          }
          
          IEnumerator DisableWindow(GameObject window,float delay)
          {
              window.GetComponent<Animator>().SetTrigger("Out");
              yield return new WaitForSeconds(delay);
              window.SetActive(false);
          }
          
          public void Unlock()
          {
              SoundManager.Instance.PlayButtonPressedSound();
              unlockButton.SetActive(false);
              StartCoroutine(DisableWindow(acknowledgementGameObject,.2f));
              switch (currentlySelectedModel)
              {
                  case ModelIDs.amazon:
                  {
                      CheckUnlock(ModelIDs.amazon, Amazon);
                      break;
                  }
                  case ModelIDs.baseball:
                  {
                      CheckUnlock(ModelIDs.baseball, Baseball);
                      break;
                  }
                  case ModelIDs.bioHazzard:
                  {
                      CheckUnlock(ModelIDs.bioHazzard, BioHaz);
                      break;
                  }
                  case ModelIDs.boxer:
                  {
                      CheckUnlock(ModelIDs.boxer, Boxer);
                      break;
                  }
                  case ModelIDs.clown:
                  {
                      CheckUnlock(ModelIDs.clown, Clown);
                      break;
                  }
                  case ModelIDs.flatyBoss:
                  {
                      CheckUnlock(ModelIDs.flatyBoss, FlatyBoss);
                      break;
                  }
                  case ModelIDs.footballer:
                  {
                      CheckUnlock(ModelIDs.footballer, Footballer);
                      break;
                  }
                  case ModelIDs.goalkeeper:
                  {
                      CheckUnlock(ModelIDs.goalkeeper, Goalkeeper);
                      break;
                  }
                  case ModelIDs.hero:
                  {
                      CheckUnlock(ModelIDs.hero, Hero);
                      break;
                  }
                  case ModelIDs.hockey:
                  {
                      CheckUnlock(ModelIDs.hockey, Hockey);
                      break;
                  }
                  case ModelIDs.karate:
                  {
                      CheckUnlock(ModelIDs.karate, Karate);
                      break;
                  }
                  case ModelIDs.repairer:
                  {
                      CheckUnlock(ModelIDs.repairer, Repairer);
                      break;
                  }
                  case ModelIDs.roller:
                  {
                      CheckUnlock(ModelIDs.roller, Roller);
                      break;
                  }
                  case ModelIDs.skater:
                  {
                      CheckUnlock(ModelIDs.skater, Skater);
                      break;
                  }
                  case ModelIDs.skier:
                  {
                      CheckUnlock(ModelIDs.skier, Skier);
                      break;
                  }
                  case ModelIDs.tennis:
                  {
                      CheckUnlock(ModelIDs.tennis, Tennis);
                      break;
                  }
                  case ModelIDs.volley:
                  {
                      CheckUnlock(ModelIDs.volley, Volleyball);
                      break;
                  }
              }
          }
          
          private void CheckUnlock(ModelIDs modelIDs, string modelName)
          {
              if (allModels[(int)modelIDs].GetCostToUnlock() > PlayerPrefs.GetInt("Gold"))
              {
                  popUpMessage.SetActive(true);
                  popUpMessage.GetComponent<Animator>().SetTrigger("In");
                  Invoke("PopUpClose", 2); unlockButton.SetActive(true);
              }
              else
              {
                  PlayerPrefs.SetString(modelName, "True");
                  allModels[(int)modelIDs].UnlockThisCharacter();
                  RemoveGold(allModels[(int)modelIDs].GetCostToUnlock());
                  PlayerPrefs.SetInt("ChoosenCharacter", (int)modelIDs);
                  popUpMessage2Text.text = $"You Have Unlocked The {modelName}";
                  popUpMessage2.SetActive(true);
                  popUpMessage2.GetComponent<Animator>().SetTrigger("In");
                  Invoke("PopUpClose", 2);
              }
          }
          
          public void PopUpClose()
          {
              SoundManager.Instance.PlayButtonPressedSound();
              if (popUpMessage.activeSelf)
                  StartCoroutine(DisableWindow(popUpMessage,0.2f));
              else if(popUpMessage2.activeSelf)
                  StartCoroutine(DisableWindow(popUpMessage2, 0.2f));
          }
          
          public IEnumerator InitializeUI()
          {
              coinInfo.text = PlayerPrefs.GetInt("Gold").ToString();
              unlockButton.SetActive(false);
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

        public void AddGold(int amount)
        {
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + amount);
            coinInfo.text = PlayerPrefs.GetInt("Gold").ToString();
        }
        
        public void RemoveGold(int amount)
        {
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") - amount);
            coinInfo.text = PlayerPrefs.GetInt("Gold").ToString();
        }

        public void OpenPrivacyPolicypr()
        {
            Application.OpenURL(_privacyPolicyURL);
            //Application.OpenURL("https://unconditionalgames.blogspot.com/2020/01/privacy-policy-this-privacy-policy.html");
        }
    }
}