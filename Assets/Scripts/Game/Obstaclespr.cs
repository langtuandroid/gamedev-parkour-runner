using MainControllers;
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

        private PlayerScriptpr _playerpr;
        public ObstaclesTypes obstaclesType;

        [Inject]
        private void  Context(PlayerScriptpr player)
        {
            _playerpr = player;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SoundManager.Instance.PlayBoostSound();
                AndroidManagerpr.HapticFeedback();
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
            
                Enemypr enemypr = other.GetComponent<Enemypr>();

                switch (obstaclesType)
                {
                    case ObstaclesTypes.Booster:
                    {
                        enemypr.forwardMoveSpeed += 1;
                        if (!enemypr.grounded && !GameManager.Instance.gamePaused)
                        {
                            enemypr.forwardMoveSpeed += 2;
                            enemypr.jumpStrength += 10;
                            enemypr.playerAnimator.speed += 0.1f;
                        }
                        StartCoroutine(enemypr.Jump());
                        break;
                    }
                    case ObstaclesTypes.GoUnder:
                    {
                        if (!GameManager.Instance.gamePaused)
                        {
                            enemypr.forwardMoveSpeed += .5f;
                            enemypr.jumpStrength += 0.5f;
                            StartCoroutine(enemypr.PerformSlide());
                            enemypr.playerAnimator.speed += 0.04f;
                        }

                        break;
                    }
                    case ObstaclesTypes.JumpOver:
                    {
                        if (!GameManager.Instance.gamePaused)
                        {
                            enemypr.forwardMoveSpeed += .5f;
                            enemypr.jumpStrength += 0.5f;
                            StartCoroutine(enemypr.JumpOverObstacles());
                            enemypr.playerAnimator.speed += 0.04f;
                        }
                        break;
                    }
                    case ObstaclesTypes.SideBooster:
                    {
                        if (!GameManager.Instance.gamePaused)
                        {
                            enemypr.jumpStrength += .5f;
                            enemypr.forwardMoveSpeed += .5f;
                            enemypr.playerAnimator.speed += 0.04f;
                        }
                        break;
                    }
                    case ObstaclesTypes.Vaultable:
                    {
                        if (!GameManager.Instance.gamePaused)
                        {
                            enemypr.jumpStrength += .5f;
                            enemypr.forwardMoveSpeed += .5f;
                            enemypr.playerAnimator.speed += 0.04f;
                        }
                        StartCoroutine(enemypr.Vault());
                        break;
                    }
                }
            }

        }
    }
}
