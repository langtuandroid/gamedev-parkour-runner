using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;
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
        amazon, //Key in playerPref: Amazon
        baseball, //Key in playerPref: Baseball
        bioHazzard, //Key in playerPref: BioHazz
        boxer,
        clown,
        flatyBoss,
        footballer,
        goalkeeper,
        hero,
        hockey,
        karate,
        repairer,
        roller,
        skater,
        skier,
        tennis,
        volley
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
            Instantiate(modelsToSpawn[PlayerPrefs.GetInt("ChoosenCharacter")], spawnLocation).GetComponent<Animator>().SetTrigger("Selected");
        StartCoroutine( InitializeUI());
        loadingDone = true;
    }
    public void ResertLevelProgression()
    {
        PlayerPrefs.SetInt("LevelProgression", 1);
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
        int level = PlayerPrefs.GetInt("LevelProgression", 2);
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
        SoundManager.Instance.PlayButtonPressedSound();
        Destroy(spawnLocation.GetChild(0).gameObject);
        currentlySelectedModel = ModelIDs.amazon;
        Instantiate(modelsToSpawn[(int)ModelIDs.amazon], spawnLocation).GetComponent<Animator>().SetTrigger("Selected"); ;
        if (!PlayerPrefs.HasKey("Amazon"))
        {
            PlayerPrefs.SetString("Amazon", "False");
            unlockButton.SetActive(true);
        }
        else if (PlayerPrefs.GetString("Amazon") == "True")
        {
            PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.amazon);
            unlockButton.SetActive(false);
        }
        else
        {
            unlockButton.SetActive(true);
        }
    }
    public void ChangeToBaseball()
    {
        SoundManager.Instance.PlayButtonPressedSound(); Destroy(spawnLocation.GetChild(0).gameObject);
        currentlySelectedModel = ModelIDs.baseball;
        Instantiate(modelsToSpawn[(int)ModelIDs.baseball], spawnLocation).GetComponent<Animator>().SetTrigger("Selected");
        if (!PlayerPrefs.HasKey("Baseball"))
        {
            PlayerPrefs.SetString("Baseball", "False");
            unlockButton.SetActive(true);
        }
        else if (PlayerPrefs.GetString("Baseball") == "True")
        {
            PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.baseball);
            unlockButton.SetActive(false);
        }
        else
        {
            unlockButton.SetActive(true);
        }
    }
    public void ChangeToBioHaz()
    {
        SoundManager.Instance.PlayButtonPressedSound(); Destroy(spawnLocation.GetChild(0).gameObject);
        currentlySelectedModel = ModelIDs.bioHazzard;
        Instantiate(modelsToSpawn[(int)ModelIDs.bioHazzard], spawnLocation).GetComponent<Animator>().SetTrigger("Selected");
        if (!PlayerPrefs.HasKey("BioHaz"))
        {
            PlayerPrefs.SetString("BioHaz", "False");
            unlockButton.SetActive(true);
        }
        else if (PlayerPrefs.GetString("BioHaz") == "True")
        {
            PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.bioHazzard);
            unlockButton.SetActive(false);
        }
        else
        {
            unlockButton.SetActive(true);
        }
    }
    public void ChangeToBoxer()
    {
        SoundManager.Instance.PlayButtonPressedSound(); Destroy(spawnLocation.GetChild(0).gameObject);
        currentlySelectedModel = ModelIDs.boxer;
        Instantiate(modelsToSpawn[(int)ModelIDs.boxer], spawnLocation).GetComponent<Animator>().SetTrigger("Selected");
        if (!PlayerPrefs.HasKey("Boxer"))
        {
            PlayerPrefs.SetString("Boxer", "False");
            unlockButton.SetActive(true);
        }
        else if (PlayerPrefs.GetString("Boxer") == "True")
        {
            PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.boxer);
            unlockButton.SetActive(false);
        }
        else
        {
            unlockButton.SetActive(true);
        }
    }
    public void ChangeToClown()
    {
        SoundManager.Instance.PlayButtonPressedSound(); Destroy(spawnLocation.GetChild(0).gameObject);
        currentlySelectedModel = ModelIDs.clown;
        Instantiate(modelsToSpawn[(int)ModelIDs.clown], spawnLocation).GetComponent<Animator>().SetTrigger("Selected");
        if (!PlayerPrefs.HasKey("Clown"))
        {
            PlayerPrefs.SetString("Clown", "False");
            unlockButton.SetActive(true);
        }
        else if (PlayerPrefs.GetString("Clown") == "True")
        {
            PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.clown);
            unlockButton.SetActive(false);
        }
        else
        {
            unlockButton.SetActive(true);
        }
    }
    public void ChangeToSkater()
    {
        SoundManager.Instance.PlayButtonPressedSound(); Destroy(spawnLocation.GetChild(0).gameObject);
        currentlySelectedModel = ModelIDs.skater;
        Instantiate(modelsToSpawn[(int)ModelIDs.skater], spawnLocation).GetComponent<Animator>().SetTrigger("Selected");
        if (!PlayerPrefs.HasKey("Skater"))
        {
            PlayerPrefs.SetString("Skater", "False");
            unlockButton.SetActive(true);
        }
        else if (PlayerPrefs.GetString("Skater") == "True")
        {
            PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.skater);
            unlockButton.SetActive(false);
        }
        else
        {
            unlockButton.SetActive(true);
        }
    }
    public void ChangeToFlatyBoss()
    {
        SoundManager.Instance.PlayButtonPressedSound(); Destroy(spawnLocation.GetChild(0).gameObject);
        currentlySelectedModel = ModelIDs.flatyBoss;
        Instantiate(modelsToSpawn[(int)ModelIDs.flatyBoss], spawnLocation).GetComponent<Animator>().SetTrigger("Selected");
        if (!PlayerPrefs.HasKey("FlatyBoss"))
        {
            PlayerPrefs.SetString("FlatyBoss", "False");
            unlockButton.SetActive(true);
        }
        else if (PlayerPrefs.GetString("FlatyBoss") == "True")
        {
            PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.flatyBoss);
            unlockButton.SetActive(false);
        }
        else
        {
            unlockButton.SetActive(true);
        }
    }
    public void ChangeToFootballer()
    {
        SoundManager.Instance.PlayButtonPressedSound(); Destroy(spawnLocation.GetChild(0).gameObject);
        currentlySelectedModel = ModelIDs.footballer;
        Instantiate(modelsToSpawn[(int)ModelIDs.footballer], spawnLocation).GetComponent<Animator>().SetTrigger("Selected");
        if (!PlayerPrefs.HasKey("Footballer"))
        {
            PlayerPrefs.SetString("Footballer", "False");
            unlockButton.SetActive(true);
        }
        else if (PlayerPrefs.GetString("Footballer") == "True")
        {
            PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.footballer);
            unlockButton.SetActive(false);
        }
        else
        {
            unlockButton.SetActive(true);
        }
    }
    public void ChangeToRoller()
    {
        SoundManager.Instance.PlayButtonPressedSound(); Destroy(spawnLocation.GetChild(0).gameObject);
        currentlySelectedModel = ModelIDs.roller;
        Instantiate(modelsToSpawn[(int)ModelIDs.roller], spawnLocation).GetComponent<Animator>().SetTrigger("Selected");
        if (!PlayerPrefs.HasKey("Roller"))
        {
            PlayerPrefs.SetString("Roller", "False");
            unlockButton.SetActive(true);
        }
        else if (PlayerPrefs.GetString("Roller") == "True")
        {
            PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.roller);
            unlockButton.SetActive(false);
        }
        else
        {
            unlockButton.SetActive(true);
        }
    }
    public void ChangeToGoalKeeper()
    {
        SoundManager.Instance.PlayButtonPressedSound(); Destroy(spawnLocation.GetChild(0).gameObject);
        currentlySelectedModel = ModelIDs.goalkeeper;
        Instantiate(modelsToSpawn[(int)ModelIDs.goalkeeper], spawnLocation).GetComponent<Animator>().SetTrigger("Selected");
        if (!PlayerPrefs.HasKey("Goalkeeper"))
        {
            PlayerPrefs.SetString("Goalkeeper", "False");
            unlockButton.SetActive(true);
        }
        else if (PlayerPrefs.GetString("Goalkeeper") == "True")
        {
            PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.goalkeeper);
            unlockButton.SetActive(false);
        }
        else
        {
            unlockButton.SetActive(true);
        }
    }
    public void ChangeToHero()
    {
        SoundManager.Instance.PlayButtonPressedSound(); Destroy(spawnLocation.GetChild(0).gameObject);
        currentlySelectedModel = ModelIDs.hero;
        Instantiate(modelsToSpawn[(int)ModelIDs.hero], spawnLocation).GetComponent<Animator>().SetTrigger("Selected");
        if (!PlayerPrefs.HasKey("Hero"))
        {
            PlayerPrefs.SetString("Hero", "False");
            unlockButton.SetActive(true);
        }
        else if (PlayerPrefs.GetString("Hero") == "True")
        {
            PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.hero);
            unlockButton.SetActive(false);
        }
        else
        {
            unlockButton.SetActive(true);
        }
    }
    public void ChangeToHockey()
    {
        SoundManager.Instance.PlayButtonPressedSound(); Destroy(spawnLocation.GetChild(0).gameObject);
        currentlySelectedModel = ModelIDs.hockey;
        Instantiate(modelsToSpawn[(int)ModelIDs.hockey], spawnLocation).GetComponent<Animator>().SetTrigger("Selected");
        if (!PlayerPrefs.HasKey("Hockey"))
        {
            PlayerPrefs.SetString("Hockey", "False");
            unlockButton.SetActive(true);
        }
        else if (PlayerPrefs.GetString("Hockey") == "True")
        {
            PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.hockey);
            unlockButton.SetActive(false);
        }
        else
        {
            unlockButton.SetActive(true);
        }
    }
    public void ChangeToKarate()
    {
        SoundManager.Instance.PlayButtonPressedSound(); Destroy(spawnLocation.GetChild(0).gameObject);
        currentlySelectedModel = ModelIDs.karate;
        Instantiate(modelsToSpawn[(int)ModelIDs.karate], spawnLocation).GetComponent<Animator>().SetTrigger("Selected");
        if (!PlayerPrefs.HasKey("Karate"))
        {
            PlayerPrefs.SetString("Karate", "False");
            unlockButton.SetActive(true);
        }
        else if (PlayerPrefs.GetString("Karate") == "True")
        {
            PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.karate);
            unlockButton.SetActive(false);
        }
        else
        {
            unlockButton.SetActive(true);
        }
    }
    public void ChangeToRepairer()
    {
        SoundManager.Instance.PlayButtonPressedSound(); Destroy(spawnLocation.GetChild(0).gameObject);
        currentlySelectedModel = ModelIDs.repairer;
        Instantiate(modelsToSpawn[(int)ModelIDs.repairer], spawnLocation).GetComponent<Animator>().SetTrigger("Selected");
        if (!PlayerPrefs.HasKey("Repairer"))
        {
            PlayerPrefs.SetString("Repairer", "False");
            unlockButton.SetActive(true);
        }
        else if (PlayerPrefs.GetString("Repairer") == "True")
        {
            PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.repairer);
            unlockButton.SetActive(false);
        }
        else
        {
            unlockButton.SetActive(true);
        }
    }
    public void ChangeToSkier()
    {
        SoundManager.Instance.PlayButtonPressedSound(); Destroy(spawnLocation.GetChild(0).gameObject);
        currentlySelectedModel = ModelIDs.skier;
        Instantiate(modelsToSpawn[(int)ModelIDs.skier], spawnLocation).GetComponent<Animator>().SetTrigger("Selected");
        if (!PlayerPrefs.HasKey("Skier"))
        {
            PlayerPrefs.SetString("Skier", "False");
            unlockButton.SetActive(true);
        }
        else if (PlayerPrefs.GetString("Skier") == "True")
        {
            PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.skier);
            unlockButton.SetActive(false);
        }
        else
        {
            unlockButton.SetActive(true);
        }
    }
    public void ChangeToTennis()
    {
        SoundManager.Instance.PlayButtonPressedSound(); Destroy(spawnLocation.GetChild(0).gameObject);
        currentlySelectedModel = ModelIDs.tennis;
        Instantiate(modelsToSpawn[(int)ModelIDs.tennis], spawnLocation).GetComponent<Animator>().SetTrigger("Selected");
        if (!PlayerPrefs.HasKey("Tennis"))
        {
            PlayerPrefs.SetString("Tennis", "False");
            unlockButton.SetActive(true);
        }
        else if (PlayerPrefs.GetString("Tennis") == "True")
        {
            PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.tennis);
            unlockButton.SetActive(false);
        }
        else
        {
            unlockButton.SetActive(true);
        }
    }
    public void ChangeToVolleyBall()
    {
        SoundManager.Instance.PlayButtonPressedSound(); Destroy(spawnLocation.GetChild(0).gameObject);
        currentlySelectedModel = ModelIDs.volley;
        Instantiate(modelsToSpawn[(int)ModelIDs.volley], spawnLocation).GetComponent<Animator>().SetTrigger("Selected");
        if (!PlayerPrefs.HasKey("Volleyball"))
        {
            PlayerPrefs.SetString("Volleyball", "False");
            unlockButton.SetActive(true);
        }
        else if (PlayerPrefs.GetString("Volleyball") == "True")
        {
            PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.volley);
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
                    if(allModels[(int)ModelIDs.amazon].GetCostToUnlock() > PlayerPrefs.GetInt("Gold"))
                    {
                        popUpMessage.SetActive(true);
                        popUpMessage.GetComponent<Animator>().SetTrigger("In");
                        Invoke("PopUpClose", 2);
                        unlockButton.SetActive(true);
                    }

                    else
                    {
                        PlayerPrefs.SetString("Amazon", "True");
                        allModels[(int)ModelIDs.amazon].UnlockThisCharacter();
                        RemoveGold(allModels[(int)ModelIDs.amazon].GetCostToUnlock());
                        PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.amazon);
                        popUpMessage2Text.text = "You Have Unlocked The Amazon!";
                        popUpMessage2.SetActive(true);
                        popUpMessage2.GetComponent<Animator>().SetTrigger("In");
                        Invoke("PopUpClose", 2);

                    }
                    break;
                }
            case ModelIDs.baseball:
                {
                    if (allModels[(int)ModelIDs.baseball].GetCostToUnlock() > PlayerPrefs.GetInt("Gold"))
                    {
                        popUpMessage.SetActive(true);
                        popUpMessage.GetComponent<Animator>().SetTrigger("In");
                        Invoke("PopUpClose", 2); unlockButton.SetActive(true);
                    }

                    else
                    {
                        PlayerPrefs.SetString("Baseball", "True");
                        allModels[(int)ModelIDs.baseball].UnlockThisCharacter();
                        RemoveGold(allModels[(int)ModelIDs.baseball].GetCostToUnlock());
                        PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.baseball);
                        popUpMessage2Text.text = "You Have Unlocked The Baseball!";
                        popUpMessage2.SetActive(true);
                        popUpMessage2.GetComponent<Animator>().SetTrigger("In");
                        Invoke("PopUpClose", 2);


                    }
                    break;
                }
            case ModelIDs.bioHazzard:
                {
                    if (allModels[(int)ModelIDs.bioHazzard].GetCostToUnlock() > PlayerPrefs.GetInt("Gold"))
                    {
                        popUpMessage.SetActive(true); popUpMessage.GetComponent<Animator>().SetTrigger("In"); Invoke("PopUpClose", 2); unlockButton.SetActive(true);
                    }

                    else
                    {
                        PlayerPrefs.SetString("BioHaz", "True");
                        allModels[(int)ModelIDs.bioHazzard].UnlockThisCharacter();
                        RemoveGold(allModels[(int)ModelIDs.bioHazzard].GetCostToUnlock());
                        PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.bioHazzard);
                        popUpMessage2Text.text = "You Have Unlocked The BioHazzard!";
                        popUpMessage2.SetActive(true);
                        popUpMessage2.GetComponent<Animator>().SetTrigger("In");
                        Invoke("PopUpClose", 2);


                    }
                    break;
                }
            case ModelIDs.boxer:
                {
                    if (allModels[(int)ModelIDs.boxer].GetCostToUnlock() > PlayerPrefs.GetInt("Gold"))
                    {
                        popUpMessage.SetActive(true); popUpMessage.GetComponent<Animator>().SetTrigger("In"); Invoke("PopUpClose", 2); unlockButton.SetActive(true);
                    }

                    else
                    {
                        PlayerPrefs.SetString("Boxer", "True");
                        allModels[(int)ModelIDs.boxer].UnlockThisCharacter();
                        RemoveGold(allModels[(int)ModelIDs.boxer].GetCostToUnlock());
                        PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.boxer);
                        popUpMessage2Text.text = "You Have Unlocked The Boxer!";
                        popUpMessage2.SetActive(true);
                        popUpMessage2.GetComponent<Animator>().SetTrigger("In");
                        Invoke("PopUpClose", 2);

                    }
                    break;
                }
            case ModelIDs.clown:
                {
                    if (allModels[(int)ModelIDs.clown].GetCostToUnlock() > PlayerPrefs.GetInt("Gold"))
                    {
                        popUpMessage.SetActive(true); popUpMessage.GetComponent<Animator>().SetTrigger("In"); Invoke("PopUpClose", 2); unlockButton.SetActive(true);
                    }

                    else
                    {
                        PlayerPrefs.SetString("Clown", "True");
                        allModels[(int)ModelIDs.clown].UnlockThisCharacter();
                        RemoveGold(allModels[(int)ModelIDs.clown].GetCostToUnlock());
                        PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.clown);
                        popUpMessage2Text.text = "You Have Unlocked The Clown!";
                        popUpMessage2.SetActive(true);
                        popUpMessage2.GetComponent<Animator>().SetTrigger("In");
                        Invoke("PopUpClose", 2);


                    }
                    break;
                }
            case ModelIDs.flatyBoss:
                {
                    if (allModels[(int)ModelIDs.flatyBoss].GetCostToUnlock() > PlayerPrefs.GetInt("Gold"))
                    {
                        popUpMessage.SetActive(true); popUpMessage.GetComponent<Animator>().SetTrigger("In"); Invoke("PopUpClose", 2); unlockButton.SetActive(true);
                    }

                    else
                    {
                        PlayerPrefs.SetString("FlatyBoss", "True");
                        allModels[(int)ModelIDs.flatyBoss].UnlockThisCharacter();
                        RemoveGold(allModels[(int)ModelIDs.flatyBoss].GetCostToUnlock());
                        PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.flatyBoss);
                        popUpMessage2Text.text = "You Have Unlocked The Flaty Boss!";
                        popUpMessage2.SetActive(true);
                        popUpMessage2.GetComponent<Animator>().SetTrigger("In");
                        Invoke("PopUpClose", 2);
                    }
                    break;
                }
            case ModelIDs.footballer:
                {
                    if (allModels[(int)ModelIDs.footballer].GetCostToUnlock() > PlayerPrefs.GetInt("Gold"))
                    {
                        popUpMessage.SetActive(true); popUpMessage.GetComponent<Animator>().SetTrigger("In"); Invoke("PopUpClose", 2); unlockButton.SetActive(true);
                    }

                    else
                    {
                        PlayerPrefs.SetString("Footballer", "True");
                        allModels[(int)ModelIDs.footballer].UnlockThisCharacter();
                        RemoveGold(allModels[(int)ModelIDs.footballer].GetCostToUnlock());
                        PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.footballer);
                        popUpMessage2Text.text = "You Have Unlocked The FootBaller!";
                        popUpMessage2.SetActive(true);
                        popUpMessage2.GetComponent<Animator>().SetTrigger("In");
                        Invoke("PopUpClose", 2);
                    }
                    break;
                }
            case ModelIDs.goalkeeper:
                {
                    if (allModels[(int)ModelIDs.goalkeeper].GetCostToUnlock() > PlayerPrefs.GetInt("Gold"))
                    {
                        popUpMessage.SetActive(true); popUpMessage.GetComponent<Animator>().SetTrigger("In"); Invoke("PopUpClose", 2); unlockButton.SetActive(true);
                    }

                    else
                    {
                        PlayerPrefs.SetString("Goalkeeper", "True");
                        allModels[(int)ModelIDs.goalkeeper].UnlockThisCharacter();
                        RemoveGold(allModels[(int)ModelIDs.goalkeeper].GetCostToUnlock());
                        PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.goalkeeper);
                        popUpMessage2Text.text = "You Have Unlocked The Goal Keeper!";
                        popUpMessage2.SetActive(true);
                        popUpMessage2.GetComponent<Animator>().SetTrigger("In");
                        Invoke("PopUpClose", 2);

                    }
                    break;
                }
            case ModelIDs.hero:
                {
                    if (allModels[(int)ModelIDs.hero].GetCostToUnlock() > PlayerPrefs.GetInt("Gold"))
                    {
                        popUpMessage.SetActive(true); popUpMessage.GetComponent<Animator>().SetTrigger("In"); Invoke("PopUpClose", 2); unlockButton.SetActive(true);
                    }

                    else
                    {
                        PlayerPrefs.SetString("Hero", "True");
                        allModels[(int)ModelIDs.hero].UnlockThisCharacter();
                        RemoveGold(allModels[(int)ModelIDs.hero].GetCostToUnlock());
                        PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.hero);
                        popUpMessage2Text.text = "You Have Unlocked The Hero!";
                        popUpMessage2.SetActive(true);
                        popUpMessage2.GetComponent<Animator>().SetTrigger("In");
                        Invoke("PopUpClose", 2);

                    }
                    break;
                }
            case ModelIDs.hockey:
                {
                    if (allModels[(int)ModelIDs.hockey].GetCostToUnlock() > PlayerPrefs.GetInt("Gold"))
                    {
                        popUpMessage.SetActive(true); popUpMessage.GetComponent<Animator>().SetTrigger("In"); Invoke("PopUpClose", 2); unlockButton.SetActive(true);
                    }

                    else
                    {
                        PlayerPrefs.SetString("Hockey", "True");
                        allModels[(int)ModelIDs.hockey].UnlockThisCharacter();
                        RemoveGold(allModels[(int)ModelIDs.hockey].GetCostToUnlock());
                        PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.hockey);
                        popUpMessage2Text.text = "You Have Unlocked The Hockey Player!";
                        popUpMessage2.SetActive(true);
                        popUpMessage2.GetComponent<Animator>().SetTrigger("In");
                        Invoke("PopUpClose", 2);

                    }
                    break;
                }
            case ModelIDs.karate:
                {
                    if (allModels[(int)ModelIDs.karate].GetCostToUnlock() > PlayerPrefs.GetInt("Gold"))
                    {
                        popUpMessage.SetActive(true); popUpMessage.GetComponent<Animator>().SetTrigger("In"); Invoke("PopUpClose", 2); unlockButton.SetActive(true);
                    }

                    else
                    {
                        PlayerPrefs.SetString("Karate", "True");
                        allModels[(int)ModelIDs.karate].UnlockThisCharacter();
                        RemoveGold(allModels[(int)ModelIDs.karate].GetCostToUnlock());
                        PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.karate);
                        popUpMessage2Text.text = "You Have Unlocked The Karate Master!";
                        popUpMessage2.SetActive(true);
                        popUpMessage2.GetComponent<Animator>().SetTrigger("In");
                        Invoke("PopUpClose", 2);

                    }
                    break;
                }
            case ModelIDs.repairer:
                {
                    if (allModels[(int)ModelIDs.repairer].GetCostToUnlock() > PlayerPrefs.GetInt("Gold"))
                    {
                        popUpMessage.SetActive(true); popUpMessage.GetComponent<Animator>().SetTrigger("In"); Invoke("PopUpClose", 2); unlockButton.SetActive(true);
                    }

                    else
                    {
                        PlayerPrefs.SetString("Repairer", "True");
                        allModels[(int)ModelIDs.repairer].UnlockThisCharacter();
                        RemoveGold(allModels[(int)ModelIDs.repairer].GetCostToUnlock());
                        PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.repairer);
                        popUpMessage2Text.text = "You Have Unlocked The Repairer!";
                        popUpMessage2.SetActive(true);
                        popUpMessage2.GetComponent<Animator>().SetTrigger("In");
                        Invoke("PopUpClose", 2);

                    }
                    break;
                }
            case ModelIDs.roller:
                {
                    if (allModels[(int)ModelIDs.roller].GetCostToUnlock() > PlayerPrefs.GetInt("Gold"))
                    {
                        popUpMessage.SetActive(true); popUpMessage.GetComponent<Animator>().SetTrigger("In"); Invoke("PopUpClose", 2); unlockButton.SetActive(true);
                    }

                    else
                    {
                        PlayerPrefs.SetString("Roller", "True");
                        allModels[(int)ModelIDs.roller].UnlockThisCharacter();
                        RemoveGold(allModels[(int)ModelIDs.roller].GetCostToUnlock());
                        PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.roller);
                        popUpMessage2Text.text = "You Have Unlocked The Roller!";
                        popUpMessage2.SetActive(true);
                        popUpMessage2.GetComponent<Animator>().SetTrigger("In");
                        Invoke("PopUpClose", 2);

                    }
                    break;
                }
            case ModelIDs.skater:
                {
                    if (allModels[(int)ModelIDs.skater].GetCostToUnlock() > PlayerPrefs.GetInt("Gold"))
                    {
                        popUpMessage.SetActive(true); popUpMessage.GetComponent<Animator>().SetTrigger("In"); Invoke("PopUpClose", 2); unlockButton.SetActive(true);
                    }

                    else
                    {
                        PlayerPrefs.SetString("Skater", "True");
                        allModels[(int)ModelIDs.skater].UnlockThisCharacter();
                        RemoveGold(allModels[(int)ModelIDs.skater].GetCostToUnlock());
                        PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.skater);
                        popUpMessage2Text.text = "You Have Unlocked The Skater!";
                        popUpMessage2.SetActive(true);
                        popUpMessage2.GetComponent<Animator>().SetTrigger("In");
                        Invoke("PopUpClose", 2);

                    }
                    break;
                }
            case ModelIDs.skier:
                {
                    if (allModels[(int)ModelIDs.skier].GetCostToUnlock() > PlayerPrefs.GetInt("Gold"))
                    {
                        popUpMessage.SetActive(true); popUpMessage.GetComponent<Animator>().SetTrigger("In"); Invoke("PopUpClose", 2); unlockButton.SetActive(true);
                    }

                    else
                    {
                        PlayerPrefs.SetString("Skier", "True");
                        allModels[(int)ModelIDs.skier].UnlockThisCharacter();
                        RemoveGold(allModels[(int)ModelIDs.skier].GetCostToUnlock());
                        PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.skier);
                        popUpMessage2Text.text = "You Have Unlocked The Skier!";
                        popUpMessage2.SetActive(true);
                        popUpMessage2.GetComponent<Animator>().SetTrigger("In");
                        Invoke("PopUpClose", 2);

                    }
                    break;
                }
            case ModelIDs.tennis:
                {
                    if (allModels[(int)ModelIDs.tennis].GetCostToUnlock() > PlayerPrefs.GetInt("Gold"))
                    {
                        popUpMessage.SetActive(true); popUpMessage.GetComponent<Animator>().SetTrigger("In"); Invoke("PopUpClose", 2); unlockButton.SetActive(true);
                    }

                    else
                    {
                        PlayerPrefs.SetString("Tennis", "True");
                        allModels[(int)ModelIDs.tennis].UnlockThisCharacter();
                        RemoveGold(allModels[(int)ModelIDs.tennis].GetCostToUnlock());
                        PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.tennis);
                        popUpMessage2Text.text = "You Have Unlocked The Tennis Player!";
                        popUpMessage2.SetActive(true);
                        popUpMessage2.GetComponent<Animator>().SetTrigger("In");
                        Invoke("PopUpClose", 2);

                    }
                    break;
                }
            case ModelIDs.volley:
                {
                    if (allModels[(int)ModelIDs.volley].GetCostToUnlock() > PlayerPrefs.GetInt("Gold"))
                    {
                        popUpMessage.SetActive(true); popUpMessage.GetComponent<Animator>().SetTrigger("In"); Invoke("PopUpClose", 2); unlockButton.SetActive(true);
                    }

                    else
                    {
                        PlayerPrefs.SetString("Volleyball", "True");
                        allModels[(int)ModelIDs.volley].UnlockThisCharacter();
                        RemoveGold(allModels[(int)ModelIDs.volley].GetCostToUnlock());
                        PlayerPrefs.SetInt("ChoosenCharacter", (int)ModelIDs.volley);
                        popUpMessage2Text.text = "You Have Unlocked The Volleyball Player!";
                        popUpMessage2.SetActive(true);
                        popUpMessage2.GetComponent<Animator>().SetTrigger("In");
                        Invoke("PopUpClose", 2);

                    }
                    break;
                }

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
        coinInfo.text =PlayerPrefs.GetInt("Gold").ToString();
    }
    public void RemoveGold(int amount)
    {
        PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") - amount);
        coinInfo.text =PlayerPrefs.GetInt("Gold").ToString();
    }

    public void PP()
    {
        Application.OpenURL("https://unconditionalgames.blogspot.com/2020/01/privacy-policy-this-privacy-policy.html");
    }
}