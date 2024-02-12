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
                if (!_playerpr.speedEffectpr.isPlaying)
                    _playerpr.speedEffectpr.Play();
                _playerpr.speedUpEffectpr.Play();

                if (_playerpr.forwardMoveSpeedpr >= 5.5f)
                    _playerpr.trailEffectpr.gameObject.SetActive(true);
                ParticleSystem.MainModule main = _playerpr.speedEffectpr.main;
                main.maxParticles += 50;
                switch (obstaclesType)
                {
                
                    case ObstaclesTypes.Booster:
                    {
                        if (_playerpr.forwardMoveSpeedpr < _playerpr.defaultForwardMoveSpeedpr + 4 && !GameManager.Instance.gamePaused)
                        {
                            _playerpr.forwardMoveSpeedpr += .7f;
                            _playerpr.jumpStrengthpr += 0.5f;
                            _playerpr.playerAnimatorpr.speed += 0.06f;

                            if (!_playerpr.Groundedpr)
                            {
                                _playerpr.forwardMoveSpeedpr += 2;
                                _playerpr.jumpStrengthpr += 10;
                                _playerpr.playerAnimatorpr.speed += 0.12f;
                            }
                            else if(_playerpr.forwardMoveSpeedpr > _playerpr.defaultForwardMoveSpeedpr + 2.8f)
                            {
                                _playerpr.forwardMoveSpeedpr += 3;
                                _playerpr.jumpStrengthpr += 5;
                                _playerpr.playerAnimatorpr.speed += 0.12f;
                            }
                            else

                            {
                                _playerpr.forwardMoveSpeedpr += .7f;
                                _playerpr.jumpStrengthpr += 0.5f;
                                _playerpr.playerAnimatorpr.speed += 0.06f;
                            }
                        }
                        StartCoroutine(_playerpr.Jumppr());
                        break;
                    }
                    case ObstaclesTypes.GoUnder:
                    {
                        if (_playerpr.forwardMoveSpeedpr < _playerpr.defaultForwardMoveSpeedpr + 4 && !GameManager.Instance.gamePaused)
                        {
                            _playerpr.forwardMoveSpeedpr += .7f;
                            _playerpr.jumpStrengthpr += 0.5f;
                            _playerpr.playerAnimatorpr.speed += 0.06f;
                        }
                        StartCoroutine(_playerpr.PerformSlidepr());
                        break;
                    }
                    case ObstaclesTypes.JumpOver:
                    {
                        if (_playerpr.forwardMoveSpeedpr < _playerpr.defaultForwardMoveSpeedpr + 4 && !GameManager.Instance.gamePaused)
                        {
                            _playerpr.forwardMoveSpeedpr += .7f;
                            _playerpr.jumpStrengthpr += 0.5f;
                            _playerpr.playerAnimatorpr.speed += 0.06f;
                        }

                        StartCoroutine(_playerpr.JumpOverObstaclespr());
                        break;
                    }
                    case ObstaclesTypes.SideBooster:
                    {
                        if (_playerpr.forwardMoveSpeedpr < _playerpr.defaultForwardMoveSpeedpr + 4 && !GameManager.Instance.gamePaused)
                        {
                            _playerpr.jumpStrengthpr += .5f;
                            _playerpr.forwardMoveSpeedpr += .7f;
                            _playerpr.playerAnimatorpr.speed += 0.06f;
                        }
                        break;
                    }
                    case ObstaclesTypes.Vaultable:
                    {
                        if (_playerpr.forwardMoveSpeedpr < _playerpr.defaultForwardMoveSpeedpr + 4 &&
                            !GameManager.Instance.gamePaused)
                        {
                            _playerpr.jumpStrengthpr += .5f;
                            _playerpr.forwardMoveSpeedpr += .7f;
                            _playerpr.playerAnimatorpr.speed += 0.06f;
                        }

                        StartCoroutine(_playerpr.Vaultpr());
                        break;
                    }
                    case ObstaclesTypes.Knockback:
                    {
                        if (_playerpr.forwardMoveSpeedpr < _playerpr.defaultForwardMoveSpeedpr + 4 && !GameManager.Instance.gamePaused)
                        {
                            _playerpr.jumpStrengthpr -= 0.1f;
                            _playerpr.forwardMoveSpeedpr -= 0.1f;
                            _playerpr.playerAnimatorpr.speed -= 0.01f;
                        }
                        _playerpr.CanKnockback = true;
                        StartCoroutine(_playerpr.Knockbackpr());

                        break;
                    }
                }
            }
            else if (other.CompareTag("Enemy"))
            {
            
                Enemypr enemypr = other.GetComponent<Enemypr>();

                switch (obstaclesType)
                {
                    case ObstaclesTypes.Booster:
                    {
                        enemypr.forwardMoveSpeedpr += 1;
                        if (!enemypr.groundedpr && !GameManager.Instance.gamePaused)
                        {
                            enemypr.forwardMoveSpeedpr += 2;
                            enemypr.jumpStrengthpr += 10;
                            enemypr.playerAnimatorpr.speed += 0.1f;
                        }
                        StartCoroutine(enemypr.Jumppr());
                        break;
                    }
                    case ObstaclesTypes.GoUnder:
                    {
                        if (!GameManager.Instance.gamePaused)
                        {
                            enemypr.forwardMoveSpeedpr += .5f;
                            enemypr.jumpStrengthpr += 0.5f;
                            StartCoroutine(enemypr.PerformSlidepr());
                            enemypr.playerAnimatorpr.speed += 0.04f;
                        }

                        break;
                    }
                    case ObstaclesTypes.JumpOver:
                    {
                        if (!GameManager.Instance.gamePaused)
                        {
                            enemypr.forwardMoveSpeedpr += .5f;
                            enemypr.jumpStrengthpr += 0.5f;
                            StartCoroutine(enemypr.JumpOverObstaclespr());
                            enemypr.playerAnimatorpr.speed += 0.04f;
                        }
                        break;
                    }
                    case ObstaclesTypes.SideBooster:
                    {
                        if (!GameManager.Instance.gamePaused)
                        {
                            enemypr.jumpStrengthpr += .5f;
                            enemypr.forwardMoveSpeedpr += .5f;
                            enemypr.playerAnimatorpr.speed += 0.04f;
                        }
                        break;
                    }
                    case ObstaclesTypes.Vaultable:
                    {
                        if (!GameManager.Instance.gamePaused)
                        {
                            enemypr.jumpStrengthpr += .5f;
                            enemypr.forwardMoveSpeedpr += .5f;
                            enemypr.playerAnimatorpr.speed += 0.04f;
                        }
                        StartCoroutine(enemypr.Vaultpr());
                        break;
                    }
                }
            }

        }
    }
}
