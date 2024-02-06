using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UIManagerpr : MonoBehaviour
    {
        public Transform endGameWindow;
        public GameObject nextButton;
        public GameObject retryButton;
        public Text infoTextWindow;
        PlayerScript player;
        public static UIManagerpr Instance;
        public GameObject handUI;
        public GameObject gameStartButton;
        public GameObject shopButton;
        public FloatingJoystick joystick;
        public Text playerPos;
        public Text endPlayerPosition;
        public Animator gameRunning;
        public GameObject pauseMenu;
        public Animator UImanagerAnimator;
        public Text popUpText;
        public GameObject popUpMessageWindow;
        public Text levelInfo;
        public Text coinInfo;
        public GameObject coinCounter;
        public ParticleSystem coinExplosion;
        public ParticleControlScript coinExplosionSettings;
        int goldAtStart = 0;
        int goldEarned = 0;
    
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
        }
        private void Start()
        {
            coinInfo.text = PlayerPrefs.GetInt("Gold").ToString();
            goldAtStart = PlayerPrefs.GetInt("Gold");
            player = FindObjectOfType<PlayerScript>();
            player.joystick = joystick;
            player.GetComponent<DistanceMeter>().playerPos = playerPos;
            UImanagerAnimator = GetComponentInChildren<Animator>();
            UImanagerAnimator.SetTrigger("In");
            coinCounter.SetActive(true);


        }
        private void Update()
        {
            //if (Application.platform == RuntimePlatform.Android)
            //{
            if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.Instance.gameEnded && GameManager.Instance.gameStarted)
            {
                if (!GameManager.Instance.gamePaused)
                {
                    GameManager.Instance.PauseGame();
                    if (!pauseMenu.activeSelf)
                    {
                        pauseMenu.SetActive(true);
                        pauseMenu.GetComponent<Animator>().SetTrigger("In");
                    }
                }
                else
                {
                    ResumeGame();
                    ClosePauseMenu();
                }

                return;
            }
            //}
        }
        public void EnableEndWindowUI()
        {
            StartCoroutine(ShowEndGameDetails());
        }
        IEnumerator ShowEndGameDetails()
        {
            gameRunning.SetTrigger("End");
            yield return new WaitForSeconds(3);
            coinCounter.SetActive(true);
            endGameWindow.gameObject.SetActive(true);
        
            if (player.gameObject.GetComponent<DistanceMeter>().positionInRace == 1)
            {
            
                retryButton.SetActive(false);
                infoTextWindow.gameObject.SetActive(false);
                PlayerPrefs.SetInt("LevelProgression", PlayerPrefs.GetInt("LevelProgression",2) + 1);
                endPlayerPosition.text = player.GetComponent<DistanceMeter>().playerPos.text;
                goldEarned = Random.Range(100, 200);
                coinExplosionSettings.coinsCount = goldEarned / 3;
                yield return new WaitForSeconds(1);
                coinExplosion.gameObject.SetActive(true);
                yield return new WaitForSeconds(1);
                for (int i =0;i< goldEarned; i++ )
                {
                    AddGold(1);
                    yield return null;
                }
            }
            else
            {
                nextButton.SetActive(false);
                endPlayerPosition.text = player.GetComponent<DistanceMeter>().playerPos.text;
                goldEarned = Random.Range(10, 100);
                coinExplosionSettings.coinsCount = goldEarned / 3;
                yield return new WaitForSeconds(1);
                coinExplosion.gameObject.SetActive(true);
                yield return new WaitForSeconds(1);
                for (int i = 0; i < goldEarned; i++)
                {
                    AddGold(1);
                    yield return null;
                }
            }
        }

        public void LoadLevel()
        {

            SoundManager.Instance.PlayButtonPressedSound();
            if (PlayerPrefs.GetInt("Gold") != goldAtStart + goldEarned)
                PlayerPrefs.SetInt("Gold", goldAtStart + goldEarned);
            StartCoroutine(LoadLevelAsync());
            //AdManager.instance.ShowAd();

        }

        public IEnumerator LoadLevelAsync()
        {
            AsyncOperation loadingprogress = SceneManager.LoadSceneAsync(PlayerPrefs.GetInt("LevelProgression",2));
            loadingprogress.allowSceneActivation = true;
            while (!loadingprogress.isDone)
            {
                yield return null;
            }
        }

        public void StartGamePressed()
        {
            handUI.SetActive(false);
            levelInfo.text = (PlayerPrefs.GetInt("LevelProgression",2)-1).ToString();
            gameRunning.SetTrigger("Start");
            UImanagerAnimator.SetTrigger("Out");
            gameStartButton.SetActive(false);
            coinCounter.SetActive(false);
            GameManager.Instance.StartGame();
        }
        public void ShowPopUp()
        {
            SoundManager.Instance.PlayButtonPressedSound();
            popUpMessageWindow.SetActive(true);        
            popUpMessageWindow.GetComponent<Animator>().SetTrigger("In");
            popUpText.text = "You Will Lose Any Unsaved Progress!";

        }

        public void ClosePopUp()
        {
            SoundManager.Instance.PlayButtonPressedSound();
            StartCoroutine(DisableWindow(popUpMessageWindow));
        }
        public void GoToMainMenu()
        {
            SoundManager.Instance.PlayButtonPressedSound();
            SceneManager.LoadScene(0);
        }
        public void ClosePauseMenu()
        {
            SoundManager.Instance.PlayButtonPressedSound();
            StartCoroutine(DisableWindow(pauseMenu));
        }
        IEnumerator DisableWindow(GameObject window)
        {
            window.GetComponent<Animator>().SetTrigger("Out");
            yield return new WaitForSeconds(0.2f);
            window.SetActive(false);

        }

        public void ResumeGame()
        {
            SoundManager.Instance.PlayButtonPressedSound();
            ClosePauseMenu();
            if(popUpMessageWindow.activeSelf)
            {
                StartCoroutine(DisableWindow(popUpMessageWindow));
            }
            GameManager.Instance.ResumeGame();

        }
        public void AddGold(int amount)
        {
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + amount);
            coinInfo.text = PlayerPrefs.GetInt("Gold").ToString();
        }

    }
}