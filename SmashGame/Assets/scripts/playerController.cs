using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

    public float speed;
    public int playerNumber;
    public Rigidbody body;
    private bool jumping;
    private bool onPlat;
    private bool onStage;
    private bool falling;
    private GameObject currentPlat;
    public GameObject feet;
    public Animator animator;
    public Collider neutralA;
    public bool RightLooking;
    public bool onLedge;
    public bool StopInput;
    public float hitDamage;
    public float health;
    private bool takeDamage;
    private float timeHit;
    private float nextHit;
    private float UpHit;
    private float SideHit;



    // Use this for initialization
    void Start () {
        falling = true;
        takeDamage = true;
        nextHit = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
        if (!takeDamage && Time.time > timeHit)
        {
            takeDamage = true;
            timeHit = Time.time + nextHit;
            Debug.Log("reset");
        }

        float movX2 = Input.GetAxis("LeftJoystickX_P" + playerNumber.ToString());


        if (movX2 == 0 || !falling)
        {
            animator.SetInteger("move_P" + playerNumber.ToString(), 0);
        }
        if (movX2 > 0 || movX2 < 0)
        {
            animator.SetInteger("move_P" + playerNumber.ToString(), 1);
        }
        if (body.velocity.y < 0 || falling)
        {
            animator.SetInteger("move_P" + playerNumber.ToString(), 3);
        }
        if (body.velocity.y > 0)
        {
            animator.SetInteger("move_P" + playerNumber.ToString(), 2);
        }

        if (Input.GetButtonDown("A_P" + playerNumber.ToString()))
        {
            animator.SetInteger("move_P" + playerNumber.ToString(), 4);
            setDamage(0.3f, 20, 0, 0.1f);
        }
        if (onLedge)
        {
            animator.SetInteger("move_P" + playerNumber.ToString(), 5);
        }

        //animator.SetFloat("state", movX2);
        //Debug.Log(movX2);
    }

    void FixedUpdate()
    {

        //Movement
        float movX = Input.GetAxis("LeftJoystickX_P" + playerNumber.ToString());
        float movY = Input.GetAxis("LeftJoystickY_P" + playerNumber.ToString());
        transform.position += new Vector3(movX * speed, 0, 0);

        if (onLedge)
        {
            if (movX == 0 && movY == 0)
            {
                StopInput = false;
            }
            body.useGravity = false;
            body.velocity = Vector3.zero;
            if (!StopInput)
            {
                if (movX >= 0.5 || movX <= -0.5 || movY >= 0.5 || movY <= -0.5)
                {
                    onLedge = false;
                }
            }
        } else
        {
            body.useGravity = true;
            StopInput = true;
        }

        //Character Looking Direction
        if (movX > 0 && !jumping)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            RightLooking = true;
            
        } else if (movX < 0 && !jumping)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            RightLooking = false;
        }

        //Jumping
        if (movY > 0.85 && !jumping)
        {
            body.velocity = new Vector3(0, movY * 7, 0);
            jumping = true;
            falling = true;
        }

        //Crouching or Falling
        if (movY < -0.85)
        {
            if (onPlat)
            {
                falling = false;
                Physics.IgnoreCollision(currentPlat.GetComponent<Collider>(), GetComponent<Collider>());
            } else if (!jumping)
            {
                transform.localScale = new Vector3(0.25f, 0.1f, 0.25f);
            } else
            {

            }
        } else
        {
            transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }

        //Update Objects Below
        if (falling)
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("plat"))
            {
                Physics.IgnoreCollision(go.GetComponent<Collider>(), GetComponent<Collider>(), false);
            }
        }

        //if (!jumping && !falling)
        //{
        //    body.velocity = new Vector3(0, 0, 0);
        //}

        //Update Objects Above
        RaycastHit hit;
        Debug.DrawRay(feet.transform.position, Vector3.up);
        if (Physics.Raycast(feet.transform.position, Vector3.up, out hit))
        {
            if (hit.collider.gameObject.tag == "plat")
            {
                Physics.IgnoreCollision(hit.collider, GetComponent<Collider>());
            }
        }
        //Debug.Log(movY);
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("touch");
        if (col.gameObject.tag == "stage")
        {
            Debug.Log("stage");
            jumping = false;
            falling = false;
            onStage = true;
            if (currentPlat != null)
            {
                Physics.IgnoreCollision(currentPlat.GetComponent<Collider>(), GetComponent<Collider>(), false);
            }
        }
        if (col.gameObject.tag == "plat")
        {
            Debug.Log("plat");
            jumping = false;
            falling = false;
            onPlat = true;
            body.velocity = new Vector3(0, 0, 0);
            Debug.Log(falling);
            currentPlat = col.gameObject;
        }
        if (col.gameObject.tag == "death")
        {
            transform.position = new Vector3(0, 3, 0);
            health = 0;
        }
        if (col.gameObject.tag == "player")
        {
            Debug.Log("player");
        }
        

    }

    void OnCollisionExit()
    {
        onPlat = false;
    }

    public bool GetRightLooking()
    {
        return RightLooking;
    }

    public void setOnLedge(bool set)
    {
        onLedge = set;
    }

    //set Damage of player
    IEnumerator setDamage(float attackTime, float damage, float Up, float Side)
    {
        float hitTime = Time.time + attackTime;
        //new WaitForSeconds(attackTime);
        //if (Time.time >= hitTime)
        //{
            hitDamage = damage;
        UpHit = Up;
        SideHit = Side;
            Debug.Log("Damage dealt: " + hitDamage);
        //}
        return null;
    }


    //check if hit
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "hotBox" && takeDamage)
        {
            float damageTaken = col.GetComponentInParent<playerController>().hitDamage;
            health -= damageTaken;
            Debug.Log("Remaining health: " + health);
            if (damageTaken != 0)
            {
                if (col.GetComponentInParent<playerController>().RightLooking)
                {
                    body.velocity = new Vector3(col.GetComponentInParent<playerController>().SideHit * -health, col.GetComponentInParent<playerController>().UpHit * -health, 0);
                } else
                {
                    body.velocity = new Vector3(col.GetComponentInParent<playerController>().SideHit * health, col.GetComponentInParent<playerController>().UpHit * -health, 0);
                }

            }
            takeDamage = false;
        }
    }
}
