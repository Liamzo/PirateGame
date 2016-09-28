using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour {

    protected GameObject player;
    protected Animator animator;

    protected string chargeName;
    protected string lightName;
    protected string heavyName;

    protected string handUsed;

    protected int damage;

    protected enum WepState {
        Idle,
        Charging,
        Attacking,
        Waiting,
    };
    protected WepState wepState;

    public float attackTimer;
    protected float attackHeavyTime;

    protected float curWaitTimer;
    protected float waitTime;


    protected virtual void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = player.GetComponent<Animator>();      
    }

    protected virtual void Update () {
        if (wepState == WepState.Charging) {
            attackTimer += Time.deltaTime;

            if (Input.GetAxisRaw(handUsed) == 0) {
                wepState = WepState.Attacking;
                attackExecute();
            }
        }

        if (wepState == WepState.Attacking) {
            GetComponent<BoxCollider>().enabled = true;
        }

        if (wepState == WepState.Waiting) {
            curWaitTimer += Time.deltaTime;

            if (curWaitTimer >= waitTime) {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                curWaitTimer = 0f;
                wepState = WepState.Idle;
            }
        } else {
            curWaitTimer = 0f;
        }
    }

    // Called when an attack is started
    public virtual void  attackStart () {
        animator.SetTrigger(chargeName);
        wepState = WepState.Charging;
        attackTimer = 0f;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }


    // Called when the attack is to be executed, plays attack animation
    protected abstract void attackExecute ();

    // Called when attack is completed
    public virtual void attackEnd () {
        wepState = WepState.Waiting;
        GetComponent<BoxCollider>().enabled = false;
    }

    public virtual void interupt () {
        wepState = WepState.Idle;
        GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        curWaitTimer = 0f;
        attackTimer = 0f;
    }

}
