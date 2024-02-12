using System.Collections;
using UnityEngine;

namespace MainControllers
{
    public class PlayerScriptpr : MonoBehaviour
    {
        public bool CanKnockback
        {
            get => canKnockbackpr;
            set => canKnockbackpr = value;
        }
        public bool CanMove
        {
            get => canMovepr;
            set => canMovepr = value;
        }
        public bool Groundedpr
        {
            get => groundedpr;
            set => groundedpr = value;
        }
        
        
        private bool canClimbpr = true;
        private bool canJumppr = true;
        private bool canMoveForwardpr = true;
        private bool canGrabSideWallspr = true;
        private bool canSlidepr = true;
        private bool canPuffpr = true;
        private bool canVaultpr = true;
        private bool canMovepr = true;
        private bool canKnockbackpr;
        

        [Header("Settings")]
        public FloatingJoystick joystickpr;
        public float rotationSpeedpr;
        public float forwardMoveSpeedpr;
        public float defaultForwardMoveSpeedpr;
        public float upMoveSpeedpr;
        private float defaultUpMoveSpeedpr;
        private float knockbackForcepr = 15f;
        private float knockbackDurationpr = 1f;
        public float raycastMaxDistancepr;
        public LayerMask environmentpr;
        public float strafeSpeedpr;
        private float defaultStrafeSpeedpr;
        public float jumpStrengthpr;
        private float defaultJumpStrengthpr;
        public float obstaclesJumpStrengthpr;
        private float defaultObstaclesJumpStrengthpr;
        public float rotationSmoothingpr;
        private Quaternion leftRotationTargetpr;
        private Quaternion rightRotationTargetpr;
        private Quaternion frontRotationTargetLeftpr;
        private Quaternion frontRotationTargetRightpr;
        public Transform lastCheckpointpr;
        private float onAirTimepr;

        [Header("Player State Bools")]
        private bool groundedpr;
        private bool onLeftWallpr;
        private bool onRightWallpr;
        private bool onFrontWallpr;
        private bool onFrontWallSecondarypr;
        private Vector3 forceDirectionpr;

        [Header("Animator Hash")]
        private int animatorGroundedpr;
        private int animatorLeftWallpr;
        private int animatorRightWallpr;
        private int animatorFrontWallpr;
        private int animatorJumppr;
        private int animatorSlidepr;
        private int animatorFallpr;
        private int animatorStandUppr;
        private int animatorGetUppr;

        [Header("Referrences")]
        public Transform raycastPositionBottompr;
        public Transform raycastPositionToppr;
        public Rigidbody playerRigidbodypr;
        public Animator playerAnimatorpr;
        public Transform playerBodypr;

        [Header("Effects")]
        public ParticleSystem jumpPuffpr;
        public ParticleSystem landPuffpr;
        public ParticleSystem speedEffectpr;
        public ParticleSystem speedUpEffectpr;
        public TrailRenderer trailEffectpr;
        public ParticleSystem confettiEffectpr;
        public ParticleSystem fallingConfettipr;

        // [Header("UI")]
        // public Text playerName;
        // public Text playerPosition;

        private void Awake()
        {
            animatorGroundedpr = Animator.StringToHash("Grounded");
            animatorLeftWallpr = Animator.StringToHash("LeftWall");
            animatorRightWallpr = Animator.StringToHash("RightWall");
            animatorFrontWallpr = Animator.StringToHash("FrontWall"); 
            animatorJumppr = Animator.StringToHash("Jump");
            animatorSlidepr = Animator.StringToHash("Slide");
            animatorFallpr = Animator.StringToHash("Fall");
        }
        
        private void Start()
        {
            playerRigidbodypr = GetComponent<Rigidbody>();
        
            //playerAnimator.SetBool("CanRun", true);
            leftRotationTargetpr = Quaternion.Euler(0, -45, 0);
            rightRotationTargetpr = Quaternion.Euler(0, 45, 0);
            frontRotationTargetLeftpr = Quaternion.Euler(0, 0, 20);
            frontRotationTargetRightpr = Quaternion.Euler(0, 0, -20);

            defaultForwardMoveSpeedpr = forwardMoveSpeedpr;
            defaultJumpStrengthpr = jumpStrengthpr;
            defaultObstaclesJumpStrengthpr = obstaclesJumpStrengthpr;
            defaultStrafeSpeedpr = strafeSpeedpr;
            defaultUpMoveSpeedpr = upMoveSpeedpr;
        }

        private void Update()
        {
            if (Physics.Raycast(raycastPositionBottompr.position, transform.forward, 2, environmentpr) && Groundedpr)
            {
                StartCoroutine(Jumppr());
            }
        }
        
        private void FixedUpdate()
        {
            float mY = joystickpr.Vertical;
            float mX = joystickpr.Horizontal;
            float pX = Input.GetAxis("Horizontal");
            float pY = Input.GetAxis("Vertical");

            CheckPlayerStatepr();
            if (onRightWallpr)
            {
                mX = Mathf.Clamp(mX, -1, 0);
                pX = Mathf.Clamp(pX, -1, 0);
            }
            else if (onLeftWallpr)
            {
                mX = Mathf.Clamp(mX, 0, 1);
                pX = Mathf.Clamp(pX, 0, 1);
            }
            else if(!CanMove)
            {
                mX = 0;
                mY = 0;
            }

            if (canMoveForwardpr && CanMove)
            {
                transform.Translate(Vector3.forward * Time.fixedDeltaTime * forwardMoveSpeedpr, Space.World);
                // transform.position += Vector3.forward * forwardMoveSpeed * Time.fixedDeltaTime;
            }
            
            if (onFrontWallpr || onFrontWallSecondarypr)
            {
                MoveUppr();
            }
        

            if (!Groundedpr && !onRightWallpr && !onLeftWallpr && !onFrontWallpr)
            {
                onAirTimepr += Time.deltaTime;
                var velocity = playerRigidbodypr.velocity;
                velocity = new Vector3(0, velocity.y, velocity.z);
                playerRigidbodypr.velocity = velocity;
                transform.Translate(Vector3.right * Time.deltaTime * strafeSpeedpr * mX, Space.World);
                transform.Translate(Vector3.right * Time.deltaTime * strafeSpeedpr * pX, Space.World);
            }
            else
            {
                onAirTimepr = 0;
                transform.Translate(Vector3.right * Time.deltaTime * strafeSpeedpr * mX, Space.World);
                transform.Translate(Vector3.right * Time.deltaTime * strafeSpeedpr * pX, Space.World);
            }
        
            if (mX < -0.2f)
            {
                if (!onLeftWallpr && !onRightWallpr && !onFrontWallpr)
                    playerBodypr.rotation = Quaternion.Slerp(playerBodypr.rotation, leftRotationTargetpr, rotationSmoothingpr);
                else if (onFrontWallpr)
                    playerBodypr.rotation = Quaternion.Slerp(playerBodypr.rotation, frontRotationTargetLeftpr, rotationSmoothingpr);
            }
            else if (mX > 0.2f)
            {
                if(!onLeftWallpr && !onRightWallpr && !onFrontWallpr)
                    playerBodypr.rotation = Quaternion.Slerp(playerBodypr.rotation, rightRotationTargetpr, rotationSmoothingpr);
                else if(onFrontWallpr)
                    playerBodypr.rotation = Quaternion.Slerp(playerBodypr.rotation, frontRotationTargetRightpr, rotationSmoothingpr);
            }
            else
            {
                playerBodypr.rotation = Quaternion.Slerp(playerBodypr.rotation, Quaternion.Euler(0, 0, 0), rotationSmoothingpr);
            }

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawRay(raycastPositionBottompr.position, transform.forward * raycastMaxDistancepr);
            Gizmos.DrawRay(raycastPositionBottompr.position, transform.forward * raycastMaxDistancepr);
            Gizmos.DrawRay(raycastPositionBottompr.position, transform.right * raycastMaxDistancepr);
            Gizmos.DrawRay(raycastPositionBottompr.position, -transform.right * raycastMaxDistancepr);
            Gizmos.DrawRay(raycastPositionBottompr.position, -transform.up * .35f);
            Gizmos.DrawRay(raycastPositionBottompr.position, transform.forward * 2);
        }

        private void CheckPlayerStatepr()
        {
         bool bottomRaycastHit = Physics.Raycast(raycastPositionBottompr.position, transform.forward, raycastMaxDistancepr, environmentpr);
         bool topRaycastHit = Physics.Raycast(raycastPositionToppr.position, transform.forward, raycastMaxDistancepr, environmentpr);
         bool leftWallRaycastHit = Physics.Raycast(transform.position, -transform.right, raycastMaxDistancepr, environmentpr);
         bool rightWallRaycastHit = Physics.Raycast(transform.position, transform.right, raycastMaxDistancepr, environmentpr);
         
         if (bottomRaycastHit || topRaycastHit)
         {
             onFrontWallpr = true;
             onFrontWallSecondarypr = topRaycastHit;
             StartCoroutine(ClimbUppr());
             ResetMovementValuespr();
             playerAnimatorpr.SetBool(animatorFrontWallpr, true);
             forceDirectionpr = Vector3.up;
         }
         else
         {
             onFrontWallpr = false;
             onFrontWallSecondarypr = false;
             playerAnimatorpr.SetBool(animatorFrontWallpr, false);
         }

         if (Physics.Raycast(raycastPositionBottompr.position, -transform.up, .35f, environmentpr))
         {
             Groundedpr = true;
             playerAnimatorpr.SetBool(animatorGroundedpr, true);
             forceDirectionpr = Vector3.up;
             if (GameManager.Instance.gameStarted)
                 StartCoroutine(LandPuffpr());
         }
         else
         {
             Groundedpr = false;
             playerAnimatorpr.SetBool(animatorGroundedpr, false);
         }

         if (leftWallRaycastHit)
         {
             onLeftWallpr = true;
             playerAnimatorpr.SetBool(animatorLeftWallpr, true);
             if (!Groundedpr)
                 playerRigidbodypr.velocity = new Vector3(playerRigidbodypr.velocity.x, Mathf.Clamp(playerRigidbodypr.velocity.y, -3f, 30), playerRigidbodypr.velocity.z);
             forceDirectionpr = Vector3.up + new Vector3(0.2f, 0, 0);
         }
         else
         {
             onLeftWallpr = false;
             playerAnimatorpr.SetBool(animatorLeftWallpr, false);
         }

         if (rightWallRaycastHit)
         {
             onRightWallpr = true;
             playerAnimatorpr.SetBool(animatorRightWallpr, true);
             if (!Groundedpr)
                 playerRigidbodypr.velocity = new Vector3(playerRigidbodypr.velocity.x, Mathf.Clamp(playerRigidbodypr.velocity.y, -3f, 30), playerRigidbodypr.velocity.z);
             forceDirectionpr = Vector3.up + new Vector3(-0.2f, 0, 0);
         }
         else
         {
             onRightWallpr = false;
             playerAnimatorpr.SetBool(animatorRightWallpr, false);
         }
        }

        private void MoveUppr()
        {
            playerRigidbodypr.velocity = Vector3.zero;
            transform.Translate(transform.up * upMoveSpeedpr * Time.deltaTime , Space.World);
        }
        
        public void ResetMovementValuespr()
        {
            strafeSpeedpr = defaultStrafeSpeedpr;
            forwardMoveSpeedpr = defaultForwardMoveSpeedpr;
            jumpStrengthpr = defaultJumpStrengthpr;
            playerAnimatorpr.speed = 1.2f;
            speedEffectpr.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
            speedEffectpr.maxParticles = 0;
            trailEffectpr.gameObject.SetActive(false);
        }
        
        public IEnumerator Jumppr()
        {
            if (!canJumppr) yield break;
            playerRigidbodypr.velocity = Vector3.zero;
            jumpPuffpr.Stop();
            jumpPuffpr.Play();
            canJumppr = false;
            playerRigidbodypr.AddForce(forceDirectionpr * jumpStrengthpr, ForceMode.Impulse);
            Groundedpr = false;
            playerAnimatorpr.SetBool(animatorGroundedpr, false);
            playerAnimatorpr.SetTrigger(animatorJumppr);
        
            yield return new WaitForSeconds(0.1f);
            if (jumpStrengthpr > defaultJumpStrengthpr)
                jumpStrengthpr = defaultJumpStrengthpr;
            canJumppr = true;
        }
        
        public IEnumerator ClimbUppr()
        {
            if (!canClimbpr) yield break;
        
            playerRigidbodypr.velocity = Vector3.zero;
            canMoveForwardpr = false;
            strafeSpeedpr = 2;
            canClimbpr = false;
            playerRigidbodypr.useGravity = false;
        
            onFrontWallpr = true;
            playerAnimatorpr.SetBool(animatorFrontWallpr, true);

            yield return new WaitUntil(() => !onFrontWallSecondarypr);
            StartCoroutine(JumpOverObstaclespr());
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            playerAnimatorpr.SetBool(animatorFrontWallpr, false);
            playerRigidbodypr.useGravity = true;
            canMoveForwardpr = true;
            strafeSpeedpr = defaultStrafeSpeedpr;
            canClimbpr = true;
        }

        public IEnumerator JumpOverObstaclespr()
        {
            if (!canJumppr) yield break;
            playerRigidbodypr.velocity = Vector3.zero;
            canJumppr = false;
            playerRigidbodypr.AddForce(forceDirectionpr * obstaclesJumpStrengthpr, ForceMode.Impulse);
            Groundedpr = false;
            playerAnimatorpr.SetBool(animatorGroundedpr, false);
            playerAnimatorpr.SetTrigger("JumpOverObstacles");

            yield return new WaitForSeconds(0.3f);
            canJumppr = true;
        }

        public IEnumerator PerformSlidepr()
        {
            if(!canSlidepr)
            {   
                yield break;
            }
            canSlidepr = false;
            playerAnimatorpr.SetTrigger(animatorSlidepr);
            yield return new WaitForSeconds(.5f);
            canSlidepr = true;

        }
        
        public IEnumerator LandPuffpr()
        {
            if (!canPuffpr) yield break;

            canPuffpr = false;
            landPuffpr.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            landPuffpr.Play();
            yield return new WaitUntil(() => !Groundedpr);

            canPuffpr = true;
        }
        
        public IEnumerator Vaultpr()
        {
            if (!canVaultpr) yield break;
            canVaultpr = false;
            canJumppr = false;
            playerAnimatorpr.SetTrigger("Vault");
            playerRigidbodypr.AddForce(forceDirectionpr * 3, ForceMode.Impulse);
            yield return new WaitForSeconds(0.3f);
            canVaultpr = true;
            canJumppr = true;
        }
        
        public IEnumerator Knockbackpr()
        {
            if (!CanKnockback) yield break;
    
            CanKnockback = false;
            canMoveForwardpr = false; // Предотвращаем движение вперед
            playerRigidbodypr.velocity = Vector3.zero; // Сброс скорости
            playerRigidbodypr.AddForce(-transform.forward * knockbackForcepr, ForceMode.Impulse);
            //grounded = false;
            playerAnimatorpr.CrossFade(animatorFallpr,0.1f);
       
            yield return new WaitForSeconds(knockbackDurationpr); // Ждем определенное время
            canMoveForwardpr = true; // Разрешаем движение вперед снова
            CanKnockback = true; // Позволяем снова использовать отбрасывание
        }
    }
}

