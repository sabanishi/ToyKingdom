using UnityEngine;

public class Maki : Enemy
{
    [SerializeField] private Player player;
    [SerializeField] private GroundCheck check;
    [SerializeField] private GameObject ShotPrefab;
    [SerializeField] private GameObject MakiUnit;
    private float attackCount;
    private float shotSpeed = 5;

    private float backStepCount;
    private Transform _transform;
    private bool isAttack;
    private bool isShot;
    private float waitCount;
    private bool isExcess;
    private bool isCreateaUnit;
    private float startCount;

    [SerializeField]private float minX;
    [SerializeField] private float maxX;

    private Sprite note1, note2, note3, note4, note5;
    private bool isFront;

    private const int ENEMYLAYER = 8;
    private const int ENEMYNOTCOLLIDELAYER = 14;

    public override void Start()
    {
        base.Start();
        _transform = transform;

        note1 = Resources.Load<Sprite>("note1");
        note2 = Resources.Load<Sprite>("note2");
        note3 = Resources.Load<Sprite>("note3");
        note4 = Resources.Load<Sprite>("note4");
        note5 = Resources.Load<Sprite>("note5");
    }
    protected override void Move()
    {
        if (sr.isVisible || nonVisibleAct)
        {

            if (!check.IsGround())
            {
                if (isFront)
                {
                    animator.Play("maki_frontStep");
                }
                else
                {
                    animator.Play("maki_backStep");
                }
            }
            else if (isAttack)
            {
                animator.Play("maki_guitar");
                this.gameObject.layer = ENEMYLAYER;
            }
            else
            {
                animator.Play("maki_wait");
                this.gameObject.layer = ENEMYLAYER;
            }

            float xSpeed = 0;
            float ySpeed = 0;
            if (backStepCount == 0)
            {
                AttackAct();
                waitCount += Time.deltaTime;

                if ((Mathf.Abs(_transform.position.x - player.transform.position.x) <=3f && check.IsGround())||waitCount>=3.0f)
                {
                    if (_transform.position.x >= maxX)
                    {
                        xSpeed = -10f;
                        this.gameObject.layer = ENEMYNOTCOLLIDELAYER;
                    }
                    else if (_transform.position.x <= minX)
                    {
                        xSpeed = 10f;
                        this.gameObject.layer = ENEMYNOTCOLLIDELAYER;
                    }
                    else if (waitCount >= 4.0f)
                    {
                        int random = Random.Range(0, 2);
                        if (random == 0)
                        {
                            xSpeed = -7f;
                        }
                        else
                        {
                            xSpeed = 7f;
                        }
                        this.gameObject.layer = ENEMYLAYER;
                    }
                    else
                    {
                        this.gameObject.layer = ENEMYLAYER;
                        if (_transform.position.x <= player.transform.position.x)
                        {
                            xSpeed = -7f;
                        }
                        else
                        {
                            xSpeed = 7f;
                        }
                    }
                    if(((_transform.position.x<=player.transform.position.x)&&xSpeed<0)||
                        ((_transform.position.x > player.transform.position.x)&& xSpeed > 0)){
                        isFront = false;
                    }
                    else
                    {
                        isFront = true;
                    }
                    backStepCount = 1f;
                    ySpeed = 4f;
                    waitCount = 0f;
                    attackCount = 0f;
                }
           
            }
            else
            {
                backStepCount -= Time.deltaTime;
                if (backStepCount < 0)
                {
                    backStepCount = 0;
                }
                attackCount = 0;
                isAttack = false;
                isShot = false;
            }
            if (check.IsGround())
            {
                rb.velocity = new Vector2(xSpeed, ySpeed);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            }

            if (!isCreateaUnit && ((float)hp / MaxHp <= 0.5f))
            {
                CreateUnit();
                isCreateaUnit = true;
            }
        }
        else
        {
            rb.Sleep();
        }

        if (_transform.position.x <= player.transform.position.x)
        {
            _transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
        }
        else
        {
            _transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        }

        Color color = sr.material.color;
        if (gameObject.layer == ENEMYNOTCOLLIDELAYER)
        { 
            color.a = 0.6f;
        }
        else
        {
            color.a = 1.0f;
        }
        sr.material.color = color;

        if (startCount <= 3.0f)
        {
            startCount += Time.deltaTime;
            if (startCount > 3.0f)
            {
                CreateUnit();
            }
        }
    }

    private void AttackAct()
    {
        if (attackCount <= 0)
        {
            attackCount = 1.1f;
            isAttack = true;
            isShot = false;
            isExcess = false;
        }
        else
        {
            attackCount -= Time.deltaTime;
            int random = Random.Range(0,7);

            if (attackCount <= 1.01f && !isShot)
            {
                isShot = true;
                if ((float)hp / MaxHp >= 0.5f)
                {
                    switch (random)
                    {
                        case 0:
                            for (int i = 0; i <= 4; i++)
                            {
                                CreateShot(i * 45f);
                            }
                            break;
                        case 1:
                            for (int i = 0; i <= 4; i++)
                            {
                                CreateShot(15 + i * 37.5f);
                            }
                            break;
                        case 2:
                            for (int i = 0; i <= 4; i++)
                            {
                                CreateShot(10 + i * 40f);
                            }
                            break;
                        case 3:
                            for (int i = 0; i <= 4; i++)
                            {
                                CreateShot(5 + i * 42.5f);
                            }
                            break;
                        case 4:
                            for (int i = 0; i <= 4; i++)
                            {
                                CreateShot(2 + i * 44f);
                            }
                            break;
                        case 5:
                            for (int i = 0; i <= 4; i++)
                            {
                                CreateShot(1 + i * 44.5f);
                            }
                            break;
                        case 6:
                            for (int i = 0; i <= 4; i++)
                            {
                                CreateShot(3 + i * 43.5f);
                            }
                            break;

                        default:
                            Debug.Log("MakiのAttackActでバグ");
                            break;
                    }
                }
                else
                {
                    switch (random)
                    {
                        case 0:
                            for (int i = 0; i <= 7; i++)
                            {
                                CreateShot(i * 25.7f);
                            }
                            break;
                        case 1:
                            for (int i = 0; i <= 7; i++)
                            {
                                CreateShot(15 + i * 21.42f);
                            }
                            break;
                        case 2:
                            for (int i = 0; i <= 7; i++)
                            {
                                CreateShot(10 + i * 22.86f);
                            }
                            break;
                        case 3:
                            for (int i = 0; i <= 7; i++)
                            {
                                CreateShot(5 + i * 24.29f);
                            }
                            break;
                        case 4:
                            for (int i = 0; i <= 7; i++)
                            {
                                CreateShot(2 + i * 25.14f);
                            }
                            break;
                        case 5:
                            for (int i = 0; i <= 7; i++)
                            {
                                CreateShot(1 + i * 25.43f);
                            }
                            break;
                        case 6:
                            for (int i = 0; i <= 7; i++)
                            {
                                CreateShot(3 + i * 24.86f);
                            }
                            break;

                        default:
                            Debug.Log("MakiのAttackActでバグ");
                            break;
                    }
                }
            }
       
            if (attackCount <= 0.76f)
            {
                isAttack = false;
            }
            if (attackCount <= 0.3f)
            {
                if ((float)hp / MaxHp >= 0.5f&&!isExcess)
                {
                    attackCount = 0.7f;
                    isExcess = true;
                }
            }
        }
    }

    private void CreateShot(float rotate)
    {
        GameObject shot = Instantiate(ShotPrefab, _transform.position, Quaternion.identity);
        shot.GetComponent<NoteShot>().SetSpeed(shotSpeed * Mathf.Cos(rotate * Mathf.Deg2Rad), shotSpeed * Mathf.Sin(rotate * Mathf.Deg2Rad));
        SpriteRenderer shotSprite = shot.GetComponent<SpriteRenderer>();
        int random = Random.Range(0, 5);
        switch (random)
        {
            case 0:
                shotSprite.sprite = note1;
                break;
            case 1:
                shotSprite.sprite = note2;
                break;
            case 2:
                shotSprite.sprite = note3;
                break;
            case 3:
                shotSprite.sprite = note4;
                break;
            case 4:
                shotSprite.sprite = note5;
                break;
            default:
                Debug.Log("MakkiのCreateShotでバグ");
                break;
        }
    }

    private void CreateUnit()
    {
       for(int i = 0; i <= 1; i++)
        {
            GameObject unit = Instantiate(MakiUnit, _transform.position, Quaternion.identity);
            MakiUnit unitScript = unit.GetComponent<MakiUnit>();
            unitScript.SetGapX(0.02f + i * 0.0025f);
            unitScript.SetXSpeed(0.5f + i * 1f);
            unitScript.SetYSpeed(1f + i * 2f);
            unitScript.SetPlayer(player);
            unitScript.SetCoolTime(8.0f - i*1.0f);
            if (i == 0)
            {
                unitScript.SetisSearchPlayer(false);
            }
            else
            {
                unitScript.SetisSearchPlayer(true);
                unitScript.SetMinMaxX(minX, maxX);
            }
        }
    }
}
