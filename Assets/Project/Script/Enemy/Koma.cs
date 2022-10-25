using UnityEngine;

public class Koma:Enemy
{
    [SerializeField] private float attackCoolTime;
    [SerializeField] private GameObject ShotPrefab;
    private float attackCount;
    private float shotSpeed = 5;
    protected override void Move()
    {
        if (sr.isVisible || nonVisibleAct)
        {
            if (attackCount > attackCoolTime)
            {
                attackCount = 0;
                CreateShot();
                
            }
            else
            {
                attackCount += Time.deltaTime;
            }
        }
        else
        {
            rb.Sleep();
        }
    }

    private void CreateShot()
    {
        KomaShot shot = Instantiate(ShotPrefab, transform.position, Quaternion.identity).GetComponent<KomaShot>();
        int rotate = Random.Range(30, 150);
        shot.SetSpeed(shotSpeed * Mathf.Cos(rotate * Mathf.Deg2Rad), shotSpeed * Mathf.Sin(rotate * Mathf.Deg2Rad));
    }
}
