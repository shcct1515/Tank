using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float offsetAngle = -90f;

    public GameObject bulletPrefab;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;
    public float recoilForce = 30f;
    public float maxSpeed = 30f;
    public float friction = 0.95f;
    private Rigidbody2D rb;
    public Transform GunPoint;

    private Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Shoot(Vector2 dir)
    {
        GameObject bullet = Instantiate(
            bulletPrefab,
            GunPoint.position,
            Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg)
        );

        bullet.GetComponent<Classic_Bullet>().SetDirection(dir);
        rb.AddForce(-dir.normalized * recoilForce, ForceMode2D.Impulse);
    }

    void Update()
    {
        // Xoay theo chuột
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = (mousePos - transform.position);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + offsetAngle);

        // Bắn đạn
        if (Input.GetMouseButton(0) && Time.time > nextFireTime)
        {
            Shoot(direction);
            nextFireTime = Time.time + fireRate;
        }
    }

    void FixedUpdate()
    {
        // Giới hạn tốc độ và giảm tốc (friction)
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
        else
        {
            rb.linearVelocity *= friction;
        }
    }
}