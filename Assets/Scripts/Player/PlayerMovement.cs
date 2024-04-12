using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private Vector3 moveVector;
        Rigidbody rb;
        float hoppHeight;
        float firstHoppHeight;
        bool isHopping;

        [SerializeField] float playerSpeed = 1f;
        [SerializeField] float hoppSpeed = 1f;
        [SerializeField] float hoppDistance = 1f;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            hoppHeight = firstHoppHeight = transform.position.y + hoppDistance;
        }

        void FixedUpdate()
        {
            MovePlayer();
        }

        private void Update()
        {
            if (moveVector.magnitude > 0.1f && !isHopping)
            {
                isHopping = true;
                StartCoroutine(HoppAfterSeconds(hoppSpeed));
            }
            else if (moveVector.magnitude <= 0.1f)
            {
                isHopping = false;
            }
        }

        IEnumerator HoppAfterSeconds(float hoppTime)
        {
            if (isHopping)
            {
                HoppPlayer();
                yield return new WaitForSeconds(1 / hoppTime);
                StartCoroutine(HoppAfterSeconds(hoppSpeed));
            }
            else if (hoppHeight != firstHoppHeight)
            {
                HoppPlayer();
            }
        }

        private void HoppPlayer()
        {
            var childPos = transform.GetChild(0).transform.position;
            transform.GetChild(0).transform.position = new Vector3(childPos.x, childPos.y + hoppHeight, childPos.z);
            hoppHeight = -hoppHeight;
        }

        private void MovePlayer()
        {
            rb.MovePosition(transform.position + moveVector * playerSpeed * Time.deltaTime);
        }

        public void OnMovePlayer(InputAction.CallbackContext context)
        {
            var inputVector = context.ReadValue<Vector2>();
            moveVector = new Vector3(inputVector.x, 0, inputVector.y);

            if (inputVector.x > 0)
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