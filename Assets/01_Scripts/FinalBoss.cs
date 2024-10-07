using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalBoss : MonoBehaviour
{
    public int maxHealth = 4; 
    private float currentHealth;
    public float moveSpeed = 2.0f;
    public float attackCooldown = 1.5f; 
    private float attackTimer = 0;

    public Bullet projectilePrefab; 
    public Transform firePoint; 
    public Transform player;
    public Image lifeBar;

    private Vector2 targetPosition;
    private bool isMovingToStartPosition = true;
    public enum BossPhase { Phase1, Phase2, Phase3, Phase4 }
    public BossPhase currentPhase;

    float life = 4;

    void Start()
    {
        currentHealth = maxHealth;
        currentPhase = BossPhase.Phase1;
        StartCoroutine(BossBehavior());
        targetPosition = new Vector2(transform.position.x, transform.position.y - 3f);
        lifeBar.fillAmount = life / maxHealth;
    }

    void Update()
    {

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
		lifeBar.fillAmount = currentHealth / maxHealth;

		if (currentHealth <= 1)
        {
            currentPhase = BossPhase.Phase4;
        }
        else if (currentHealth <= 2)
        {
            currentPhase = BossPhase.Phase3;
        }
        else if (currentHealth <= 3)
        {
            currentPhase = BossPhase.Phase2;
        }


        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Final Boss Defeated!");
        Destroy(gameObject);
    }
    IEnumerator BossBehavior()
    {
        while (currentHealth > 0)
        {
            switch (currentPhase)
            {
                case BossPhase.Phase1:
                    Phase1Behavior();
                    break;
                case BossPhase.Phase2:
                    Phase2Behavior();
                    break;
                case BossPhase.Phase3:
                    Phase3Behavior();
                    break;
                case BossPhase.Phase4:
                    Phase4Behavior();
                    break;
            }
            yield return null;
        }
    }

    void Phase1Behavior()
    {
        if (isMovingToStartPosition)
        {
  
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMovingToStartPosition = false;
            }
        }
        else if (attackTimer <= 0)
        {
            StartCoroutine(ShootBulletsContinuously(1.0f)); 
            attackTimer = attackCooldown;
        }
    }

  
    void Phase2Behavior()
    {
        MoveSideToSide();
        if (attackTimer <= 0)
        {
            StartCoroutine(ShootBulletsContinuously(0.8f)); 
            attackTimer = attackCooldown;
        }
    }


    void Phase3Behavior()
    {
        MoveSideToSide();
        if (attackTimer <= 0)
        {
            StopCoroutine(ShootBulletsContinuously(1f));
            StartCoroutine(ShootBulletsAtPlayerContinuously(0.6f)); 
            attackTimer = attackCooldown * 0.75f; 
        }
    }

  
    void Phase4Behavior()
    {
        if (attackTimer <= 0)
        {
            Kamikaze();
            attackTimer = attackCooldown * 1.5f;
        }
    }

   
    void MoveSideToSide()
    {
        float newX = Mathf.PingPong(Time.time * moveSpeed, 8) - 4; 
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    IEnumerator ShootBulletsContinuously(float delayBetweenShots)
    {
        while (currentHealth > 0)
        {
            ShootForward();
            yield return new WaitForSeconds(delayBetweenShots);
        }
    }

    IEnumerator ShootBulletsAtPlayerContinuously(float delayBetweenShots)
    {
        while (currentHealth > 0)
        {
            ShootAtPlayer();
            yield return new WaitForSeconds(delayBetweenShots);
        }
    }

    void ShootForward()
    {
        Bullet b = Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0, 0, 180));
        
    }

    void ShootAtPlayer()
    {
        Vector2 direction = (player.position - firePoint.position).normalized;
        Bullet b = Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0, 0, 180));
        b.GetComponent<Rigidbody2D>().velocity = direction * 5f; 
    }

    void Kamikaze()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime * 2);
    }
}
