using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable
{
    public static PlayerController Instance;

    [SerializeField] float jumpForce;
    [SerializeField] float downForce;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;

    bool grounded;
    float jumpBuffer;

    Rigidbody2D rigidBody;
    Animator animator;
    DanInput inputActions;

    WeaponManager weaponManager;
    InventoryManager inventoryManager;

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
        health = 100;

        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        weaponManager = GetComponent<WeaponManager>();
        inventoryManager = GetComponent<InventoryManager>();

        inputActions = new DanInput();
        inputActions.Enable();

        inputActions.Dan.Jump.performed += delegate { Jump(); };
        inputActions.Dan.Punch.performed += delegate { Punch(); };
        inputActions.Dan.Fire.performed += delegate { Fire(); };
        inputActions.Dan.NextWeapon.performed += (value) => weaponManager.NextWeapon();
    }

    void Update()
    {
        jumpBuffer -= Time.deltaTime;
        jumpBuffer = Mathf.Clamp(jumpBuffer, 0, jumpBuffer);

        if (jumpBuffer > 0 && grounded)
        {
            InitiateJump();
        }

        grounded = checkGrounded();
        TryMove();
    }

    void LateUpdate()
    {
        if (movt == 0)
            rigidBody.velocity -= new Vector2(rigidBody.velocity.x, 0) * 0.25f;

        rigidBody.angularVelocity = 0;
        rigidBody.rotation = 0;
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
        var check = Physics2D.Raycast(transform.position, Vector3.down, 1.1f);
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

    public void TakeDamage(int damage)
    {
        if (Health <= 0) return;

        Health -= damage;
        if (Health <= 0) Die();
    }

    public void Die()
    {
        if (Dead) return;
        animator.Play("Death", 0, 0);
        Dead = true;
    }
}
