using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{

    private int score;
    public int scoreTimed;

    public static ScoreTracker Instance;
    public Text ScoreText, scoreTextTimed;
    public Text HighScoreText, highScoreTextTimed;

    public Animator animator;
    public Animator addTimeAnimator;

    public int Score
    {
        get
        {
            return score;
        }

        set
        {
            if (!FindObjectOfType<GameManager>().isTimedMode)
            {
                score += value;
                if (value > 0)
                    animator.Play("AddScore");
                animator.transform.GetChild(1).GetComponent<Text>().text = "+" + value.ToString();
                ScoreText.text = score.ToString();
                SaveScore();

                if (PlayerPrefs.GetInt("HighScore") < score)
                {
                    PlayerPrefs.SetInt("HighScore", score);
                    HighScoreText.text = score.ToString();
#if UNITY_ANDROID
                    //GPS.ReportScore(score, GPGSIds.leaderboard_endless_highscore);
                    //FindObjectOfType<GPS>().LoadFromCloud(true);
#elif UNITY_IOS
                    IOS.ReportScore(score, "makeit_endless");

#endif
                }
            }
            else
            {
                scoreTimed += value;
                if (value > 0)
                    addTimeAnimator.Play("AddScore");
                addTimeAnimator.transform.GetChild(1).GetComponent<Text>().text = "+" + (Mathf.Sqrt(value)-3).ToString();
                scoreTextTimed.text = scoreTimed.ToString();
                SaveScore();

                if (PlayerPrefs.GetInt("HighScoreTimed") < scoreTimed)
                {
                    PlayerPrefs.SetInt("HighScoreTimed", scoreTimed);
                    highScoreTextTimed.text = scoreTimed.ToString();
#if UNITY_ANDROID
                    //GPS.ReportScore(scoreTimed, GPGSIds.leaderboard_timed_highscore);
                    //FindObjectOfType<GPS>().LoadFromCloud(true);
#elif UNITY_IOS
                                        IOS.ReportScore(score, "makeit_timed");

#endif
                }
            }
        }
    }

    void Awake()
    {

        //PlayerPrefs.DeleteAll ();
        Instance = this;        

        if (!PlayerPrefs.HasKey("HighScore"))
            PlayerPrefs.SetInt("HighScore", 0);

        HighScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();


        ScoreText.text = PlayerPrefs.GetInt("prevScore").ToString();
        score = PlayerPrefs.GetInt("prevScore", 0);
        
        
        if (!PlayerPrefs.HasKey("HighScoreTimed"))
            PlayerPrefs.SetInt("HighScoreTimed", 0);

        highScoreTextTimed.text = PlayerPrefs.GetInt("HighScoreTimed").ToString();


        scoreTextTimed.text = 0.ToString();
        scoreTimed = 0;
        
    }

    void SaveScore()
    {
        if (!FindObjectOfType<GameManager>().isTimedMode)
            PlayerPrefs.SetInt("score", score);
        else PlayerPrefs.SetInt("scoreTimed", scoreTimed);
    }

    public void SetNewScore()
    {
        if (!FindObjectOfType<GameManager>().isTimedMode)
        {

            if (!PlayerPrefs.HasKey("HighScore"))
                PlayerPrefs.SetInt("HighScore", 0);

            HighScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();

            ScoreText.text = PlayerPrefs.GetInt("score").ToString();
            score = PlayerPrefs.GetInt("score", 0);
        }
        else
        {
            if (!PlayerPrefs.HasKey("HighScoreTimed"))
                PlayerPrefs.SetInt("HighScoreTimed", 0);

            highScoreTextTimed.text = PlayerPrefs.GetInt("HighScoreTimed").ToString();

            scoreTextTimed.text = PlayerPrefs.GetInt("scoreTimed").ToString();
            scoreTimed = PlayerPrefs.GetInt("scoreTimed", 0);
        }
    }
}
