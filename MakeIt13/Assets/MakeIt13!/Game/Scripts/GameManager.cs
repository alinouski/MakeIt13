using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum GameState
{
    Playing,
    GameOver,
    WaitingForMoveToEnd
}


public class GameManager : MonoBehaviour
{

    // NEW AFTER ADDED DELAYS
    public GameState State;
    [Range(0, 2f)]
    public float delay; //Delay between the falling tiles
    //private bool moveMade;
    private bool[] lineMoveComplete = new bool[5] { true, true, true, true, true };
    // NEW AFTER ADDED DELAYS

    //public GameObject YouWonText;
    public GameObject GameOverText;
    public Text GameOverScoreText;
    public GameObject GameOverPanel;

    [HideInInspector]
    public Tile[,] AllTiles = new Tile[5, 5];
    private List<Tile[]> columns = new List<Tile[]>();
    private List<Tile[]> rows = new List<Tile[]>();
    private List<Tile> EmptyTiles = new List<Tile>();

    [HideInInspector]
    public List<GameObject> mergeTiles = new List<GameObject>();

    public GameObject cancelDrag;

    public LineRenderer lineRenderer;

    
    public bool isTimedMode = false;
    [ConditionalField("isTimedMode", true)]
    public float maxTime;
    [ConditionalField("isTimedMode", true)]
    public float currentTime;
    [ConditionalField("isTimedMode", true)]
    public Text timer;

    //public int highestNumber = 0;
    // Use this for initialization
    void Start()
    {
        mergeTiles.Clear();

        Tile[] AllTilesOneDim = GameObject.FindObjectsOfType<Tile>();
        foreach (Tile t in AllTilesOneDim)
        {
            t.Number = 0;
            AllTiles[t.indRow, t.indCol] = t;
            EmptyTiles.Add(t);
        }

        columns.Add(new Tile[] { AllTiles[0, 0], AllTiles[1, 0], AllTiles[2, 0], AllTiles[3, 0], AllTiles[4, 0] });
        columns.Add(new Tile[] { AllTiles[0, 1], AllTiles[1, 1], AllTiles[2, 1], AllTiles[3, 1], AllTiles[4, 1] });
        columns.Add(new Tile[] { AllTiles[0, 2], AllTiles[1, 2], AllTiles[2, 2], AllTiles[3, 2], AllTiles[4, 2] });
        columns.Add(new Tile[] { AllTiles[0, 3], AllTiles[1, 3], AllTiles[2, 3], AllTiles[3, 3], AllTiles[4, 3] });
        columns.Add(new Tile[] { AllTiles[0, 4], AllTiles[1, 4], AllTiles[2, 4], AllTiles[3, 4], AllTiles[4, 4] });

        rows.Add(new Tile[] { AllTiles[0, 0], AllTiles[0, 1], AllTiles[0, 2], AllTiles[0, 3], AllTiles[0, 4] });
        rows.Add(new Tile[] { AllTiles[1, 0], AllTiles[1, 1], AllTiles[1, 2], AllTiles[1, 3], AllTiles[1, 4] });
        rows.Add(new Tile[] { AllTiles[2, 0], AllTiles[2, 1], AllTiles[2, 2], AllTiles[2, 3], AllTiles[2, 4] });
        rows.Add(new Tile[] { AllTiles[3, 0], AllTiles[3, 1], AllTiles[3, 2], AllTiles[3, 3], AllTiles[3, 4] });
        rows.Add(new Tile[] { AllTiles[4, 0], AllTiles[4, 1], AllTiles[4, 2], AllTiles[4, 3], AllTiles[4, 4] });

        State = GameState.Playing;

        for (int i = 0; i < columns.Count; i++)
        {
            for (int j = 0; j < rows.Count; j++)
            {
                AllTiles[i, j].Number = 0;
            }
        }

        if (!PlayerPrefs.HasKey("saveExists") || isTimedMode)
            for (int i = 0; i < 25; i++)
            {
                Invoke("Generate", 0.6f + i * 0.04f);
            }
        else
        {
            LoadBoard(false);
        }

        if (isTimedMode)
        {
            timer.gameObject.SetActive(true);
            currentTime = maxTime;
        }

    }

    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("show_ads") != 1)
            FindObjectOfType<AdmobScript>().ShowBanner();
    }

    private void OnDisable()
    {
        //FGLEnhance.HideBannerAd();
    }

    public void LoadBoard(bool undo)
    {
        if (!PlayerPrefs.HasKey("saveExists")) return;
        if (undo && ScoreTracker.Instance.Score == PlayerPrefs.GetInt("prevScore")) return;
        for (int i = 0; i < columns.Count; i++)
        {
            for (int j = 0; j < rows.Count; j++)
            {
                StartCoroutine(InvokeAppearAnim(i, j, 0.6f + i * 0.3f + j * 0.035f));
            }
        }

        if (undo)
        {
            ScoreTracker.Instance.Score = PlayerPrefs.GetInt("prevScore") - ScoreTracker.Instance.Score;
            ScoreTracker.Instance.ScoreText.text = PlayerPrefs.GetInt("prevScore").ToString();
        }
    }

    private IEnumerator InvokeAppearAnim(int x, int y, float time)
    {
        float timespent = 0;
        while (timespent < time)
        {
            timespent += Time.deltaTime;
            yield return null;
        }
        AllTiles[x, y].Number = PlayerPrefs.GetInt("save " + x.ToString() + y.ToString());
        AllTiles[x, y].PlayAppearAnimation();
    }

    private void Update()
    {
        if (!cancelDrag.activeSelf && mergeTiles.Count > 0)
        {
            cancelDrag.SetActive(true);
        }
        else if (cancelDrag.activeSelf && mergeTiles.Count == 0)
        {
            cancelDrag.SetActive(false);
        }

        if (Input.touchCount > 1)
        {
            AllTiles[0, 0].ResetmergeTiles();
            EmptymergedTiles();
            lineRenderer.positionCount = 0;
        }

        if (isTimedMode)
        {
            if (currentTime <= 0 && State != GameState.GameOver)
            {
                GameOver();
                return;
            }

            if (currentTime > 0) currentTime -= Time.deltaTime;

            timer.text = ((int)currentTime).ToString();
        }
    }

    private void GameOver()
    {
        State = GameState.GameOver;
        if (isTimedMode)
            GameOverScoreText.text = ScoreTracker.Instance.scoreTimed.ToString();
        else
        {
            PlayerPrefs.DeleteKey("saveExists");
            GameOverScoreText.text = ScoreTracker.Instance.Score.ToString();
        }
        GameOverPanel.SetActive(true);

        if (Random.Range(0, 3) == 1 && PlayerPrefs.GetInt("shared") == 0) FindObjectOfType<Main>().ShowShareRateWindow();
        else FindObjectOfType<AdmobScript>().ShowInterstitialAd();

        StartCoroutine(GameOverFadeIn());
    }


    IEnumerator GameOverFadeIn()
    {
        GameOverPanel.GetComponent<Image>().color = new Color(160f / 255f, 122f / 255f, 156f / 255f, 0);
        float alpha = 0;
        while (GameOverPanel.GetComponent<Image>().color.a < 225f / 255f)
        {
            alpha += Time.deltaTime;
            GameOverPanel.GetComponent<Image>().color = new Color(160f / 255f, 122f / 255f, 156f / 255f, alpha);
            yield return null;
        }
    }

    void CanMove()
    {
        Debug.Log("CanMove()");
        //You have possible moves if there is a '1' tile and a '2' tile near each other
        for (int i = 0; i < columns.Count; i++)
        {
            for (int j = 0; j < rows.Count; j++)
            {
                if (AllTiles[i, j].Number == 1)
                {
                    for (int i2 = 0; i2 < columns.Count; i2++)
                    {
                        for (int j2 = 0; j2 < rows.Count; j2++)
                        {
                            if (Mathf.Abs(AllTiles[i, j].indCol - AllTiles[i2, j2].indCol) <= 1 &&
                            Mathf.Abs(AllTiles[i, j].indRow - AllTiles[i2, j2].indRow) <= 1)
                            {
                                if (AllTiles[i2, j2].Number == 2 || AllTiles[i2, j2].Number == 1)
                                {
                                    if (i2 != i || j != j2)
                                        return;
                                }
                            }
                        }
                    }
                }
            }
        }
        Debug.Log("GAMEOVER");
        GameOver();
    }

    public void NewGameButtonHandler()
    {
        currentTime = maxTime;

        if (!isTimedMode)
        {
            PlayerPrefs.DeleteKey("saveExists");
            ScoreTracker.Instance.ScoreText.text = "0";
            ScoreTracker.Instance.Score = 0;
            PlayerPrefs.DeleteKey("score");
            PlayerPrefs.DeleteKey("prevScore");
        }

        if (isTimedMode)
        {
            ScoreTracker.Instance.scoreTextTimed.text = "0";
            ScoreTracker.Instance.scoreTimed = 0;
            PlayerPrefs.DeleteKey("scoreTimed");
        }

        ScoreTracker.Instance.SetNewScore();

        Tile[] AllTilesOneDim = GameObject.FindObjectsOfType<Tile>();
        EmptyTiles.Clear();
        foreach (Tile t in AllTilesOneDim)
        {
            t.Number = 0;
            AllTiles[t.indRow, t.indCol] = t;
            EmptyTiles.Add(t);
        }

        for (int i = 0; i < columns.Count; i++)
        {
            for (int j = 0; j < rows.Count; j++)
            {
                AllTiles[i, j].Number = 0;

            }
        }

        for (int i = 0; i < 25; i++)
        {
            Invoke("Generate", 0.6f + i * 0.04f);
        }

        GameOverPanel.SetActive(false);

        State = GameState.Playing;

        FindObjectOfType<AdmobScript>().ShowInterstitialAd();
    }

    bool MakeOneMoveDownIndex(Tile[] LineOfTiles)
    {
        for (int i = 0; i < LineOfTiles.Length - 1; i++)
        {
            //MOVE BLOCK 
            if (LineOfTiles[i].Number == 0 && LineOfTiles[i + 1].Number != 0)
            {
                LineOfTiles[i].Number = LineOfTiles[i + 1].Number;
                LineOfTiles[i + 1].Number = 0;
                return true;
            }
        }
        return false;
    }

    bool MakeOneMoveUpIndex(Tile[] LineOfTiles, Vector3[] originalPositions)
    {
        for (int i = LineOfTiles.Length - 1; i > 0; i--)
        {
            //MOVE BLOCK 
            if (LineOfTiles[i].Number == 0 && LineOfTiles[i - 1].Number != 0)
            {
                LineOfTiles[i].Number = LineOfTiles[i - 1].Number;
                LineOfTiles[i - 1].Number = 0;

                StartCoroutine(SmoothMoveTile(LineOfTiles, delay / 1.1f, i, originalPositions));
                return true;
            }

        }
        return false;
    }

    IEnumerator SmoothMoveTile(Tile[] LineOfTiles, float time, int which, Vector3[] originalPositions)
    {
        Vector3 startPosition = originalPositions[which - 1];
        Vector3 endPosition = originalPositions[which];
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            LineOfTiles[which].transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / time);

            yield return null;
        }

        LineOfTiles[which].transform.position = endPosition;
    }

    public void Generate()
    {
        if (EmptyTiles.Count > 0)
        {
            int indexForNewNumber = Random.Range(0, EmptyTiles.Count);
            int randomNum = Random.Range(0, 100);

            EmptyTiles[indexForNewNumber].transform.GetChild(0).localScale = Vector2.zero;

            if (randomNum < 5)
                EmptyTiles[indexForNewNumber].Number = 4;
            else if (randomNum < 18)
                EmptyTiles[indexForNewNumber].Number = 3;
            else if (randomNum < 45)
                EmptyTiles[indexForNewNumber].Number = 2;
            else
                EmptyTiles[indexForNewNumber].Number = 1;

            EmptyTiles[indexForNewNumber].PlayAppearAnimation();

            EmptyTiles.RemoveAt(indexForNewNumber);
        }
    }

    public void GenerateAll()
    {
        for (int i = 0; i <= EmptyTiles.Count + 1; i++)
        {
            Invoke("Generate", 0.05f * i);
        }

    }

    public void UpdateEmptyTiles()
    {
        EmptyTiles.Clear();
        foreach (Tile t in AllTiles)
        {
            if (t.Number == 0)
                EmptyTiles.Add(t);
        }
    }

    public void Move()
    {
        //moveMade = false;
        StartCoroutine(MoveCoroutine());
        /*else 
		{
			for (int i =0; i< rows.Count; i++) 
			{
                while (MakeOneMoveUpIndex(columns[i]))
                {
                    moveMade = true;
                }
            }

			if (moveMade) 
			{
                Debug.Log("MoveMade");
				UpdateEmptyTiles ();
                GenerateAll();
                Invoke("CanMove", 0.6f);
			
			}
		}*/
    }

    IEnumerator MoveOneLineUpIndexCoroutine(Tile[] line, int index)
    {
        lineMoveComplete[index] = false;

        Vector3[] originalPositions = new Vector3[line.Length];
        for (int i = 0; i < line.Length; i++) originalPositions[i] = line[i].transform.position;

        while (MakeOneMoveUpIndex(line, originalPositions))
        {
            //moveMade = true;
            yield return new WaitForSeconds(delay);
        }
        lineMoveComplete[index] = true;
    }

    IEnumerator MoveOneLineDownIndexCoroutine(Tile[] line, int index)
    {
        lineMoveComplete[index] = false;
        while (MakeOneMoveDownIndex(line))
        {
            //moveMade = true;
            yield return new WaitForSeconds(delay);
        }
        lineMoveComplete[index] = true;
    }


    IEnumerator MoveCoroutine()
    {
        State = GameState.WaitingForMoveToEnd;

        // start moving each line with delays
        for (int i = 0; i < columns.Count; i++)
            StartCoroutine(MoveOneLineUpIndexCoroutine(columns[i], i));

        // Wait until the move is over in all lines
        while (!(lineMoveComplete[0] && lineMoveComplete[1] && lineMoveComplete[2] && lineMoveComplete[3] && lineMoveComplete[4]))
            yield return null;


        Invoke("CanMove", 0.6f);
        UpdateEmptyTiles();
        GenerateAll();

        
        StopAllCoroutines();
    }

    public void SaveBoard()
    {
        if (isTimedMode) return;
        for (int i = 0; i < columns.Count; i++)
        {
            for (int j = 0; j < rows.Count; j++)
            {
                PlayerPrefs.SetInt("save " + i.ToString() + j.ToString(), AllTiles[i, j].Number);
            }
        }
    }

    public void EmptymergedTiles()
    {
        if (State != GameState.WaitingForMoveToEnd)
            mergeTiles.Clear();
    }
}
