using UnityEngine;
using TMPro;
using DG.Tweening;

public class EnemyControl : MonoBehaviour
{
    private int numberOfStickmans;
    [SerializeField] private TextMeshPro CounterTxt;
    [SerializeField] private GameObject stickMan;
    [Range(0f, 1f)] [SerializeField] private float Distance, Radius;
    private Transform enemy;
    private bool attack;
    

    void Start()
    {
        for (int i = 0; i < Random.Range(10,30); i++)
        {
            Instantiate(stickMan, transform.position, new Quaternion(0f, 180f, 0f, 1f), transform);
        }

        CounterTxt.text = transform.childCount .ToString();
        FormatStickMan();
    }

    public void FormatStickMan()
    {
        for (int i = numberOfStickmans; i < transform.childCount; i++)
        {
            var x = Distance * Mathf.Sqrt(i) * Mathf.Cos(i * Radius);
            var z = Distance * Mathf.Sqrt(i) * Mathf.Sin(i * Radius);
            var newPos = new Vector3(x, 0.6f, z);
            transform.transform.GetChild(i).localPosition = newPos;
        }
    }

    void Update()
    {
        numberOfStickmans = transform.childCount - 1;
        if (attack && transform.childCount>1)
        {
            var enemyDirection = enemy.position - transform.position;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).rotation = Quaternion.Slerp(transform.GetChild(i).rotation, Quaternion.LookRotation(enemyDirection, Vector3.up), Time.deltaTime * 3f);
                if (enemy.childCount>1)
                {
                    var distance = enemy.GetChild(1).position - transform.GetChild(i).position;
                    if (distance.magnitude < 4f)
                    {
                        transform.GetChild(i).position = Vector3.Lerp(transform.GetChild(i).position, enemy.GetChild(1).position, Time.deltaTime * 2f);
                    }
                }
            }
        }
    }
  
    public void AttackEnemy(Transform enemyForce)
    {
        enemy = enemyForce;
        attack = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().SetBool("run", true);
        }
    }
    public void StopAttackEnemy()
    {
        attack = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().SetBool("run",false);
        }
    }
}

