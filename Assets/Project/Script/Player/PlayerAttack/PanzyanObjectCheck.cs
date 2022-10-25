using UnityEngine;
using System.Collections;

public class PanzyanObjectCheck : MonoBehaviour
{
    [HideInInspector] public bool isOn = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player"||collision.tag!="AkaneAttack")
        {
            isOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player"||collision.tag!="AkaneAttack")
        {
            isOn = false;
        }
    }
}
