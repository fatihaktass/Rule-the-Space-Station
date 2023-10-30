using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    CharacterController characterController;
    public float playerSpeed = 10f;
    public float gravity = -14f;
    public int PlayerHealth = 100;
    public bool haveKey = false;
    public Text infoText;
    Vector3 gravityVector;
    public static int resetBoss = 0;
    public bool napsin;
    public GameObject enemyObject;

    //GroundCheck
    public Transform groundCheckpoint;
    public float groundCheckRadius = 0.35f;
    public LayerMask groundLayer;
    public LayerMask bossGroundLayer;
    public bool isGrounded;
    public float jumpSpeed = 3f;

    //UI
    public Text healthText;
    public CanvasGroup damageScreenUI;
    public Slider healthSlider;

    //Audio
    public AudioSource playerHurtSource;
    public AudioSource openingSound;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        gameManager = FindObjectOfType<GameManager>();
        damageScreenUI.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        GroundCheck();
        JumpAndGravity();
        DamageScreenCleaner();
    }

    void MovePlayer()
    {
        // Mouse pozisyonuna göre hareket etme
        Vector3 moveVector = (Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward).normalized;
        characterController.Move(moveVector * playerSpeed * Time.deltaTime);
    }

    void GroundCheck()
    {
        // Zemin tespit etmek için
        isGrounded = Physics.CheckSphere(groundCheckpoint.position, groundCheckRadius, groundLayer);
    }

    void JumpAndGravity()
    {
        // Gravity
        gravityVector.y += gravity * Time.deltaTime;
        characterController.Move(gravityVector * Time.deltaTime);

        // Gravity dengeleme
        if (isGrounded && gravityVector.y < 0)
        {
            gravityVector.y = -3f;
        }

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            gravityVector.y = Mathf.Sqrt(jumpSpeed * -2f * gravity); // or jumpSpeed;


        }
    }

    public void PlayerTakeDamage(int damageAmount)
    {
        PlayerHealth -= damageAmount;
        healthSlider.value -= damageAmount;
        HealthTextUpdate();
        damageScreenUI.alpha = 1f;
        playerHurtSource.Play();

        if (PlayerHealth <= 0)
        {
            PlayerDeath();
            HealthTextUpdate();
            healthSlider.value = 0;
            
        }
    }

    public void PlayerHealthPot(int potAmount)
    {
        if (PlayerHealth < 100)
        {
            PlayerHealth += potAmount;
            healthSlider.value += potAmount;
            HealthTextUpdate();
        }
        if (PlayerHealth >= 100)
        {
            PlayerHealth = 100;
            healthSlider.value = 100;
            HealthTextUpdate();
        }
        
    }

    public void PlayerDeath()
    {
        //SceneManager'dan sahneyi tekrar yükleyebiliriz.
        gameManager.RestartGame();
    }

    void HealthTextUpdate()
    {
        healthText.text = PlayerHealth.ToString();
    }

    void DamageScreenCleaner()
    {
        if (damageScreenUI.alpha > 0)
        {
            damageScreenUI.alpha -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Portal"))
        {
            gameManager.WinLevel();
        }
        if (other.CompareTag("Door"))
        {
            FindObjectOfType<DoorTrigger>().tetiklendiMi = true;
            openingSound.Play();
        }
        if (other.CompareTag("Door2"))
        {
            FindObjectOfType<DoorTrigger2>().tetiklendiMi = true;
            openingSound.Play();
            if (resetBoss == 1)
            {
                napsin = true;
                enemyObject.SetActive(false);
            }
        }
        if (other.CompareTag("Door3"))
        {
            if (haveKey == false && resetBoss == 0)
            {
                InfoTextUpdate("Anahtarýn yok, " +
                    "diðer kapýyý açmanýn yolunu bul ve anahtarý al");
            }            
            if (haveKey == true || resetBoss == 1)
            {
                resetBoss++;
                if (resetBoss > 1)
                {
                    resetBoss = 1;
                }             
                FindObjectOfType<DoorTrigger3>().tetiklendiMi = true;
                openingSound.Play();
            }
        }
        if (other.CompareTag("Door2Key"))
        {
            haveKey = true;
        }
    }

    void InfoTextUpdate(string myText)
    {
        infoText.color = Color.red;
        infoText.CrossFadeAlpha(1f,0f,false);
        infoText.gameObject.SetActive(true);
        infoText.text = myText;
        infoText.CrossFadeAlpha(0f, 5f, false);
    }
}
