using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int score = 0;

    [SerializeField] int lifes;
    [SerializeField] Text txtScore;
    [SerializeField] Text txtLifes;

    public void UpdateScore(int points){ score += points; }
    public void UpdateLifes(int numLifes) { lifes += numLifes;}


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI() {
        txtScore.text = string.Format("{0,3:D3}",score);
        txtLifes.text = lifes.ToString();
    }
}
