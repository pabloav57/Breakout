using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{

    Rigidbody2D rb;
    AudioSource sfx;

    int hitCount;

    int brickCount;

    int sceneId;

    //[SerializeField] Transform paddle;

    GameObject paddle;
    bool halved;

    [SerializeField] float force;
    [SerializeField] float forceInc;
    [SerializeField] float delay;
    [SerializeField] float hitOffset;

    [SerializeField] AudioClip sfxPaddle;
    [SerializeField] AudioClip sfxBrick;
    [SerializeField] AudioClip sfxWall;
    [SerializeField] AudioClip sfxFail;
    [SerializeField] AudioClip sfxNextLevel;

    Dictionary<string, int> bricks = new Dictionary<string, int>
    {
        {"brick-y", 10},
        {"brick-g", 15},
        {"brick-a", 20},
        {"brick-r", 25},
        {"brick-pass", 25}
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sceneId = SceneManager.GetActiveScene().buildIndex;
        rb = GetComponent<Rigidbody2D>();

        sfx = GetComponent<AudioSource>();

        paddle = GameObject.FindWithTag("paddle");

        Invoke("LaunchBall", delay);
    }

    private void LaunchBall()
    {
        // reset position and velocity
        transform.position = Vector3.zero;
        rb.linearVelocity = Vector2.zero;

        // get random direction
        float dirX, dirY = -1;
        dirX = Random.Range(0,2) == 0 ? -1 : 1;
        Vector2 dir = new Vector2(dirX, dirY);
        dir.Normalize();

        // apply force

        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        string tag = other.gameObject.tag;

        if(bricks.ContainsKey(tag))
        {
            DestroyBrick(other.gameObject);
        }

        else if(tag == "paddle")
        {
            sfx.clip = sfxPaddle;
            sfx.Play();

            // paddle position
            UnityEngine.Vector3 paddle = other.gameObject.transform.position;

            // get the contact point
            Vector2 contact = other.GetContact(0).point;

            if((rb.linearVelocity.x < 0) && contact.x > (paddle.x + hitOffset) || 
                (rb.linearVelocity.x > 0) && contact.x < (paddle.x - hitOffset))
            {
                rb.linearVelocity = new Vector2(-rb.linearVelocity.x, rb.linearVelocity.y);
            }

            // Increment the hit count
            hitCount++;
            if(hitCount % 4 == 0)
            {
                Debug.Log(hitCount + " -> Incremento Velocidad");
                rb.AddForce(rb.linearVelocity.normalized * forceInc, ForceMode2D.Impulse);

            }
        }

        else if(tag == "wall-top" || tag == "wall-lateral" || tag == "brick-rock"){

            sfx.clip = sfxWall;
            sfx.Play();

            if(!halved && tag == "wall-top")
            {
                // Reducir el tamaño de la pala
                HalvePaddle(true);

            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "wall-bottom")
        {
            sfx.clip = sfxFail;
            sfx.Play();
            GameController.UpdateLifes(-1);

            // Restaurar la pala a su tamaño original
            if(halved)
            HalvePaddle(false);

            Invoke("LaunchBall", delay);
        }

        else if(other.tag == "brick-pass")
        {
            DestroyBrick(other.gameObject);
        }
    }

    void DestroyBrick(GameObject obj)
    {
                    sfx.clip = sfxBrick;
            sfx.Play();
            // update player score
            GameController.UpdateScore(bricks[obj.tag]);

            Destroy(obj);

            ++brickCount;
            if(brickCount == GameController.totalBricks[sceneId])
            {
                sfx.clip = sfxNextLevel;
                sfx.Play();

                rb.linearVelocity = Vector2.zero;
                GetComponent<SpriteRenderer>().enabled = false;

                Invoke("NextScene", 3);
            }
    }

    void HalvePaddle(bool halve)
    {
        halved = halve;
        UnityEngine.Vector3 scale = paddle.transform.localScale;
        paddle.transform.localScale = halved ?
            new Vector3(scale.x * 0.5f, scale.y, scale.z):
            new Vector3(scale.x * 2f, scale.y, scale.z);
    }

    void NextScene()
    {
        int nextId = sceneId + 1;
        if(nextId == GameController.totalBricks.Count)
            nextId = 0;

        SceneManager.LoadScene(sceneId + 1);
    }
}
