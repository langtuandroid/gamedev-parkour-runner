using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public enum ObstaclesTypes
    {
        JumpOver,
        GoUnder,
        Booster,
        SideBooster,
        Vaultable

    }

    public PlayerScript player;
    public ObstaclesTypes obstaclesType;


    private void Start()
    {
        player = FindObjectOfType<PlayerScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.PlayBoostSound();
            AndroidManager.HapticFeedback();
            if (!player.speedEffect.isPlaying)
                player.speedEffect.Play();
            player.speedUpEffect.Play();

            if (player.forwardMoveSpeed >= 5.5f)
                player.trailEffect.gameObject.SetActive(true);
            ParticleSystem.MainModule main = player.speedEffect.main;
            main.maxParticles += 50;
            switch (obstaclesType)
            {
                
                case ObstaclesTypes.Booster:
                    {
                        if (player.forwardMoveSpeed < player.defaultForwardMoveSpeed + 4 && !GameManager.Instance.gamePaused)
                        {
                            player.forwardMoveSpeed += .7f;
                            player.jumpStrength += 0.5f;
                            player.playerAnimator.speed += 0.06f;

                            if (!player.grounded)
                            {
                                player.forwardMoveSpeed += 2;
                                player.jumpStrength += 10;
                                player.playerAnimator.speed += 0.12f;
                            }
                            else if(player.forwardMoveSpeed > player.defaultForwardMoveSpeed + 2.8f)
                            {
                                player.forwardMoveSpeed += 3;
                                player.jumpStrength += 5;
                                player.playerAnimator.speed += 0.12f;
                            }
                            else

                            {
                                player.forwardMoveSpeed += .7f;
                                player.jumpStrength += 0.5f;
                                player.playerAnimator.speed += 0.06f;
                            }
                        }
                        StartCoroutine(player.Jump());
                        break;
                    }
                case ObstaclesTypes.GoUnder:
                    {
                        if (player.forwardMoveSpeed < player.defaultForwardMoveSpeed + 4 && !GameManager.Instance.gamePaused)
                        {
                            player.forwardMoveSpeed += .7f;
                            player.jumpStrength += 0.5f;
                            player.playerAnimator.speed += 0.06f;
                        }
                        StartCoroutine(player.PerformSlide());
                        break;
                    }
                case ObstaclesTypes.JumpOver:
                    {
                        if (player.forwardMoveSpeed < player.defaultForwardMoveSpeed + 4 && !GameManager.Instance.gamePaused)
                        {
                            player.forwardMoveSpeed += .7f;
                            player.jumpStrength += 0.5f;
                            player.playerAnimator.speed += 0.06f;
                        }

                        StartCoroutine(player.JumpOverObstacles());
                        break;
                    }
                case ObstaclesTypes.SideBooster:
                    {
                        if (player.forwardMoveSpeed < player.defaultForwardMoveSpeed + 4 && !GameManager.Instance.gamePaused)
                        {
                            player.jumpStrength += .5f;
                            player.forwardMoveSpeed += .7f;
                            player.playerAnimator.speed += 0.06f;
                        }
                        break;
                    }
                case ObstaclesTypes.Vaultable:
                    {
                        if (player.forwardMoveSpeed < player.defaultForwardMoveSpeed + 4 && !GameManager.Instance.gamePaused)
                        {
                            player.jumpStrength += .5f;
                            player.forwardMoveSpeed += .7f;
                            player.playerAnimator.speed += 0.06f;
                        }
                        StartCoroutine(player.Vault());

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
