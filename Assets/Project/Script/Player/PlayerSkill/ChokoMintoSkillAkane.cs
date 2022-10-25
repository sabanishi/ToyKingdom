using UnityEngine;
using System.Collections;

public class ChokoMintoSkillAkane : PlayerSkill
{
    private bool HalfActionFlag;
    private bool isActing;

    private float Current_chargeCount;
    private float Require_chargeCount1 =0.66f;
    private float Require_chargeCount2 = 1.26f;

    public GameObject RedStar;
    public int healHp;

    public override void Act()
    {
        if (isActing)
        {
            Current_chargeCount += Time.deltaTime;
        }

        if (!HalfActionFlag&&(Current_chargeCount>=Require_chargeCount1))
        {
            ChargeAction1();
            HalfActionFlag = true;
        }

        if (Current_chargeCount >= Require_chargeCount2)
        {
            HalfActionFlag = false;
            _Player.isActionSkill = false;
            isActing = false;
            _Player.animator.Play("akane_wait");
            Current_chargeCount = 0;
        }

    }

    private void ChargeAction1()
    {
        for (int i = 0; i <= 8; i++)
        {
            Star star = Instantiate(RedStar, _Player.transform.position, Quaternion.identity).GetComponent<Star>();
            star.SetSpeed2(i * 40,3.5f);
            star.SetCount(0.6f);
        }
        _Player.SetHp(_Player.GetHp() + healHp);
    }

    public override void ButtonDownAct()
    {
        if (_Player.GetSp() >= spConsumption)
        {
            _Player.animator.Play("akane_chokominto");
            _Player.rb.velocity = new Vector2(0, 0);
            _Player.isActionSkill = true;
            isActing = true;
            _Player.SetSp(_Player.GetSp() - spConsumption);
        }
    }
}

