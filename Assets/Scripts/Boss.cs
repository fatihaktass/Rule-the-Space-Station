using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    Animator animator;
    public GameObject spineSword, handSword;
    public int EnemyHealth = 1000;
    public Text healthText;
    public Slider healthSlider;
    public GameObject FinalPanel;

    //NavMesh
    public NavMeshAgent bossAgent;
    public Transform player;
    public LayerMask enemyGroundLayer, playerLayer;

    //Detecting
    public float sightRange, attackRange;
    public bool enemySightRange, enemyAttackRange;
    bool isStopping;

    //Attack
    public float attackDelay;
    public bool isAttacking;
    public bool isKicking;
    public int attackPoint;

    //Audio
    public AudioSource swordGeldi;
    public AudioSource swordBosAtak;
    public AudioSource kickSound;
    public AudioSource growlSound;

    Vector3 targetPosition;
    bool isWalking;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        isWalking = true;   
        animator = GetComponent<Animator>();
        bossAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        enemySightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        enemyAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (enemySightRange && !enemyAttackRange)
        {
            FindObjectOfType<WeaponManager>().amIFire = false;
            player.GetComponent<PlayerController>().playerSpeed = 5f;
            if (isWalking == true)
            {
                EquipAndScream();
            }
            if (isWalking == false)
            {
                Detecting();
                animator.SetBool("StartWalking", true);
                animator.SetBool("MeleeAttack", false);
                animator.SetBool("MeleeKick", false);
            }
                  
        }
        else if (enemySightRange && enemyAttackRange)
        {
            Attacking();          
            animator.SetBool("StartWalking", false);
            if (attackPoint < 5)
            {
                animator.SetBool("MeleeAttack", true);
            }
            if (attackPoint > 5 && attackPoint < 10)
            {
                animator.SetBool("MeleeKick", true);
                isKicking = true;
            }
        }
    }

    void Detecting()
    {

            bossAgent.SetDestination(player.position);
            transform.LookAt(targetPosition);
            FindObjectOfType<WeaponManager>().amIFire = true;
        
    }
    void DetectingActive()
    {
        isWalking = false;
    }

    void EquipAndScream()
    {
        if (isWalking == true)
        {
            animator.SetBool("EquipSword", true);
            animator.SetBool("BattleCry", true);
            Invoke("DetectingActive", 3.5f);
        }
    }

    void Attacking()
    {
        attackPoint = Random.Range(1, 10);
        bossAgent.SetDestination(player.position);
        transform.LookAt(targetPosition);

        if (isAttacking == false)
        {
            isAttacking = true;
            Invoke(nameof(ResetAttack), attackDelay);
        }
        
    }

    void ResetAttack()
    {
        isAttacking = false;
        isKicking = false;
    }

    public void EnemyTakeDamage(int damageAmount)
    {
        EnemyHealth -= damageAmount;
        healthSlider.value -= damageAmount;
        healthText.text = EnemyHealth.ToString();
        
        if (EnemyHealth <= 0)
        {
            EnemyDeath();
            healthText.text = healthSlider.value.ToString();
        }
    }

    void EnemyDeath()
    {
        animator.SetBool("Dying", true);
        gameObject.GetComponent<NavMeshAgent>().speed = 0;
        Invoke("PanelGetir", 2.5f);
    }

    void PanelGetir()
    {
        FinalPanel.SetActive(true);
        FindObjectOfType<FinishPanel>().isActive = true;
        FindObjectOfType<WeaponManager>().bossDead = false;
    }

    void EquipSword()
    {
        spineSword.SetActive(false);
        handSword.SetActive(true);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    void SwordComing()
    {
        swordGeldi.Play();
    }

    void KickAttack()
    {
        kickSound.Play();
    }

    void SwordAttack()
    {

        swordBosAtak.Play();

    }

    void Ciglik()
    { 
        growlSound.Play();
    }

}
