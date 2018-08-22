using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{

    public int indRow;
    public int indCol;

    public int Number
    {
        get
        {
            return number;
        }
        set
        {
            number = value;
            if (number == 0)
                SetEmpty();
            else
            {
                ApplyStyleFromHolder(number - 1);
                SetVisible();
            }
        }
    }


    private int number;

    private Text TileText;
    private Image TileImage;
    private Animator anim;

    public List<GameObject> mergeTiles;
    public GameObject draggedOver, draggedOver2;

    private GameManager gm;

    private ThemeManager tm;


    private void OnEnable()
    {
        ThemeChange();
    }

    public void ThemeChange()
    {
        if (number > 0)
            ApplyStyleFromHolder(number - 1);
    }

    void Awake()
    {
        anim = GetComponent<Animator>();
        TileText = GetComponentInChildren<Text>();
        TileImage = transform.Find("NumberedCell").GetComponent<Image>();

        tm = FindObjectOfType<ThemeManager>();
       }

    public void PlayMergeAnimation()
    {
        anim.SetTrigger("Merge");
    }

    public void PlayAppearAnimation()
    {
        anim.SetTrigger("Appear");
    }

    void ApplyStyleFromHolder(int index)
    {
        TileText.text = TileStyleHolder.Instance.TileStylesHolder[tm.currentThemeId].Number[index].ToString();

        TileText.color = TileStyleHolder.Instance.TileStylesHolder[tm.currentThemeId].TextColor[index];
        TileImage.color = TileStyleHolder.Instance.TileStylesHolder[tm.currentThemeId].TileColor[index];
    }

    void ApplyStyle(int num)
    {
        switch (num)
        {
            case 1:
                ApplyStyleFromHolder(0);
                break;
            case 2:
                ApplyStyleFromHolder(1);
                break;
            case 3:
                ApplyStyleFromHolder(2);
                break;
            case 4:
                ApplyStyleFromHolder(3);
                break;
            case 5:
                ApplyStyleFromHolder(4);
                break;
            case 6:
                ApplyStyleFromHolder(5);
                break;
            case 7:
                ApplyStyleFromHolder(6);
                break;
            case 8:
                ApplyStyleFromHolder(7);
                break;
            case 9:
                ApplyStyleFromHolder(8);
                break;
            case 10:
                ApplyStyleFromHolder(9);
                break;
            case 11:
                ApplyStyleFromHolder(10);
                break;
            case 12:
                ApplyStyleFromHolder(11);
                break;
            default:
                Debug.LogError("Check the numbers that you pass to ApplyStyle!");
                break;
        }
    }

    private void SetVisible()
    {
        TileImage.enabled = true;
        TileText.enabled = true;
    }

    private void SetEmpty()
    {
        TileImage.enabled = false;
        TileText.enabled = false;
    }

    // Use this for initialization
    void Start()
    {
        //Adding the triggers manually
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
        trigger.triggers.Add(entry);

        trigger = GetComponent<EventTrigger>();
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.EndDrag;
        entry.callback.AddListener((data) => { OnEndDrag(); });
        trigger.triggers.Add(entry);

        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (gm.State != GameState.Playing) return;
        if (eventData.pointerEnter == null) return;

        if (eventData.pointerEnter.name.Contains("TileNumber"))
        {
            if (!gm.mergeTiles.Contains(eventData.pointerEnter.transform.parent.parent.gameObject)) //If new tile dragged
            {
                if (draggedOver == null || Mathf.Abs(eventData.pointerEnter.transform.parent.parent.GetComponent<Tile>().indCol - draggedOver.transform.parent.parent.GetComponent<Tile>().indCol) <= 1 &&
                    Mathf.Abs(eventData.pointerEnter.transform.parent.parent.GetComponent<Tile>().indRow - draggedOver.transform.parent.parent.GetComponent<Tile>().indRow) <= 1) //check if the tile is neighbouring
                {

                    if (draggedOver == null || eventData.pointerEnter.transform.parent.parent.GetComponent<Tile>().Number == draggedOver.transform.parent.parent.GetComponent<Tile>().number + 1)
                    {
                        if (gm.mergeTiles.Count > 1 && gm.mergeTiles[1].GetComponent<Tile>().Number == 1) return;
                        eventData.pointerEnter.transform.parent.parent.GetComponent<RectTransform>().localScale = new Vector3(1.12f, 1.12f, 1);

                        gm.mergeTiles.Add(eventData.pointerEnter.transform.parent.parent.gameObject);
                        draggedOver = eventData.pointerEnter.gameObject;

                        gm.lineRenderer.positionCount++;
                        gm.lineRenderer.SetPosition(gm.lineRenderer.positionCount - 1, gm.mergeTiles[gm.lineRenderer.positionCount - 1].transform.localPosition + Vector3.down * 15);


                    }
                    else if (gm.mergeTiles.Count == 1 && eventData.pointerEnter.transform.parent.parent.GetComponent<Tile>().Number == 1 &&
                    gm.mergeTiles[0].GetComponent<Tile>().Number == 1)
                    {
                        eventData.pointerEnter.transform.parent.parent.GetComponent<RectTransform>().localScale = new Vector3(1.12f, 1.12f, 1);

                        gm.mergeTiles.Add(eventData.pointerEnter.transform.parent.parent.gameObject);
                        draggedOver = eventData.pointerEnter.gameObject;

                        gm.lineRenderer.positionCount++;
                        gm.lineRenderer.SetPosition(gm.lineRenderer.positionCount - 1, gm.mergeTiles[gm.lineRenderer.positionCount - 1].transform.localPosition + Vector3.down*15);
                    }
                }
            }
            else
            {
                if (gm.mergeTiles.Count > 1)
                    if (draggedOver2.transform.parent.parent.name.Equals(gm.mergeTiles[gm.mergeTiles.Count - 2].name) && draggedOver != eventData.pointerEnter)
                    {
                        gm.mergeTiles[gm.mergeTiles.Count - 1].GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1);


                        gm.mergeTiles.Remove(gm.mergeTiles[gm.mergeTiles.Count - 1]);
                        draggedOver = gm.mergeTiles[gm.mergeTiles.Count - 1].GetComponentInChildren<Text>().gameObject;

                        gm.lineRenderer.positionCount--;

                    }
            }
            draggedOver2 = eventData.pointerEnter;
        }
    }

    public void OnEndDrag()
    {
        //if (gm.State == GameState.Playing)
        //{
            StartCoroutine(OnEndDragCoroutine());
           
        //}
        gm.lineRenderer.positionCount = 0;
    }

    IEnumerator OnEndDragCoroutine()
    {
        ResetmergeTiles();
        if (gm.mergeTiles.Count > 1) //if at least two tiles were dragged
        {
            gm.State = GameState.WaitingForMoveToEnd;
            if (gm.mergeTiles[gm.mergeTiles.Count - 1].GetComponent<Tile>().Number == 1
                || gm.mergeTiles[gm.mergeTiles.Count - 1].GetComponent<Tile>().Number == gm.mergeTiles.Count)
            {
                Save();

                //Drag animation
                Vector3[] starterPositions = new Vector3[gm.mergeTiles.Count - 1];
                for (int i = 0; i < gm.mergeTiles.Count - 1; i++)
                {
                    starterPositions[i] = gm.mergeTiles[i].transform.localPosition;
                }
                Vector3 startingPos;
                float dlt;
                for (int i = 0; i < gm.mergeTiles.Count - 1; i++)
                {
                    float elapsedTime = 0;
                    startingPos = gm.mergeTiles[i].transform.localPosition;
                    while (elapsedTime < 0.13f)
                    {
                        dlt = elapsedTime / 0.13f;
                        for (int j = 0; j < i; j++)
                        {
                            gm.mergeTiles[j].transform.localPosition = Vector3.Lerp(startingPos, gm.mergeTiles[i + 1].transform.localPosition, dlt);
                        }
                        try
                        {
                            gm.mergeTiles[i].transform.localPosition = Vector3.Lerp(startingPos, gm.mergeTiles[i + 1].transform.localPosition, dlt);
                        }
                        catch
                        {
                            Debug.Log("");
                        }
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                }

                for (int i = 0; i < gm.mergeTiles.Count - 1; i++)
                {
                    gm.mergeTiles[i].GetComponent<Tile>().Number = 0;
                }

                for (int i = 0; i < gm.mergeTiles.Count - 1; i++)
                {
                    gm.mergeTiles[i].transform.localPosition = starterPositions[i];
                }

                gm.mergeTiles[gm.mergeTiles.Count - 1].GetComponent<Tile>().PlayMergeAnimation();
                gm.mergeTiles[gm.mergeTiles.Count - 1].GetComponent<Tile>().Number++;

                FindObjectOfType<AudioManager>().PlayMerged();

                if (!gm.isTimedMode)
                    PlayerPrefs.SetInt("prevScore", ScoreTracker.Instance.Score);
                ScoreTracker.Instance.Score = (gm.mergeTiles[gm.mergeTiles.Count - 1].GetComponent<Tile>().Number + 1) * (gm.mergeTiles[gm.mergeTiles.Count - 1].GetComponent<Tile>().Number + 1);
                if (gm.isTimedMode)
                {
                    gm.currentTime += gm.mergeTiles[gm.mergeTiles.Count - 1].GetComponent<Tile>().Number - 2;
                }

                Move();
                //Invoke("Move", 0.25f);
            }
        }

        gm.mergeTiles.Clear();
        draggedOver = null;
        gm.State = GameState.Playing;
    }

    private void Move()
    {
        GameObject.FindObjectOfType<GameManager>().Move();
    }

    private void Save()
    {
        if (gm.isTimedMode) return;
        FindObjectOfType<GameManager>().SaveBoard();
        PlayerPrefs.SetInt("saveExists", 1);
    }

    public void ResetmergeTiles()
    {
        foreach (GameObject g in gm.mergeTiles)
        {
            g.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1);

            //Color c = g.GetComponentsInChildren<Image>()[1].color;
        }
    }
}
