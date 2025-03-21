using UnityEngine;

public class PlayerObject : MonoBehaviour, IDamageable
{
    public int health = 100;

    public void DealDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
