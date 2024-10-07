using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpType powerUpType;
    public float value;
    public float shieldDuration = 5f;
    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveForward();
    }
    void MoveForward()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player p = collision.GetComponent<Player>();
            if (p != null)
            {
                ApplyPowerUp(p);
                Destroy(gameObject);
            }
        }
    }
    void ApplyPowerUp(Player player)
    {
        switch (powerUpType)
        {
            case PowerUpType.ExtraDamage:
                player.damage += value;
                Debug.Log("Daño Extra");
                break;
            case PowerUpType.ExtraSpeed:
                player.speed += value;
                Debug.Log("Velocidad Extra");
                break;
            case PowerUpType.ProjectileSpeed:
                player.bulletSpeed += value;
                Debug.Log("Velocidad del Proyectil");
                break;
            case PowerUpType.CritChance:
                player.hasCritChancePowerUp = true;
                Debug.Log("Probabilidad de Crítico activada (30%)");
                break;
            case PowerUpType.FireRate:
                player.timeBtwShoot -= value;
                if(player.timeBtwShoot <= 0)
                     player.timeBtwShoot = 0.1f;
                Debug.Log("Velocidad de Disparo Aumentada");
                break;
            case PowerUpType.Shield:
                player.ActivateShield(shieldDuration);
                Debug.Log(" Escudo Activo");
                break;
        }
    }

    public enum PowerUpType
    {
        ExtraDamage,
        ExtraSpeed,
        ProjectileSpeed,
        CritChance,
        FireRate,
        Shield
    }
}