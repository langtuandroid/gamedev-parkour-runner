using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ChangeAnimator : MonoBehaviour
{
    public RuntimeAnimatorController mainMenuAnimatorpr;
    [SerializeField]
    private int _animationIDpr;
    [SerializeField]
    private TrailRenderer leftHandTrail, rightHandTrail;
    [SerializeField]
    private int _costToUnlockprpr;
    [SerializeField]
    private Image _lockerpr;
    [SerializeField]
    public GameObject _unlockedBannerpr;

    //private Renderer material;
    private Animator currentAnimator;

    private void Start()
    {
        leftHandTrail.enabled = false;
        rightHandTrail.enabled = false;
        
        
        currentAnimator = GetComponent<Animator>();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            currentAnimator.runtimeAnimatorController = mainMenuAnimatorpr;
            currentAnimator.SetInteger("AnimationID", _animationIDpr);
        }
        else
        {
            currentAnimator.SetBool("NotMainMenu", true);
        }
    }
    public IEnumerator EnableTrails()
    {
        yield return new WaitForSeconds(0.1f);
        leftHandTrail.enabled = true;
        rightHandTrail.enabled = true;
    }
    public void LockThisCharacter()
    {
        //material = gameObject.transform.Find("body").GetComponent<Renderer>();
        _lockerpr.gameObject.SetActive(true);
        _unlockedBannerpr.SetActive(false);
        //material.materials[0].SetColor("_BaseColor", Color.gray);
        //material.materials[1].SetColor("_BaseColor", Color.gray);
    }
    public void UnlockThisCharacter()
    {
        //material = gameObject.transform.Find("body").GetComponent<Renderer>();
        _lockerpr.gameObject.SetActive(false);
        _unlockedBannerpr.SetActive(true);
        //material.materials[0].SetColor("_BaseColor", Color.white);
        //material.materials[1].SetColor("_BaseColor", Color.yellow);
    }
    public void AlreadyUnlocked()
    {
        //material = gameObject.transform.Find("body").GetComponent<Renderer>();
        _lockerpr.gameObject.SetActive(false);
        _unlockedBannerpr.SetActive(true);
        //material.materials[0].SetColor("_BaseColor", Color.white);
        //material.materials[1].SetColor("_BaseColor", Color.yellow);

    }
    public int GetCostToUnlock()
    {
        return _costToUnlockprpr;
    }
}