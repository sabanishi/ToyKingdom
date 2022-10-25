using UnityEngine;
using System.Collections;

public class AkaneAirAttack : PlayerAttack
{

    private new void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        { 
            player.Somersault();
        };
        base.OnTriggerStay2D(collision);
    }
}
