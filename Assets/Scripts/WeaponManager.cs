using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public int minDamage, maxDamage;
    public Camera playerCamera;
    public float range = 300f;
    public int damageAmount = 40;
    public bool amIFire = true;
    public bool bossDead = true;

    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public AudioSource gunFireSound;

    EnemyScript enemyScript;
    AlienEnemy alienEnemy;
    Boss bossFighter;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.timeScale > 0 && amIFire == true && bossDead == true)
        {
            Fire();
            gunFireSound.Play();
            muzzleFlash.Play();
        }
    }

    void Fire()
    {
        RaycastHit hit;
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            enemyScript = hit.transform.GetComponent<EnemyScript>();
            alienEnemy = hit.transform.GetComponent<AlienEnemy>();
            bossFighter = hit.transform.GetComponent<Boss>();
            // hit.point ateþ ettiðimiz nokta, hit.normal normal texture'u
            Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));


            if(enemyScript != null)
            enemyScript.EnemyTakeDamage(Random.Range(minDamage, maxDamage));

            if(alienEnemy != null)
                alienEnemy.EnemyTakeDamage(Random.Range(minDamage, maxDamage));

            if(bossFighter != null)
                bossFighter.EnemyTakeDamage(Random.Range(5, 10));
        }
    }


}
