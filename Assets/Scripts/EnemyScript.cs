using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public int EnemyHealth = 200;
    public Animator enemyAnimator;
    GameManager gameManager;

    // NavMesh 
    public NavMeshAgent enemyAgent;
    public Transform player;
    public LayerMask enemyGroundLayer;
    public LayerMask playerLayer;

    //Patrolling
    public Vector3 walkPoint;
    public float walkPointRange;
    public bool walkPointSet;

    //Detecting
    public float sightRange, attackRange;
    public bool enemySightRange, enemyAttackRange;

    //Attack
    public float attackDelay;
    public bool isAttacking;
    public Transform attackPoint;
    public GameObject projectile;
    public float ProjectileForce = 18f;

    //Particle
    public ParticleSystem deadEffect;

    //Audio
    public AudioSource hurtSound;

    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        player = GameObject.Find("Player").transform;
    }


    void Update()
    {
        enemySightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        enemyAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!enemySightRange && !enemyAttackRange)
        {
            //Patrolling koruduðu yerde koordinatlar arasý geçiþ yapacak
            Patrolling();
            enemyAnimator.SetBool("Patrolling", true);
            enemyAnimator.SetBool("PlayerAttacking", false);
            enemyAnimator.SetBool("PlayerDetecting", false);
        }
        else if (enemySightRange && !enemyAttackRange)
        {
            //Detecting bize doðru gelmesini saðlayacaðýz
            DetectPlayer();
            enemyAnimator.SetBool("PlayerDetecting", true);
            enemyAnimator.SetBool("Patrolling", false);
            enemyAnimator.SetBool("PlayerAttacking", false);
        }
        else if (enemySightRange && enemyAttackRange)
        {
            //Attack player
            
            AttackPlayer();
            enemyAnimator.SetBool("PlayerAttacking", true);
            enemyAnimator.SetBool("PlayerDetecting", false);
            enemyAnimator.SetBool("Patrolling", false);            
        }
    }

    public void EnemyTakeDamage(int damageAmount)
    {
        EnemyHealth -= damageAmount;
        hurtSound.Play();

        if (EnemyHealth <= 0)
        {
            EnemyDeath();
        }

    }

    void EnemyDeath()
    {
        Destroy(gameObject);
        gameManager = FindObjectOfType<GameManager>();
        gameManager.AddKill();
        Vector3 objeInst = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        Instantiate(deadEffect, objeInst, Quaternion.identity);
    }

    void Patrolling()
    {
        if (walkPointSet == false)
        {
            float randomZPos = Random.Range(-walkPointRange, walkPointRange);
            float randomXPos = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomXPos, transform.position.y, transform.position.z + randomZPos);

            if (Physics.Raycast(walkPoint, -transform.up, 2f, enemyGroundLayer))
            {
                walkPointSet = true; // altýmýzda yer var mý yok mu onu kontrol ediyoruz
            }
        }

        if (walkPointSet == true)
        {
            enemyAgent.SetDestination(walkPoint); // yer yoksa yürüyebileceði alanlardan birine gidiyor
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false; // walkpointe gidip durmamasý için döngüye sokuyoruz
        }
    }

    void DetectPlayer()
    {
        enemyAgent.SetDestination(player.transform.position);
        transform.LookAt(player);
    }

    void AttackPlayer()
    {
        enemyAgent.SetDestination(transform.position);
        transform.LookAt(player);
        
        if (isAttacking == false)
        {
            //Atak türü
            Rigidbody rb = Instantiate(projectile, attackPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * ProjectileForce, ForceMode.Impulse);

            isAttacking = true;
            Invoke("ResetAttack", attackDelay);
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
