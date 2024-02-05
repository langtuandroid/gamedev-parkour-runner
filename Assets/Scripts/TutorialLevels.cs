using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialLevels : MonoBehaviour
{
    bool slowdown = false;
    bool speedup = false;
    public int tutorialNo;
    public Animator textAnimator;
    public Text text;
    
    private void Update()
    {
        if(slowdown)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0.3f, 0.005f);
        }
        if (speedup)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1, 0.005f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            textAnimator.SetTrigger("SlideIn");
            switch(tutorialNo)
            {
                case 0:
                    {
                        text.text = "Auto Jump";
                        break;
                    }
                case 1:
                    {
                        text.text = "Auto Roll";
                        break;
                    }
                case 2:
                    {
                        text.text = "Auto Jump Over";
                        break;
                    }
                case 3:
                    {
                        text.text = "Auto Vault";
                        break;
                    }
                case 4:
                    {
                        text.text = "Auto Wall Run";
                        break;
                    }
                case 5:
                    {
                        text.text = "Auto Climb";
                        break;
                    }
                case 6:
                    {
                        text.text = "Wall Jump";
                        break;
                    }

            }
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            textAnimator.SetTrigger("SlideOut");
        }
    }
}
