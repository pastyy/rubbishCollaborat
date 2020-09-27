using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int pointPerRubbish = 10;
    public int pointPerCollide = 10;
    public int pointPerJump = 10;
    public int pointPerGrade = 10;
    public int point = 100;
    public int bagCapacity = 6;
    public Transform feetPos;
    public Transform headPos;
    public float checkRadius;
    public float jumpForce;
    public Animator animator; 
    public float jumpTime;
    
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isCollided;
    private float moveInput;
    private float jumpTimeCounter;
    private bool isJumping = false;
    private bool isthrow;
    //private Stack<Rubbish> backpack = new Stack<Rubbish>();
    
   // private float inverseMoveTime;  //与速度同义，每次运动一单位

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
    }


    // Update is called once per frame
    void Update()
    {
        isthrow = Input.GetKeyDown(KeyCode.DownArrow);
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius);
        if (isGrounded == true) 
           
            Debug.Log("grounded");
        // Debug.Log(Input.GetKeyDown(KeyCode.Space));
        if (isGrounded == false) { Debug.Log(animator.GetBool("jump")); Debug.Log("this is whether jump is true"); }
        if (isGrounded == true && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("it should jump");
            isJumping = true;
            jumpTimeCounter = jumpTime;
            animator.SetBool("jump", true);
           
            Debug.Log("condition jump is true. it should transit");
            rb.velocity = Vector2.up * jumpForce;

        }
        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {

        
            if (jumpTimeCounter > 0){
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
                
            }
            else
            {
                isJumping = false;
            }
        if (Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = false;
            }
            
        }
        if (isGrounded == true && animator.GetBool("jump"))
        {
            animator.SetBool("jump", false);
        }

        


    }

    /*
    public void PlayerJump()
    {

    }
    public void PlayerCollide()
    {
        isCollided = Physics2D.CheckRadius(headPos.position, checkRadius);
        if (isCollided == true)
        {
            animator.SetTrigger("collide");
        }
    }
    public void PlayerThrow()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Rubbish")
        {
            backpack.Push(other.gameObject);
            animator.SetTrigger("pick");
            other.gameObject.SetActive(false);
        }
        if (other.tag == "Grade")
        {
            score += pointPerGrade;
           // Player.scoreText.text = "+" + pointPerGrade + "score" + score;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "bag")
        {
            bagCapacity += 1;
            other.gameObject.SetActive(false);
        }
    //需要用到gamemanager/boardmanager，改变运行速度
        else if (other.tag == "speed")
        {
            time -= 1000;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "slow")
        {
            time += 1000;
            other.gameObject.SetActive(false);
        }

    //以上
        else if (other.tag == "RandCard")
        {
            
        }
        else if (other.tag == "GarbageCan")
        {
             if (backpack.Count != 0 && isthrow)
            {
                animator.SetTrigger("throw");
                GameObject rbs = backpack.Pop();
                CheckIfRight(rbs, other.gameObject);

            }
        }
    }
    public void CheckIfRight(GameObject rubbish, GameObject rubbishcan) { }*/
}
