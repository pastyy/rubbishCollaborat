using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;

public class GameManager : MonoBehaviour
{
    public int life = 1;
    public int score = 100;

    public int sortTimes = 0;

    public float turnDelay = .1f;
    public float time = 2000;
    public float collideDelay = 3f;

    public static GameManager instance = null;
    public BoardManager boardScript;
    public static List<GameObject> bag;

    private Text levelText;
    private GameObject bagImage;
    private int level = 1;

    private bool bagsDelete;
    private bool timeTurn = true;
    private bool death;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    private void OnLevelWasLoaded(int index)
    {
        level = (int)Mathf.Sqrt(sortTimes) + 1;
        life++;
    }

    private void LoadSortTime(int index)
    {
        sortTimes++;
        Deletebags();
    }

    void changeTimeTurn(object source, ElapsedEventArgs e)
    {
        if (timeTurn == true)
            timeTurn = false;
        else
            timeTurn = true;
    }

    void InitGame()
    {
        if (timeTurn)
        {
            time = 2200 - 200*level;            
        }

        boardScript.SetupScene();
        System.Timers.Timer timer = new System.Timers.Timer();
        timer.Enabled = true;
        timer.Interval = 2500;
        timer.Start();
        timer.AutoReset = true;
        timer.Elapsed += new System.Timers.ElapsedEventHandler(changeTimeTurn);
    }

    public void GameOver()
    {
        enabled = false;
    }



    IEnumerator Deletebags()
    {
        bagsDelete = true;

        yield return new WaitForSeconds(turnDelay);
        if (bag.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }
        bag.RemoveAt(bag.Count - 1);
        yield return new WaitForSeconds(turnDelay);

        bagsDelete = false;
    }


    
}
