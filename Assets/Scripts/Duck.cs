using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{
    public Animator animator;
    public float fallingSpeed = 0.1f;
    public int flyMinSpeed = 8;
    public int flyMaxSpeed = 10;
    public Transform topLeft;
    public Transform bottomRight;

    private DuckStates state;
    private Vector3 movement;
    private float flySpeed;

    private List<Vector3> movementList = new List<Vector3>() {
        new Vector3(1, 0),  // Right
        new Vector3(-1, 0), // Left
        new Vector3(0, 1),  // Up
        new Vector3(1, 0.5f), // rightUp - 30
        new Vector3(0.5f, 1), // rightup - 60
        new Vector3(-1, 0.5f), // leftUp - 30
        new Vector3(-0.5f, 1), // leftUp - 60
        new Vector3(1, -0.5f), // rightDown - 30
        new Vector3(0.5f, -1), // rightDown - 60
        new Vector3(-1, -0.5f), // left-down - 30
        new Vector3(-0.5f, -1), // left-down - 60
    };

    enum DuckStates
    {
        Flying,
        Falling,
        Hit,
        FlyAway
    }

    // Use this for initialization
    void Start()
    {
        state = DuckStates.Flying;
        ChangeMovement();

        flySpeed = Random.Range(flyMinSpeed, flyMaxSpeed);
    }

    // Update is called once per frame
    void Update()
    {  
        switch (state)
        {
            case DuckStates.Flying:
                transform.Translate(movement * flySpeed * Time.deltaTime);
                
                var time = TimeElapsed();
                var canMove = CanMove();
                
                if (!canMove || time)
                    ChangeMovement();

                break;
            case DuckStates.Hit: break;
            case DuckStates.Falling:
                transform.position -= Vector3.up * fallingSpeed;
                
                // if touch ground, destroy object
                if (transform.position.y <= 0)
                    Destroy(gameObject);

                break;
            case DuckStates.FlyAway:
                transform.Translate(movement * flySpeed * Time.deltaTime);
                break;
        }
    }

    public void Hit()
    {
        // remove the collider component
        Destroy(GetComponent<Collider>());

        animator.Play("Hit");
        Invoke("Fall", 1);
        state = DuckStates.Hit;
    }

    public void Fall()
    {
        animator.Play("Die");
        state = DuckStates.Falling;
    }

    private float timeElapsed = 0;
    private float delay = 2;
    private bool TimeElapsed()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > delay)
        {
            timeElapsed = 0;
            delay = Random.Range(0, 3);
            return true;
        }
        return false;
    }

    private bool CanMove()
    {
        // check if can move based on position and movement

        var pos = transform.localPosition;
        var nextPos = pos + movement;

        // x movement
        if (movement.x > 0 && nextPos.x > bottomRight.localPosition.x) return false;
        else if (movement.x < 0 && nextPos.x < topLeft.localPosition.x) return false;

        // y movement
        if (movement.y > 0 && nextPos.y > topLeft.localPosition.y) return false;
        else if (movement.y < 0 && nextPos.y < bottomRight.localPosition.y) return false;

        return true;
    }

    public void FlyAway()
    {
        // can't fly away if wasn't flying 
        if (state != DuckStates.Flying) return;

        state = DuckStates.FlyAway;
        movement = new Vector3(0, 1);
        UpdateAnimator(movement);

        Invoke("DestroySelf", 3);
    }

    private void ChangeMovement()
    {
        var counter = 0;
        while (!CanMove() || counter < 100)
        {
            counter++;
            var val = Random.Range(0, movementList.Count);
            movement = movementList[val];
        }

        if (movement.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (movement.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);


        UpdateAnimator(movement);
    }

    private void UpdateAnimator(Vector3 movement)
    {
        if (movement.x != 0 && Mathf.Abs(movement.y) <= 0.5f)
        {
            animator.Play("FlyRight");
        }
        else if (movement.x == 0 && movement.y == 1)
        {
            transform.localScale = new Vector3(1, 1, 1);
            animator.Play("FlyUp");
        }

        if (Mathf.Abs(movement.y) == 1 && Mathf.Abs(movement.x) == 0.5f)
        {
            animator.Play("FlyUpRight");
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}