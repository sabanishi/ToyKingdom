using UnityEngine;
using System.Collections;

public abstract class PlayerSkill : MonoBehaviour
{
    protected Player _Player;
    protected string ActionButton;
    public int spConsumption;

    public PlayerSkill Init(string actionButton, Player player)
    {
        this._Player = player;
        this.ActionButton = actionButton;

        return this;
    }

    public abstract void Act();

    public abstract void ButtonDownAct();
}
