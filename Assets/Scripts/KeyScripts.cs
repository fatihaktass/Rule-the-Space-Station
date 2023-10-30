using UnityEngine;
using UnityEngine.UI;

public class KeyScripts : MonoBehaviour
{ 
    public Text infoText;
    public Transform player;
    public bool canLook;

    private void Update()
    {
        if (canLook == true)
        {
            transform.LookAt(player);
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            infoText.CrossFadeAlpha(1, 0, false);
            infoText.gameObject.SetActive(true);
            infoText.color = Color.green;
            infoText.text = "Anahtar envantere eklendi";
            FindObjectOfType<PlayerController>().haveKey = true;
            ChangeAlpha();
            Destroy(gameObject);
        }
    }

    void ChangeAlpha()
    {
        infoText.CrossFadeAlpha(0f, 5f, false);
    }
}
