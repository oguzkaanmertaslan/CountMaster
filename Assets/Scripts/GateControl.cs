using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GateControl : MonoBehaviour
{
    public TextMeshPro GateNo;
    public int gateCount;
    public bool multiply;
    public bool isTrigger=false;
    void Start()
    {
      
        if (multiply)
        {
            gateCount = Random.Range(1, 3);
            GateNo.text ="x"+ gateCount.ToString();
        }
        else
        {
            gateCount = Random.Range(20, 80);
            if (gateCount % 2 !=0)
            {
                gateCount += 1;
            }
            GateNo.text ="+"+ gateCount.ToString();
        }
        
    }
}
