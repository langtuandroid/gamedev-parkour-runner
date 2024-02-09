using UnityEngine;

namespace Game
{
    public class Groundpr : MonoBehaviour
    {
        private PlayerScript playerpr;
        private Enemy enemypr;
        private void Start()
        {
            playerpr = FindObjectOfType<PlayerScript>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                playerpr.transform.position = playerpr.lastCheckpoint.position;
                playerpr.ResetMovementValues();
            }
            else if (other.tag == "Enemy")
            {
                enemypr = other.GetComponent<Enemy>();
                enemypr.ResetMovementValues();
                enemypr.transform.position = enemypr.lastCheckpoint.position;
            }

        }
    }
}