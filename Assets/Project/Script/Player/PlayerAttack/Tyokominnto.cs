using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tyokominnto : PlayerAttack
{
    private SpriteRenderer sr = null;
    private Rigidbody2D rb;
    private bool isRight;
    public void SetIsRight(bool isright)
    {
        isRight = isright;
        if (isRight)
        {
            transform.localScale = new Vector3(0.3f,0.3f, 1);
        }
        else
        {
            transform.localScale = new Vector3(-0.3f,0.3f, 1);
        }
    }
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    { 
        float xSpeed = 0;
        if (isRight)
        {
            xSpeed = 8.0f;
        }
        else
        {
            xSpeed = -8.0f;
        }
        rb.velocity = new Vector2(xSpeed,0);
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    public void SetPlayer(Player Player)
    {
        this.player = Player;
    }

    protected new void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
        if (collision.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
