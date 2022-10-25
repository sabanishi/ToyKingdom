using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trump : Enemy
{
    [Header("通常状態の画像の名前")] public string normalImage;
    [Header("やられ状態の画像の名前")] public string damageImage;
    [Header("壁接触判定")] public EnemyCollisionCheck wallCheck;
    [Header("床接触判定")] public GroundCheck groundCheck;
 
    private bool isRightAct = false;

    protected override void Move()
    {
        if (sr.isVisible || nonVisibleAct)
        {
            if (wallCheck.isOn||!groundCheck.IsGround())
            {
                isRightAct = !isRightAct;
            }
            int xVector = -1;
            if (isRightAct)
            {
                xVector = 1;
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            rb.velocity = new Vector2(xVector * speed, rb.velocity.y);
        }
        else
        {
            rb.Sleep();
        }
    }

    protected override void PlayNormalImage()
    {
        animator.Play(normalImage);
    }
    protected override void PlayDamageImage()
    {
        animator.Play(damageImage);
    }
}
