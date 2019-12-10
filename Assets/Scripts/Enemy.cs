using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] float health = 100;
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] int scoreValue = 150;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
     float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
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
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        AudioSource.PlayClipAtPoint(enemyShoot, Camera.main.transform.position, shootingVolume);
        GameObject laser = Instantiate(
            laserPrefab,
            transform.position,
            Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);

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
        if(randomNumber <= 15)
        {
            SpawnPills();
        }
    }
}
