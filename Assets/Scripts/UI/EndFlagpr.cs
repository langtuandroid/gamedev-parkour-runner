using System.Collections.Generic;
using System.Linq;
using Game;
using MainControllers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class EndFlagpr : MonoBehaviour
    {
        [SerializeField]
        private float _distanceBetweenPlayerpr;
        [SerializeField] 
        private float _distanceBeweenPlayerAtStartpr;
        [SerializeField]
        private List<DistanceMeterpr> _distanceFromtheEndpr;
        [SerializeField]
        private int _startPospr = 0;
        
        private  Slider _progressSliderpr;
        private PlayerScriptpr _playerpr;
        private Transform _playerParentpr;
        
        [Inject]
        private void  Context(PlayerScriptpr player)
        {
            _playerpr = player;
            _playerParentpr = player.gameObject.transform.parent;
        }

        private void Start()
        {
            _progressSliderpr = FindObjectOfType<Slider>();
            _distanceBeweenPlayerAtStartpr = Vector3.Distance(transform.position, _playerpr.transform.position);
            foreach (Transform playersPos in _playerParentpr)
            {
                _distanceFromtheEndpr.Add(playersPos.GetComponent<DistanceMeterpr>());
            }
        }
        
        private void Update()
        {
            _distanceBetweenPlayerpr = Vector3.Distance(transform.position, _playerpr.transform.position);
            _progressSliderpr.value = 100 -  (_distanceBetweenPlayerpr / _distanceBeweenPlayerAtStartpr) * 100 ;

            {
                for (int i = _startPospr; i < _playerParentpr.childCount; i++)
                {
                    _distanceFromtheEndpr[i].distancepr = DistanceFinderpr(_distanceFromtheEndpr[i].transform);
                }
                _distanceFromtheEndpr = _distanceFromtheEndpr.OrderBy(i => i.GetComponent<DistanceMeterpr>().distancepr).ToList();

                for (int i = _startPospr; i < _distanceFromtheEndpr.Count; i++)
                {
                    _distanceFromtheEndpr[i].positionInRacepr = i + 1;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.Instance.EndGame();
                gameObject.SetActive(false);
            }
            else if (other.CompareTag("Enemy"))
            {
                _startPospr++;
                other.gameObject.SetActive(false);
            }
        }

        private float DistanceFinderpr(Transform player)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            return distance;
        }
    }
}
