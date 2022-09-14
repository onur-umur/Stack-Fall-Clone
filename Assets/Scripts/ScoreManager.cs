using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int score;
    private Text scoreTxt;

    private void Awake()
    {
        
        makeSingleton();
        scoreTxt = GameObject.Find("ScoreTxt").GetComponent<Text>();

    }
    private void makeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        addScore(0);
    }

    // Update is called once per frame
    void Update()
    {

        if (scoreTxt == null)
        {
            scoreTxt = GameObject.Find("ScoreTxt").GetComponent<Text>();
        }


    }
    public void addScore(int Value)
    {
        score += Value;
        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        scoreTxt.text = score.ToString();
    }

    public void ResetScore()
    {
        score = 0;
    }
}
