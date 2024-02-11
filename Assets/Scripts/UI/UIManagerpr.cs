using System.Collections;
using Game;
using MainControllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class UIManagerpr : MonoBehaviour
    {
        [SerializeField] 
        private Button _pausepr;
        public static UIManagerpr Instancepr;
        PlayerScriptpr playerpr;
        public FloatingJoystick joystickpr;
        public Transform endGameWindowpr;
        public GameObject nextButtonpr;
        public GameObject retryButtonpr;
        public Text infoTextWindowpr;
        public GameObject handUIpr;
        public GameObject gameStartButtonpr;
        public GameObject shopButtonpr;
        public Text playerPospr;
        public Text endPlayerPositionpr;
        public Animator gameRunningpr;
        public GameObject pauseMenupr;
        public Animator UImanagerAnimatorpr;
        public Text popUpTextpr;
        public GameObject popUpMessageWindowpr;
        public Text levelInfopr;
        public Text coinInfopr;
        public GameObject coinCounterpr;
        public ParticleSystem coinExplosionpr;
        public ParticleControlScript coinExplosionSettingspr;
        int goldAtStartpr = 0;
        int goldEarnedpr = 0;
    
        private void Awake()
        {
            if (!Instancepr)
            {
                Instancepr = this;
            }
            _pausepr.onClick.AddListener(PausedGamepr);
        }
        private void Start()
        {
            coinInfopr.text = PlayerPrefs.GetInt("Gold").ToString();
            goldAtStartpr = PlayerPrefs.GetInt("Gold");
            playerpr = FindObjectOfType<PlayerScriptpr>();
            playerpr.joystick = joystickpr;
            playerpr.GetComponent<DistanceMeterpr>().playerPospr = playerPospr;
            UImanagerAnimatorpr = GetComponentInChildren<Animator>();
            UImanagerAnimatorpr.SetTrigger("In");
            coinCounterpr.SetActive(true);
        }

        private void OnDestroy()
        {
            _pausepr.onClick.RemoveListener(PausedGamepr);
        }

        private void PausedGamepr()
        {
            if (!GameManager.Instance.gamePaused)
            {
                GameManager.Instance.PauseGame();
                if (!pauseMenupr.activeSelf)
                {
                    pauseMenupr.SetActive(true);
                    pauseMenupr.GetComponent<Animator>().SetTrigger("In");
                }
            }
            else
            {
                ResumeGamepr();
                ClosePauseMenupr();
            }
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.Instance.gameEnded && GameManager.Instance.gameStarted)
            {
                PausedGamepr();
            }
        }
        public void EnableEndWindowUIpr()
        {
            StartCoroutine(ShowEndGameDetailspr());
        }
        IEnumerator ShowEndGameDetailspr()
        {
            gameRunningpr.SetTrigger("End");
            yield return new WaitForSeconds(3);
            coinCounterpr.SetActive(true);
            endGameWindowpr.gameObject.SetActive(true);
        
            if (playerpr.gameObject.GetComponent<DistanceMeterpr>().positionInRacepr == 1)
            {
            
                retryButtonpr.SetActive(false);
                infoTextWindowpr.gameObject.SetActive(false);
                PlayerPrefs.SetInt("LevelProgression", PlayerPrefs.GetInt("LevelProgression",2) + 1);
                endPlayerPositionpr.text = playerpr.GetComponent<DistanceMeterpr>().playerPospr.text;
                goldEarnedpr = Random.Range(100, 200);
                coinExplosionSettingspr.coinsCount = goldEarnedpr / 3;
                yield return new WaitForSeconds(1);
                coinExplosionpr.gameObject.SetActive(true);
                yield return new WaitForSeconds(1);
                for (int i =0;i< goldEarnedpr; i++ )
                {
                    AddGoldpr(1);
                    yield return null;
                }
            }
            else
            {
                nextButtonpr.SetActive(false);
                endPlayerPositionpr.text = playerpr.GetComponent<DistanceMeterpr>().playerPospr.text;
                goldEarnedpr = Random.Range(10, 100);
                coinExplosionSettingspr.coinsCount = goldEarnedpr / 3;
                yield return new WaitForSeconds(1);
                coinExplosionpr.gameObject.SetActive(true);
                yield return new WaitForSeconds(1);
                for (int i = 0; i < goldEarnedpr; i++)
                {
                    AddGoldpr(1);
                    yield return null;
                }
            }
        }

        public void LoadLevelpr()
        {

            SoundManager.Instance.PlayButtonPressedSound();
            if (PlayerPrefs.GetInt("Gold") != goldAtStartpr + goldEarnedpr)
                PlayerPrefs.SetInt("Gold", goldAtStartpr + goldEarnedpr);
            StartCoroutine(LoadLevelAsyncpr());
            //AdManager.instance.ShowAd();

        }

        public IEnumerator LoadLevelAsyncpr()
        {
            AsyncOperation loadingprogress = SceneManager.LoadSceneAsync(PlayerPrefs.GetInt("LevelProgression",2));
            loadingprogress.allowSceneActivation = true;
            while (!loadingprogress.isDone)
            {
                yield return null;
            }
        }

        public void StartGamePressedpr()
        {
            handUIpr.SetActive(false);
            levelInfopr.text = (PlayerPrefs.GetInt("LevelProgression",2)-1).ToString();
            gameRunningpr.SetTrigger("Start");
            UImanagerAnimatorpr.SetTrigger("Out");
            gameStartButtonpr.SetActive(false);
            coinCounterpr.SetActive(false);
            GameManager.Instance.StartGame();
        }
        public void ShowPopUppr()
        {
            SoundManager.Instance.PlayButtonPressedSound();
            popUpMessageWindowpr.SetActive(true);        
            popUpMessageWindowpr.GetComponent<Animator>().SetTrigger("In");
            popUpTextpr.text = "You Will Lose Any Unsaved Progress!";

        }

        public void ClosePopUppr()
        {
            SoundManager.Instance.PlayButtonPressedSound();
            StartCoroutine(DisableWindowpr(popUpMessageWindowpr));
        }
        public void GoToMainMenupr()
        {
            SoundManager.Instance.PlayButtonPressedSound();
            SceneManager.LoadScene(0);
        }
        public void ClosePauseMenupr()
        {
            SoundManager.Instance.PlayButtonPressedSound();
            StartCoroutine(DisableWindowpr(pauseMenupr));
        }
        IEnumerator DisableWindowpr(GameObject window)
        {
            window.GetComponent<Animator>().SetTrigger("Out");
            yield return new WaitForSeconds(0.2f);
            window.SetActive(false);

        }

        public void ResumeGamepr()
        {
            SoundManager.Instance.PlayButtonPressedSound();
            ClosePauseMenupr();
            if(popUpMessageWindowpr.activeSelf)
            {
                StartCoroutine(DisableWindowpr(popUpMessageWindowpr));
            }
            GameManager.Instance.ResumeGame();

        }
        public void AddGoldpr(int amount)
        {
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + amount);
            coinInfopr.text = PlayerPrefs.GetInt("Gold").ToString();
        }

    }
}