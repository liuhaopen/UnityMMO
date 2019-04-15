using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct HealthStateData : IComponentData 
{
    public float health;
    public float maxHealth;     
    public int deathTick;
    public Entity killedBy;

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
    }
    
    public void ApplyDamage(ref DamageEvent damageEvent, int tick)
    {
        if (health <= 0)
            return;

        health -= damageEvent.damage;
        if (health <= 0)
        {
            killedBy = damageEvent.instigator;
            deathTick = tick;
            health = 0;
        }
    }
}
