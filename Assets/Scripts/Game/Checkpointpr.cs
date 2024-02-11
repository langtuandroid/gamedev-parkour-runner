using UnityEngine;
using Zenject;

namespace Game
{
    public class Checkpointpr : MonoBehaviour
    {
        private readonly string _playerTagpr = "Player";
        private readonly string _enemyTagpr = "Enemy";
        
        public Transform[] correspondingPlatform;
        private PlayerScript _playerpr;
        
        [Inject]
        private void  Context(PlayerScript player)
        {
            _playerpr = player;
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
