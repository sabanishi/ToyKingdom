using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    private float count=0.25f;
    private float xSpeed;
    private float ySpeed;
    private float s=6.0f;
    void Start()
    {
       
    }

    public void SetSpeed(float rotate)
    {
        xSpeed = s * Mathf.Cos(rotate * Mathf.Deg2Rad);
        ySpeed = s * Mathf.Sin(rotate * Mathf.Deg2Rad);
    }

    public void SetSpeed2(float rotate,float speed)
    {
        xSpeed = speed * Mathf.Cos(rotate * Mathf.Deg2Rad);
        ySpeed = speed * Mathf.Sin(rotate * Mathf.Deg2Rad);
    }

    public void SetCount(float count)
    {
        this.count = count;
    }

    void Update()
    {
        transform.position += new Vector3(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime, 0);
        count -= Time.deltaTime;
        if (count <= 0)
        {
            Destroy(gameObject);
        }
    }
}
