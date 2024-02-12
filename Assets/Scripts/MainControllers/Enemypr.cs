using System.Collections;
using UnityEngine;

namespace MainControllers
{
    public class Enemypr : MonoBehaviour
    {
        private bool canClimbpr = true;
        private bool canJumppr = true;
        private bool canMoveForwardpr = true;
        private bool canGrabSideWallspr = true;
        private bool canSlidepr = true;
        private bool canPuffpr = true;
        private bool canVaultpr = true;
        private bool canMoveLeftpr = true;
        private bool canMoveRightpr = true;
        public bool canNavigatepr = false;
        public bool canMovepr = false;
        
        [Header("Settings")]
        public float rotationSpeedpr;
        public float forwardMoveSpeedpr;
        private float defaultForwardMoveSpeedpr;
        public float upMoveSpeedpr;
        private float defaultUpMoveSpeedpr;
        public float raycastMaxDistancepr;
        public LayerMask environmentpr;
        public float strafeSpeedpr;
        private float defaultStrafeSpeedpr;
        public float jumpStrengthpr;
        private float _defaultJumpStrengthpr;
        public float obstaclesJumpStrengthpr;
        private float _defaultObstaclesJumpStrengthpr;
        public float rotationSmoothingpr;
        private Quaternion _leftRotationTargetpr;
        private Quaternion _rightRotationTargetpr;
        private Quaternion _frontRotationTargetLeftpr;
        private Quaternion _frontRotationTargetRightpr;
        public float onAirTimepr;
        public Collider[] collidersNearEnemypr;
        public Transform nearestPlatformpr;
        public float pXpr;
        public float pYpr;
        public float smoothTranslationpr;
        public Transform lastCheckpointpr;
        private PlayerScriptpr _playerpr;
        //public Text playerPos, playerName;
        
        [Header("Player State Bools")]
        public bool groundedpr;
        private bool onLeftWallpr;
        private bool onRightWallpr;
        private bool onFrontWallpr;
        private bool onFrontWallSecondary;
        private Vector3 forceDirectionpr;

        [Header("Animator Hash")]
        private int animatorGroundedpr;
        private int animatorLeftWallpr;
        private int animatorRightWallpr;
        private int animatorFrontWallpr;
        private int animatorJumppr;
        private int animatorSlidepr;

        [Header("Referrences")]
        public Transform forwarRaycastPositionpr;
        public Transform raycastPositionToppr;
        public Rigidbody playerRigidbodypr;
        public Animator playerAnimatorpr;
        public CapsuleCollider playerColliderpr;
        public Transform playerBodypr;

        [Header("Effects")]
        public ParticleSystem jumpPuffpr;
        public ParticleSystem landPuffpr;

        private void Awake()
        {
            animatorGroundedpr = Animator.StringToHash("Grounded");
            animatorLeftWallpr = Animator.StringToHash("LeftWall");
            animatorRightWallpr = Animator.StringToHash("RightWall");
            animatorFrontWallpr = Animator.StringToHash("FrontWall");
            animatorJumppr = Animator.StringToHash("Jump");
            animatorSlidepr = Animator.StringToHash("Slide");

        }
    
        private void Start()
        {
            playerRigidbodypr = GetComponent<Rigidbody>();
            playerColliderpr = GetComponent<CapsuleCollider>();
            if(canMovepr)
                playerAnimatorpr.SetBool("CanRun", true);
            _leftRotationTargetpr = Quaternion.Euler(0, -45, 0);
            _rightRotationTargetpr = Quaternion.Euler(0, 45, 0);
            _frontRotationTargetLeftpr = Quaternion.Euler(0, 0, 20);
            _frontRotationTargetRightpr = Quaternion.Euler(0, 0, -20);

            defaultForwardMoveSpeedpr = forwardMoveSpeedpr;
            _defaultJumpStrengthpr = jumpStrengthpr;
            _defaultObstaclesJumpStrengthpr = obstaclesJumpStrengthpr;
            defaultStrafeSpeedpr = strafeSpeedpr;
            defaultUpMoveSpeedpr = upMoveSpeedpr;
        
        }

        private void Update()
        {
            if (Physics.Raycast(forwarRaycastPositionpr.position, transform.forward, 2, environmentpr) && groundedpr)
            {
                StartCoroutine(Jumppr());
            }
        }
        
        private void FixedUpdate()
        {
            CheckPlayerStatepr();
            StartCoroutine(NavigateToNextPlatformpr());
        
            if (canMoveForwardpr && canMovepr)
            {
                transform.Translate(Vector3.forward * Time.fixedDeltaTime * forwardMoveSpeedpr, Space.World);
            }
            if(groundedpr || onFrontWallpr || onRightWallpr || onLeftWallpr)
                playerBodypr.localRotation = Quaternion.Slerp(playerBodypr.transform.rotation, Quaternion.identity, rotationSmoothingpr);
            //Applying the pX and pY values
            if (onFrontWallpr && canMovepr)
            {
                MoveUppr();
            }
        }

        private void CheckPlayerStatepr()
        {
            bool topRaycastHit = Physics.Raycast(raycastPositionToppr.position, transform.forward, raycastMaxDistancepr, environmentpr);
            bool forwardRaycastHit = Physics.Raycast(forwarRaycastPositionpr.position, transform.forward, raycastMaxDistancepr, environmentpr);
            bool bottomRaycastHit = Physics.Raycast(forwarRaycastPositionpr.position, -transform.up, raycastMaxDistancepr, environmentpr);
            bool leftWallRaycastHit = Physics.Raycast(transform.position, -transform.right, raycastMaxDistancepr, environmentpr);
            bool rightWallRaycastHit = Physics.Raycast(transform.position, transform.right, raycastMaxDistancepr, environmentpr);
        
            if (topRaycastHit)
            {
                onFrontWallSecondary = true;
                forceDirectionpr = Vector3.up;
            }
            else
            {
                onFrontWallSecondary = false;
            }
    
            if (forwardRaycastHit)
            {
                onFrontWallpr = true;
                forceDirectionpr = Vector3.up;
            }
            else
            {
                onFrontWallpr = false;
            }
    
            if (onFrontWallSecondary && onFrontWallpr)
            {
                playerAnimatorpr.SetBool(animatorFrontWallpr, true);
                StartCoroutine(ClimbUppr());
            }
            else if (!onFrontWallpr && onFrontWallSecondary)
            {
                playerAnimatorpr.SetBool(animatorFrontWallpr, true);
                StartCoroutine(ClimbUppr());
            }
            else
            {
                playerAnimatorpr.SetBool(animatorFrontWallpr, false);
            }
    
            if (bottomRaycastHit)
            {
                groundedpr = true;
                canNavigatepr = false;
                forceDirectionpr = Vector3.up;
                playerAnimatorpr.SetBool(animatorGroundedpr, true);
    
                if (GameManager.Instance.gameStarted)
                    StartCoroutine(LandPuffpr());
            }
            else
            {
                groundedpr = false;
                if (!GameManager.Instance.gameEnded)
                    canNavigatepr = true;
                playerAnimatorpr.SetBool(animatorGroundedpr, false);
            }
    
            if (leftWallRaycastHit)
            {
                onLeftWallpr = true;
    
                if (!groundedpr)
                    playerRigidbodypr.velocity = new Vector3(playerRigidbodypr.velocity.x, Mathf.Clamp(playerRigidbodypr.velocity.y, -3f, 30), playerRigidbodypr.velocity.z);
    
                playerAnimatorpr.SetBool(animatorLeftWallpr, true);
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
                if (!groundedpr)
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
            transform.Translate(transform.up * upMoveSpeedpr * Time.deltaTime, Space.World);
        }
    
        public void ResetMovementValuespr()
        {
            strafeSpeedpr = defaultStrafeSpeedpr;
            forwardMoveSpeedpr = defaultForwardMoveSpeedpr;
            jumpStrengthpr = _defaultJumpStrengthpr;
            playerAnimatorpr.speed = 1.2f;
        }

        private IEnumerator NavigateToNextPlatformpr()
        {
            if (nearestPlatformpr == null || !canMovepr)
                yield break;
            yield return new WaitForSeconds(.1f);
            if (!canNavigatepr || onLeftWallpr || onRightWallpr || onFrontWallpr)
            {
                yield break;
            }
            Vector3 position = transform.InverseTransformPoint(nearestPlatformpr.position);
            Vector3 targetVector = transform.position - nearestPlatformpr.position;
            if (position.x < -1)
            {
                //Left
                playerBodypr.localRotation = Quaternion.Slerp(playerBodypr.transform.rotation, _leftRotationTargetpr, rotationSmoothingpr);
                //transform.Translate(-strafeSpeed * Time.fixedDeltaTime * Vector3.right * smoothTranslation);
                playerRigidbodypr.AddForce(-transform.right * jumpStrengthpr * Time.deltaTime * smoothTranslationpr * targetVector.magnitude, ForceMode.Acceleration);
                playerRigidbodypr.velocity = new Vector3(Mathf.Clamp(playerRigidbodypr.velocity.x, -4, 4), playerRigidbodypr.velocity.y, playerRigidbodypr.velocity.z);
            }
            else if(position.x > 1)
            {
                //Right
                playerBodypr.localRotation = Quaternion.Slerp(playerBodypr.transform.rotation, _rightRotationTargetpr, rotationSmoothingpr);
                //transform.Translate(strafeSpeed * Time.fixedDeltaTime * Vector3.right * smoothTranslation);
                playerRigidbodypr.AddForce(transform.right * jumpStrengthpr * Time.deltaTime * smoothTranslationpr * targetVector.magnitude, ForceMode.Acceleration);
                playerRigidbodypr.velocity = new Vector3(Mathf.Clamp(playerRigidbodypr.velocity.x, -4, 4), playerRigidbodypr.velocity.y, playerRigidbodypr.velocity.z);
            }
            else
            {
                //UpFront
                playerBodypr.localRotation = Quaternion.Slerp(playerBodypr.transform.rotation, Quaternion.identity, rotationSmoothingpr);
            }
        }

        public IEnumerator Jumppr()
        {
            if (!canJumppr) yield break;
        
            canNavigatepr = true;
            playerRigidbodypr.velocity = Vector3.zero;
            jumpPuffpr.Stop();
            jumpPuffpr.Play();
            canJumppr = false;
            playerRigidbodypr.AddForce(forceDirectionpr * jumpStrengthpr, ForceMode.Impulse);
            groundedpr = false;
            playerAnimatorpr.SetBool(animatorGroundedpr, false);
            playerAnimatorpr.SetTrigger(animatorJumppr);

            yield return new WaitForSeconds(0.2f);
            if (jumpStrengthpr > _defaultJumpStrengthpr)
                jumpStrengthpr = _defaultJumpStrengthpr;
            canJumppr = true;
        }
    
        private IEnumerator ClimbUppr()
        {
            if (!canClimbpr) yield break;
            ResetMovementValuespr();
            playerRigidbodypr.velocity = Vector3.zero;
            canMoveForwardpr = false;
            strafeSpeedpr = 4;
            canClimbpr = false;
            playerRigidbodypr.useGravity = false;

            onFrontWallpr = true;
            playerAnimatorpr.SetBool(animatorFrontWallpr, true);

            yield return new WaitUntil(() => !onFrontWallpr);
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
            groundedpr = false;
            playerAnimatorpr.SetBool(animatorGroundedpr, false);
            playerAnimatorpr.SetTrigger("JumpOverObstacles");

            yield return new WaitForSeconds(0.3f);
            canJumppr = true;
        }

        public IEnumerator PerformSlidepr()
        {
            if (!canSlidepr)
            {
                yield break;
            }
            canSlidepr = false;
            playerAnimatorpr.SetTrigger(animatorSlidepr);
            yield return new WaitForSeconds(.5f);
            canSlidepr = true;

        }
    
        private IEnumerator LandPuffpr()
        {
            if (!canPuffpr) yield break;

            canPuffpr = false;
            landPuffpr.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            landPuffpr.Play();
            yield return new WaitUntil(() => !groundedpr);

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
   
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Player"))
                Physics.IgnoreCollision(playerColliderpr, collision.collider);
        }
    }
}

