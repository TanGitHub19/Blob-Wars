using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 50f;
    public float lifeTime = 3f;
    public int damage = 10;
    public GameObject impactEffect;

    private Vector3 direction;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime; 
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            Debug.Log("Hit Target! Dealt " + damage + " damage.");

            if (impactEffect != null)
                Instantiate(impactEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

    /// <summary>
    //
    /// </summary>
    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
        transform.forward = direction; 

        Debug.Log("Projectile Direction Set To: " + direction);
    }

}
