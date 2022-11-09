using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementControl : MonoBehaviour
{
    [SerializeField] private PlayerInputControl playerInputController;
    [SerializeField] private PlayerControl playerControl;
    [SerializeField] private float forwardMovementSpeed;
    [SerializeField] private float horizontalMovementSpeed;
    [SerializeField] private float horizontalLimitValue;

    private float newPositionX;
    void Update()
    {
        if (playerControl.attack)
        {
            var direction = new Vector3(playerControl.enemy.position.x, transform.position.y, playerControl.enemy.position.z) - transform.position;

            for (int i = 0; i < transform.childCount; i++)
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
                            , playerControl.enemy.GetChild(1).GetChild(0).position.z), Time.deltaTime * 3f);
                    }
                }
            }
            else
            {
                playerControl.attack = false;
                playerControl.FormatStickMan();

                for (int i = 1; i < transform.childCount; i++)
                {
                    transform.GetChild(i).rotation = Quaternion.Slerp(transform.GetChild(i).rotation, Quaternion.identity, Time.deltaTime * 2f);
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
