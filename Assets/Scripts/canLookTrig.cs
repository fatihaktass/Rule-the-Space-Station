using UnityEngine;

public class canLookTrig : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<KeyScripts>().canLook = true;
            Destroy(gameObject);
        }
    }
}
