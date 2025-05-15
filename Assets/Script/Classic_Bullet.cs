using UnityEngine;

public class Classic_Bullet : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        } else if (collision.gameObject.CompareTag("Player"))
        {
            
        }
    }
}
