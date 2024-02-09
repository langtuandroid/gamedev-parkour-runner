using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ChangeAnimator : MonoBehaviour
{
    public RuntimeAnimatorController mainMenuAnimator;
    Animator currentAnimator;
    public int animationID;
    public Image locker;
    public TrailRenderer leftHandTrail, rightHandTrail;
    public Renderer material;
    public int costToUnlock;
    public GameObject unlockedBanner;

    private void Start()
    {
        leftHandTrail.enabled = false;
        rightHandTrail.enabled = false;
        
        
        currentAnimator = GetComponent<Animator>();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            currentAnimator.runtimeAnimatorController = mainMenuAnimator;
            currentAnimator.SetInteger("AnimationID", animationID);
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
        material = gameObject.transform.Find("body").GetComponent<Renderer>();
        locker.gameObject.SetActive(true);
        unlockedBanner.SetActive(false);
        //material.materials[0].SetColor("_BaseColor", Color.gray);
        //material.materials[1].SetColor("_BaseColor", Color.gray);
    }
    public void UnlockThisCharacter()
    {
        material = gameObject.transform.Find("body").GetComponent<Renderer>();
        locker.gameObject.SetActive(false);
        unlockedBanner.SetActive(true);
        //material.materials[0].SetColor("_BaseColor", Color.white);
        //material.materials[1].SetColor("_BaseColor", Color.yellow);
    }
    public void AlreadyUnlocked()
    {
        material = gameObject.transform.Find("body").GetComponent<Renderer>();
        locker.gameObject.SetActive(false);
        unlockedBanner.SetActive(true);
        //material.materials[0].SetColor("_BaseColor", Color.white);
        //material.materials[1].SetColor("_BaseColor", Color.yellow);

    }
    public int GetCostToUnlock()
    {
        return costToUnlock;
    }
}