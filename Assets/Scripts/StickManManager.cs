using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class StickManManager : MonoBehaviour
{
    public GameObject blueBlood;
    public GameObject redBlood;
    RaycastHit hit;

    private void Update()
    {
       ParticleEffect();
    }
    private void ParticleEffect()
    {
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, 1f))
        {
            if (hit.transform.name=="Cylinder")
            {
               Destroy(hit.transform.gameObject);

            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("red") && other.transform.parent.childCount>0)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if (other.CompareTag("destroy"))
        {

            //Instantiate(blueBlood, transform.position, Quaternion.identity);
            Destroy(gameObject);
            //StartCoroutine(Timer());

        }
        switch (other.tag)
        {
            case "red":
                if (other.transform.parent.childCount>0)
                {
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                }
                break;
            case "jump":
                transform.DOJump(new Vector3(1f,0f, 70.637f), 1f, 1, 1f).SetEase(Ease.Flash);
                break;
        }
    }
   // IEnumerator Timer()
    //{
       // PlayerControl.PlayerControlInstance.FormatStickMan();
        //yield return new WaitForSecondsRealtime(3f);
    //}
}
