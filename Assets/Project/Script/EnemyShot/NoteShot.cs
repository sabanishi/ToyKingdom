using UnityEngine;
using System.Collections;

public class NoteShot :EnemyShot
{
    private float xSpeed;
    private float ySpeed;
    private bool isSetSpeed;
    public SpriteRenderer getSr()
    {
        return sr;
    }
    protected override void Move()
    {
        if (!isSetSpeed)
        {
            isSetSpeed = true;
            rb.velocity = new Vector2(xSpeed, ySpeed); 
        }
    }

    public void SetSpeed(float xspeed,float yspeed)
    {
        xSpeed = xspeed;
        ySpeed = yspeed;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.collider.tag == "Player" || collision.collider.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
