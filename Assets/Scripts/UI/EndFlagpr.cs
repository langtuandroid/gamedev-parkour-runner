using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EndFlagpr : MonoBehaviour
    {
        public Slider progressSlider;
        [SerializeField]
        private float _distanceBetweenPlayerpr;
        [SerializeField] 
        private float _distanceBeweenPlayerAtStartpr;
        [SerializeField]
        private PlayerScript _playerpr;
        [SerializeField]
        private List<DistanceMeterpr> _distanceFromtheEndpr;
        [SerializeField] 
        private Transform _playerParentpr;
        [SerializeField]
        private int _startPospr = 0;

        private void Start()
        {
            _playerpr = FindObjectOfType<PlayerScript>();
            _distanceBeweenPlayerAtStartpr = Vector3.Distance(transform.position, _playerpr.transform.position);
            progressSlider = FindObjectOfType<Slider>();
            foreach (Transform playersPos in _playerParentpr)
            {
                _distanceFromtheEndpr.Add(playersPos.GetComponent<DistanceMeterpr>());
            }
        }
        
        private void Update()
        {
            _distanceBetweenPlayerpr = Vector3.Distance(transform.position, _playerpr.transform.position);
            progressSlider.value = 100 -  (_distanceBetweenPlayerpr / _distanceBeweenPlayerAtStartpr) * 100 ;

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
            if (other.tag == "Player")
            {
                GameManager.Instance.EndGame();
                gameObject.SetActive(false);
            }
            else if (other.tag == "Enemy")
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
