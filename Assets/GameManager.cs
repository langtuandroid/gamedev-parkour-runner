using System.Collections;
using System.Collections.Generic;
using Game;
using UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerScript player;
    public CameraControlspr mainCamera;
    public Transform allPlayersTransform;
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
        player = FindObjectOfType<PlayerScript>();
        player.canMove = false;
        spawnedModel = Instantiate(availableModels[PlayerPrefs.GetInt("ChoosenCharacter")], player.transform);
        player.playerAnimator = spawnedModel.GetComponent<Animator>();
        player.playerAnimator.speed = 1.2f;
        player.playerBody = spawnedModel.transform;
        player.playerBody.transform.localScale = new Vector3(.4f, .4f, .4f);
        player.playerBody.transform.localRotation = Quaternion.Euler(0, 0, 0);
        StartCoroutine(spawnedModel.GetComponent<ChangeAnimator>().EnableTrails());
        foreach (Transform players in allPlayersTransform)
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
            mainCamera.CameraSmoothing = Mathf.Lerp(mainCamera.CameraSmoothing, 0, 0.01f);
            main.simulationSpeed = Mathf.Lerp(main.simulationSpeed,0,0.005f);
        }

    }

    public void StartGame()
    {
        if (gameStarted) return;
        gameStarted = true;
        player.canMove = true;
        player.playerAnimator.SetBool("CanRun", true);
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
        
        player = FindObjectOfType<PlayerScript>();
        main = player.speedEffect.main;
        mainCamera = FindObjectOfType<CameraControlspr>();
        PauseGame();
        StartCoroutine(StopPlayer());
        gameEnded = true;
        UIManagerpr.Instancepr.EnableEndWindowUIpr();
        //AdManager.instance.ShowInterstitial();
    }
   
    IEnumerator StopPlayer()
    {
        yield return new WaitForSeconds(.1f);
        mainCamera.GetComponent<Animator>().SetTrigger("End Game");
        player.canMove = false;
        player.playerRigidbody.isKinematic = true;
        player.playerAnimator.speed = 0;
        player.confettiEffect.Play();
        player.fallingConfetti.Play();
    }
    
    public void PauseGame()
    {
        //AdManager.instance.ShowInterstitial();
        gamePaused = true;
        int i = 0;
        playerAnimatorSpeed = player.playerAnimator.speed;
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
        player.canMove = false;
        player.playerAnimator.speed = 0;
        player.playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
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
        player.canMove = true;
        player.playerAnimator.speed = playerAnimatorSpeed;
        player.playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        gamePaused = false;
    }
    public float GetLightAmount()
    {
        return lightAmount;
    }
}
