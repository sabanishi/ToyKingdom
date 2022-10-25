using UnityEngine;
using System.Collections;

public class BigChokominto : PlayerAttack
{
    public GameObject ChokomintoBall;
    public GameObject YellowStar;

    private SpriteRenderer sr = null;
    private Rigidbody2D rb;
    private bool isRight;
    private bool isMove;
    private float size=0.1f;
    public bool isGrowing=true;

    public void StartMoving()
    {
        isMove = true;
        this.damage = (int)(8 * (1 + size));
    }
    
    public void SetIsRight(bool isright)
    {
        isRight = isright;
        if (isRight)
        {
            transform.localScale = new Vector3(0.1f, 0.1f, 1);
        }
        else
        {
            transform.localScale = new Vector3(-0.1f, 0.1f, 1);
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
        if (isMove)
        {
            if (isRight)
            {
                xSpeed = 8.0f;
            }
            else
            {
                xSpeed = -8.0f;
            }
        }
        else
        {
            if (isGrowing)
            {
                size += Time.deltaTime;
            }
            if (isRight)
            {
                transform.localScale = new Vector3(size,size, 1);
            }
            else
            {
                transform.localScale = new Vector3(-size,size, 1);
            }
        }
        rb.velocity = new Vector2(xSpeed, 0);
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
        
        if (collision.tag !="Player")
        {
            Destroy(gameObject);
            if (size >= 0.9f)
            {
                for(int i = 0; i <= 8; i++)
                {
                    ChokomintoBall ball = Instantiate(ChokomintoBall,transform.position, Quaternion.identity).GetComponent<ChokomintoBall>();
                    ball.SetRotate(50+i * 10);
                    ball.SetPlayer(player);
                }
                for(int i = 0; i <= 5; i++)
                {
                    GameObject star= Instantiate(YellowStar, transform.position, Quaternion.identity);
                    star.GetComponent<Star>().SetSpeed2(i * 60,4);
                    star.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
    }
}
