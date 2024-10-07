using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public EnemyType type;
    public float maxLife = 3;
    public float speed = 2;
    public float timeBtwShoot = 1.5f;
    float timer = 0;
    public float range = 4;
    float life = 3;
    public float damage = 1;
    public float bulletSpeed = 4f;
    bool targetInRange = false;
    public int scorePoints = 1; 
    Transform target;
    public Transform firePoint;
    public Bullet bulletPrefab;
    public GameObject[] powerUps;
    public GameObject deathParticlesPrefab;
    public Image lifeBar;
    void Start()
    {
        GameObject ship = GameObject.FindGameObjectWithTag("Player");
        target = ship.transform;
        life = maxLife;
        lifeBar.fillAmount = life / maxLife;
    }

    void Update()
    {
        switch (type)
        {
            case EnemyType.Normal:
                scorePoints = 1;
                MoveForward();
                break;
            case EnemyType.NormalShoot:
                scorePoints = 2;
                MoveForward();
                Shoot();
                break;
            case EnemyType.Kamikase:
                scorePoints = 4;
                if (targetInRange)
                {
                    RotateToTarget();
                    MoveForward(2);
                }
                else
                {
                    MoveForward();
                    SearchTarget();
                }
                break;
            case EnemyType.Sniper:
                scorePoints = 3;
                if (targetInRange)
                {
                    RotateToTarget();
                    Shoot();
                }
                else
                {
                    MoveForward();
                    SearchTarget();
                }
                break;
        }
        
    }
    public void TakeDamage(float dmg)
    {
        life -= dmg;
        lifeBar.fillAmount = life / maxLife;
        if (life <= 0)
        {
            Instantiate(deathParticlesPrefab, transform.position, transform.rotation);
            TrySpawnPowerUp();
            Spawner.instance.AddScore(scorePoints);
            Spawner.instance.EnemyDestroyed();
            Destroy(gameObject);
        }
    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void MoveForward(float m)
    {
        transform.Translate(Vector3.forward * speed * m * Time.deltaTime);
    }

    void RotateToTarget()
    {
        Vector3 dir = target.position - transform.position;
        float angleY = Mathf.Atan2 (dir.x, dir.z) * Mathf.Rad2Deg + 0;
        transform.rotation = Quaternion.Euler(0, angleY, 0);
    }

    void SearchTarget()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if(distance <= range)
        {
            targetInRange = true;
        }
        else
        {
            targetInRange = false;
        }
    }

    void Shoot()
    {
        if (timer < timeBtwShoot)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            Bullet b = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            b.damage = damage;
            b.speed = bulletSpeed;
            
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            p.TakeDamage(damage);
            Spawner.instance.AddScore();
            Spawner.instance.EnemyDestroyed();
            Destroy(gameObject);
        }
        else if(collision.gameObject.CompareTag("Destroyer"))
        {
            Destroy(gameObject);
            Spawner.instance.EnemyDestroyed();
        }
    }
    
    void TrySpawnPowerUp()
    {
        float spawnChance = Random.Range(0f, 100f);
        if (spawnChance <= 100f)
        {
            int powerUpIndex = Random.Range(0, powerUps.Length);
            Instantiate(powerUps[powerUpIndex], transform.position, transform.rotation);
        }
    }
}

public enum EnemyType
{
    Normal,
    NormalShoot,
    Kamikase,
    Sniper
}