using UnityEngine;

public class BossKick : MonoBehaviour
{
    public int damage;
    PlayerController playerController;
    Boss boss;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        boss = FindObjectOfType<Boss>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && boss.isKicking == true)
        {
            playerController.PlayerTakeDamage(20);
        }
    }

}
