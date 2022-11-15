using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;

public class StickManManager : MonoBehaviour
{
    public GameObject redBlood;
    public GameObject blood;
    public GameObject mainCamera;
    public Transform player;
    [SerializeField] PlayerMovementControl pmc;
    [Range(0f, 1f)] [SerializeField] private float Distance, Radius;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("destroy"))
        {
           Instantiate(blood, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        switch (other.tag)
        {
            case "red":
                if (other.transform.parent.childCount>0)
                {
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                    Instantiate(blood, transform.position, Quaternion.identity);
                    Instantiate(redBlood, transform.position, Quaternion.identity);
                }
                break;
            case "jump":
                var newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 4f);
                transform.DOJump(newPosition, 1f, 1, 1f).SetEase(Ease.Flash).OnComplete(PlayerControl.PlayerControlInstance.FormatStickMan);
                break;
        }
        if (other.CompareTag("stair"))
        {
            transform.parent.parent = null;
            transform.parent = null;
            transform.GetComponent<Animator>().SetBool("run", false);
            GetComponent<Rigidbody>().isKinematic = GetComponent<Collider>().isTrigger = false;
            if (!PlayerControl.PlayerControlInstance.moveTheCamera)
                PlayerControl.PlayerControlInstance.moveTheCamera = true;

           
            if (PlayerControl.PlayerControlInstance.player.transform.childCount == 2)
            {
                pmc.forwardMovementSpeed = 0;
                other.GetComponent<Renderer>().material.DOColor(new Color(0.4f, 0.98f, 0.65f), 0.5f).SetLoops(1000, LoopType.Yoyo)
                    .SetEase(Ease.Flash);
            }
        }
    }
 }
     
  

