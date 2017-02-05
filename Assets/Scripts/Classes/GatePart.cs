using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GateLevel
{
    public int levelId;
    public int maxHealth;
    public Sprite sprite;
}

public class GatePart : CastleUpgradablePart, IDamagableInterface {
    private int currentHealth;
    private int maxHealth;
    private Sprite currSprite;
    private SpriteRenderer mySpriteRenderer;
    private Collider2D gateCollider;
    public CastleGateLevelsSO levelConfig;

    void Start()
    {
        gateCollider = GetComponent<Collider2D>();
        //mySpriteRenderer = GetComponent<SpriteRenderer>();
        InitGate();
    }

    void InitGate()
    {
        maxHealth = levelConfig.levels[0].maxHealth;
        currentHealth = maxHealth;
       // currSprite = levelConfig.levels[0].sprite;
       // mySpriteRenderer.Sprite = currSprite;
    }

    public void OnNewRound()
    {
        currentHealth = maxHealth;
    }

    public override void Upgrade()
    {
        base.Upgrade();
        if (currentLevel < levelConfig.levels.Length)
        {
            maxHealth = levelConfig.levels[currentLevel].maxHealth;
         //   currSprite = levelConfig.levels[currentLevel].sprite;
           // mySpriteRenderer.sprite = currSprite;
        }
    }

    public void ArrowHit(int arrowDamage, Collider2D col)
    {
        ReceiveDamage(arrowDamage);
    }

    public void NormalHit(int attackDamage, Collider2D col)
    {
        ReceiveDamage(attackDamage);
    }

    public void ReceiveDamage(int damage)
    {
        currentHealth -= damage;
        CheckIfDead();
    }

    void CheckIfDead()
    {
        if (currentHealth <= 0)
        {
            //TODO ADD DESTROYED SPRITE CODE
            gateCollider.enabled = false;
        }
    }


}
