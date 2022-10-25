using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectScript : MonoBehaviour
{
    [System.NonSerialized] public Animator animator;
    [System.NonSerialized] public Rigidbody2D rb;
   
    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
}
