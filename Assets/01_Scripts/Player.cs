using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    float life = 3;
    public float maxLife = 3;
    float timer = 0;
    public int bullets = 10;
    int currentBullets;
    bool canShoot = true;
    public Rigidbody rb;
    public Transform firePoint;
    public Bullet bulletPrefab;
    public float damage = 1f;
    public float speed = 2f;
    public float timeBtwShoot = 1.5f;
    public float bulletSpeed = 5f;
    public bool hasCritChancePowerUp = false;
    public bool shieldActive = false;
    public GameObject shieldObject;
    public Text lifeText;
    public GameObject deathParticlesPrefab;
    public Image lifeBar;
    void Start()
    {
        Debug.Log("Inició el juego");
        currentBullets = bullets;
        life = maxLife;
        lifeBar.fillAmount = life / maxLife;
        shieldObject.SetActive(false);
        lifeText.text = "Life = " + life;
    }

    void Update()
    {
        Movement();
        Reload();
        CheckIfCanShoot();
        Shoot();
    }

    public void ActivateShield(float duration)
    {
        shieldActive = true;
        shieldObject.SetActive(true);
        Invoke("DeactivateShield", duration);
    }

    void DeactivateShield()
    {
        shieldActive = false;
        shieldObject.SetActive(false);
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(x, 0, z) * speed;
    }

    public void TakeDamage(float dmg)
    {
        if (!shieldActive)
        {
            life -= dmg;
            lifeBar.fillAmount = life / maxLife;
            lifeText.text = "Life = " + life;
            if (life <= 0)
            {
                Instantiate(deathParticlesPrefab, transform.position, transform.rotation);
                gameObject.SetActive(false);
                Invoke("ReloadScene", 1f);
            }
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene("Game");
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canShoot && currentBullets > 0)
        {
            Bullet b = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            if (hasCritChancePowerUp)
            {
                float randomValue = Random.Range(0f, 100f);
                if (randomValue <= 30f)
                {
                    b.damage = damage * 2;
                    Debug.Log("¡Golpe crítico! Daño: " + b.damage);
                }
                else
                {
                    b.damage = damage;
                    Debug.Log("Disparo normal. Daño: " + b.damage);
                }
            }
            else
            {
                b.damage = damage;
                Debug.Log("Disparo normal. Daño: " + b.damage);
            }
            b.speed = bulletSpeed;
            currentBullets--;
            canShoot = false;
        }
    }
    void Reload()
    {
        if (currentBullets == 0 && Input.GetKeyDown(KeyCode.R))
        {
            currentBullets = bullets;
        }
    }
    void CheckIfCanShoot()
    {
        if (!canShoot)
        {
            if (timer < timeBtwShoot)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                canShoot = true;
            }
        }
    }
}
