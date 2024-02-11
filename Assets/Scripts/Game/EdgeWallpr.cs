using MainControllers;
using UnityEngine;
using Zenject;

namespace Game
{
    public class EdgeWallpr : MonoBehaviour
    {
        private PlayerScriptpr _playerpr;
        
        [Inject]
        private void  Context(PlayerScriptpr player)
        {
            _playerpr = player;
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(_playerpr.Jump());
            }
            else if(other.CompareTag("Enemy"))
            {
                StartCoroutine(other.GetComponent<Enemypr>().Jump());
            }
        }

    }
}
