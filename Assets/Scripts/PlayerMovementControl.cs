using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class PlayerMovementControl : MonoBehaviour
{
    [SerializeField] private PlayerInputControl playerInputController;
    [SerializeField] private PlayerControl playerControl;
    [SerializeField] public float forwardMovementSpeed;
    [SerializeField] private float horizontalMovementSpeed;
    [SerializeField] private float horizontalLimitValue;
    private float newPositionX;

    void Update()
    {
        if (playerControl.attack)
        {
            var direction = new Vector3(playerControl.enemy.position.x, transform.position.y, playerControl.enemy.position.z) - transform.position;

            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).rotation = Quaternion.Slerp(transform.GetChild(i).rotation, Quaternion.LookRotation(direction, Vector3.up), Time.deltaTime * 3f);
            }

            if (playerControl.enemy.GetChild(1).childCount>1)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    var Distance = playerControl.enemy.GetChild(1).GetChild(0).position - transform.GetChild(i).position;
                    if (Distance.magnitude<4f)
                    {
                        transform.GetChild(i).position = Vector3.Lerp(transform.GetChild(i).position,
                            new Vector3(playerControl.enemy.GetChild(1).GetChild(0).position.x, transform.GetChild(i).position.y
                            , playerControl.enemy.GetChild(1).GetChild(0).position.z), Time.deltaTime * 1f);
                    }
                }
            }
            else
            {
                playerControl.attack = false;
                playerControl.FormatStickMan();

                for (int i = 1; i < transform.childCount; i++)
                {
                    transform.GetChild(i).rotation = Quaternion.identity;
                }
                playerControl.enemy.gameObject.SetActive(false);
            }
            if (transform.childCount==1)
            {
                playerControl.enemy.transform.GetChild(1).GetComponent<EnemyControl>().StopAttackEnemy();
                gameObject.SetActive(false);
            }
        }
        else
        {
            SetPlayerForwardMovement();
            SetPlayerHorizontalMovement();
            if (!playerControl.attack && transform.GetChild(0).rotation != Quaternion.identity)
            {
                for (int i = 1; i < transform.childCount; i++)
                {
                    transform.GetChild(i).rotation = Quaternion.Slerp(transform.GetChild(i).rotation, Quaternion.identity, Time.deltaTime * 2f);
                }
            }
        }
        if (playerControl.moveTheCamera && transform.childCount > 1)
        {
            var cinemachineTransposer = playerControl.SecondCamera.GetComponent<CinemachineVirtualCamera>()
              .GetCinemachineComponent<CinemachineTransposer>();

            var cinemachineComposer = playerControl.SecondCamera.GetComponent<CinemachineVirtualCamera>()
                .GetCinemachineComponent<CinemachineComposer>();

            cinemachineTransposer.m_FollowOffset = new Vector3(4.5f, Mathf.Lerp(cinemachineTransposer.m_FollowOffset.y,
                transform.GetChild(1).position.y + 2f, Time.deltaTime * 1f), -5f);

            cinemachineComposer.m_TrackedObjectOffset = new Vector3(0f, Mathf.Lerp(cinemachineComposer.m_TrackedObjectOffset.y,
                4f, Time.deltaTime * 1f), 0f);

        }
    }
    private void SetPlayerForwardMovement()
    {
        transform.Translate(Vector3.forward * forwardMovementSpeed * Time.deltaTime);
    }
    private void SetPlayerHorizontalMovement()
    {
        newPositionX = transform.position.x + (playerInputController.HorizontalValue * horizontalMovementSpeed * Time.fixedDeltaTime);
        newPositionX = Mathf.Clamp(newPositionX, -horizontalLimitValue, horizontalLimitValue);
        transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);
    }
}
