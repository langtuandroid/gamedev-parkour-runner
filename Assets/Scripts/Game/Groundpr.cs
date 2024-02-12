using MainControllers;
using UnityEngine;
using Zenject;

namespace Game
{
    public class Groundpr : MonoBehaviour
    {
        private Enemypr enemypr;
        private PlayerScriptpr _playerpr;
        
        [Inject]
        private void  Context(PlayerScriptpr player)
        {
            _playerpr = player;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _playerpr.transform.position = _playerpr.lastCheckpointpr.position;
                _playerpr.ResetMovementValuespr();
            }
            else if (other.CompareTag("Enemy"))
            {
                enemypr = other.GetComponent<Enemypr>();
                enemypr.ResetMovementValuespr();
                enemypr.transform.position = enemypr.lastCheckpointpr.position;
            }

        }
    }
}