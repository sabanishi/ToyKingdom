using System;
using UnityEngine;
public class Tako:Enemy
{
    [SerializeField] private Player player;
    [Header("方向転換する頻度")][SerializeField] private float TurnCoolTime;
    private Transform _transform;
    private Transform playerTransform;
    private float coolTime;

    public override void Start()
    {
        base.Start();
        _transform = transform;
        playerTransform = player.gameObject.transform;
    }

    protected override void Move()
    {
        if (sr.isVisible || nonVisibleAct)
        {
            if (coolTime > TurnCoolTime)
            {
                if (transform.position.x > playerTransform.position.x)
                {
                    rb.velocity = new Vector2(-speed, 0);
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    rb.velocity = new Vector2(speed, 0);
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                coolTime = 0;
            }
            else
            {
                coolTime += Time.deltaTime;
            }
        }
        else
        {
            rb.Sleep();
        }
    }
}
