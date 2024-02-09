using System.Collections;
using System.Collections.Generic;
using Game;
using UI;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public List<Enemy> enemies;
    public ParticleSystem.MainModule main;
    public bool gameEnded = false;
    public bool gameStarted = false;
    public GameObject[] availableModels;
    public GameObject spawnedModel;
    public float[] enemiesAnimatorSpeed;
    public float playerAnimatorSpeed;
    public bool gamePaused = false;
    public float lightAmount;
    
    private  PlayerScript _playerpr;
    private CameraControlspr _mainCamerapr;
    private Transform _allPlayersTransformpr;

    [Inject]
    private void  Context(PlayerScript player, CameraControlspr mainCamera)
    {
        _playerpr = player;
        _mainCamerapr = mainCamera;
        _allPlayersTransformpr = _playerpr.gameObject.transform.parent;
    }
    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        SoundManager.Instance.PlayerBGMusicTwo();
        _playerpr = FindObjectOfType<PlayerScript>();
        _playerpr.canMove = false;
        spawnedModel = Instantiate(availableModels[PlayerPrefs.GetInt("ChoosenCharacter")], _playerpr.transform);
        _playerpr.playerAnimator = spawnedModel.GetComponent<Animator>();
        _playerpr.playerAnimator.speed = 1.2f;
        _playerpr.playerBody = spawnedModel.transform;
        _playerpr.playerBody.transform.localScale = new Vector3(.4f, .4f, .4f);
        _playerpr.playerBody.transform.localRotation = Quaternion.Euler(0, 0, 0);
        StartCoroutine(spawnedModel.GetComponent<ChangeAnimator>().EnableTrails());
        foreach (Transform players in _allPlayersTransformpr)
        {
            if (players.GetComponent<Enemy>() != null)
            {
                Enemy enemyScript = players.GetComponent<Enemy>();
                enemies.Add(enemyScript);
                enemyScript.canMove = false;
                GameObject gg =  Instantiate(availableModels[Random.Range(0,availableModels.Length)], players.transform);
                enemyScript.playerBody = gg.transform;
                enemyScript.playerAnimator = gg.GetComponent<Animator>();
                enemyScript.playerAnimator.speed = 1.2f;
                enemyScript.playerBody.transform.localScale = new Vector3(.4f, .4f, .4f);
                enemyScript.playerBody.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            
        }

        //AdManager.instance.LoadBanner();
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
        _playerpr.canMove = true;
        _playerpr.playerAnimator.SetBool("CanRun", true);
        if (enemies.Count != 0)
        {
            foreach (Enemy ene in enemies)
            {
                ene.canMove = true;
                ene.playerAnimator.SetBool("CanRun", true);
                
            }
        }
    }


    public void EndGame()
    {
        SoundManager.Instance.PlayEndSound();
        
        _playerpr = FindObjectOfType<PlayerScript>();
        main = _playerpr.speedEffect.main;
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
        _playerpr.canMove = false;
        _playerpr.playerRigidbody.isKinematic = true;
        _playerpr.playerAnimator.speed = 0;
        _playerpr.confettiEffect.Play();
        _playerpr.fallingConfetti.Play();
    }
    
    public void PauseGame()
    {
        //AdManager.instance.ShowInterstitial();
        gamePaused = true;
        int i = 0;
        playerAnimatorSpeed = _playerpr.playerAnimator.speed;
        foreach (Enemy ene in enemies)
        {
            if (ene != null)
            {
                ene.canMove = false;
                enemiesAnimatorSpeed[i] = ene.playerAnimator.speed;
                ene.playerAnimator.speed = 0;
                ene.playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
                i++;
            }
        }
        _playerpr.canMove = false;
        _playerpr.playerAnimator.speed = 0;
        _playerpr.playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void ResumeGame()
    {
        int i = 0;
        foreach (Enemy ene in enemies)
        {
            if (ene != null)
            {
                ene.canMove = true;
                ene.playerAnimator.speed = enemiesAnimatorSpeed[i];
                ene.playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                i++;
            }
        }
        _playerpr.canMove = true;
        _playerpr.playerAnimator.speed = playerAnimatorSpeed;
        _playerpr.playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        gamePaused = false;
    }
    public float GetLightAmount()
    {
        return lightAmount;
    }
}
