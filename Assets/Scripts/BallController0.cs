using UnityEngine;
using System.Collections.Generic;

public class BallController0 : MonoBehaviour
{

    Rigidbody2D rb;
    AudioSource sfx;

    [SerializeField] float force;
    [SerializeField] float delay;

    [SerializeField] AudioClip sfxPaddle;
    [SerializeField] AudioClip sfxBrick;
    [SerializeField] AudioClip sfxWall;

    List<string> bricks = new List<string>
    {
        {"brick-y"},
        {"brick-g"},
        {"brick-a"},
        {"brick-r"}
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        sfx = GetComponent<AudioSource>();

        Invoke("LaunchBall", delay);
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

        if(bricks.Contains(tag))
        {
            sfx.clip = sfxBrick;
            sfx.Play();
            Destroy(other.gameObject);
        }

        else if(tag == "paddle")
        {
            sfx.clip = sfxPaddle;
            sfx.Play();

            // paddle position
            UnityEngine.Vector3 paddle = other.gameObject.transform.position;
        }

        else if(tag == "wall-top" || tag == "wall-lateral"){

            sfx.clip = sfxWall;
            sfx.Play();
        }
    }
}
