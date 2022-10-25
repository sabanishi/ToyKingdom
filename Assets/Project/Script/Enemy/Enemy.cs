using UnityEngine;
using System.Collections;

public abstract class Enemy : GameObjectScript
{
    [Header("画面外でも行動する")] public bool nonVisibleAct;
    [Header("攻撃された時の無敵時間")] public float damageCoolTime;
    [Header("攻撃された時の硬直時間")] public float damageStanTime;
    [Header("攻撃力")] public int attackDamage;
    protected SpriteRenderer sr = null;

    public GameObject RedStar;
    public GameObject BlueStar;
    public GameObject YellowStar;
    public GameObject WhiteStar;
    public GameObject GreenStar;
    public GameObject RedEffect;
    public GameObject BlueEffect;

    public float speed;
    public int MaxHp;

    protected int hp;
    protected bool isDown;
    protected float coolTimeCount;
    protected float stanTimeCount;

    public override void Start()
    {
        base.Start();
        sr = GetComponent<SpriteRenderer>();
        hp = MaxHp;
    }

    protected void FixedUpdate()
    {
        if (!isDown)
        {
           Move();
        }

        if (coolTimeCount > 0)
        {
            coolTimeCount -= Time.deltaTime;
            if (coolTimeCount <= 0)
            {
                coolTimeCount = 0;
            }
        }
        if (stanTimeCount > 0)
        {
            stanTimeCount -= Time.deltaTime;
            if (stanTimeCount <= 0)
            {
                stanTimeCount = 0;
                isDown = false;
                PlayNormalImage();
            }
        }
    }

    public bool OnDamage(PlayerAttack attack)
    { 
        if (coolTimeCount <= 0)
        {
            hp -= attack.damage;
            coolTimeCount = damageCoolTime;
            stanTimeCount = damageStanTime;
            if (stanTimeCount > 0)
            {
                isDown = true;
            }
            PlayDamageImage();
            rb.velocity = new Vector2(0, 0);
            if (hp <= 0)
            {
                Die();
            }
            ActionWhenDamage(attack);
            GameObject effect = null;
            if (attack.gameObject.tag == "AkaneAttack")
            {
                effect = RedEffect;
            }
            if (attack.gameObject.tag == "AoiAttack")
            {
                effect = BlueEffect;
            }
            if (effect != null)
            {
                float startRotate;
                if (attack.gameObject.transform.position.x > this.transform.position.x)
                {
                    startRotate = -40f-Random.Range(0,40);
                }
                else
                {
                    startRotate = 100f+Random.Range(0,40);
                }
                Vector3 _transform = new Vector3((attack.gameObject.transform.position.x + transform.position.x) / 2, attack.transform.position.y, 0);
                for (int i = 0; i <= 3; i++)
                {
                    AttackEffect attackEffect = Instantiate(effect, _transform, Quaternion.identity).GetComponent<AttackEffect>();
                    attackEffect.SetSpeed(startRotate + 40 * i);
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void Die()
    {
        Destroy(gameObject);
        for (int i = 0; i <= 4; i++)
        {
            switch (i)
            {
                case 0:
                    Instantiate(RedStar, transform.position, Quaternion.identity).GetComponent<Star>().SetSpeed(0);
                    break;
                case 1:
                    Instantiate(WhiteStar, transform.position, Quaternion.identity).GetComponent<Star>().SetSpeed(72);
                    break;
                case 2:
                    Instantiate(GreenStar, transform.position, Quaternion.identity).GetComponent<Star>().SetSpeed(144);
                    break;
                case 3:
                    Instantiate(YellowStar, transform.position, Quaternion.identity).GetComponent<Star>().SetSpeed(216);
                    break;
                case 4:
                    Instantiate(BlueStar, transform.position, Quaternion.identity).GetComponent<Star>().SetSpeed(288);
                    break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.collider.GetComponent<Player>().Damage(attackDamage,gameObject);
        }
    }

    protected abstract void Move();

    protected virtual void PlayNormalImage() { }
    protected virtual void PlayDamageImage() { }

    protected virtual void ActionWhenDamage(PlayerAttack attack) {}
}
