using UnityEngine;

namespace Game
{
    public class Checkpointpr : MonoBehaviour
    {
        private readonly string _playerTagpr = "Player";
        private readonly string _enemyTagpr = "Enemy";
    
        private PlayerScript _playerpr;
        public Transform[] correspondingPlatform;
        private void Start()
        {
            _playerpr = FindObjectOfType<PlayerScript>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(_playerTagpr))
            {
                _playerpr.lastCheckpoint = transform;
            }

            else if(other.CompareTag(_enemyTagpr) && correspondingPlatform.Length != 0)
            {
                Enemy enemy = other.GetComponent<Enemy>();
                enemy.nearestPlatform = correspondingPlatform[Random.Range(0, correspondingPlatform.Length)];
                enemy.lastCheckpoint = transform;
            }
        }
    }
}
