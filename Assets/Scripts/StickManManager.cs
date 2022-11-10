using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class StickManManager : MonoBehaviour
{
    [SerializeField] Tower towerList;
    [SerializeField] PlayerControl pc;
    public GameObject redBlood;
    public GameObject blood;
    private Animator StickManAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("red") && other.transform.parent.childCount>0)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if (other.CompareTag("destroy"))
        {
           Instantiate(blood, transform.position, Quaternion.identity);
            Destroy(gameObject);
            StartCoroutine(Timer());

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
        if (other.CompareTag("stair"))
        {
            transform.parent.parent = null; 
            transform.parent = null; 
            GetComponent<Rigidbody>().isKinematic = GetComponent<Collider>().isTrigger = false;
            StickManAnimator.SetBool("run", false);

            if (!PlayerControl.PlayerControlInstance.moveTheCamera)
                PlayerControl.PlayerControlInstance.moveTheCamera = true;

            if (PlayerControl.PlayerControlInstance.player.transform.childCount == 2)
            {
                other.GetComponent<Renderer>().material.DOColor(new Color(0.4f, 0.98f, 0.65f), 0.5f).SetLoops(1000, LoopType.Yoyo)
                    .SetEase(Ease.Flash);
            }

        }
    }
    IEnumerator Timer()
    {
        PlayerControl.PlayerControlInstance.FormatStickMan();
       yield return new WaitForSecondsRealtime(3f);
    }
}
