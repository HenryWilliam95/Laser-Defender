using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    float health = 150f;

    public float bulletSpeed;
    public GameObject laserBullet;

    public float shotsPerSecond = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile bullet = collision.gameObject.GetComponent<Projectile>();

        if(bullet)
        {
            health -= bullet.GetDamage();
            bullet.Hit();

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        float probability = Time.deltaTime * shotsPerSecond;

        if(Random.value < probability)
            Fire();
    }

    void Fire()
    {
        GameObject bullet = Instantiate(laserBullet, new Vector3(transform.position.x, transform.position.y - 1), Quaternion.identity) as GameObject;
        bullet.GetComponent<Rigidbody2D>().velocity = Vector3.down * bulletSpeed;
    }
}
