﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float offsetAngle = -90f;

    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;
    public float recoilForce = 50f;
    public float maxSpeed = 50f;
    public float friction = 0.98f;
    private Rigidbody2D rb;
    public Transform GunPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Shoot(Vector2 direction)
    {
        float bulletAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject bullet = Instantiate(bulletPrefab, GunPoint.position, Quaternion.Euler(0, 0, bulletAngle));
        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
        rbBullet.velocity = direction.normalized * bulletSpeed;
        Vector2 recoilDirection = -direction.normalized;
        rb.AddForce(recoilDirection * recoilForce, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            rb.velocity = Vector2.zero;
        }
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 direction = new Vector2(
            mousePos.x - transform.position.x,
            mousePos.y - transform.position.y
        );

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + offsetAngle);

        if (Input.GetMouseButton(0) && Time.time > nextFireTime)
        {
            Shoot(direction);
            nextFireTime = Time.time + fireRate;
        }

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        else
        {
            rb.velocity *= friction;
        }


    }
}
