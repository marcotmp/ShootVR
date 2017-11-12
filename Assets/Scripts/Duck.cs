using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour 
{
    
    public Animator animator;
    public float fallingSpeed = 0.1f;
    public float flySpeed = 0.1f;

    private DuckStates state;
    private Vector3 movement;

    enum DuckStates
    {
        Flying,
        Falling,
        Hit
    }

	// Use this for initialization
	void Start () 
    {
        state = DuckStates.Flying;

        // SelectRandomMovement();
        movement = new Vector3(0.5f, 1, 0); // 60dg
	}
	
	// Update is called once per frame
	void Update () 
    {
        switch(state)
        {
            case DuckStates.Flying:
                
                transform.position += movement * flySpeed;

                // check if it is time to select another move or if collide with borders
                if (TimeElapsed() || !CanMove())
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
        }
       
	}

    public void Hit()
    {
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

    private void ChangeMovement()
    {
        var val = Random.Range(0, 11);
        //print("Selecting " + val);
        movement = movementList[val];

        if (movement.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (movement.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        

        // if right or left
        if (movement.x != 0 && movement.y == 0.0f)
            animator.Play("Right");

        // if up
        else if (movement.y == 0 && movement.y == 1f )
        {
            transform.localScale = new Vector3(1, 1, 1);
            animator.Play("Up");
        }

        print("" + movement + "  " + "");
    }
}
