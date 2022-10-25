using System;
using UnityEngine;

public class KomaShot:EnemyShot
{
    private float xSpeed;
    private float ySpeed;
    private bool isSetSpeed;
    protected override void Move()
    {
        if (!isSetSpeed)
        {
            isSetSpeed = true;
            rb.velocity = new Vector2(xSpeed, ySpeed);
            float rotate0 = Mathf.Atan2(ySpeed, xSpeed);
            transform.rotation = Quaternion.Euler(0, 0, rotate0*Mathf.Rad2Deg+180);
        }
        float rotate = Mathf.Atan2(rb.velocity.y,rb.velocity.x);
        transform.rotation = Quaternion.Euler(0, 0, rotate*Mathf.Rad2Deg+180);
    }

    public void SetSpeed(float xspeed,float yspeed)
    {
        xSpeed = xspeed;
        ySpeed = yspeed;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.collider.tag =="Player"||collision.collider.tag=="Ground")
        {
            Destroy(gameObject);
        }
    }
}
