using UnityEngine;
using System.Collections;

public class Panzyan: PlayerAttack
{
    private SpriteRenderer sr = null;
    public Rigidbody2D rb;
    private bool isRight=true;
    public bool IsRight()
    {
        return isRight;
    }
    private bool isPankoro = false;
    public void Pankoro()
    {
        isPankoro = true;
    }
    public bool IsPankoro()
    {
        return isPankoro;
    }

    public float speed = 4f;
    public PanzyanObjectCheck check1;
    public PanzyanObjectCheck check2;
    public GameObject Explosion;

    private float explosionCount = 0;
    

    public void SetIsRight(bool isright)
    {
        isRight = isright;
        if (isRight)
        {
            transform.localScale = new Vector3(1f,1f, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1f,1f, 1);
        }
    }
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    { 
        if (explosionCount > 0)
        {
            rb.velocity = new Vector2(0, 0);
            explosionCount -= Time.deltaTime;
            if (explosionCount <= 0)
            {
                Vector3 position;
                if ((check1.isOn && isRight) || (check2.isOn && !isRight))
                {
                    position = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
                }
                else
                {
                    position = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
                }
                Explosion explosion = Instantiate(Explosion, position, Quaternion.identity).GetComponent<Explosion>();
                explosion.SetPlayer(player);
                Destroy(gameObject);
            }
        }
        else
        {
            float xSpeed = 0;
            if (isPankoro)
            {
                if (isRight)
                {
                    xSpeed = speed;
                }
                else
                {
                    xSpeed = -speed;
                }
                rb.velocity = new Vector2(xSpeed, rb.velocity.y);

                if (check1.isOn || check2.isOn)
                {
                    explosionCount = 0.1f;
                }
            }
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

    protected new void OnTriggerStay2D(Collider2D collision){ }
}
