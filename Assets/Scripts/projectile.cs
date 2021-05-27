using UnityEngine;

public class projectile : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    public int damage = 10;

    public void Launch(Vector2 vector)
    {
        rigidbody.velocity = vector;
    }
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other)
            other.GetComponent<Player>().TakeDamage(damage);
        if (other.gameObject.layer == 8 || other.gameObject.layer == 10)
            Destroy(gameObject);
    }
}
