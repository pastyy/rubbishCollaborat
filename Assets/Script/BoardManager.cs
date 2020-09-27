using System.Collections.Generic;
using UnityEngine;
using System;
using System.Timers;

public class BoardManager : MonoBehaviour
{

    public float floordelay = .1f;
    public float garbagedelay = 10f;
    public float garbagecandelay = 20f;
    public float tooldelay = 16f;

    public int level = 1;
    public int columns = 12;
    public int rows = 9;
    public GameObject[] garbage;
    public GameObject[] floortiles;
    public GameObject[] garbagecan;
    public GameObject bag;
    public GameObject[] tools;
    public bool flooravailable = true;
    public bool garbageavailable = false;
    public bool garbagecanavailable = false;
    public bool toolavailable = false;
    public bool ifcollide = false;

    private Transform boardHolder;
    private bool changeavailable = true;
    public List<int> tileposition = new List<int> { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };

    void Boardsetup()
    {
        // 将水平的道路呈现在游戏中
        boardHolder = new GameObject("Board").transform;
        for (int x = 1; x <= columns; x++)
        {
            GameObject toInstantiate = floortiles[UnityEngine.Random.Range(0, floortiles.Length)];

            GameObject instance = Instantiate(toInstantiate, new Vector2((x-3)/7.2f-0.33f, -0.15f), Quaternion.identity) as GameObject;

            instance.transform.SetParent(boardHolder);
        }
        Vector2 bagpos = new Vector2(-0.4f, -0.35f);
        Instantiate(bag, bagpos, Quaternion.identity);
    }

    public int[] Setdifficulty(int level)
    {
        // 输入当前的level，返回positionchange
        int[] pos;
        if (level <= 3)
        {
            pos = new int[] { 0, 0, 0, 0, 0, 1, -1, -1 };
            return pos;
        }
        else if (level <= 5)
        {
            pos = new int[] { 0, 0, 0, 0, 1, -1, 1, -1 };
            return pos;
        }

        else if (level <= 8)
        {
            pos = new int[] { 0, 0, 0, 0, 1, 1, 1, -1 };
            return pos;
        }

        else if (level <= 10)
        {
            pos = new int[] { 0, 0, 0, 1, -1, 1, 1, -1 };
            return pos;
        }
        pos = new int[] { 1, 1, 1, 1, 1, -1, -1, -1 };
        return pos;
    }

    public int randomposition()
    {
        // 判断tileposition的后三位是否相同，并据此在positionchange中随机抽取一个数并return
        int[] positionchange = Setdifficulty(level);
        int change;

        if (tileposition[tileposition.Count-1] == tileposition[tileposition.Count-2] && tileposition[tileposition.Count-1] == tileposition[tileposition.Count-3] && tileposition[tileposition.Count-2] == tileposition[tileposition.Count-3])
            changeavailable = true;
        else
            changeavailable = false;
        if (tileposition[tileposition.Count-1] >= 7)
            change = -1;
        else if (tileposition[tileposition.Count-1] <= 2)
            change = 1;

        if (changeavailable == true)
        {
            change = positionchange[UnityEngine.Random.Range(0, positionchange.Length)];
            changeavailable = false;
        }
        else
            change = 0;
        return change;
    }

    public int Updatefloors(GameObject[] floortiles, int lastpos)
    {
        // 随机在右边某个位置生成一个地板，返回值是新生成地板的位置
        int change = randomposition();
        int newpos = lastpos + change;
        return newpos;
    }


    public void Updateobject(GameObject[] tileArray, int newpos, bool heightavailable = true)
    {
        // 随机在新生成的地板上生成一个道具
        int[] heights = { 1, 2 };
        int heightchoice = heights[UnityEngine.Random.Range(0, 2)];
        if (heightavailable == true)
            heightchoice = 1;
        Vector2 objectpos = new Vector2(12, newpos + heightchoice-4);
        GameObject objectchoice = tileArray[UnityEngine.Random.Range(0, tileArray.Length)];
        Instantiate(objectchoice, objectpos, Quaternion.identity);


    }

    private void Setupnew(object source, ElapsedEventArgs e)
    {
        // 作为updatefloors和updatetools的结合，并且定时调用这个函数，以达到不断更新的目的
        Debug.Log("hello");
        if (flooravailable == true)
        {
            int lastpos = tileposition[tileposition.Count-1];
            int newpos = Updatefloors(floortiles, lastpos);
            tileposition.Add(newpos);
            GameObject floorchoice = floortiles[UnityEngine.Random.Range(0, floortiles.Length)];
            Instantiate(floorchoice, new Vector2(12, newpos-4), Quaternion.identity);

            if (garbagecanavailable == true)
            {

                Updateobject(garbagecan, newpos, true);
                garbageavailable = false;
                return;
            }
            if (garbageavailable == true)
            {
                int happen = UnityEngine.Random.Range(0, 100);
                if (happen < 20)
                    Updateobject(garbage, newpos);
                garbageavailable = false;
                return;
            }
            if (toolavailable == true)
            {
                int coincident = UnityEngine.Random.Range(0, 100);
                if (coincident < 30)
                    Updateobject(tools, newpos);
                toolavailable = false;
                return;
            }
            flooravailable = false;
        }
    }
    public void SetupScene()
    {
        Boardsetup();
        System.Timers.Timer floortimer = new System.Timers.Timer();
        floortimer.Enabled = true;
        floortimer.Interval = 100;
        floortimer.Start();
        floortimer.AutoReset = true;
        floortimer.Elapsed += new System.Timers.ElapsedEventHandler(Setupnew);

        System.Timers.Timer garbagetimer = new System.Timers.Timer();
        garbagetimer.Enabled = true;
        garbagetimer.Interval = 10000;
        garbagetimer.Start();
        garbagetimer.Elapsed += new System.Timers.ElapsedEventHandler(changegarbagestate);

        System.Timers.Timer garbagecantimer = new System.Timers.Timer();
        garbagecantimer.Enabled = true;
        garbagecantimer.Interval = 20000;
        garbagecantimer.Start();
        garbagecantimer.Elapsed += new System.Timers.ElapsedEventHandler(changegarbagecanstate);

        System.Timers.Timer tooltimer = new System.Timers.Timer();
        garbagetimer.Enabled = true;
        garbagetimer.Interval = 16000;
        tooltimer.Start();
        garbagetimer.Elapsed += new System.Timers.ElapsedEventHandler(changegarbagestate);

        System.Timers.Timer collidetimer = new System.Timers.Timer();
        garbagetimer.Enabled = true;
        garbagetimer.Interval = 3000;
        collidetimer.Start();
        garbagetimer.Elapsed += new System.Timers.ElapsedEventHandler(changefloorstate);
    }
    private void changefloorstate(object source, ElapsedEventArgs e)
    {
        flooravailable = true;
    }

    private void changegarbagestate(object source, ElapsedEventArgs e)
    {
        garbageavailable = true;
    }

    private void changegarbagecanstate(object source, ElapsedEventArgs e)
    {
        garbagecanavailable = true;
    }

    private void changetoolstate(object source, ElapsedEventArgs e)
    {
        toolavailable = true;
    }
}
