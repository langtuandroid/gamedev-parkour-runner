﻿using UnityEngine;

namespace Game
{
    public class CameraControlspr : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _offsetpr;
        [SerializeField]
        private Vector3 _playerPositionpr;
        [SerializeField]
        private PlayerScript _playerpr;
        [SerializeField]
        private Vector3 _velocitypr = Vector3.zero;
        [SerializeField]
        private float _cameraSmoothingpr = 0.2f;

        public float CameraSmoothing
        {
            get => _cameraSmoothingpr;
            set => _cameraSmoothingpr = value;
        }

        private void Start()
        {
            _offsetpr = transform.position - _playerpr.transform.position;
        }
        
        private void FixedUpdate()
        {
            _playerPositionpr = _playerpr.transform.position;
            transform.position = Vector3.SmoothDamp(transform.position, _playerPositionpr + _offsetpr, ref _velocitypr, CameraSmoothing);
        }
    }
}
