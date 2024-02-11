using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MainControllers
{
    public class Enemypr : MonoBehaviour
    {
        bool canClimb = true;
        bool canJump = true;
        bool canMoveForward = true;
        bool canGrabSideWalls = true;
        bool canSlide = true;
        bool canPuff = true;
        bool canVault = true;
        bool canMoveLeft = true;
        bool canMoveRight = true;
        public bool canNavigate = false;
        public bool canMove = false;
        [Header("Settings")]
        public float rotationSpeed;
        public float forwardMoveSpeed;
        float defaultForwardMoveSpeed;
        public float upMoveSpeed;
        float defaultUpMoveSpeed;
        public float raycastMaxDistance;
        public LayerMask environment;
        public float strafeSpeed;
        float defaultStrafeSpeed;
        public float jumpStrength;
        float defaultJumpStrength;
        public float obstaclesJumpStrength;
        float defaultObstaclesJumpStrength;
        public float rotationSmoothing;
        Quaternion leftRotationTarget;
        Quaternion rightRotationTarget;
        Quaternion frontRotationTargetLeft;
        Quaternion frontRotationTargetRight;
        public float onAirTime;
        public Collider[] collidersNearEnemy;
        public Transform nearestPlatform;
        public float pX;
        public float pY;
        public float smoothTranslation;
        public Transform lastCheckpoint;
        PlayerScriptpr player;
        public Text playerPos, playerName;
        [Header("Player State Bools")]

        public bool grounded;
        public bool onLeftWall;
        public bool onRightWall;
        public bool onFrontWall;
        public bool onFrontWallSecondary;
        Vector3 forceDirection;

        [Header("Animator Hash")]

        public int animatorGrounded;
        public int animatorLeftWall;
        public int animatorRightWall;
        public int animatorFrontWall;
        public int animatorJump;
        public int animatorSlide;

        [Header("Referrences")]
        public Transform forwarRaycastPosition;
        public Transform raycastPositionTop;
        public Rigidbody playerRigidbody;
        public Animator playerAnimator;
        public CapsuleCollider playerCollider;
        public Transform playerBody;

        [Header("Effects")]
        public ParticleSystem jumpPuff;
        public ParticleSystem landPuff;

        private void Awake()
        {
            animatorGrounded = Animator.StringToHash("Grounded");
            animatorLeftWall = Animator.StringToHash("LeftWall");
            animatorRightWall = Animator.StringToHash("RightWall");
            animatorFrontWall = Animator.StringToHash("FrontWall");
            animatorJump = Animator.StringToHash("Jump");
            animatorSlide = Animator.StringToHash("Slide");

        }
    
        private void Start()
        {
            playerRigidbody = GetComponent<Rigidbody>();
            playerCollider = GetComponent<CapsuleCollider>();
            if(canMove)
                playerAnimator.SetBool("CanRun", true);
            leftRotationTarget = Quaternion.Euler(0, -45, 0);
            rightRotationTarget = Quaternion.Euler(0, 45, 0);
            frontRotationTargetLeft = Quaternion.Euler(0, 0, 20);
            frontRotationTargetRight = Quaternion.Euler(0, 0, -20);

            defaultForwardMoveSpeed = forwardMoveSpeed;
            defaultJumpStrength = jumpStrength;
            defaultObstaclesJumpStrength = obstaclesJumpStrength;
            defaultStrafeSpeed = strafeSpeed;
            defaultUpMoveSpeed = upMoveSpeed;
        
        }

        private void Update()
        {
            if (Physics.Raycast(forwarRaycastPosition.position, transform.forward, 2, environment) && grounded)
            {
                StartCoroutine(Jump());
            }
        }
        private void FixedUpdate()
        {
            CheckPlayerState();
            StartCoroutine(NavigateToNextPlatform());
        
            if (canMoveForward && canMove)
            {
                transform.Translate(Vector3.forward * Time.fixedDeltaTime * forwardMoveSpeed, Space.World);
            }
            if(grounded || onFrontWall || onRightWall || onLeftWall)
                playerBody.localRotation = Quaternion.Slerp(playerBody.transform.rotation, Quaternion.identity, rotationSmoothing);
            //Applying the pX and pY values
            if (onFrontWall && canMove)
            {
                MoveUp();
            }
        }

        private void CheckPlayerState()
        {
            bool topRaycastHit = Physics.Raycast(raycastPositionTop.position, transform.forward, raycastMaxDistance, environment);
            bool forwardRaycastHit = Physics.Raycast(forwarRaycastPosition.position, transform.forward, raycastMaxDistance, environment);
            bool bottomRaycastHit = Physics.Raycast(forwarRaycastPosition.position, -transform.up, raycastMaxDistance, environment);
            bool leftWallRaycastHit = Physics.Raycast(transform.position, -transform.right, raycastMaxDistance, environment);
            bool rightWallRaycastHit = Physics.Raycast(transform.position, transform.right, raycastMaxDistance, environment);
        
            if (topRaycastHit)
            {
                onFrontWallSecondary = true;
                forceDirection = Vector3.up;
            }
            else
            {
                onFrontWallSecondary = false;
            }
    
            if (forwardRaycastHit)
            {
                onFrontWall = true;
                forceDirection = Vector3.up;
            }
            else
            {
                onFrontWall = false;
            }
    
            if (onFrontWallSecondary && onFrontWall)
            {
                playerAnimator.SetBool(animatorFrontWall, true);
                StartCoroutine(ClimbUp());
            }
            else if (!onFrontWall && onFrontWallSecondary)
            {
                playerAnimator.SetBool(animatorFrontWall, true);
                StartCoroutine(ClimbUp());
            }
            else
            {
                playerAnimator.SetBool(animatorFrontWall, false);
            }
    
            if (bottomRaycastHit)
            {
                grounded = true;
                canNavigate = false;
                forceDirection = Vector3.up;
                playerAnimator.SetBool(animatorGrounded, true);
    
                if (GameManager.Instance.gameStarted)
                    StartCoroutine(LandPuff());
            }
            else
            {
                grounded = false;
                if (!GameManager.Instance.gameEnded)
                    canNavigate = true;
                playerAnimator.SetBool(animatorGrounded, false);
            }
    
            if (leftWallRaycastHit)
            {
                onLeftWall = true;
    
                if (!grounded)
                    playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, Mathf.Clamp(playerRigidbody.velocity.y, -3f, 30), playerRigidbody.velocity.z);
    
                playerAnimator.SetBool(animatorLeftWall, true);
                forceDirection = Vector3.up + new Vector3(0.2f, 0, 0);
            }
            else
            {
                onLeftWall = false;
                playerAnimator.SetBool(animatorLeftWall, false);
            }
    
            if (rightWallRaycastHit)
            {
                onRightWall = true;
                playerAnimator.SetBool(animatorRightWall, true);
                if (!grounded)
                    playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, Mathf.Clamp(playerRigidbody.velocity.y, -3f, 30), playerRigidbody.velocity.z);
                forceDirection = Vector3.up + new Vector3(-0.2f, 0, 0);
            }
            else
            {
                onRightWall = false;
                playerAnimator.SetBool(animatorRightWall, false);
            }
        }

    
        private void MoveUp()
        {
            playerRigidbody.velocity = Vector3.zero;
            transform.Translate(transform.up * upMoveSpeed * Time.deltaTime, Space.World);
        }
    
        public void ResetMovementValues()
        {
            strafeSpeed = defaultStrafeSpeed;
            forwardMoveSpeed = defaultForwardMoveSpeed;
            jumpStrength = defaultJumpStrength;
            playerAnimator.speed = 1.2f;
        }

        private IEnumerator NavigateToNextPlatform()
        {
            if (nearestPlatform == null || !canMove)
                yield break;
            yield return new WaitForSeconds(.1f);
            if (!canNavigate || onLeftWall || onRightWall || onFrontWall)
            {
                yield break;
            }
            Vector3 position = transform.InverseTransformPoint(nearestPlatform.position);
            Vector3 targetVector = transform.position - nearestPlatform.position;
            if (position.x < -1)
            {
                //Left
                playerBody.localRotation = Quaternion.Slerp(playerBody.transform.rotation, leftRotationTarget, rotationSmoothing);
                //transform.Translate(-strafeSpeed * Time.fixedDeltaTime * Vector3.right * smoothTranslation);
                playerRigidbody.AddForce(-transform.right * jumpStrength * Time.deltaTime * smoothTranslation * targetVector.magnitude, ForceMode.Acceleration);
                playerRigidbody.velocity = new Vector3(Mathf.Clamp(playerRigidbody.velocity.x, -4, 4), playerRigidbody.velocity.y, playerRigidbody.velocity.z);
            }
            else if(position.x > 1)
            {
                //Right
                playerBody.localRotation = Quaternion.Slerp(playerBody.transform.rotation, rightRotationTarget, rotationSmoothing);
                //transform.Translate(strafeSpeed * Time.fixedDeltaTime * Vector3.right * smoothTranslation);
                playerRigidbody.AddForce(transform.right * jumpStrength * Time.deltaTime * smoothTranslation * targetVector.magnitude, ForceMode.Acceleration);
                playerRigidbody.velocity = new Vector3(Mathf.Clamp(playerRigidbody.velocity.x, -4, 4), playerRigidbody.velocity.y, playerRigidbody.velocity.z);
            }
            else
            {
                //UpFront
                playerBody.localRotation = Quaternion.Slerp(playerBody.transform.rotation, Quaternion.identity, rotationSmoothing);
            }
        }

        public IEnumerator Jump()
        {
            if (!canJump) yield break;
        
            canNavigate = true;
            playerRigidbody.velocity = Vector3.zero;
            jumpPuff.Stop();
            jumpPuff.Play();
            canJump = false;
            playerRigidbody.AddForce(forceDirection * jumpStrength, ForceMode.Impulse);
            grounded = false;
            playerAnimator.SetBool(animatorGrounded, false);
            playerAnimator.SetTrigger(animatorJump);

            yield return new WaitForSeconds(0.2f);
            if (jumpStrength > defaultJumpStrength)
                jumpStrength = defaultJumpStrength;
            canJump = true;
        }
    
        private IEnumerator ClimbUp()
        {
            if (!canClimb) yield break;
            ResetMovementValues();
            playerRigidbody.velocity = Vector3.zero;
            canMoveForward = false;
            strafeSpeed = 4;
            canClimb = false;
            playerRigidbody.useGravity = false;

            onFrontWall = true;
            playerAnimator.SetBool(animatorFrontWall, true);

            yield return new WaitUntil(() => !onFrontWall);
            StartCoroutine(JumpOverObstacles());
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            playerAnimator.SetBool(animatorFrontWall, false);
            playerRigidbody.useGravity = true;
            canMoveForward = true;
            strafeSpeed = defaultStrafeSpeed;
            canClimb = true;
        }
    
        public IEnumerator JumpOverObstacles()
        {
            if (!canJump) yield break;
            playerRigidbody.velocity = Vector3.zero;
            canJump = false;
            playerRigidbody.AddForce(forceDirection * obstaclesJumpStrength, ForceMode.Impulse);
            grounded = false;
            playerAnimator.SetBool(animatorGrounded, false);
            playerAnimator.SetTrigger("JumpOverObstacles");

            yield return new WaitForSeconds(0.3f);
            canJump = true;
        }

        public IEnumerator PerformSlide()
        {
            if (!canSlide)
            {
                yield break;
            }
            canSlide = false;
            playerAnimator.SetTrigger(animatorSlide);
            yield return new WaitForSeconds(.5f);
            canSlide = true;

        }
    
        private IEnumerator LandPuff()
        {
            if (!canPuff) yield break;

            canPuff = false;
            landPuff.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            landPuff.Play();
            yield return new WaitUntil(() => !grounded);

            canPuff = true;
        }
    
        public IEnumerator Vault()
        {
            if (!canVault) yield break;
            canVault = false;
            canJump = false;
            playerAnimator.SetTrigger("Vault");
            playerRigidbody.AddForce(forceDirection * 3, ForceMode.Impulse);
            yield return new WaitForSeconds(0.3f);
            canVault = true;
            canJump = true;
        }
   
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Player"))
                Physics.IgnoreCollision(playerCollider, collision.collider);
        }
    }
}

