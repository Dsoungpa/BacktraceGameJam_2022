using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2d;
    private Renderer boxCastReference;

    [Header("General")]
    [SerializeField] private LayerMask platformLayerMask;
    [SerializeField] private bool isGrounded;
    [SerializeField] public float horizontalInput;
    [SerializeField] private GameObject boxCastObject;

    [Header("Script Reference")]
    [SerializeField] private HealthBarScript healthBarScript;
    [SerializeField] private CameraShake cameraShakeScript;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float acceleration = 200f;
    [SerializeField] private float deceleration = 10f;
    private Vector2 movementForce;

    [Header("Fall")]
    [SerializeField] private float downwardVelocity;
    [SerializeField] private float velocityThreshhold = 50f;
    [SerializeField] private float fallShakeDuration = 0.5f;
    [SerializeField] private float fallShakeMagnitude = 0.1f;
    [SerializeField] private float fallShakeMagnitudeFade = 0.98f;
    private float previousVelocity;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 75f;
    [SerializeField] private float fallMultiplier = 25f;
    [SerializeField] private float lowJumpMultiplier = 60f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float extraJumps = 0;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private float extraJumpsCounter;
    private bool jumpRequest = false;
    public bool doubleJumpOffCooldown;
    private float groundedCheckHeight = 0.05f;

    [Header("Double Jump Cooldown")]
    [SerializeField] private Image doubleJumpIcon;
    [SerializeField] private float jumpCooldown = 5f;
    [SerializeField] private float jumpCost = 3f;
    [SerializeField] private float doubleJumpCost = 15f;

    [Header("Health")]
    [SerializeField] private float maxHealth = 1000f;
    [SerializeField] private float healthOverTime = 1f;
    [SerializeField] private float healthDisplay;
   
    [Header("Animations")]
    [SerializeField] private PlayerAnimations Anims;

    [Header("Audio")]
    [SerializeField] private AudioSource Pickup;
    [SerializeField] private AudioSource jumpAudio;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        boxCastReference = boxCastObject.GetComponent<Renderer>();
    }

    void Start() {
        healthBarScript.SetMaxHealth(maxHealth);
        healthDisplay = maxHealth;

        doubleJumpOffCooldown = true;
        doubleJumpIcon.fillAmount = 0;
    }

    void Update() {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        movementForce = new Vector2(horizontalInput, 0);

        if (IsGrounded()) {
            if(doubleJumpOffCooldown){
                extraJumpsCounter = extraJumps;
            }
            coyoteTimeCounter = coyoteTime;
            Anims.animator.SetBool("IsJumping",false);
        }else {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump")) {
            jumpBufferCounter = jumpBufferTime;
        }else {
            jumpBufferCounter -= Time.deltaTime;
        }

        if ((coyoteTimeCounter > 0f || extraJumpsCounter > 0) && jumpBufferCounter > 0f) {
            jumpRequest = true;
        }

        healthBarScript.currentHealth -= healthOverTime * Time.deltaTime;
        healthDisplay = healthBarScript.currentHealth;

        if (doubleJumpOffCooldown == false) {
            doubleJumpIcon.fillAmount -= 1 / jumpCooldown * Time.deltaTime;
            if (doubleJumpIcon.fillAmount <= 0) {
                doubleJumpIcon.fillAmount = 0;
                doubleJumpOffCooldown = true;
            }
        }
    }

    void FixedUpdate() {
        Walk(movementForce);
        ApplyGroundLinearDrag();
        if (jumpRequest) {
            Jump();
            jumpRequest = false;
            coyoteTimeCounter = 0;
            jumpBufferCounter = 0;
        }
        JumpHeightVariation();

        // Fall Shake
        downwardVelocity = rb.velocity.y;
        if (rb.velocity.y == 0 && previousVelocity < -velocityThreshhold) {
            StartCoroutine(cameraShakeScript.CameraShakeController(fallShakeDuration, fallShakeMagnitude, fallShakeMagnitudeFade));
        }
        previousVelocity = downwardVelocity;
    }

    void Walk(Vector2 moveDirection) {
        rb.AddForce(new Vector2(moveDirection.x * acceleration, rb.velocity.y));
        if (Mathf.Abs(rb.velocity.x) > moveSpeed) {
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -moveSpeed, moveSpeed), rb.velocity.y);
        }
    }

    void Jump() {
        if (!IsGrounded() && extraJumpsCounter > 0){
            extraJumpsCounter -= 1;
            healthBarScript.currentHealth -= doubleJumpCost;
            doubleJumpOffCooldown = false;
            doubleJumpIcon.fillAmount = 1;
        }

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        healthBarScript.currentHealth -= jumpCost;

        Anims.animator.SetBool("IsJumping",true);
        jumpAudio.Play();
    }

    private void ApplyGroundLinearDrag() {
        if (Input.GetAxis("Horizontal") == 0 && IsGrounded()) {
            if (Mathf.Abs(horizontalInput) < 0.4f || changingDirection()) {
                rb.drag = deceleration;
            }else {
                rb.drag = 0;
            }
        }
    }

    private bool changingDirection() {
        if ((rb.velocity.x > 0f && horizontalInput < 0f) || (rb.velocity.x < 0f && horizontalInput > 0f)) {
            return true;
        }
        return false;
    }

    private bool IsGrounded() {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCastReference.bounds.center, boxCastReference.bounds.size, 0f, Vector2.down, groundedCheckHeight, platformLayerMask);
        return raycastHit.collider != null;
    }

    private void JumpHeightVariation() {
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Collectible"){
            Pickup.Play();
        }
    }

}