using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] float health = 100;
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] int scoreValue = 150;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    float shotCounter = 0;
    [SerializeField] float projectileFiringPeriod = 0.1f;
    [SerializeField] float projectileSpeed = 10f;

    [Header("Pickups")]
    [SerializeField] GameObject healingPill;
    int randomNumber = 0;

    [Header("Audio")]
    [SerializeField] AudioClip enemyShoot;
    [SerializeField] [Range(0, 1)] float shootingVolume = 100f;
    [SerializeField] AudioClip enemyDeath;
    [SerializeField] [Range(0, 1)] float deathVolume = 100f;


    void Start()
    {
        
    }

    void Update()
    {
        CountDownAndShoot();
    }
    IEnumerator FiringPeriod()
    {
        yield return new WaitForSeconds(2);
    }
    IEnumerator WaitBeforeShoot()
    {
        yield return new WaitForSeconds(2);
        shotCounter = 0;
    }
    private void CountDownAndShoot()
    {
            StartCoroutine (Fire());
    }

    IEnumerator Fire()
    {
        while (true)
        {
            AudioSource.PlayClipAtPoint(enemyShoot, Camera.main.transform.position, shootingVolume);
            GameObject laser = Instantiate(
                laserPrefab,
                new Vector3(transform.position.x - 1, transform.position.y),
                Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);

         

            yield return new WaitForSeconds(1f);
        }
    }

    public void SpawnPills()
    {
        GameObject Pill = Instantiate(
            healingPill,
            transform.position,
            Quaternion.identity) as GameObject;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        AudioSource.PlayClipAtPoint(enemyDeath, Camera.main.transform.position, deathVolume);
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        randomNumber = UnityEngine.Random.Range(0, 100);
        if (randomNumber <= 15)
        {
            SpawnPills();
        }
    }
}
