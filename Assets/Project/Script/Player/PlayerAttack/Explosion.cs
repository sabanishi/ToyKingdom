using UnityEngine;
using System.Collections;

public class Explosion : PlayerAttack
{
    private SpriteRenderer sr = null;
    private Rigidbody2D rb;
    private float Count;
    public float destroyCount=1.1f;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Count += Time.deltaTime;
        if (Count >= destroyCount)
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    public void SetPlayer(Player Player)
    {
        this.player = Player;
    }
}

