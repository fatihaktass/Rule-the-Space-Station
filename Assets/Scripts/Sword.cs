using UnityEngine;

public class Sword : MonoBehaviour
{
    public int swordDamage;
    Boss boss;

    private void Start()
    {
        boss = FindObjectOfType<Boss>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && boss.isAttacking == true) 
        {
            FindObjectOfType<PlayerController>().PlayerTakeDamage(swordDamage);
        }
    }

}
