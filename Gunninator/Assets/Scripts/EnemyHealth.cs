using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;


    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void Update()
    {
        if(health <= 0) Destroy(gameObject);
    }
}
