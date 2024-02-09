using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TutorialLevelspr : MonoBehaviour
    {
        private readonly int SlideIn = Animator.StringToHash("SlideIn");
        private readonly int SlideOut = Animator.StringToHash("SlideOut");
        private readonly string _player = "Player";
        
        [SerializeField]
        private bool slowdownpr = false;
        [SerializeField]
        private bool speeduppr = false;
        [SerializeField]
        private int tutorialNopr;
        [SerializeField]
        private Animator textAnimatorpr;
        [SerializeField]
        private Text textpr;
        

        private void Update()
        {
            if(slowdownpr)
            {
                Time.timeScale = Mathf.Lerp(Time.timeScale, 0.3f, 0.005f);
            }
            if (speeduppr)
            {
                Time.timeScale = Mathf.Lerp(Time.timeScale, 1, 0.005f);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(_player))
            {
                textAnimatorpr.SetTrigger(SlideIn);
                textpr.text = tutorialNopr switch
                {
                    0 => "Auto Jump",
                    1 => "Auto Roll",
                    2 => "Auto Jump Over",
                    3 => "Auto Vault",
                    4 => "Auto Wall Run",
                    5 => "Auto Climb",
                    6 => "Wall Jump",
                    _ => textpr.text
                };
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_player))
            {
                textAnimatorpr.SetTrigger(SlideOut);
            }
        }
    }
}
