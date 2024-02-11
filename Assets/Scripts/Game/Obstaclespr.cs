using UnityEngine;
using Zenject;

namespace Game
{
    public class Obstaclespr : MonoBehaviour
    {
        public enum ObstaclesTypes
        {
            JumpOver,
            GoUnder,
            Booster,
            SideBooster,
            Vaultable,
            Knockback

        }

        private PlayerScript _playerpr;
        public ObstaclesTypes obstaclesType;

        [Inject]
        private void  Context(PlayerScript player)
        {
            _playerpr = player;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SoundManager.Instance.PlayBoostSound();
                AndroidManager.HapticFeedback();
                if (!_playerpr.speedEffect.isPlaying)
                    _playerpr.speedEffect.Play();
                _playerpr.speedUpEffect.Play();

                if (_playerpr.forwardMoveSpeed >= 5.5f)
                    _playerpr.trailEffect.gameObject.SetActive(true);
                ParticleSystem.MainModule main = _playerpr.speedEffect.main;
                main.maxParticles += 50;
                switch (obstaclesType)
                {
                
                    case ObstaclesTypes.Booster:
                    {
                        if (_playerpr.forwardMoveSpeed < _playerpr.defaultForwardMoveSpeed + 4 && !GameManager.Instance.gamePaused)
                        {
                            _playerpr.forwardMoveSpeed += .7f;
                            _playerpr.jumpStrength += 0.5f;
                            _playerpr.playerAnimator.speed += 0.06f;

                            if (!_playerpr.grounded)
                            {
                                _playerpr.forwardMoveSpeed += 2;
                                _playerpr.jumpStrength += 10;
                                _playerpr.playerAnimator.speed += 0.12f;
                            }
                            else if(_playerpr.forwardMoveSpeed > _playerpr.defaultForwardMoveSpeed + 2.8f)
                            {
                                _playerpr.forwardMoveSpeed += 3;
                                _playerpr.jumpStrength += 5;
                                _playerpr.playerAnimator.speed += 0.12f;
                            }
                            else

                            {
                                _playerpr.forwardMoveSpeed += .7f;
                                _playerpr.jumpStrength += 0.5f;
                                _playerpr.playerAnimator.speed += 0.06f;
                            }
                        }
                        StartCoroutine(_playerpr.Jump());
                        break;
                    }
                    case ObstaclesTypes.GoUnder:
                    {
                        if (_playerpr.forwardMoveSpeed < _playerpr.defaultForwardMoveSpeed + 4 && !GameManager.Instance.gamePaused)
                        {
                            _playerpr.forwardMoveSpeed += .7f;
                            _playerpr.jumpStrength += 0.5f;
                            _playerpr.playerAnimator.speed += 0.06f;
                        }
                        StartCoroutine(_playerpr.PerformSlide());
                        break;
                    }
                    case ObstaclesTypes.JumpOver:
                    {
                        if (_playerpr.forwardMoveSpeed < _playerpr.defaultForwardMoveSpeed + 4 && !GameManager.Instance.gamePaused)
                        {
                            _playerpr.forwardMoveSpeed += .7f;
                            _playerpr.jumpStrength += 0.5f;
                            _playerpr.playerAnimator.speed += 0.06f;
                        }

                        StartCoroutine(_playerpr.JumpOverObstacles());
                        break;
                    }
                    case ObstaclesTypes.SideBooster:
                    {
                        if (_playerpr.forwardMoveSpeed < _playerpr.defaultForwardMoveSpeed + 4 && !GameManager.Instance.gamePaused)
                        {
                            _playerpr.jumpStrength += .5f;
                            _playerpr.forwardMoveSpeed += .7f;
                            _playerpr.playerAnimator.speed += 0.06f;
                        }
                        break;
                    }
                    case ObstaclesTypes.Vaultable:
                    {
                        if (_playerpr.forwardMoveSpeed < _playerpr.defaultForwardMoveSpeed + 4 &&
                            !GameManager.Instance.gamePaused)
                        {
                            _playerpr.jumpStrength += .5f;
                            _playerpr.forwardMoveSpeed += .7f;
                            _playerpr.playerAnimator.speed += 0.06f;
                        }

                        StartCoroutine(_playerpr.Vault());
                        break;
                    }
                    case ObstaclesTypes.Knockback:
                    {
                        if (_playerpr.forwardMoveSpeed < _playerpr.defaultForwardMoveSpeed + 4 && !GameManager.Instance.gamePaused)
                        {
                            _playerpr.jumpStrength -= 0.1f;
                            _playerpr.forwardMoveSpeed -= 0.1f;
                            _playerpr.playerAnimator.speed -= 0.01f;
                        }
                        _playerpr.canKnockback = true;
                        StartCoroutine(_playerpr.Knockback());

                        break;
                    }
                }
            }
            else if (other.tag == "Enemy")
            {
            
                Enemy enemy = other.GetComponent<Enemy>();

                switch (obstaclesType)
                {
                    case ObstaclesTypes.Booster:
                    {
                        enemy.forwardMoveSpeed += 1;
                        if (!enemy.grounded && !GameManager.Instance.gamePaused)
                        {
                            enemy.forwardMoveSpeed += 2;
                            enemy.jumpStrength += 10;
                            enemy.playerAnimator.speed += 0.1f;
                        }
                        StartCoroutine(enemy.Jump());
                        break;
                    }
                    case ObstaclesTypes.GoUnder:
                    {
                        if (!GameManager.Instance.gamePaused)
                        {
                            enemy.forwardMoveSpeed += .5f;
                            enemy.jumpStrength += 0.5f;
                            StartCoroutine(enemy.PerformSlide());
                            enemy.playerAnimator.speed += 0.04f;
                        }

                        break;
                    }
                    case ObstaclesTypes.JumpOver:
                    {
                        if (!GameManager.Instance.gamePaused)
                        {
                            enemy.forwardMoveSpeed += .5f;
                            enemy.jumpStrength += 0.5f;
                            StartCoroutine(enemy.JumpOverObstacles());
                            enemy.playerAnimator.speed += 0.04f;
                        }
                        break;
                    }
                    case ObstaclesTypes.SideBooster:
                    {
                        if (!GameManager.Instance.gamePaused)
                        {
                            enemy.jumpStrength += .5f;
                            enemy.forwardMoveSpeed += .5f;
                            enemy.playerAnimator.speed += 0.04f;
                        }
                        break;
                    }
                    case ObstaclesTypes.Vaultable:
                    {
                        if (!GameManager.Instance.gamePaused)
                        {
                            enemy.jumpStrength += .5f;
                            enemy.forwardMoveSpeed += .5f;
                            enemy.playerAnimator.speed += 0.04f;
                        }
                        StartCoroutine(enemy.Vault());
                        break;
                    }
                }
            }

        }
    }
}
