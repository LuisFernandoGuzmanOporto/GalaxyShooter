using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float timeToDestroy = 4f;
    public float damage = 1;
    public bool playerBullet = false;
    public GameObject impactParticlesPrefab;

    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

	
	void OnTriggerEnter(Collider collision)
    {
        Instantiate(impactParticlesPrefab, transform.position, transform.rotation);
        if (collision.gameObject.CompareTag("Enemy") && playerBullet)
        {
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            e.TakeDamage(damage);
            Destroy(gameObject);
        }else if (collision.gameObject.CompareTag("Player") && !playerBullet)
        {
            Player p = collision.gameObject.GetComponent<Player>();
            p.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Final Boss") && playerBullet) //daño en boss
        {
            FinalBoss b = collision.gameObject.GetComponent<FinalBoss>();
            b.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}