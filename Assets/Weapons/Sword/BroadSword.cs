using UnityEngine;
using System.Collections;
using System;

public class BroadSword : Weapon {

    float dashDist;
    float dashSpeed;

    new void Start () {
        base.Start();
        wepState = WepState.Idle;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        chargeName = "SwordCharge";
        lightName = "SwordLight";
        heavyName = "SwordHeavy";

        handUsed = "RightHand";

        dashDist = 1f;
        dashSpeed = 10f;

        attackTimer = 0f;
        attackHeavyTime = 1.2f;

        curWaitTimer = 0f;
        waitTime = 0.75f;

        damage = 2;
    }


    // Update is called once per frame
    new void Update () {
        base.Update();
	}

    protected override void  attackExecute () {
        if (attackTimer < attackHeavyTime) {
            animator.SetTrigger(lightName);
        } else if (attackTimer >= attackHeavyTime) {
            animator.SetTrigger(heavyName);
            player.GetComponent<Player>().dashMovement(player.transform.TransformDirection(-player.transform.forward), dashDist, dashSpeed);
        }
    }

    void OnTriggerEnter (Collider col) {
        if (col.GetComponent<IDamageable>() != null) {
            col.GetComponent<IDamageable>().takeDamage(damage);
        }
    }
}
