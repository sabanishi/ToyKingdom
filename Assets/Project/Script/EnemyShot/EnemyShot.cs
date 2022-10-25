using System;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    [Header("画面外で消えないかどうか")] public bool isNotDeleteScreenOut;
    [Header("攻撃力")] public int attackDamage;

    protected SpriteRenderer sr;
    protected Rigidbody2D rb;

    public void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected void FixedUpdate()
    {
        if (!isNotDeleteScreenOut && !sr.isVisible)
        {
            Destroy(gameObject);
        }
        Move();
    }

    protected virtual void Move() { }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.collider.GetComponent<Player>().Damage(attackDamage, gameObject);
        }
    }
}
