using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable
{
    public static PlayerController Instance;

    [SerializeField] HUDManager hudManager;

    [SerializeField] float jumpForce;
    [SerializeField] float downForce;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;

    bool grounded;
    float jumpBuffer;

    Rigidbody2D rigidBody;
    Animator animator;
    DanInput inputActions;

    [HideInInspector] public WeaponManager weaponManager;
    [HideInInspector] public InventoryManager inventoryManager;
    GameOverScreen gameOverScreen;

    [SerializeField] Animator slash;
    [SerializeField] GameObject slashObject;

    float movt;

    public int Health { get { return health; } set { health = value; } }
    public bool Dead { get { return dead; } set { dead = value; } }

    bool dead;
    int health;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        dead = false;
        health = 50;

        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        weaponManager = GetComponent<WeaponManager>();
        inventoryManager = GetComponent<InventoryManager>();
        hudManager = FindObjectOfType<HUDManager>();
        gameOverScreen = FindObjectOfType<GameOverScreen>();

        inputActions = new DanInput();
        inputActions.Enable();

        inputActions.Dan.Jump.performed += delegate { Jump(); };
        inputActions.Dan.Punch.performed += delegate { Punch(); };
        inputActions.Dan.Fire.performed += delegate { Fire(); };
        inputActions.Dan.NextWeapon.performed += (value) => weaponManager.NextWeapon();
    }

    void Update()
    {
        if (Dead)
        {
            rigidBody.velocity = Vector3.zero;
            return;
        }

        jumpBuffer -= Time.deltaTime;
        jumpBuffer = Mathf.Clamp(jumpBuffer, 0, jumpBuffer);

        if (jumpBuffer > 0 && grounded)
        {
            InitiateJump();
        }

        grounded = checkGrounded();
        TryMove();

        if (transform.position.y <= -50)
        {
            TakeDamage(100);
        }
    }

    void LateUpdate()
    {
        if (movt == 0)
            rigidBody.velocity -= new Vector2(rigidBody.velocity.x, 0) * 0.25f;

        rigidBody.angularVelocity = 0;
        rigidBody.rotation = 0;

        hudManager.HealthSlider.value = Health;
    }

    void Jump()
    {
        jumpBuffer = 0.3f;
    }
    void InitiateJump()
    {
        Debug.Log("Jumped!");
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
        jumpBuffer = 0;
    }

    void TryMove()
    {
        movt = inputActions.Dan.Movt.ReadValue<float>();
        animator.SetFloat("Movt", Mathf.Abs(movt));

        if (movt == 0) return;

        if (movt > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (movt < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        rigidBody.velocity = new Vector2(movt * walkSpeed, rigidBody.velocity.y);
    }

    void Punch()
    {
        Debug.Log("Punch!");

        slashObject.SetActive(true);
        slash.Play("Slash", 0, 0);
    }

    void Fire()
    {
        Debug.Log("Fire!");

        weaponManager.FireWeapon();
    }

    bool checkGrounded()
    {
        var check = Physics2D.CircleCast(transform.position, 0.5f, Vector3.down, 1.1f);
        return check.transform != null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var isPickup = collision.gameObject.GetComponent<Pickup>();
        if (isPickup)
        {
            if (isPickup.type == PickUpType.Knife)
            {
                weaponManager.AddAmmo(WeaponType.Knife, 5);
            }
            else if (isPickup.type == PickUpType.Rifle)
            {
                weaponManager.AddAmmo(WeaponType.Rilfe, 20);
            }
            else if (isPickup.type == PickUpType.Coin)
            {
                inventoryManager.AddCoin(10);
            }

            Destroy(isPickup.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.root.CompareTag("Platform"))
        {
            collision.transform.root.GetComponent<Platform>().Attach(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.root.CompareTag("Platform") && !Dead)
        {
            collision.transform.root.GetComponent<Platform>().Dettach(transform);
        }
    }

    public void TakeDamage(int damage)
    {
        if (Health <= 0 || Health >= 100) return;

        Health -= damage;

        Handheld.Vibrate();
        FindObjectOfType<CameraShaker>().ShakeCam(0.15f, 0.35f);

        if (Health <= 0 || Health >= 100) Die(Health >= 100);
    }

    public void Die(bool vaporise)
    {
        if (Dead) return;

        if (!vaporise)
        {
            animator.Play("Death", 0, 0);
        }
        else
        {
            animator.Play("Vapor", 0, 0);
        }

        GetComponent<Collider2D>().enabled = false;
        rigidBody.isKinematic = true;
        gameOverScreen.EndSequence();

        Dead = true;
    }

    private void OnDrawGizmos()
    {

    }
}
