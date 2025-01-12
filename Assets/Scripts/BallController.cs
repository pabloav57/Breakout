using UnityEngine;
using System.Collections.Generic;
using System.Numerics;

public class BallController : MonoBehaviour
{

    Rigidbody2D rb;
    AudioSource sfx;

    [SerializeField] float force;
    [SerializeField] float delay;
    [SerializeField] float hitOffset;

    [SerializeField] GameController game;

    [SerializeField] AudioClip sfxPaddle;
    [SerializeField] AudioClip sfxBrick;
    [SerializeField] AudioClip sfxWall;
    [SerializeField] AudioClip sfxFail;

    Dictionary<string, int> bricks = new Dictionary<string, int>
    {
        {"brick-y", 10},
        {"brick-g", 15},
        {"brick-a", 20},
        {"brick-r", 25}
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        sfx = GetComponent<AudioSource>();

        Invoke("LaunchBall", delay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LaunchBall()
    {
        // reset position and velocity
        transform.position = UnityEngine.Vector3.zero;
        rb.linearVelocity = UnityEngine.Vector2.zero;

        // get random direction
        float dirX, dirY = -1;
        dirX = Random.Range(0,2) == 0 ? -1 : 1;
        UnityEngine.Vector2 dir = new UnityEngine.Vector2(dirX, dirY);
        dir.Normalize();

        // apply force

        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        string tag = other.gameObject.tag;

        if(bricks.ContainsKey(tag))
        {
            sfx.clip = sfxBrick;
            sfx.Play();
            // update player score
            game.UpdateScore(bricks[tag]);
            Destroy(other.gameObject);
        }

        else if(tag == "paddle")
        {
            sfx.clip = sfxPaddle;
            sfx.Play();

            // paddle position
            UnityEngine.Vector3 paddle = other.gameObject.transform.position;

            // get the contact point
            UnityEngine.Vector2 contact = other.GetContact(0).point;

            if((rb.linearVelocity.x < 0) && contact.x > (paddle.x + hitOffset) || 
                (rb.linearVelocity.x > 0) && contact.x < (paddle.x - hitOffset))
            {
                rb.linearVelocity = new UnityEngine.Vector2(-rb.linearVelocity.x, rb.linearVelocity.y);
            }
        }

        else if(tag == "wall-top" || tag == "wall-lateral"){

            sfx.clip = sfxWall;
            sfx.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "wall-bottom")
        {
            sfx.clip = sfxFail;
            sfx.Play();
            game.UpdateLifes(-1);

            Invoke("LaunchBall", delay);
        }
    }
}
