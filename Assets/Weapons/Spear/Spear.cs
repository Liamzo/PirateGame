using UnityEngine;
using System.Collections;
using System;

public class Spear : Weapon, IAttackBlock {

    String blockName;
    bool blockReset;
    
    new void Start () {
        base.Start();
        wepState = WepState.Idle;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        chargeName = "SpearCharge";
        lightName = "SpearLight";
        heavyName = "SpearHeavy";
        blockName = "SpearBlock";
        blockReset = true;

        handUsed = "RightHand";

        attackTimer = 0f;
        attackHeavyTime = 1.5f;

        curWaitTimer = 0f;
        waitTime = 0.75f;
    }
	
	new void Update () {
        base.Update();

        if (Input.GetAxisRaw(handUsed) == 0) {
            blockReset = true;
        }
    }

    protected override void attackExecute () {
        if (attackTimer < attackHeavyTime) {
            animator.SetTrigger(lightName);
        } else if (attackTimer >= attackHeavyTime) {
            animator.SetTrigger(heavyName);
        }
    }

    public void attackBlock () {
        if (blockReset == true) {
            animator.SetTrigger(blockName);
            wepState = WepState.Attacking;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<BoxCollider>().enabled = true;

            blockReset = false;
        }
    }
}
