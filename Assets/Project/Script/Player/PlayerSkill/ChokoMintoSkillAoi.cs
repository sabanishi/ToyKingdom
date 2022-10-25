using UnityEngine;
using System.Collections;
using System;

public class ChokoMintoSkillAoi : PlayerSkill
{
    private bool ActionFlag1;
    private bool ActionFlag2;
    private int FlagCount;

    private float Current_chargeCount;
    private float Require_chargeCount=1.0f;

    public GameObject Bullet;
    public GameObject BlueStar;

    BigChokominto bullet;
    
    public override void Act()
    {
        if (Input.GetButtonUp(ActionButton))
        { 
            ActionFlag1 = false;
            ActionFlag2 = false;
            FlagCount = 0;
            if (bullet != null)
            {
                bullet.StartMoving();
                bullet = null;
            }
            _Player.animator.Play("aoi_wait");
            _Player.rb.gravityScale = 1;
            _Player.isActionSkill = false;
            _Player.Wakka.SetActive(false);
            Vector3 scale = _Player.Wakka.transform.localScale;
            _Player.Wakka.transform.localScale = new Vector3(scale.x / 2, scale.y / 2, scale.z);
            Current_chargeCount = 0;
        }

        if (Input.GetButton(ActionButton))
        {
            Current_chargeCount += Time.deltaTime;
            _Player.rb.velocity = new Vector2(0, 0);
        }

         if (Input.GetButton(ActionButton) && !ActionFlag1)
         {
            if (Current_chargeCount / Require_chargeCount >= 0.3f)
            {
                ChargeAction();
                ActionFlag1 = true;
            }
        }
        if (Input.GetButton(ActionButton) && !ActionFlag2)
        {
            if (Current_chargeCount / Require_chargeCount >= 0.6f)
            {
                ChargeAction();
                ActionFlag2 = true;
            }
        }
        if (Input.GetButton(ActionButton)&&Current_chargeCount-Require_chargeCount>=FlagCount*0.6f)
        {
            ChargeAction();
            FlagCount++;
        }
        

        if (Current_chargeCount >= Require_chargeCount)
        {
            if (bullet != null)
            {
                bullet.isGrowing = false;
            }
        }


    }

    private void CreateShot()
    {
        float x = _Player.transform.position.x;
        if (_Player.transform.localScale.x == 1)
        {
            x += 0.55f;
        }
        else
        {
            x -= 0.55f;
        }
        bullet = null;
        bullet = Instantiate(Bullet, new Vector3(x, _Player.transform.position.y - 0.1f, _Player.transform.position.z), Quaternion.identity).GetComponent<BigChokominto>();
        bullet.SetPlayer(_Player);
        if (_Player.transform.localScale.x == 1)
        {
            bullet.SetIsRight(true);
        }
        else
        {
            bullet.SetIsRight(false);
        }
    }

    private void ChargeAction()
    {
        for (int i = 0; i <= 4; i++)
        {
            Instantiate(BlueStar,_Player.transform.position, Quaternion.identity).GetComponent<Star>().SetSpeed(i * 72);
        }
    }

    public override void ButtonDownAct()
    {
        if (_Player.GetSp() >= spConsumption)
        {
            _Player.rb.gravityScale = 0;
            _Player.animator.Play("aoi_chokominto");
            _Player.isActionSkill = true;
            CreateShot();
            _Player.Wakka.SetActive(true);
            Vector3 scale = _Player.Wakka.transform.localScale;
            _Player.Wakka.transform.localScale = new Vector3(scale.x * 2, scale.y * 2, scale.z);
            _Player.SetSp(_Player.GetSp() - spConsumption);
        }
    }
}
