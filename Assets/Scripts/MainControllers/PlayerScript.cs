using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    bool canClimb = true;
    bool canJump = true;
    public bool canMoveForward = true;
    bool canGrabSideWalls = true;
    bool canSlide = true;
    bool canPuff = true;
    bool canVault = true;
    public bool canMove = true;
    public bool canKnockback;

    [Header("Settings")]
    public FloatingJoystick joystick;
    public float rotationSpeed;
    public float forwardMoveSpeed;
    public float defaultForwardMoveSpeed;
    public float upMoveSpeed;
    float defaultUpMoveSpeed;
    private float knockbackForce = 15f;
    private float knockbackDuration = 1f;
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
    public Transform lastCheckpoint;
    public float onAirTime;

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
    public int animatorFall;
    public int animatorStandUp;
    public int animatorGetUp;

    [Header("Referrences")]
    public Transform raycastPositionBottom;
    public Transform raycastPositionTop;
    public Rigidbody playerRigidbody;
    public Animator playerAnimator;
    public CapsuleCollider playerCollider;
    public Transform playerBody;

    [Header("Effects")]
    public ParticleSystem jumpPuff;
    public ParticleSystem landPuff;
    public ParticleSystem speedEffect;
    public ParticleSystem speedUpEffect;
    public TrailRenderer trailEffect;
    public ParticleSystem confettiEffect;
    public ParticleSystem fallingConfetti;

    [Header("UI")]
    public Text playerName;
    public Text playerPosition;


    private void Awake()
    {
        animatorGrounded = Animator.StringToHash("Grounded");
        animatorLeftWall = Animator.StringToHash("LeftWall");
        animatorRightWall = Animator.StringToHash("RightWall");
        animatorFrontWall = Animator.StringToHash("FrontWall"); 
        animatorJump = Animator.StringToHash("Jump");
        animatorSlide = Animator.StringToHash("Slide");
        animatorFall = Animator.StringToHash("Fall");
    }
    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        
        //playerAnimator.SetBool("CanRun", true);
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
        if (Physics.Raycast(raycastPositionBottom.position, transform.forward, 2, environment) && grounded)
        {
                StartCoroutine(Jump());
        }
    }
    private void FixedUpdate()
    {
        float mY = joystick.Vertical;
        float mX = joystick.Horizontal;
        float pX = Input.GetAxis("Horizontal");
        float pY = Input.GetAxis("Vertical");

        CheckPlayerState();
        if (onRightWall)
        {
            mX = Mathf.Clamp(mX, -1, 0);
            pX = Mathf.Clamp(pX, -1, 0);
        }
        else if (onLeftWall)
        {
            mX = Mathf.Clamp(mX, 0, 1);
            pX = Mathf.Clamp(pX, 0, 1);
        }
        else if(!canMove)
        {
            mX = 0;
            mY = 0;
        }

        if (canMoveForward && canMove)
        {
             transform.Translate(Vector3.forward * Time.fixedDeltaTime * forwardMoveSpeed, Space.World);
           // transform.position += Vector3.forward * forwardMoveSpeed * Time.fixedDeltaTime;

        }

        

        if (onFrontWall || onFrontWallSecondary)
        {
            MoveUp();
        }
        

        if (!grounded && !onRightWall && !onLeftWall && !onFrontWall)
        {
            onAirTime += Time.deltaTime;
            var velocity = playerRigidbody.velocity;
            velocity = new Vector3(0, velocity.y, velocity.z);
            playerRigidbody.velocity = velocity;
            transform.Translate(Vector3.right * Time.deltaTime * strafeSpeed * mX, Space.World);
            transform.Translate(Vector3.right * Time.deltaTime * strafeSpeed * pX, Space.World);
        }
        else
        {
            onAirTime = 0;
            transform.Translate(Vector3.right * Time.deltaTime * strafeSpeed * mX, Space.World);
            transform.Translate(Vector3.right * Time.deltaTime * strafeSpeed * pX, Space.World);
        }
        
        if (mX < -0.2f)
        {
            if (!onLeftWall && !onRightWall && !onFrontWall)
                playerBody.rotation = Quaternion.Slerp(playerBody.rotation, leftRotationTarget, rotationSmoothing);
            else if (onFrontWall)
                playerBody.rotation = Quaternion.Slerp(playerBody.rotation, frontRotationTargetLeft, rotationSmoothing);
        }
        else if (mX > 0.2f)
        {
            if(!onLeftWall && !onRightWall && !onFrontWall)
                playerBody.rotation = Quaternion.Slerp(playerBody.rotation, rightRotationTarget, rotationSmoothing);
            else if(onFrontWall)
                playerBody.rotation = Quaternion.Slerp(playerBody.rotation, frontRotationTargetRight, rotationSmoothing);
        }
        else
        {
            playerBody.rotation = Quaternion.Slerp(playerBody.rotation, Quaternion.Euler(0, 0, 0), rotationSmoothing);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(raycastPositionBottom.position, transform.forward * raycastMaxDistance);
        Gizmos.DrawRay(raycastPositionBottom.position, transform.forward * raycastMaxDistance);
        Gizmos.DrawRay(raycastPositionBottom.position, transform.right * raycastMaxDistance);
        Gizmos.DrawRay(raycastPositionBottom.position, -transform.right * raycastMaxDistance);
        Gizmos.DrawRay(raycastPositionBottom.position, -transform.up * .35f);
        Gizmos.DrawRay(raycastPositionBottom.position, transform.forward * 2);
    }
    public void CheckPlayerState()
    {

        if (Physics.Raycast(raycastPositionBottom.position, transform.forward, raycastMaxDistance , environment))
        {
            onFrontWall = true;
            forceDirection = Vector3.up;
            
        }

        else
        {
            onFrontWall = false;
        }

        if (Physics.Raycast(raycastPositionTop.position, transform.forward, raycastMaxDistance, environment))
        {
            onFrontWallSecondary = true;
            forceDirection = Vector3.up;
           
        }
        else
        {
            onFrontWallSecondary = false;
        }

        if(onFrontWallSecondary && onFrontWall)
        {
            playerAnimator.SetBool(animatorFrontWall, true);
            StartCoroutine(ClimbUp());
            ResetMovementValues();
        }
        else if(!onFrontWall && onFrontWallSecondary)
        {
            playerAnimator.SetBool(animatorFrontWall, true);
            StartCoroutine(ClimbUp());
            ResetMovementValues();
        }
        else
        {
            playerAnimator.SetBool(animatorFrontWall, false);
        }




        if (Physics.Raycast(raycastPositionBottom.position, -transform.up, .35f,environment))
        {
            grounded = true;
            forceDirection = Vector3.up;
            playerAnimator.SetBool(animatorGrounded, true);
            
            if(GameManager.Instance.gameStarted)
                StartCoroutine(LandPuff());
        }
        else
        {
            grounded = false;
            playerAnimator.SetBool(animatorGrounded, false);
        }
        if (Physics.Raycast(transform.position, -transform.right, raycastMaxDistance, environment))
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
        if (Physics.Raycast(transform.position, transform.right, raycastMaxDistance, environment))
        {
            onRightWall= true;
            playerAnimator.SetBool(animatorRightWall, true);
            if(!grounded)
                playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x,Mathf.Clamp( playerRigidbody.velocity.y,-3f,30 ), playerRigidbody.velocity.z);
            forceDirection = Vector3.up + new Vector3(-.2f, 0, 0);
        }
        else
        {
            onRightWall = false;
            playerAnimator.SetBool(animatorRightWall, false);
        }
    }
    public void MoveUp()
    {
        playerRigidbody.velocity = Vector3.zero;
        transform.Translate(transform.up * upMoveSpeed * Time.deltaTime , Space.World);
    }



    public IEnumerator Jump()
    {
        if (!canJump) yield break;
        playerRigidbody.velocity = Vector3.zero;
        jumpPuff.Stop();
        jumpPuff.Play();
        canJump = false;
        playerRigidbody.AddForce(forceDirection * jumpStrength, ForceMode.Impulse);
        grounded = false;
        playerAnimator.SetBool(animatorGrounded, false);
        playerAnimator.SetTrigger(animatorJump);
        
        yield return new WaitForSeconds(0.1f);
        if (jumpStrength > defaultJumpStrength)
            jumpStrength = defaultJumpStrength;
        canJump = true;
    }
    public IEnumerator ClimbUp()
    {
        if (!canClimb) yield break;
        
        playerRigidbody.velocity = Vector3.zero;
        canMoveForward = false;
        strafeSpeed = 2;
        canClimb = false;
        playerRigidbody.useGravity = false;
        
        onFrontWall = true;
        playerAnimator.SetBool(animatorFrontWall, true);

        yield return new WaitUntil(() => !onFrontWallSecondary);
        StartCoroutine(JumpOverObstacles());
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        playerAnimator.SetBool(animatorFrontWall, false);
        playerRigidbody.useGravity = true;
        canMoveForward = true;
        strafeSpeed = defaultStrafeSpeed;
        canClimb = true;
    }

    public void ResetMovementValues()
    {
        strafeSpeed = defaultStrafeSpeed;
        forwardMoveSpeed = defaultForwardMoveSpeed;
        jumpStrength = defaultJumpStrength;
        playerAnimator.speed = 1.2f;
        speedEffect.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
        speedEffect.maxParticles = 0;
        trailEffect.gameObject.SetActive(false);
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
        if(!canSlide)
        {   
            yield break;
        }
        canSlide = false;
        playerAnimator.SetTrigger(animatorSlide);
        yield return new WaitForSeconds(.5f);
        canSlide = true;

    }
    public IEnumerator LandPuff()
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
    public IEnumerator Knockback()
    {
        if (!canKnockback) yield break;
    
        canKnockback = false;
        canMoveForward = false; // Предотвращаем движение вперед
        playerRigidbody.velocity = Vector3.zero; // Сброс скорости
        playerRigidbody.AddForce(-transform.forward * knockbackForce, ForceMode.Impulse);
        //grounded = false;
        playerAnimator.CrossFade(animatorFall,0.1f);
       
        yield return new WaitForSeconds(knockbackDuration); // Ждем определенное время
        canMoveForward = true; // Разрешаем движение вперед снова
        canKnockback = true; // Позволяем снова использовать отбрасывание
    }
}

