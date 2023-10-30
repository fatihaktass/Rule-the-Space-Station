using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public GameObject door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.SetActive(true);
            Destroy(gameObject);
        }
    }
}
