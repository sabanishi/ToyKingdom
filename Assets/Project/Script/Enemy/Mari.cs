using UnityEngine;
using System.Collections;

public class Mari : Enemy
{
    public bool isRight;
    protected override void Move()
    {
        if (sr.isVisible || nonVisibleAct)
        {
            if (isRight)
            {
                transform.Rotate(0, 0,-1f, Space.Self);
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else
            {
                transform.Rotate(0, 0, 1f, Space.Self);
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
        }
        else
        {
            rb.Sleep();
        }
    }

    protected override void ActionWhenDamage(PlayerAttack attack)
    {
        float xSpeed;
        if (attack.transform.position.x > transform.position.x)
        {
            xSpeed = 2.0f;
        }
        else
        {
            xSpeed = 2.0f;
        }
        rb.velocity = new Vector2(xSpeed, 2.0f);
    }
}
