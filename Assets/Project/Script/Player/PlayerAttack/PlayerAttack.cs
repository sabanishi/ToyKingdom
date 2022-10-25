using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage;
    [Header("一度攻撃しても消えない")] public bool isEternal;
    protected string enemytag = "Enemy";
    protected string groundtag = "Ground";
    [SerializeField] protected Player player;

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == enemytag)
        {
            if (collision.GetComponent<Enemy>().OnDamage(this))
            {
                Debug.Log(damage);
                player.SetSp(player.GetSp() + damage);
            }
            if (!isEternal)
            {
                gameObject.SetActive(false);
            }
        }
    }
}