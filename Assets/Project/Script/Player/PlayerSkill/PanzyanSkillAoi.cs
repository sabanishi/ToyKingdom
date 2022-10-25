using UnityEngine;
using System.Collections;

public class PanzyanSkillAoi : PlayerSkill
{

    public GameObject PanzyanPrefab;
    private Panzyan panzyan;
    private float Current_chargeCount;
    private float Require_chargeCount = 0.8f;
    private bool isActing;

    public override void Act()
    {
        if (isActing)
        {
            Current_chargeCount += Time.deltaTime;
            _Player.rb.velocity = new Vector2(0,_Player.rb.velocity.y);
            if (Current_chargeCount >= Require_chargeCount)
            {
                _Player.isActionSkill = false;
                isActing = false;
                if (panzyan != null)
                {
                    panzyan.Pankoro();
                    panzyan = null;
                }
                Current_chargeCount = 0f;
                _Player.animator.Play("aoi_wait");
            }
        }
    }

    private void CreatePanzyan()
    {
        float x = _Player.transform.position.x;
        if (_Player.transform.localScale.x == 1)
        {
            x += 1.6f;
        }
        else
        {
            x -= 1.6f;
        }
        panzyan = null;
        panzyan = Instantiate(PanzyanPrefab, new Vector3(x, _Player.transform.position.y - 0.1f, _Player.transform.position.z), Quaternion.identity).GetComponent<Panzyan>();
        panzyan.SetPlayer(_Player);
        if (_Player.transform.localScale.x == 1)
        {
            panzyan.SetIsRight(true);
        }
        else
        {
            panzyan.SetIsRight(false);
        }
    }

    public override void ButtonDownAct()
    {
        if (_Player.GetSp() >= spConsumption)
        {
            _Player.animator.Play("aoi_panzyan");
            _Player.isActionSkill = true;
            isActing = true;
            CreatePanzyan();
            _Player.SetSp(_Player.GetSp() - spConsumption);
        }
        else
        {
            //ぷぷーみたいな音入れる
        }
    }
}
