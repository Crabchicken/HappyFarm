using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    idle,
    walk,
    attack,
    interact,
    stagger
}

public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentState;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Animator animator;
    private Vector3 change;
    private Vector3 cameraPos;
    private bool NeedMove = false;

    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
    }

    // Update is called once per frame
    void Update()
    {
        //change = Input.mousePosition;
        //Vector3 mousePos = Input.mousePosition;
        //cameraPos = Camera.main.ScreenToWorldPoint(change);
        //change.x = Input.GetAxisRaw("Horizontal"); ;
        //change.y = Input.GetAxisRaw("Vertical");
        if ((Input.GetMouseButtonUp(0) == true))
        {
            Vector3 mousePos = Input.mousePosition;
            cameraPos = Camera.main.ScreenToWorldPoint(mousePos);
            NeedMove = true;
        }
        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack && currentState != PlayerState.stagger)
        {
            StartCoroutine(AttackCo());
        }
        else if ((currentState == PlayerState.walk || currentState == PlayerState.idle) && NeedMove)
        {
            UpdateAnimationAndMove();
        }
        
    }

    private IEnumerator AttackCo()
    {
        currentState = PlayerState.attack;
        animator.SetBool("attacking", true);
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        currentState = PlayerState.walk;
    }

    void UpdateAnimationAndMove()
    {
        if (Vector2.Distance(transform.position, cameraPos) > 0.01)
        {
            MoveCharacter();
            animator.SetFloat("moveX", cameraPos.x);
            animator.SetFloat("moveY", cameraPos.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
            NeedMove = false;
        }
    }


    void MoveCharacter()
    {
        
        if(Input.GetButton("nitro"))
        {
            StartCoroutine(NitroCo());
        }
        else
        {
            //myRigidbody.MovePosition(transform.position + change.normalized * speed * Time.deltaTime);
             myRigidbody.MovePosition(Vector2.MoveTowards(transform.position, cameraPos, speed * Time.deltaTime));
            
        }
    }

    public void Knock(float knockTime)
    {
        StartCoroutine(KnockCo(knockTime));
    }

    private IEnumerator KnockCo(float knockTime)
    {
        if (myRigidbody != null)
        {

            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector3.zero;
            currentState = PlayerState.idle;
        }
    }

    private IEnumerator NitroCo()
    {
        //myRigidbody.MovePosition(transform.position + change.normalized * speed * 2 * Time.deltaTime);
        myRigidbody.MovePosition(Vector2.MoveTowards(transform.position, cameraPos, speed * 2 * Time.deltaTime));
        yield return new WaitForSeconds(2f);
        //myRigidbody.MovePosition(transform.position + change.normalized * speed * Time.deltaTime);
        myRigidbody.MovePosition(Vector2.MoveTowards(transform.position, cameraPos, speed * Time.deltaTime));

    }
}
