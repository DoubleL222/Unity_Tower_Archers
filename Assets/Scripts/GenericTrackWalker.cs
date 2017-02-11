using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[System.Serializable]
public enum WalkerType
{
    Enemy,
    Friendly
}

[System.Serializable]
public enum WalkingDirection
{
    Left,
    Right
}

[System.Serializable]
public enum WalkerColliderPartType
{
    Shield,
    Body,
    CritBody
}

[System.Serializable]
public struct WalkerColliderPart
{
    public Collider2D collider;
    public int arrowsReceived;

    public WalkerColliderPartType partType;

    public WalkerColliderPart( Collider2D _collider,  WalkerColliderPartType _partType)
    {
        collider = _collider;
        arrowsReceived = 0;
        partType = _partType;
    }
}

public class GenericTrackWalker : MonoBehaviour, IDamagableInterface
{
    /// SETTINGS VALUES
    private float rotationSpeed = 30.0f;
    private int rayChecksPerSecond = 10;
    private float raycheckDistance = 1;

    /////////INIT VALUES
    /// VALUES READ FROM DATA
    private int maxHealth;
    private int startingDamage;
    private float startingSpeed = 0.1f;
    private float startingAttackSpeed = 1.0f;
    private WalkerType myType;

    //VALUES SET IN INIT
    private LayerMask AttackLayerMask;

    //VALUES SET IN FOLLOW TRACK
    private int indexDiff = -1;
    private EnemyWalkingTrack myTrack;
    protected WalkingDirection walkingDirection;

    //RUNTIME VALUES
    private Animator genericWalkerAnimator;
    protected int currentHealth;
    [HideInInspector]
    public bool alive = true;
    protected bool followingTrack;
    protected bool enteredTrack = false;
    protected bool attacking = false;
    protected int nextTrackIndex;
    protected Quaternion targetRot;
    private int rayCheckClick;
    private int frameCounter = 0;
    private RaycastHit2D[] hitList;
    Vector2 forward;
    private float previousAttackTime = float.MinValue;
    private bool criticalDeath = false;
    private bool arrow = true;
    //PREFAB VALUES
    [Header("Values to be set in prefab")]
    public List<WalkerColliderPart> myColliders;
    //public BoxCollider2D trackCollider;

    public virtual void Start()
    {
        genericWalkerAnimator = GetAnimator();
        startingAttackSpeed = 1f / startingAttackSpeed;
        rayCheckClick = 20 / rayChecksPerSecond;
    }

    private Animator GetAnimator()
    {
        Animator currAr = null;
        currAr = GetComponent<Animator>();
        if (currAr == null)
        {
            currAr = GetComponentInChildren<Animator>();
        }
        return currAr;
    }

    public virtual void FixedUpdate()
    {
        frameCounter++;
        if (alive && enteredTrack)
        {
            if (followingTrack)
            {
                genericWalkerAnimator.SetTrigger("Walk");
                if (nextTrackIndex >= 0 && nextTrackIndex < myTrack.Points)
                {
                    //COOOL ENEMY MECHANIC
                    //transform.position = Vector2.Lerp(transform.position, myTrack.walkingPoints[nextTrackIndex], Time.deltaTime * speed);

                    transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRot, Time.fixedDeltaTime * rotationSpeed);
                    transform.position = Vector2.MoveTowards(transform.position, myTrack.walkingPoints[nextTrackIndex], Time.fixedDeltaTime * startingSpeed);
                    Vector2 my2dPos = new Vector2(transform.position.x, transform.position.y);
                    if (my2dPos == myTrack.walkingPoints[nextTrackIndex])
                    {
                        if (nextTrackIndex + indexDiff >= myTrack.Points || nextTrackIndex + indexDiff <= 0)
                        {
                            followingTrack = false;
                        }
                        else
                        {
                            nextTrackIndex += indexDiff;
                        }

                        float zAngle = 0;
                        targetRot = GetNextRot();
                    }
                }

            }
            if (frameCounter % rayCheckClick == 0)
            {
                if (walkingDirection == WalkingDirection.Left)
                {
                    forward = -transform.right;
                }
                else
                {
                    forward = transform.right;
                }

                Ray2D ray = new Ray2D(transform.position, forward);
                hitList = null;
                hitList = Physics2D.RaycastAll(ray.origin, ray.direction, raycheckDistance, AttackLayerMask);
                if (hitList != null)
                {
                    if (hitList.Length > 0)
                    {
                        attacking = true;
                        followingTrack = false;
                    }
                    else
                    {
                        attacking = false;
                        followingTrack = true;
                    }
                }
                else
                {
                    attacking = false;
                    followingTrack = true;
                }
            }
            if (attacking)
            {
                if (Time.time > previousAttackTime + startingAttackSpeed)
                {
                    DoAttack();
                }
            }
        }
        //transform.position = Vector2.Lerp(transform.position, );
    }

    protected virtual void DoAttack()
    {
        genericWalkerAnimator.SetTrigger("Attack");   
        if (hitList != null)
        {
            if (hitList.Length > 0)
            {
                foreach (RaycastHit2D hit in hitList)
                {
                    UtilityScript.RecursevlyLookForInterface(hit.transform).NormalHit(startingDamage, hit.collider);
                    //hit.collider.gameObject.GetComponent<IDamagableInterface>().ReceiveDamage(startingDamage);
                }
            }
        }
        previousAttackTime = Time.time;
    }

    Vector2 Get2DPos()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    Quaternion GetNextRot()
    {
        Vector2 diff = myTrack.walkingPoints[nextTrackIndex] - Get2DPos();
        if (walkingDirection == WalkingDirection.Left)
        {
            return Quaternion.Euler(new Vector3(0, 0, (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg) - 180));
        }
        else
        {
            return Quaternion.Euler(new Vector3(0, 0, (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg)));
        }
    }

    public void StartFollowTrack()
    {
        targetRot = GetNextRot();
        followingTrack = true;
        enteredTrack = true;
    }

    public void InitGenericWalker(int _maxHealth, int _startingDamage, float _startingSpeed, float _startingAttackSpeed, WalkerType _type, EnemyWalkingTrack _myTrack, LayerMask _attackMask)
    {
        AttackLayerMask = _attackMask;
        myTrack = _myTrack;
        maxHealth = _maxHealth;
        startingDamage = _startingDamage;
        startingSpeed = _startingSpeed;
        startingAttackSpeed = _startingAttackSpeed;
        myType = _type;
        currentHealth = maxHealth;
        nextTrackIndex = _type == WalkerType.Enemy ? 0 : _myTrack.Points - 1;
        alive = true;
        if (myType == WalkerType.Enemy)
        {
            indexDiff = 1;
            if (myTrack.side == EnemyWalkingTrackSide.Left)
                walkingDirection = WalkingDirection.Right;
            else
                walkingDirection = WalkingDirection.Left;
        }
        else
        {
            indexDiff = -1;
            if (myTrack.side == EnemyWalkingTrackSide.Left)
                walkingDirection = WalkingDirection.Left;
            else
                walkingDirection = WalkingDirection.Right;
        }
    }

    public void ArrowHit(int ArrowDmg, Collider2D col)
    {
        WalkerColliderPart hitPart = myColliders.Where(p => p.collider == col).First();
        arrow = true;
        switch (hitPart.partType)
        {
            case WalkerColliderPartType.Body:
                ReceiveDamage(ArrowDmg);

                break;
            case WalkerColliderPartType.CritBody:
                criticalDeath = true;
                ReceiveDamage(ArrowDmg*2);
                
                break;
            case WalkerColliderPartType.Shield:
                break;

        }
    }

    void OnDrawGizmos()
    {
        if (forward != null)
            Gizmos.DrawLine(transform.position, transform.position + raycheckDistance * new Vector3(forward.x, forward.y, 0));
    }

    public void ReceiveDamage(int damage)
    {
        Debug.Log("RECEIVED DAMAGE");
        currentHealth -= damage;
        if (!CheckIfDead())
        {
            if (arrow)
            {
                genericWalkerAnimator.SetTrigger("ReceiveArrow");
            }
            else
            {
                genericWalkerAnimator.SetTrigger("ReceiveMelee");
            }
        }
    }

    bool CheckIfDead()
    {
        if (currentHealth <= 0)
        {
           
            Die(criticalDeath);
            return true;
        }
        else
        {
            criticalDeath = false;
            return false;
        }
    }

    public void Die(bool _criticalDeath)
    {
        foreach (WalkerColliderPart part in myColliders)
        {
            part.collider.enabled = false;
        }
        if (_criticalDeath)
        {
            genericWalkerAnimator.SetTrigger("DramaticDeath");
        }
        else
        {
            genericWalkerAnimator.SetTrigger("Die");
        }
        
        alive = false;
        if(myType == WalkerType.Enemy)
            UnitSpawnManager.instance.EnemyDied();
       // GetComponentInChildren<SpriteRenderer>().color = Color.black;
       // trackCollider.enabled = false;
    }

    //CALEED WHEN RECEIVEING DAMAGE
    public void NormalHit(int attackDamage, Collider2D col)
    {
        arrow = false;
        ReceiveDamage(attackDamage);
    }
}
