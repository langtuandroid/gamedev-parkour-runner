using UnityEngine;
using Zenject;

namespace Game
{
    public class EdgeWallpr : MonoBehaviour
    {
        private PlayerScript _playerpr;
        
        [Inject]
        private void  Context(PlayerScript player)
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
                StartCoroutine(other.GetComponent<Enemy>().Jump());
            }
        }

    }
}
