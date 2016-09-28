using UnityEngine;
using System.Collections;

public interface IDamageable {
    void takeDamage (int dam);
    void stun (float dur); // Interupts should just be a really short stun
    void knockback (Vector3 dir, float dist, float speed);
}
