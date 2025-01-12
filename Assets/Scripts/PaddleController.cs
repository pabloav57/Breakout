using UnityEngine;

public class PaddleController : MonoBehaviour
{

    const float MAX_X = 4.1f;
    const float MIN_X = -3.1f;

    [SerializeField] float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = transform.position.x;

        if(x > MIN_X && Input.GetKey("left"))
        transform.Translate(-speed * Time.deltaTime, 0, 0);

        if(x < MAX_X && Input.GetKey("right"))
        transform.Translate(speed * Time.deltaTime, 0, 0);
    }
}
