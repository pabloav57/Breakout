using System.Collections;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.UI;

public class TextColorLerp : MonoBehaviour
{
    [SerializeField] Text msg;
    [SerializeField] float duration;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("ChangeColor");
    }

    IEnumerator ChangeColor()
    {
        float t = 0;
        while(t < duration)
        {
            t += Time.deltaTime;
            msg.color = Color.Lerp(Color.black, Color.white, t/duration);
            yield return null;
        }
        // Reiniciar corutina
        StartCoroutine("ChangeColor");
    }
}
