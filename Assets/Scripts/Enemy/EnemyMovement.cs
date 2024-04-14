using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        Rigidbody rb;
        Vector3 moveDirection;

        [SerializeField] float movementSpeed = 1f;
        GameObject player;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            moveDirection.x = player.transform.position.x - transform.position.x;
            moveDirection.z = player.transform.position.z - transform.position.z;
        }

        void FixedUpdate()
        {
            MoveEnemyTowardPlayer();
        }

        private void MoveEnemyTowardPlayer()
        {
            rb.MovePosition(transform.position + moveDirection * movementSpeed * Time.deltaTime);

            if (moveDirection.x > 0)
            {
                gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
            }
            else
            {
                gameObject.GetComponentInChildren<SpriteRenderer>().flipX = false;
            }
        }
    }
}