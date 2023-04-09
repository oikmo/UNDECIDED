using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour, IDamagable
{
    [SerializeField] Slider healthBar;
    [SerializeField] float health = 100f;

    public void Damage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
            Destroy(gameObject);
    }

    private void Update()
    {
        if(healthBar != null)
        {
            healthBar.value = health;
        }
        
    }
}
