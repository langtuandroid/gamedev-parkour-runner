using MainControllers;
using UnityEngine;
using Zenject;

namespace Game
{
    public class Checkpointpr : MonoBehaviour
    {
        private readonly string _playerTagpr = "Player";
        private readonly string _enemyTagpr = "Enemy";
        
        public Transform[] correspondingPlatform;
        private PlayerScriptpr _playerpr;
        
        [Inject]
        private void  Context(PlayerScriptpr player)
        {
            _playerpr = player;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(_playerTagpr))
            {
                _playerpr.lastCheckpointpr = transform;
            }

            else if(other.CompareTag(_enemyTagpr) && correspondingPlatform.Length != 0)
            {
                Enemypr enemypr = other.GetComponent<Enemypr>();
                enemypr.nearestPlatformpr = correspondingPlatform[Random.Range(0, correspondingPlatform.Length)];
                enemypr.lastCheckpointpr = transform;
            }
        }
    }
}
