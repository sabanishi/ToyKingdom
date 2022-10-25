using UnityEngine;
using System.Collections;

public class ChokomintoBall :PlayerAttack
{
    private float startCount;
    private Rigidbody2D rb;

    public float Speed;

    private void Update()
    {
        startCount += Time.deltaTime;
    }

    public void SetRotate(float rotate)
    {
        rb = GetComponent<Rigidbody2D>();
        float xSpeed = Speed * Mathf.Cos(rotate * Mathf.Deg2Rad);
        float ySpeed = Speed * Mathf.Sin(rotate * Mathf.Deg2Rad);
        this.rb.velocity = new Vector2(xSpeed, ySpeed);
    }

    public void SetPlayer(Player Player)
    {
        this.player = Player;
    }

    protected new void OnTriggerStay2D(Collider2D collision)
    {
        if (startCount >= 0.3f)
        { 
            base.OnTriggerStay2D(collision);
            if (collision.tag == enemytag || collision.tag == groundtag)
            {
                Destroy(gameObject);
            }
        }
    }
}
