using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    // Damage struct
    public int[] damagePoints = { 1, 2, 3, 4, 5, 6, 7, 8 };
    public float[] pushForces = { 2.0f ,3.0f, 4.0f , 5.0f , 6.0f , 7.0f, 8.0f, 9.0f };

    // Upgrade
    public int weaponLevel = 0;
    public SpriteRenderer spriteRenderer;

    // Swing
    private Animator anim;
    private float cooldown = 0.5f;
    private float lastSwing;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                Swing();
            }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter")
        {
            if (coll.name != "Player")
            {
                // Create a new damage object, then we'll send it to the fightwe we've hit
                Damage dmg = new Damage
                {
                    damageAmount = damagePoints[weaponLevel],
                    origin = transform.position,
                    pushForce = pushForces[weaponLevel]
                };

                coll.SendMessage("ReceiveDamage", dmg);
            }
        }
    }

    private void Swing()
    {
        anim.SetTrigger("Swing");
    }

    public void UpgradeWeapon()
    {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];

        // Change stats %%
    }

    public void SetWeaponLevel(int level)
    {
        weaponLevel = level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[level];
    }
}
