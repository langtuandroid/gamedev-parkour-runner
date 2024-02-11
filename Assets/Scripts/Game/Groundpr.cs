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
            if (other.tag == "Player")
            {
                _playerpr.transform.position = _playerpr.lastCheckpoint.position;
                _playerpr.ResetMovementValues();
            }
            else if (other.tag == "Enemy")
            {
                enemypr = other.GetComponent<Enemypr>();
                enemypr.ResetMovementValues();
                enemypr.transform.position = enemypr.lastCheckpoint.position;
            }

        }
    }
}