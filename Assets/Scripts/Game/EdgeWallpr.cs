using UnityEngine;

namespace Game
{
    public class EdgeWallpr : MonoBehaviour
    {
        private PlayerScript playerpr;
        private void Start()
        {
            playerpr = FindObjectOfType<PlayerScript>();
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(playerpr.Jump());
            }
            else if(other.tag == "Enemy")
            {
                StartCoroutine(other.GetComponent<Enemy>().Jump());
            }
        }

    }
}
