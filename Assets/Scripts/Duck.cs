using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{

    public Animator animator;
    public float fallingSpeed = 0.1f;
    public int flyMinSpeed = 8;
    public int flyMaxSpeed = 10;
    public Vector3 topLeft;
    public Vector3 bottomRight;

    private DuckStates state;
    private Vector3 movement;
    private float flySpeed;

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

        //print("state= " + state + " , movment= " + movement);

        flySpeed = Random.Range(flyMinSpeed, flyMaxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case DuckStates.Flying:

                //transform.position += movement * flySpeed * Time.deltaTime;
                transform.Translate(movement * flySpeed * Time.deltaTime);

                //print("state= " + state + " , movment= " + movement);
                // check if it is time to select another move or if collide with borders

                var time = TimeElapsed();
                var canMove = CanMove();

                //if (!canMove) print("Can't move " + canMove);

                //if (time)// || !CanMove())
                if (!canMove || time)
                    ChangeMovement();

                break;
            case DuckStates.Hit: break;
            case DuckStates.Falling:
                transform.position -= Vector3.up * fallingSpeed;
                // if touch ground, destroy object
                if (transform.position.y <= 0)
                {
                    Destroy(gameObject);
                }
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

        var pos = transform.position;
        var nextPos = pos + movement;

        //Debug.Log("movement= " +movement 
        // + "\npos= " + pos 
        // + "\nnextPos= " + nextPos 
        // + "\ntopLeft= " + topLeft 
        // + "\nbottomRight= " + bottomRight
        // + "\nright=" + (movement.x > 0 && nextPos.x > bottomRight.x)
        // + "\nleft=" + (movement.x < 0 && nextPos.x < topLeft.x)
        // + "\ntop=" + (movement.y > 0 && nextPos.y > topLeft.y)
        // + "\nbottom=" + (movement.y < 0 && nextPos.y > bottomRight.y)
        //);

        //new Bounds().Contains(pos);

        // x movement
        if (movement.x > 0 && nextPos.x > bottomRight.x) return false;
        else if (movement.x < 0 && nextPos.x < topLeft.x) return false;

        // y movement
        if (movement.y > 0 && nextPos.y > topLeft.y) return false;
        else if (movement.y < 0 && nextPos.y < bottomRight.y) return false;

        return true;
    }

    private List<Vector3> movementList = new List<Vector3>() {
        new Vector3(1, 0),
        new Vector3(-1, 0),
        new Vector3(0, 1),

        new Vector3(1, 0.5f),
        new Vector3(0.5f, 1),

        new Vector3(-1, 0.5f),
        new Vector3(-0.5f, 1),

        new Vector3(1, -0.5f),
        new Vector3(0.5f, -1),

        new Vector3(-1, -0.5f),
        new Vector3(-0.5f, -1),
    };

    public void FlyAway()
    {
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
            //print("Selecting " + val);
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

        // UpdateAnimator();

        // if right or left
        if (movement.x != 0 && Mathf.Abs(movement.y) <= 0.5f)
        {
            animator.Play("FlyRight");
            //print("FlyRight");
        }


        // if up
        else if (movement.x == 0 && movement.y == 1)
        {
            transform.localScale = new Vector3(1, 1, 1);
            animator.Play("FlyUp");
            //print("FlyUp");
        }

        if (Mathf.Abs(movement.y) == 1 && Mathf.Abs(movement.x) == 0.5f)
        {
            animator.Play("FlyUpRight");
            //print("FlyUpRight");
        }

        //print("" + movement + "  " + "");
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
