using UnityEngine;
using System.Collections;

public class PanzyanSkillAkane : PlayerSkill
{
    public GameObject PanzyanPrefab;
    private Panzyan panzyan;
    private float Current_chargeCount;
    private float Require_chargeCount = 0.5f;
    private bool isActing;
    private Vector3 startPosition;
    private bool isGround;

    public override void Act()
    { 
        if (panzyan != null)
        {
            if (!panzyan.IsPankoro())
            {
                if (isActing)
                {
                    if (!isGround)
                    {
                        if (_Player.groundChech.IsGround())
                        {
                            isGround = true;
                            startPosition = _Player.transform.position;
                            _Player.animator.Play("akane_jump_not_transition");
                        }
                    }
                    else
                    {
                        Current_chargeCount += Time.deltaTime;
                        _Player.rb.velocity = new Vector2(0, 0);
                        if (Current_chargeCount <= Require_chargeCount / 2)
                        {
                            if (_Player.transform.localScale.x == 1)
                            {
                                _Player.transform.position = new Vector3(startPosition.x + 0.6f * Current_chargeCount, startPosition.y + 12 * Current_chargeCount, startPosition.z);
                            }
                            else
                            {
                                _Player.transform.position = new Vector3(startPosition.x - 0.6f * Current_chargeCount, startPosition.y + 12 * Current_chargeCount, startPosition.z);
                            }
                        }
                        else
                        {
                            if (_Player.transform.localScale.x == 1)
                            {
                                _Player.transform.position = new Vector3(startPosition.x + 0.6f * Current_chargeCount, startPosition.y - 4 * (Current_chargeCount - Require_chargeCount / 2) + 12 * Require_chargeCount / 2, startPosition.z);
                            }
                            else
                            {
                                _Player.transform.position = new Vector3(startPosition.x - 0.6f * Current_chargeCount, startPosition.y - 4 * (Current_chargeCount - Require_chargeCount / 2) + 12 * Require_chargeCount / 2, startPosition.z);
                            }
                        }

                        if (Current_chargeCount >= Require_chargeCount)
                        {
                            isActing = false;
                            if (panzyan != null)
                            {
                                panzyan.Pankoro();
                            }
                            Current_chargeCount = 0f;
                            _Player.animator.Play("akane_panzyan");
                            if (_Player.transform.localScale.x == 1)
                            {
                                _Player.transform.position = new Vector3(startPosition.x + 0.3f, startPosition.y + 2, startPosition.z);
                            }
                            else
                            {
                                _Player.transform.position = new Vector3(startPosition.x - 0.3f, startPosition.y + 2, startPosition.z);
                            }

                        }
                    }
                }
            }
            else
            {
                Vector3 position = panzyan.transform.position;
                _Player.transform.position = new Vector3(position.x, position.y + 2, position.z);
                if (panzyan.check1.isOn || panzyan.check2.isOn)
                {
                    _Player.rb.velocity = new Vector2(0, _Player.JumpSpeed);
                    _Player.animator.Play("akane_jump1");
                    _Player.isActionSkill = false;
                    panzyan = null;
                    _Player.gameObject.layer = 7;
                    isGround = false;
                    _Player.SetIsJump(true);
                }
                if (Input.GetButtonDown("Jump")||Input.GetButtonUp("Jump"))
                {
                    _Player.rb.velocity = new Vector2(0, _Player.JumpSpeed);
                    _Player.animator.Play("akane_jump1");
                    _Player.isActionSkill = false;
                    panzyan = null;
                    _Player.gameObject.layer = 7;
                    isGround = false;
                    _Player.SetIsJump(true);
                }
            }
        }
        else
        {
            if (_Player.isActionSkill)
            {
                _Player.animator.Play("akane_wait");
                _Player.SetIsJump(false);
                _Player.isActionSkill = false;
                _Player.gameObject.layer = 7;
                isGround = false;
                panzyan = null;
                isActing = false;
            }
        }
    }

    private void CreatePanzyan()
    {
        float x = _Player.transform.position.x;
        if (_Player.transform.localScale.x == 1)
        {
            x += 0.3f;
        }
        else
        {
            x -= 0.3f;
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
        panzyan.speed = 6f;
    }

    public override void ButtonDownAct()
    {
        if (_Player.GetSp() >= spConsumption)
        {
            _Player.isActionSkill = true;
            isActing = true;
            CreatePanzyan();
            _Player.gameObject.layer = 10;
            _Player.rb.velocity = new Vector2(0, _Player.rb.velocity.y);
            _Player.SetSp(_Player.GetSp() - spConsumption);
        }
    }
}
