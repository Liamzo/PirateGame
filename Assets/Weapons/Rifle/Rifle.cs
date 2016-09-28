using UnityEngine;
using System.Collections;

public class Rifle : Weapon {

    float startAngle;
    float curAngle;
    LineRenderer line;

    new void Start () {
        base.Start();
        wepState = WepState.Idle;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<LineRenderer>().enabled = false;

        chargeName = "RifleCharge";
        lightName = "RifleAttack";
        heavyName = "RifleAttack";

        handUsed = "LeftHand";

        attackTimer = 0f;
        attackHeavyTime = 1.5f; // Time until perfect accuracy

        curWaitTimer = 0f;
        waitTime = 0.25f;

        damage = 5;

        startAngle = 60f;
        curAngle = startAngle;
        line = gameObject.GetComponent<LineRenderer>();
    }

    new void Update () {
        base.Update();

        if (wepState == WepState.Charging) {       
            Quaternion rot0 = Quaternion.Euler(0, 0, curAngle);
            Quaternion rot2 = Quaternion.Euler(0, 0, -curAngle);

            Vector3 pos = new Vector3(-0.1f, 6, 0.1f);
            line.SetPosition(0, rot0 * pos);
            line.SetPosition(2, rot2 * pos);

            if (curAngle > 0) {
                curAngle -= (startAngle/attackHeavyTime) * Time.deltaTime;
            } else {
                curAngle = 0;
            }
        }
    }

    override public void attackStart () {
        base.attackStart();
        gameObject.GetComponent<LineRenderer>().enabled = true;
    }

    protected override void attackExecute () {
        gameObject.GetComponent<LineRenderer>().enabled = false;
        animator.SetTrigger(lightName);
        
        float acc = Random.Range(-curAngle, curAngle);
        Vector3 dir = transform.TransformDirection(Quaternion.Euler(0, 0, acc) * (Vector3.up * 10));
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, 100.0f)) {
            if (hit.collider.GetComponent<IDamageable>() != null) {
                hit.collider.GetComponent<IDamageable>().takeDamage(damage);
            }           
        }
    }

    override public void attackEnd () {
        base.attackEnd();
        curAngle = startAngle;
        gameObject.GetComponent<LineRenderer>().enabled = false;
    }

    public override void interupt () {
        base.interupt();
        gameObject.GetComponent<LineRenderer>().enabled = false;
    }

}