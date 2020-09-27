using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class MovingItems : MonoBehaviour
{
    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private Rigidbody2D rb2D;
    private BoxCollider2D boxCollider;
    public float inverseMovetime;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMovetime = 1f / moveTime;
}


    protected IEnumerator SmoothMovement(Vector3 end) 
    {
        // 确保物体移动是连贯的
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMovetime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        } 
    }

    protected void move(object source, ElapsedEventArgs e)
    {
        // 用来将物体在move time的时间中平移
        Vector2 start = transform.position;
        Vector2 end = start - new Vector2(1, 0);

        StartCoroutine(SmoothMovement(end));
    }

    void Awake()
    {
        System.Timers.Timer timer = new System.Timers.Timer();
        timer.Enabled = true;
        timer.Interval = moveTime;
        timer.Start();
        timer.AutoReset = true;
        timer.Elapsed += new System.Timers.ElapsedEventHandler(move);
    }


}
