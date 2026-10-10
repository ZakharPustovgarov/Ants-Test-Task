using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    protected int health;

    protected virtual void Start()
    {
        health = maxHealth;
    }

    public bool ReciveHealing(int healing)
    {
        health += healing;

        if (health >= maxHealth)
        {
            health = maxHealth;
            return true;
        }

        return false;
    }

    public bool InflictDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            OnDeath();
            return true;
        }

        return false;
    }

    protected abstract void OnDeath();
}
