using UnityEngine;

public class PlayerObject : MonoBehaviour, IDamageable
{
    public int health = 100;

    void Awake()
    {
        // porfa no me grites xdxd
        Application.targetFrameRate = 60;
    }

    public void DealDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
