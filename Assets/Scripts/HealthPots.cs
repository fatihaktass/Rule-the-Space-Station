using UnityEngine;
using UnityEngine.UI;


public class HealthPots : MonoBehaviour
{
    PlayerController playerController;
    public Text healhtInfoText;
    public int PotAmount;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        transform.Rotate(0f, (180f * Time.deltaTime), 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerController.PlayerHealth < 100)
            {
                playerController.PlayerHealthPot(PotAmount);
                healhtInfoText.color = Color.green;
                healhtInfoText.gameObject.SetActive(true);
                healhtInfoText.CrossFadeAlpha(1, 0f, true);
                healhtInfoText.text = "Ýyileþtirme yapýldý";
                Invoke(nameof(TextGoodBye), 0.5f);
                transform.Translate(0f, 9f, 0f);
                Destroy(this.gameObject,1f);
                
            }
            else
            {
                healhtInfoText.color = Color.red;
                healhtInfoText.gameObject.SetActive(true);
                healhtInfoText.CrossFadeAlpha(1, 0f, true);
                healhtInfoText.text = "Canýnýz maksimum düzeyde";
                Invoke(nameof(TextGoodBye), 1f);
            }            
        }
    }

    void TextGoodBye()
    {
        healhtInfoText.CrossFadeAlpha(0, 1f, true);
        Invoke(nameof(ActiveFalse), 1f);
    }

    void ActiveFalse()
    {
        healhtInfoText.gameObject.SetActive(false);
    }


}
