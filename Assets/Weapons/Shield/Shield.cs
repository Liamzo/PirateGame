using UnityEngine;
using System.Collections;
using System;

public class Shield : Weapon {

    float moveSpeed = 1.5f;

    // Use this for initialization
    new void Start () {
        base.Start();
        wepState = WepState.Idle;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        chargeName = "ShieldBlock";
        lightName = "ShieldAway";

        handUsed = "LeftHand";

        curWaitTimer = 0f;
        waitTime = 0.25f;
    }
	
	// Update is called once per frame
	new void Update () {
        base.Update();
	}

    override public void attackStart () {
        base.attackStart();
        GetComponent<BoxCollider>().enabled = true;
    }

    protected override void attackExecute () {
        animator.SetTrigger(lightName);
        wepState = WepState.Waiting;
        //GetComponent<BoxCollider>().enabled = false;
    }

    public float getMoveSpeed () {
        return moveSpeed;
    } 

}
