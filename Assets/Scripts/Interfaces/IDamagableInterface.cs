using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagableInterface
{
    void ReceiveDamage(int damage);
    void NormalHit(int attackDamage, Collider2D col);
    void ArrowHit(int arrowDamage, Collider2D col);
}
