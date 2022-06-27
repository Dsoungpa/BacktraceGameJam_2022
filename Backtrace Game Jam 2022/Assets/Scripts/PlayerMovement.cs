using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    public HealthBarScript healthBar;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2d;
    public GameObject boxCastObject;
    private Renderer boxCastReference;
    [Header("General")]
    [SerializeField] private LayerMask platformLayerMask;
    [SerializeField] private bool isGrounded;
    [SerializeField] public float horizontalInput;
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float acceleration = 200f;
    [SerializeField] private float deceleration = 10f;
    private Vector2 movementForce;
    [Header("Jump")]
    [SerializeField] private float groundedCheckHeight = 0.05f;
    [SerializeField] private float jumpForce = 75f;
    [SerializeField] private float fallMultiplier = 25f;
    [SerializeField] private float lowJumpMultiplier = 60f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float coyoteTimeCounter;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float jumpBufferCounter;
    [SerializeField] private float extraJumps = 0;
    [SerializeField] private float extraJumpsCounter;
    public float jumpCooldownTimer = 5f;
    [SerializeField] private bool offCooldown;
    private bool jumpRequest = false;

    [Header("Emergency Block Cooldown")]
    [SerializeField] private Image doubleJumpIcon;


    [Header("Stamina")]
    public float maxHealth = 100;
    [SerializeField] private float jumpStaminaDecrement = 1f;
    [SerializeField] private float staminaOverTime = 1f;
    //referencetoanimations
    public PlayerAnimations Anims;
    // Start is called before the first frame update

    //collectible audio
    public AudioSource Pickup;
    public AudioSource jump;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        boxCastReference = boxCastObject.GetComponent<Renderer>();
    }
    void Start(){
        offCooldown = true;
        healthBar.SetMaxHealth(maxHealth);
        //StartCoroutine(TakeHealthConstantly());

        doubleJumpIcon.fillAmount = 0;
    }
    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        
        movementForce = new Vector2(horizontalInput, 0);
        // coyote time
        if (IsGrounded()) {
            if(offCooldown){
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

        healthBar.currentHealth -= 1 / staminaOverTime * Time.deltaTime;

        if (offCooldown == false) {
            doubleJumpIcon.fillAmount -= 1 / jumpCooldownTimer * Time.deltaTime;
            if (doubleJumpIcon.fillAmount <= 0) {
                doubleJumpIcon.fillAmount = 0;
                offCooldown = true;
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
            offCooldown = false;
            doubleJumpIcon.fillAmount = 1;
        }
        Anims.animator.SetBool("IsJumping",true);
        jump.Play();
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        
        healthBar.currentHealth -= jumpStaminaDecrement;
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
        Color rayColor;
        if (raycastHit.collider != null) {
            rayColor = Color.green;
        }else {
            rayColor = Color.red;
        }
        Debug.DrawRay(boxCastReference.bounds.center, Vector2.down * boxCastReference.bounds.extents, rayColor);
        return raycastHit.collider != null;
    }
    private void JumpHeightVariation() {
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    IEnumerator TakeHealthConstantly(){
        while(true){
            yield return new WaitForSeconds(1f);
            float currHealth = healthBar.currentHealth;
            currHealth -= staminaOverTime;
            healthBar.SetHealth(currHealth);
        }
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Collectible"){
            Pickup.Play();
        }
    }

}