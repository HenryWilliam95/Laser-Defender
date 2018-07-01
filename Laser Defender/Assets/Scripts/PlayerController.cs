using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float padding = 1f;

    private float xMin;
    private float xMax;

    public GameObject laserBullet;
    public float bulletSpeed = 5f;
    public float firingRate = 0.2f;
    public float health = 250;

	// Use this for initialization
	void Start () {
        // Distance between player and camera
        float distance = transform.position.z - Camera.main.transform.position.z;

        /*
         Vector takes screen coordinates, 0,0 is bottom left 1,1 is top right.  
         padding is used to bring the player slightly away from the edge of the screen
         */
        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xMin = leftMost.x + padding;
        xMax = rightMost.x - padding;
    }
	
	// Update is called once per frame
	void Update () {
        PlayerMovement();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("PlayerShoot", 0.000001f, firingRate);
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("PlayerShoot");
        }
    }

    void PlayerMovement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        // Restrict player to the game space
        float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    void PlayerShoot()
    {      
            GameObject bullet = Instantiate(laserBullet, new Vector3(transform.position.x, transform.position.y + 1), Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody2D>().velocity = Vector3.up * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile missle = collision.gameObject.GetComponent<Projectile>();

        if (missle)
        {
            health -= missle.GetDamage();
            missle.Hit();
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
