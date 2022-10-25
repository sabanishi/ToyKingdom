using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    private float count;
    private float xSpeed;
    private float ySpeed;
    private float s = 5.0f;
    public void SetSpeed(float rotate)
    {
        xSpeed = s * Mathf.Cos(rotate * Mathf.Deg2Rad);
        ySpeed = s * Mathf.Sin(rotate * Mathf.Deg2Rad);
    }

    void Start()
    {
        count = 0.25f;
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
