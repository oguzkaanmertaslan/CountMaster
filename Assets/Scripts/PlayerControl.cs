using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;
using DG.Tweening;

public class PlayerControl : MonoBehaviour
{
    public Transform player;
    public Transform counter;
    [SerializeField] public int numberOfStickmans;
    [Range(0f, 1f)] [SerializeField] private float Distance, Radius;
    [SerializeField] public TextMeshPro CounterTxt;
    [SerializeField] private GameObject stickMan;
    [SerializeField] public Transform enemy;
    public bool attack;
    public static PlayerControl PlayerControlInstance;
    public GameObject SecondCamera;
    public GameObject MainCamera;
    public bool moveTheCamera;

    void Start()
    {
        player = transform;
    }
    private void Update()
    {
        numberOfStickmans = transform.childCount - 1;
        CounterTxt.text = numberOfStickmans.ToString();
        PlayerControlInstance = this;
       
    }
   
    public void FormatStickMan()
    {
        for (int i = 0; i < player.childCount; i++)
        {
            var x = Distance * Mathf.Sqrt(i) * Mathf.Cos(i * Radius);
            var z = Distance * Mathf.Sqrt(i) * Mathf.Sin(i * Radius);
            var newPos = new Vector3(x, -0.5311052f, z);
            //var counterPos = new Vector3(0, 1f, 0);
            //counter.transform.DOLocalMove(counterPos, 1f);
            player.transform.GetChild(i).DOLocalMove(newPos, 1f).SetEase(Ease.OutBack);
        }
    }

    private void MakeStickMan(int number)
    {
        for (int i = numberOfStickmans; i < number; i++)
        {
            Instantiate(stickMan, transform.position, Quaternion.identity, transform);
        }
        numberOfStickmans = transform.childCount -1;

        CounterTxt.text = numberOfStickmans.ToString();
        FormatStickMan();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="gate")
        {
            other.transform.parent.GetChild(0).GetComponent<BoxCollider>().enabled = false;
            other.transform.parent.GetChild(1).GetComponent<BoxCollider>().enabled = false;
            var gateControl = other.GetComponent<GateControl>();
            if (gateControl.multiply)
            {
              MakeStickMan(numberOfStickmans * gateControl.gateCount);
              
            }
            else
            {
                MakeStickMan(numberOfStickmans + gateControl.gateCount);
            }
        }
        if (other.gameObject.tag=="enemy")
        {
            enemy = other.transform;
            attack = true;
            other.transform.GetChild(1).GetComponent<EnemyControl>().AttackEnemy(transform);
        }
        if (other.CompareTag("Finish"))
        {
            SecondCamera.SetActive(true);
            Tower.TowerInstance.CreateTower(transform.childCount - 1);
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
