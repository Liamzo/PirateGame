using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour, IDamageable {

    // State
    public enum PlayerState {
        Normal, // 0
        Charging, // 1
        ForcedMove, // 2
        Blocking, // 3
        Stunned, // 4
    };
    public PlayerState pState = PlayerState.Normal;

    public float moveSpeed;
    public float baseSpeed = 5f;
    float chargeSpeed = 2.5f;
    public float dodgeDist = 2f;
    float dodgeSpeed = 15f;

    Animator animator;
    public GameObject wep_R;
    GameObject wep_L;

    public bool canMove = true;
    bool canDodge = true;
    bool canLook = true;
    bool canAttack = true;

    // For being moved by other things like dodging, dashing or knockbacks
    Vector3 dashMove;
    float dashSpeed;

    // Health
    int maxHealth = 15;
    public int curHealth;

    // Stun
    float stunDur;

	// Use this for initialization
	void Start () {
        moveSpeed = baseSpeed;
        animator = GetComponent<Animator>();
        wep_R = GameObject.FindGameObjectWithTag("Wep_R");
        wep_L = GameObject.FindGameObjectWithTag("Wep_L");

        curHealth = maxHealth;

        stunDur = 0f;
    }

    // Update is called once per frame
    void Update () {
        if (pState == PlayerState.Normal) {
            moveSpeed = baseSpeed;
        } else if (pState == PlayerState.Charging) {
            moveSpeed = chargeSpeed;
        } else if (pState == PlayerState.Blocking) {
            moveSpeed = wep_L.GetComponent<Shield>().getMoveSpeed();
        }

        if (pState == PlayerState.Stunned) {
            stunDur -= Time.deltaTime;

            if (stunDur <= 0) {
                pState = PlayerState.Normal;
            }
        } else {
            if (canMove) {
                Vector3 move = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0, Input.GetAxis("Vertical") * moveSpeed);

                GetComponent<CharacterController>().Move(move * Time.deltaTime);

                if (move != new Vector3(0, 0, 0)) {
                    animator.SetFloat("Speed", moveSpeed);
                } else {
                    animator.SetFloat("Speed", 0);
                }
            } else if (pState == PlayerState.ForcedMove) {
                transform.position = Vector3.MoveTowards(transform.position, dashMove, dashSpeed * Time.deltaTime);
            }

            if (pState != PlayerState.ForcedMove) {
                dashMove = Vector3.zero;
                dashSpeed = 0f;
            }

            if (canLook) {
                // Looking at mouse
                Plane playerPlane = new Plane(Vector3.up, transform.position);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                float hitdist = 0.0f;
                if (playerPlane.Raycast(ray, out hitdist)) {
                    Vector3 targetPoint = ray.GetPoint(hitdist);

                    Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                    targetRotation.eulerAngles = new Vector3(90, targetRotation.eulerAngles.y, 0);
                    transform.rotation = targetRotation;
                }
            }

            // Right Hand
            if ((Input.GetAxisRaw("RightHand") == 1)) {
                if (canAttack) {
                    wep_R.GetComponent<Weapon>().attackStart();
                    wep_L.GetComponent<Weapon>().interupt();
                } else if ((pState == PlayerState.Blocking) && (wep_R.GetComponent<IAttackBlock>() != null)) {
                    wep_R.GetComponent<IAttackBlock>().attackBlock();
                }
            }

            // Left Hand
            if (canAttack) {
                if ((Input.GetAxisRaw("LeftHand") == 1)) {
                    wep_L.GetComponent<Weapon>().attackStart();
                    wep_R.GetComponent<Weapon>().interupt();
                }
            }

            // Dodging
            if ((Input.GetAxis("Jump") == 1) && (canDodge)) {
                Vector3 dodge = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                if (dodge != Vector3.zero) {
                    GetComponent<CharacterController>().Move(dodge * dodgeDist * Time.deltaTime);
                    animator.SetTrigger("DodgeBack");
                    wep_R.GetComponent<Weapon>().interupt();
                    wep_L.GetComponent<Weapon>().interupt();
                }
            }
        }
    }

    public void dashMovement (Vector3 dir, float dist, float speed) {
        dashMove = (dir * dist) + transform.position;
        dashSpeed = speed;
        pState = PlayerState.ForcedMove;
    }

    public void attackFinished (AnimationEvent ae) {
        string nextAnim = ae.stringParameter;
        int wepUsed = ae.intParameter; // 0 = Right, 1 = Left, 2 = none
        animator.SetTrigger(nextAnim);

        if (wepUsed == 0) {
            wep_R.GetComponent<Weapon>().attackEnd();
        } else if (wepUsed == 1) {
            wep_L.GetComponent<Weapon>().attackEnd();
        }
    }
    public void setMove(int i) {
        canMove = i == 0 ? false : true;
    }
    public void setDodge (int i) {
        canDodge = i == 0 ? false : true;
    }
    public void setLook (int i) {
        canLook = i == 0 ? false : true;
    }
    public void setAttack (int i) {
        canAttack = i == 0 ? false : true;
    }
    public void setState (int i) {
        pState = (PlayerState)i;
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
        pState = PlayerState.ForcedMove;
    }
}
