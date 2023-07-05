using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] float bulletSpeed = 25f;
    Rigidbody2D bulletRigidbody;
    PlayerMovement player;
    float xSpeed;
    
    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        FindObjectOfType<AudioManager>().Play("shoot");
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    void Update()
    {
        bulletRigidbody.velocity = new Vector2 (xSpeed, 0f);
    }


    void OnTriggerEnter2D(Collider2D other) 
    
    {
        if(other.tag == "Enemy")
        {
            Destroy(other.gameObject);
            FindObjectOfType<AudioManager>().Play("enemydeath");
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        Destroy(gameObject);
    }
}
