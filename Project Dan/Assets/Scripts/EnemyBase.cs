using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyType
{
    MeleeEnemy,
    RangeEnemy,
}
public class EnemyBase : MonoBehaviour, IDamageable
{
    public EnemyType enemyType;

    protected Rigidbody2D rigidBody;
    protected Animator animator;

    [SerializeField] protected float viewDistance;
    [SerializeField] protected float patrolRadius;

    protected Transform[] patrolZones;
    int current;

    protected Transform patrolTarget;
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float idleTime;
    Vector3 startingPoint;

    [SerializeField][Range(0, 100)] protected int DamageLevel;

    bool running;
    bool alerted;

    float timeSinceNotScene;
    protected Transform target;

    [Header("Health")]
    [SerializeField] Slider HealthSlider;
    void Start()
    {
        Health = 100;

        timeSinceNotScene = 5.5f;
        target = null;
        running = true;

        patrolZones = new Transform[2];
        patrolZones[0] = new GameObject().transform;
        patrolZones[1] = new GameObject().transform;

        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        startingPoint = transform.position;
        ChooseTarget();

       

    }

    void Update()
    {
        if (Dead)
        {
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = 0;
            rigidBody.rotation = 0;
            return;
        }

        idleTime -= Time.deltaTime;

        patrolZones[0].position = startingPoint + Vector3.right * patrolRadius;
        patrolZones[1].position = startingPoint - Vector3.right * patrolRadius;

        if (SeenPlayer())
        {
            Attack();
        }
        else
        {
            Patrol();
        }

        animator.SetFloat("Movt", Mathf.Abs(rigidBody.velocity.x));
    }

    private void LateUpdate()
    {
        HealthSlider.value = Health / 100f;
        HealthSlider.transform.localRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        rigidBody.angularVelocity = 0;
        rigidBody.rotation = 0;
    }

    public void ChooseTarget()
    {
        try
        {
            current = 1 - current;
            patrolTarget = patrolZones[current];

            Debug.Log(current);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }

    public virtual void Patrol()
    {

    }
    public virtual void Attack()
    {

    }

    public bool MoveTo(Transform target)
    {
        if (!GroundInFront(target))
        {
            rigidBody.velocity = Vector3.zero;
            return true;
        }

        while (idleTime > 0)
        {
            rigidBody.velocity = Vector2.zero;
            return false;
        }

        bool flag = false;

        float dir = target.position.x - transform.position.x;
        if (dir >= 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        rigidBody.velocity = walkSpeed * transform.right;

        if (Mathf.Abs(target.position.x - transform.position.x) < 1) flag = true;

        return flag;
    }

    public bool SeenPlayer()
    {
        var viewData = Physics2D.OverlapBoxAll(transform.position + transform.right * viewDistance * 0.35f, new Vector2(viewDistance, 1.5f), 0);
        foreach (var data in viewData)
        {
            if (data.CompareTag("Player"))
            {
                target = data.transform;
                timeSinceNotScene = 0;
                return true;
            }
        }

        timeSinceNotScene += Time.deltaTime;
        timeSinceNotScene = Mathf.Clamp(timeSinceNotScene, 0, 6);
        if (timeSinceNotScene > 5)
        {
            target = null;
            return false;
        }
        else
        {
            return true;
        }
    }

    protected bool GroundInFront(Transform target)
    {
        Vector3 dir = (target.position - transform.position);
        dir.y = 0;
        dir = dir.normalized;

        Debug.DrawRay(transform.position + dir, Vector3.down * 3, Color.red);

        var check = Physics2D.Raycast(transform.position + dir, Vector3.down, 3f);

        return check.transform != null;
    }

    int health;
    bool dead;
    public int Health { get { return health; } set { health = value; } }
    public bool Dead { get { return dead; } set { dead = value; } }

    public void StopWalking()
    {
        rigidBody.velocity = Vector2.zero;
    }

    protected void OnDrawGizmos()
    {
        var start = transform.position;
        if (running) start = startingPoint;

        Gizmos.DrawWireSphere(start + transform.right * patrolRadius, 1f);
        Gizmos.DrawWireSphere(start - transform.right * patrolRadius, 1f);
        Gizmos.DrawLine(start + transform.right * patrolRadius, start - transform.right * patrolRadius);

        if (patrolTarget)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(patrolTarget.position, 1.5f);
        }

        Gizmos.color = Color.green;
        if (SeenPlayer())
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireCube(transform.position + transform.right * viewDistance * 0.35f, new Vector3(viewDistance, 1.5f));
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

        GetComponent<Collider2D>().enabled = false;
        HealthSlider.gameObject.SetActive(false);
        rigidBody.isKinematic = true;

        Dead = true;
    }

    void OnDeath()
    {
        Destroy(gameObject, .5f);
    }
}
