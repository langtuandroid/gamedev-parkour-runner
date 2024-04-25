using System.Collections;
using System.Collections.Generic;
using Game;
using Integration;
using MainControllers;
using UI;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public List<Enemypr> enemies;
    public ParticleSystem.MainModule main;
    public bool gameEnded = false;
    public bool gameStarted = false;
    public GameObject[] availableModels;
    public GameObject spawnedModel;
    public float[] enemiesAnimatorSpeed;
    public float playerAnimatorSpeed;
    public bool gamePaused = false;
    public float lightAmount;
    
    private  PlayerScriptpr _playerpr;
    private CameraControlspr _mainCamerapr;
    private Transform _allPlayersTransformpr;
    private AdMobController _adMobController;
    private IAPService _iapService;
   
    private const string LoadLevelCountKey = "LoadLevelCount";

    private int loadLevelCount = 0; 

    [Inject]
    private void  Context(PlayerScriptpr player, CameraControlspr mainCamera, AdMobController adMobController, IAPService iapService)
    {
        _playerpr = player;
        _mainCamerapr = mainCamera;
        _allPlayersTransformpr = _playerpr.gameObject.transform.parent;
        _adMobController = adMobController;
        _iapService = iapService;
    }
    
    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
        loadLevelCount = PlayerPrefs.GetInt(LoadLevelCountKey, 0);
    }
    private void Start()
    {
        SoundManager.Instance.PlayerBGMusicTwo();
        _playerpr = FindObjectOfType<PlayerScriptpr>();
        _playerpr.CanMove = false;
        spawnedModel = Instantiate(availableModels[PlayerPrefs.GetInt("ChoosenCharacter")], _playerpr.transform);
        _playerpr.playerAnimatorpr = spawnedModel.GetComponent<Animator>();
        _playerpr.playerAnimatorpr.speed = 1.2f;
        _playerpr.playerBodypr = spawnedModel.transform;
        _playerpr.playerBodypr.transform.localScale = new Vector3(.4f, .4f, .4f);
        _playerpr.playerBodypr.transform.localRotation = Quaternion.Euler(0, 0, 0);
        StartCoroutine(spawnedModel.GetComponent<ChangeAnimator>().EnableTrails());
        foreach (Transform players in _allPlayersTransformpr)
        {
            if (players.GetComponent<Enemypr>() != null)
            {
                Enemypr enemyprScript = players.GetComponent<Enemypr>();
                enemies.Add(enemyprScript);
                enemyprScript.canMovepr = false;
                GameObject gg =  Instantiate(availableModels[Random.Range(0,availableModels.Length)], players.transform);
                enemyprScript.playerBodypr = gg.transform;
                enemyprScript.playerAnimatorpr = gg.GetComponent<Animator>();
                enemyprScript.playerAnimatorpr.speed = 1.2f;
                enemyprScript.playerBodypr.transform.localScale = new Vector3(.4f, .4f, .4f);
                enemyprScript.playerBodypr.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            
        }

        var _noAds = PlayerPrefs.GetInt(_adMobController.noAdsKey, 0) == 1;
        if (!_noAds)
        {
            ShowIntegration();
        }
       
    }

    private void ShowIntegration()
    {

        loadLevelCount++;

        if (loadLevelCount % 6 == 0)
        {
            _adMobController.ShowInterstitialAd();
            _adMobController.ShowBanner(true);
        }
        else if (loadLevelCount % 2 == 0)
        {
            _adMobController.ShowInterstitialAd();
            _adMobController.ShowBanner(true);
        }
        else if (loadLevelCount % 3 == 0)
        {
            _iapService.ShowSubscriptionPanel();
        }

        if (loadLevelCount >= 3)
        {
            loadLevelCount = 0;
        }
        PlayerPrefs.SetInt(LoadLevelCountKey, loadLevelCount);
        PlayerPrefs.Save(); // Сохраняем изменения в PlayerPrefs
    }
    
    private void Update()
    {

        if (gameEnded)
        {
            _mainCamerapr.CameraSmoothing = Mathf.Lerp(_mainCamerapr.CameraSmoothing, 0, 0.01f);
            main.simulationSpeed = Mathf.Lerp(main.simulationSpeed,0,0.005f);
        }

    }

    public void StartGame()
    {
        if (gameStarted) return;
        gameStarted = true;
        _playerpr.CanMove = true;
        _playerpr.playerAnimatorpr.SetBool("CanRun", true);
        if (enemies.Count != 0)
        {
            foreach (Enemypr ene in enemies)
            {
                ene.canMovepr = true;
                ene.playerAnimatorpr.SetBool("CanRun", true);
                
            }
        }
    }


    public void EndGame()
    {
        SoundManager.Instance.PlayEndSound();
        
        _playerpr = FindObjectOfType<PlayerScriptpr>();
        main = _playerpr.speedEffectpr.main;
        _mainCamerapr = FindObjectOfType<CameraControlspr>();
        PauseGame();
        StartCoroutine(StopPlayer());
        gameEnded = true;
        UIManagerpr.Instancepr.EnableEndWindowUIpr();
        //AdManager.instance.ShowInterstitial();
    }
   
    IEnumerator StopPlayer()
    {
        yield return new WaitForSeconds(.1f);
        _mainCamerapr.GetComponent<Animator>().SetTrigger("End Game");
        _playerpr.CanMove = false;
        _playerpr.playerRigidbodypr.isKinematic = true;
        _playerpr.playerAnimatorpr.speed = 0;
        _playerpr.confettiEffectpr.Play();
        _playerpr.fallingConfettipr.Play();
    }
    
    public void PauseGame()
    {
        //AdManager.instance.ShowInterstitial();
        gamePaused = true;
        int i = 0;
        playerAnimatorSpeed = _playerpr.playerAnimatorpr.speed;
        foreach (Enemypr ene in enemies)
        {
            if (ene != null)
            {
                ene.canMovepr = false;
                enemiesAnimatorSpeed[i] = ene.playerAnimatorpr.speed;
                ene.playerAnimatorpr.speed = 0;
                ene.playerRigidbodypr.constraints = RigidbodyConstraints.FreezeAll;
                i++;
            }
        }
        _playerpr.CanMove = false;
        _playerpr.playerAnimatorpr.speed = 0;
        _playerpr.playerRigidbodypr.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void ResumeGame()
    {
        int i = 0;
        foreach (Enemypr ene in enemies)
        {
            if (ene != null)
            {
                ene.canMovepr = true;
                ene.playerAnimatorpr.speed = enemiesAnimatorSpeed[i];
                ene.playerRigidbodypr.constraints = RigidbodyConstraints.FreezeRotation;
                i++;
            }
        }
        _playerpr.CanMove = true;
        _playerpr.playerAnimatorpr.speed = playerAnimatorSpeed;
        _playerpr.playerRigidbodypr.constraints = RigidbodyConstraints.FreezeRotation;
        gamePaused = false;
    }
    public float GetLightAmount()
    {
        return lightAmount;
    }
}
