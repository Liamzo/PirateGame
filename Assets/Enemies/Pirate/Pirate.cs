using UnityEngine;
using System.Collections;
using System;

public class Pirate : MonoBehaviour, IDamageable {

    // State
    public enum PirateState {
        Normal, // 0
        Charging, // 1
        ForcedMove, // 2
        Blocking, // 3
        Stunned, // 4
    };
    public PirateState pState = PirateState.Normal;

    // For being moved by other things like dodging, dashing or knockbacks
    Vector3 dashMove;
    float dashSpeed;

    // Health
    int maxHealth = 15;
    public int curHealth;

    // Stun
    float stunDur;


    void Start () {
        curHealth = maxHealth;
	}
	
	void Update () {
	
	}

    public void takeDamage (int dam) {
        // TODO: Armour when equipment it made
        curHealth -= dam;

        //TODO: More eligent eg. death anim
        if (curHealth <= 0) {
            Destroy(this.gameObject);
        }
    }

    public void stun (float dur) {
        //TODO: stun resit maybe
        stunDur = dur;
    }

    public void knockback (Vector3 dir, float dist, float speed) {
        dashMove = (dir * dist) + transform.position;
        dashSpeed = speed;
        pState = PirateState.ForcedMove;
    }

}
