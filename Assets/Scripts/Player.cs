using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Player : Mover
{
    private SpriteRenderer spriteRenderer;
    private bool isAlive = true;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (isAlive)
        {
            base.ReceiveDamage(dmg);
            GameManager.instance.OnHitpointChange();
        }
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            UpdateMotor(new Vector3(x, y, 0));
        }
    }

    public void Heal(int healingAmount)
    {
        int temp = hitpoint;
        hitpoint = Math.Min(hitpoint + healingAmount, maxHitpoint);
        if (temp != hitpoint)
        {
            GameManager.instance.ShowText("+" + (hitpoint - temp).ToString() + "hp", 20, Color.green, transform.position, Vector3.up * 10, .5f);
            GameManager.instance.OnHitpointChange();
        }
    }

    public void SwapSprite(int skinId)
    {
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinId];
    }

    public void OnLevelUp()
    {
        maxHitpoint += 5;
        hitpoint = maxHitpoint;
    }

    public void SetLevel(int level)
    {
        maxHitpoint = 10;
        for (int i = 0; i < level; i++)
            OnLevelUp();
    }

    protected override void Die()
    {
        GameManager.instance.deathMenuAnim.SetTrigger("Show");
        isAlive = false;
    }

    public void Respawn()
    {
        Heal(maxHitpoint);
        isAlive = true;
        pushDirection = Vector3.zero;
        lastImmune = Time.time;
    }
}
