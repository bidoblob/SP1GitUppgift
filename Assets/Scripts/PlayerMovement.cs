using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Transform leftFoot, rightFoot;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip[] jumpSounds;
    [SerializeField] private GameObject appleParticles, dustParticles;
    
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillColour;
    [SerializeField] private Color yellowHealth, redHealth;
    [SerializeField] private TMP_Text appleText;

    
    private float horizontalValue;
    private bool isGrounded;
    private bool canMove;
    private float rayDistance = 0.25f;
    
    private Rigidbody2D rgbd;
    private SpriteRenderer rend;
    private AudioSource audioSource;
    
    private Animator anim;
    
    private int startingHealth = 5;
    private int currentHealth = 0;
    public int applesCollected = 0;
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        currentHealth = startingHealth;
        appleText.text = "" + applesCollected;
        
        rgbd = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalValue = Input.GetAxis("Horizontal");
        
        if(horizontalValue < 0)
        {
            FlipSprite(true);
        }
        if(horizontalValue > 0)
        {
            FlipSprite(false);
        }
        
        
        if(Input.GetButtonDown("Jump") && CheckIfGrounded() == true)
        {
            Jump();
        }
        
        anim.SetFloat("MoveSpeed", Mathf.Abs(rgbd.velocity.x));
        anim.SetFloat("VerticalSpeed", rgbd.velocity.y);
        anim.SetBool("IsGrounded", CheckIfGrounded());
        
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }
        rgbd.velocity = new Vector2(horizontalValue * moveSpeed * Time.deltaTime, rgbd.velocity.y);
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Apple"))
        {
            
            Destroy(other.gameObject);
            applesCollected++;
            appleText.text = "" + applesCollected;
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(pickupSound, 0.5f);
            Instantiate(appleParticles, other.transform.position, Quaternion.identity);
            
        }
        
        if (other.CompareTag("Health"))
        {
            RestoreHealth(other.gameObject);
        }
    }
    
    private void FlipSprite(bool direction)
    {
        rend.flipX = direction;
    }
    private void Jump()
    {

        rgbd.AddForce(new Vector2(0, jumpForce));
        audioSource.pitch = 1;
        int randomValue = Random.Range(0, jumpSounds.Length);
        audioSource.PlayOneShot(jumpSounds[randomValue], 0.5f);
        Instantiate(dustParticles, transform.position, dustParticles.transform.localRotation);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Respawn();
        }
        
    }
    
    public void TakeKnockback(float knockbackForce, float upwardsForce)
    {
        canMove = false;
        rgbd.AddForce(new Vector2(knockbackForce, upwardsForce));
        Invoke("CanMoveAgain", 0.3f);
        
    }
    
    private void CanMoveAgain()
    {
        canMove = true;
    }
    
    private void Respawn()
    {
        currentHealth = startingHealth;
        UpdateHealthBar();
        transform.position = spawnPosition.position;
        rgbd.velocity = Vector2.zero;
    }
    
    private void UpdateHealthBar()
    {
        
        healthSlider.value = currentHealth;
        if(currentHealth >= 3)
        {
            fillColour.color = yellowHealth;
        }
        else
        {
            fillColour.color = redHealth;
        }
    }
    
    private void RestoreHealth(GameObject healthPickup)
    {
        if(currentHealth >= startingHealth)
        {
            return;
        }
        else
        {
            int healthToRestore = healthPickup.GetComponent<HealthPickup>().healthAmount;
            currentHealth += healthToRestore;
            UpdateHealthBar();
            Destroy(healthPickup);
            audioSource.pitch = Random.Range(1.0f, 1.1f);
            audioSource.PlayOneShot(pickupSound, 0.5f);

            
            if(currentHealth >= startingHealth)
            {
                currentHealth = startingHealth;
            }
        }
    }
    private bool CheckIfGrounded()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(leftFoot.position, Vector2.down, rayDistance, whatIsGround);
        RaycastHit2D rightHit = Physics2D.Raycast(rightFoot.position, Vector2.down, rayDistance, whatIsGround);

        if(leftHit.collider != null && leftHit.collider.CompareTag("Ground") || rightHit.collider != null && rightHit.collider.CompareTag("Ground") )
        {
            return true;
        }
        else 
        {
            return false;
        }
    }
    
    
}
