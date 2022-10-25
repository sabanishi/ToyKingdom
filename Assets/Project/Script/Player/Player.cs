using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GameObjectScript
{
    public float XSpeed = 0;
    public float JumpSpeed = 0;
    public GroundCheck groundChech;
    public GroundCheck groundCheck2;
    public GameObject AttackCheck;
    public GameObject AttackCheck2;
    public GameObject Bullet;
    public GameObject Wakka;
    public GameManager gameManager;
    public CapsuleCollider2D _collider;

    public GameObject ChokoMintoSkillAoiPrefab;
    public GameObject ChokoMintoSkillAkanePrefab;
    public GameObject PanzyanSkillAoiPrefab;
    public GameObject PanzyanSkillAkanePrefab;

    private bool isGround;
    private bool isJump;
    public void SetIsJump(bool isJump) { this.isJump = isJump; }
    private bool isSyagami;

    private bool isDown;
    private float damageCount;
    private bool isAttack1;
    private bool isAttack2;
    private bool isAttack3;
    private float attackCount;
    private bool isAirAttack;
    private SpriteRenderer sr;

    public bool isActionSkill;//スキル発動中この間ずっとプレイヤーの操作権は子クラスに移されつ、PlayerのUpdateは機能しない

    public int MaxHp;
    private int hp;

    public int MaxSp;
    private int sp;

    private bool isAoi;
    private bool changeFrag;
    private float changeCount;
    private List<PlayerSkill> nowSetSkills = new List<PlayerSkill>();
    private PlayerSkill actingSkill;

    private Transform _transform;

    private string enemyTag = "Enemy";
    private string enemyShotTag = "EnemyShot";

    public SkillEnum[] akaneActiveSkills=new SkillEnum[3];
    public SkillEnum[] aoiActiveSkills = new SkillEnum[3];

    public SkillEnum[] akanePassiveSkills = new SkillEnum[3];
    public SkillEnum[] aoiPassiveSkills = new SkillEnum[3];

    new void Start()
    {
        _transform = transform;
        changeFrag = true;
        sr = GetComponent<SpriteRenderer>();
        base.Start();
        SetHp(MaxHp);

        CreateSkillEnums();
        gameManager.SetPlayer(this);
        gameManager.SetSkillImage(akaneActiveSkills, akanePassiveSkills);
        ChangeSkills();
        SetSp(MaxSp);
        _collider.offset = new Vector2(-0.057f, -0.236f);
        _collider.size = new Vector2(0.418f, 2.000f);
    }

    private void CreateSkillEnums()
    {
        //茜:パンジャン、チョコミント　葵:チョコミント、パンジャン
        akaneActiveSkills[0] = SkillEnum.PANZYAN;
        akaneActiveSkills[1] = SkillEnum.CHOKOMINTO;

        aoiActiveSkills[0] = SkillEnum.CHOKOMINTO;
        aoiActiveSkills[1] = SkillEnum.PANZYAN;
    }

    //isAoiを切り替えた後に使用する
    private void ChangeSkills()
    {
        for(int i = 0; i < nowSetSkills.Count; i++)
        {
            if (nowSetSkills[i] != null)
            {
                Destroy(nowSetSkills[i].gameObject);
            }
        }
        nowSetSkills.Clear();

        SkillEnum[] skills;
        if (isAoi)
        {
            skills = aoiActiveSkills;
        }
        else
        {
            skills = akaneActiveSkills;
        }
        for(int i = 0; i < skills.Length; i++)
        {
            switch (i)
            {
                case 0:
                    nowSetSkills.Add(SetSkill(skills[i], "Skill1"));
                    break;
                case 1:
                    nowSetSkills.Add(SetSkill(skills[i], "Skill2"));
                    break;
                case 2:
                    nowSetSkills.Add(SetSkill(skills[i], "Skill3"));
                    break;
                default:
                    Debug.Log("PlayerのChangeSkillsでバグ"+i);
                    break;
            }
        }
    }

    private PlayerSkill SetSkill(SkillEnum skillEnum,string button)
    {
         PlayerSkill skill=null;
        if (isAoi)
        {
            switch (skillEnum)
            {
                case SkillEnum.CHOKOMINTO:
                    skill= Instantiate(ChokoMintoSkillAoiPrefab).GetComponent<ChokoMintoSkillAoi>().Init(button, this);
                    break;
                case SkillEnum.PANZYAN:
                    skill = Instantiate(PanzyanSkillAoiPrefab).GetComponent<PanzyanSkillAoi>().Init(button, this);
                    break;
                case SkillEnum.NONE:
                    break;
                default:
                    Debug.Log("PlayerのSetSKillでバグ"+skillEnum);
                    break;
            }
        }
        else
        {
            switch (skillEnum)
            {
                case SkillEnum.CHOKOMINTO:
                    skill= Instantiate(ChokoMintoSkillAkanePrefab).GetComponent<ChokoMintoSkillAkane>().Init(button, this);
                    break;
                case SkillEnum.PANZYAN:
                    skill = Instantiate(PanzyanSkillAkanePrefab).GetComponent<PanzyanSkillAkane>().Init(button, this);
                    break;
                case SkillEnum.NONE:
                    break;
                default:
                    Debug.Log("PlayerのSetSKillでバグ"+skillEnum);
                    break;
            }
        }
        return skill;
    }
  
    void Update()
    {
        if (damageCount > 0)
        {
            damageCount -= Time.deltaTime;
            if (damageCount <= 0)
            {
                damageCount = 0;
                Color color = sr.material.color;
                color.a = 1.0f;
                sr.material.color = color;
                gameObject.layer = 7;
            }
        }

            if (!isActionSkill)
            {
                if (!isDown)
                {
                    isGround = groundChech.IsGround();

                    float xSpeed = 0;
                    float ySpeed = 0;
                    if ((!isAttack1 || isAoi) && !isAirAttack)
                    {
                        xSpeed = CalculateXSpeed();
                        ySpeed = GroundCheckAndCalcutaeYSpeed();
                        JumpCheck(xSpeed, ySpeed);
                    }
                    if (isAirAttack)
                    {
                        if (_transform.localScale.x == 1)
                        {
                            xSpeed = XSpeed;
                        }
                        else
                        {
                            xSpeed = -XSpeed;
                        }
                        ySpeed = rb.velocity.y;
                        if (groundCheck2.IsGround())
                        {
                            attackCount = -0.01f;
                        }
                    }
                    Attack();
                    SyagamiCheck(xSpeed);
                    Change();
                    SetAnimation();
                    rb.velocity = new Vector2(xSpeed, ySpeed);
                }
                else
                {
                    if (damageCount < 0.2f)
                    {
                        isDown = false;
                        if (isAoi)
                        {
                            animator.Play("aoi_wait");
                        }
                        else
                        {
                            animator.Play("akane_wait");
                        }
                    }

                }
                SpecialAttack();
            }
            else
            {
                if (actingSkill != null)
                {
                    actingSkill.Act();
                }
                if (!isActionSkill)
                {
                    actingSkill = null;
                }
            }
        
    }

    private void SyagamiCheck(float xSpeed)
    {
        if (isGround&&ModifyInput.GetButton("Syagami",this)){
            isSyagami = true;
        }
        else
        {
            isSyagami = false;
        }
        if (xSpeed != 0)
        {
            isSyagami = false;
        }
        if (isSyagami)
        {
            _collider.size = new Vector2(0.405f, 1.514f);
            _collider.offset = new Vector2(-0.1f,-0.5f);
        }
        else
        {
            _collider.offset = new Vector2(-0.057f,-0.236f);
            _collider.size = new Vector2(0.418f,2.000f);
        }
    }

    private void SpecialAttack()
    {
        if (changeCount == 0)
        {
            if (ModifyInput.GetButtonDown("Skill1",this))
            {
                actingSkill = nowSetSkills[0];
                if (actingSkill != null)
                {
                    actingSkill.ButtonDownAct();
                }
            }
            if (ModifyInput.GetButtonDown("Skill2",this))
            {
                actingSkill = nowSetSkills[1];
                if (actingSkill != null)
                {
                    actingSkill.ButtonDownAct();
                }
            }
            if (ModifyInput.GetButtonDown("Skill3",this))
            {
                actingSkill = nowSetSkills[2];
                if (actingSkill != null)
                {
                    actingSkill.ButtonDownAct();
                }
            }
        }
    }

    private void Change()
    {
        if (changeCount == 0)
        {
            if (!isAirAttack && !isAttack1)
            {
                if (ModifyInput.GetButton("Change",this))
                {
                    changeCount = 0.5f;
                    changeFrag = false;
                }
            }
        }
        else
        {
            changeCount -= Time.deltaTime;
            if (changeCount < 0)
            {
                changeCount = 0;
                if (_transform.rotation.y != 0)
                {
                    _transform.rotation = new Quaternion(0, 0, 0, _transform.rotation.w);
                }
            }
            if (changeCount < 0.25f)
            {
                var angle = Quaternion.AngleAxis(360 * Time.deltaTime, Vector3.up);
                _transform.rotation = angle * _transform.rotation;
                if (!changeFrag)
                {
                    if (isAoi)
                    {
                        isAoi = false;
                        gameManager.ChangeToAkane();
                    }
                    else
                    {
                        isAoi = true;
                        gameManager.ChangeToAoi();
                    }
                    changeFrag = true;
                    ChangeSkills();
                }
            }
            else
            {
                var angle = Quaternion.AngleAxis(-360 * Time.deltaTime, Vector3.up);
                _transform.rotation = angle * _transform.rotation;
            }
        }
    }

        private float GroundCheckAndCalcutaeYSpeed()
    {
        float ySpeed = rb.velocity.y;
        if (isGround)
        {
            ySpeed = 0;
            //if (ModifyInput.GetButtonDown("Jump",this)||ModifyInput.GetButtonUp("Jump",this))
            if(ModifyInput.GetButton("Jump",this))
            {
                ySpeed = JumpSpeed;
            }
        }
        return ySpeed;
    }

    private void JumpCheck(float xSpeed,float ySpeed)
    { 
        if (isJump)
        {
            xSpeed = rb.velocity.x;
            if (xSpeed > 0)
            {
                _transform.localScale = new Vector3(1, 1, 1);
            }
            else if (xSpeed < 0)
            {
                _transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        if (ySpeed > 0)
        {
            isJump = true;
        }
        else
        {
            isJump = false;
        }
    }

    private float  CalculateXSpeed()
    {
        float xSpeed=0f;
        float horizontalKey = ModifyInput.GetAxis("Horizontal",this);
        if (horizontalKey > 0)
        {
            _transform.localScale = new Vector3(1, 1, 1);
            animator.SetBool("walk", true);
            xSpeed = XSpeed;
        }
        else if (horizontalKey < 0)
        {
            _transform.localScale = new Vector3(-1, 1, 1);
            animator.SetBool("walk", true);
            xSpeed = -XSpeed;
        }
        else
        {
            xSpeed = 0.0f;
            animator.SetBool("walk", false);
        }
        return xSpeed;
    }

    private void Attack()
    {
        if (attackCount > 0)
        {
            attackCount -= Time.deltaTime;
            if (attackCount <= 0.45f)
            {
                Wakka.SetActive(false);
            }
        }
        else if (attackCount < 0)
        {
            attackCount = 0;
            isAttack1 = false;
            isAttack2 = false;
            isAttack3 = false;
            isAirAttack = false;
            AttackCheck.SetActive(false);
            AttackCheck2.SetActive(false);
            gameObject.layer = 7;
            if (isAoi)
            {
                animator.Play("aoi_wait");
            }
            else
            {
                animator.Play("akane_wait");

            }
        }
        if (changeCount == 0)
        {
            NormalAttack();
        }
    }

    public void Somersault()
    {
        isAirAttack = false;
        AttackCheck2.SetActive(false);
        if (ModifyInput.GetAxis("Vertical",this) > 0)
        {
            float ySpeed = JumpSpeed;
            float xSpeed = 0;
            float horizontalKey = ModifyInput.GetAxis("Horizontal",this);
            if (horizontalKey > 0)
            {
                _transform.localScale = new Vector3(1, 1, 1);
                xSpeed = XSpeed;
            }
            else if (horizontalKey < 0)
            {
                _transform.localScale = new Vector3(-1, 1, 1);
                xSpeed = -XSpeed;
            }
            rb.velocity = new Vector2(xSpeed, ySpeed);
            animator.Play("akane_jump1");
        }
        else
        {
            animator.Play("akane_wait");
        }
        gameObject.layer = 7;
    }

    private void SetAnimation()
    {
        animator.SetBool("jump", isJump);
        animator.SetBool("ground", isGround);
        animator.SetBool("attack", isAttack1);
        animator.SetBool("attack2", isAttack2);
        animator.SetBool("attack3", isAttack3);
        animator.SetBool("isAoi", isAoi);
        animator.SetBool("syagami", isSyagami);
    }

    public void Damage(int damage,GameObject collision)
    {
        if (damageCount == 0)
        {
            int newHp = GetHp() - damage;
            if (newHp < 0)
            {
                newHp = 0;
            }
            SetHp(newHp);
            if (newHp > 0)
            {
                if (!isActionSkill)
                {
                    if (isAoi)
                    {
                        animator.Play("aoi_damage");
                    }
                    else
                    {
                        animator.Play("akane_damage");
                    }
                    isDown = true;
                    if (this._transform.position.x >= collision.transform.position.x)
                    {
                        rb.velocity = new Vector2(2, 3);
                    }
                    else
                    {
                        rb.velocity = new Vector2(-2, 3);
                    }
                }
                damageCount = 1.0f;
                Color color = sr.material.color;
                color.a = 0.6f;
                sr.material.color = color;
                gameObject.layer = 10;
            }
            else
            {
                //敗北処理
            }
        }
    }

    public void SetHp(int HP)
    {
        hp = HP;
        if (HP > MaxHp)
        {
            hp = MaxHp;
        }
        gameManager.SetHp(hp, MaxHp);
    }

    public int GetHp()
    {
        return hp;
    }

    public void SetSp(int SP)
    {
        sp = SP;
        if (SP > MaxSp)
        {
            sp = MaxSp;
        }
        gameManager.SetSP(sp, MaxSp);
    }

    public int GetSp()
    {
        return sp;
    }

    private void NormalAttack()
    {
        float attackKey = ModifyInput.GetAxis("NormalAttack",this);
        if (attackKey > 0)
        {
            if (isAoi)
            {
                if (attackCount < 0.1f)
                {
                    attackCount = 0.7f;
                    isAttack1 = true;
                    Wakka.SetActive(true);
                    float x = transform.position.x;
                    if (transform.localScale.x == 1)
                    {
                        x += 0.77f;
                    }
                    else
                    {
                        x -= 0.77f;
                    }
                    Tyokominnto bullet = Instantiate(Bullet, new Vector3(x, transform.position.y - 0.1f, transform.position.z), Quaternion.identity).GetComponent<Tyokominnto>();
                    bullet.SetPlayer(this);
                    if (transform.localScale.x == 1)
                    {
                        bullet.SetIsRight(true);
                    }
                    else
                    {
                        bullet.SetIsRight(false);
                    }

                }
            }
            else
            {
                if (this.isGround)
                {
                    //一発目
                    if (!isAttack1)
                    {
                        isAttack1 = true;
                        attackCount = 0.3f;
                        AttackCheck.SetActive(true);
                    }
                    else if (!isAttack2)
                    {
                        if (attackCount > 0 && attackCount < 0.1f)
                        {
                            //二発目
                            isAttack2 = true;
                            attackCount = 0.3f;
                            AttackCheck.SetActive(true);
                        }
                    }
                    else if (!isAttack3)
                    {
                        if (attackCount > 0 && attackCount < 0.1f)
                        {
                            //三発目
                            isAttack3 = true;
                            attackCount = 0.3f;
                            AttackCheck.SetActive(true);
                        }
                    }
                }
                else
                {
                    //飛び蹴り
                    isAirAttack = true;
                    AttackCheck2.SetActive(true);
                    attackCount = 3.0f;
                    animator.Play("akane_airAttack");
                    gameObject.layer = 10;
                }
            }
        }
    }
    public class ModifyInput
    {
        public static float GetAxis(string name,Player player)
        {
            if (!player.gameManager.isBossBattle)
            {
                return Input.GetAxis(name);
            }
            return 0;
        }

        public static bool GetButtonDown(string name,Player player)
        {
            if (!player.gameManager.isBossBattle)
            {
                return Input.GetButtonDown(name);
            }
            return false;
        }

        public static bool GetButtonUp(string name, Player player)
        {
            if (!player.gameManager.isBossBattle)
            {
                return Input.GetButtonUp(name);
            }
            return false;
        }

        public static bool GetButton(string name, Player player)
        {
            if (!player.gameManager.isBossBattle)
            {
                return Input.GetButton(name);
            }
            return false;
        }
    }
}
