using UnityEngine;
using UnityEngine.AI;

public class AlienEnemy : MonoBehaviour
{

    public Animator alienAnimator;
    public GameObject handWeapon;
    public GameObject legWeapon;
    public GameObject impEffect;
    public ParticleSystem fireEffect;
    public AudioSource gunSoundEffect;
    public Transform hitPlace;
    public ParticleSystem deadEffect;
    public AudioSource hurtSound;

    public int EnemyHealth = 200;
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
    public GameObject projectile;
    public float projectileForce;
    RaycastHit hit;

    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        alienAnimator = GetComponent<Animator>();
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
            alienAnimator.SetBool("isWalking", true);
            alienAnimator.SetBool("beforeDetecting", false);
            alienAnimator.SetBool("isDetecting", false);
            alienAnimator.SetBool("isAttacking", false);
        }
        else if (enemySightRange && !enemyAttackRange)
        {
            //Detecting bize doðru gelmesini saðlayacaðýz
            DetectPlayer();
            alienAnimator.SetBool("isWalking", false);
            alienAnimator.SetBool("beforeDetecting", true);
            alienAnimator.SetBool("isDetecting", true);
            alienAnimator.SetBool("isAttacking", false);
        }
        else if (enemySightRange && enemyAttackRange)
        {
            //Attack player

            AttackPlayer();
            alienAnimator.SetBool("isWalking", false);
            alienAnimator.SetBool("beforeDetecting", false);
            alienAnimator.SetBool("isDetecting", false);
            alienAnimator.SetBool("isAttacking", true);
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
            enemyAgent.SetDestination(walkPoint); 
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint; // yer yoksa yürüyebileceði alanlardan birine gidiyor

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
        Vector3 targetPosition = new Vector3(player.transform.position.x,
                                                transform.position.y,
                                                player.transform.position.z);
        transform.LookAt(targetPosition);


        if (isAttacking == false)
        {
            //Atak türü
            
            if (Physics.Raycast(hitPlace.transform.position, hitPlace.transform.forward, out hit, 30f))
            {
                Rigidbody rb = Instantiate(projectile, hitPlace.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * projectileForce, ForceMode.Impulse);
                Instantiate(impEffect, hit.point, Quaternion.LookRotation(hit.normal));

                fireEffect.Play();
                gunSoundEffect.Play();
            }

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
    void EquipGun()
    {
        legWeapon.SetActive(false);
        handWeapon.SetActive(true);
    }


}
