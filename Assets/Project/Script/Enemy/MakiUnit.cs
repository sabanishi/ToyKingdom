using UnityEngine;
using System.Collections;

public class MakiUnit : MonoBehaviour
{
    [SerializeField] private float height;
    [SerializeField] private GameObject ShotPrefab;

    private bool isAttack;
    private bool firstAttack;
    private bool secondAttack;
    private float attackCount;
    private bool isHighHeight;
    private float time;

    private Sprite note6, note7, note8, note9, note10;

    private float gapX;
    public void SetGapX(float x)
    {
        gapX = x;
    }
    private float xSpeed;
    public void SetXSpeed(float xspeed)
    {
        xSpeed = xspeed;
    }
    private float ySpeed;
    public void SetYSpeed(float yspeed)
    {
        ySpeed = yspeed;
    }

    private float coolTime;
    public void SetCoolTime(float _coolTime)
    {
        coolTime = _coolTime;
    }

    private Player player;
    public void SetPlayer(Player _player)
    {
        player = _player;
    }
    private bool isSearchPlayer;
    public void SetisSearchPlayer(bool isSearch)
    {
        isSearchPlayer = isSearch;
    }
    private float minX, maxX;
    public void SetMinMaxX(float _minX,float _maxX)
    {
        minX = _minX;
        maxX = _maxX;
    }
    private float waitCount;

    private Transform _transform;
    private Rigidbody2D rb;

    void Start()
    {
        _transform = transform;
        rb = GetComponent<Rigidbody2D>();
        note6 = Resources.Load<Sprite>("note6");
        note7 = Resources.Load<Sprite>("note7");
        note8 = Resources.Load<Sprite>("note8");
        note9 = Resources.Load<Sprite>("note9");
        note10 = Resources.Load<Sprite>("note10");
    }

   
    void Update()
    {
        float x =0;
        float y =0;

        if (!isHighHeight)
        {
            if (_transform.position.y <= height)
            {
                y += ySpeed;
            }
            else
            {
                isHighHeight = true;
            }
        }
        else
        {
            y = 0;
        }

        if (!isAttack)
        {
            if (isSearchPlayer)
            {
                x = rb.velocity.x;
                if (_transform.position.x >= maxX)
                {
                    x =- xSpeed;
                }
                else if (_transform.position.x <= minX)
                {
                    x =xSpeed;
                }
                else if (waitCount >= 4.0f)
                {
                    int random = Random.Range(0, 2);
                    if (random == 0)
                    {
                        x=-xSpeed;
                    }
                    else
                    {
                        x=xSpeed;
                    }
                    waitCount = 0;
                }
                waitCount+=Time.deltaTime;
            }
            else
            {
                if (Mathf.Abs(player.transform.position.x - _transform.position.x) >= gapX)
                {
                    if (player.transform.position.x > _transform.position.x)
                    {
                        x += xSpeed;
                    }
                    else
                    {
                        x -= xSpeed;
                    }
                }
            }
        }

        Attack();
        time += Time.deltaTime;
        y += Mathf.PerlinNoise(time, 0);
        y -= 0.5f;

        rb.velocity = new Vector2(x, y);
    }

    private void Attack()
    {
        if (attackCount <= 0)
        {
            isAttack = false;
            firstAttack = false;
            secondAttack = false;
            attackCount = coolTime+3.0f;
        }

        if (attackCount <= 3.0f && !isAttack)
        {
            isAttack = true;
        }

        if (attackCount <= 2.0f&&!firstAttack)
        {
            firstAttack = true;
            int random = Random.Range(0, 361);
            for(int i = 0; i <= 8; i++)
            {
                CreateShot(random + i * 40);
            }
        }
        if (attackCount <= 1.0f && !secondAttack)
        {
            secondAttack = true;
            int random = Random.Range(0, 361);
            for (int i = 0; i <= 8; i++)
            {
                CreateShot(random + i * 40);
            }
        }
        attackCount -= Time.deltaTime;
    }

    private void CreateShot(float rotate)
    {
        GameObject shot = Instantiate(ShotPrefab, _transform.position, Quaternion.identity);
        shot.GetComponent<NoteShot>().SetSpeed(3* Mathf.Cos(rotate * Mathf.Deg2Rad),3 * Mathf.Sin(rotate * Mathf.Deg2Rad));
        SpriteRenderer shotSprite = shot.GetComponent<SpriteRenderer>();
        int random = Random.Range(0, 5);
        switch (random)
        {
            case 0:
                shotSprite.sprite = note6;
                break;
            case 1:
                shotSprite.sprite = note7;
                break;
            case 2:
                shotSprite.sprite = note8;
                break;
            case 3:
                shotSprite.sprite = note9;
                break;
            case 4:
                shotSprite.sprite = note10;
                break;
            default:
                Debug.Log("MakkiのCreateShotでバグ");
                break;
        }
    }
}
