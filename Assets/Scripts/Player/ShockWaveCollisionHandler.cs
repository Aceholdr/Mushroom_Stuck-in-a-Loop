using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Player
{
    public class ShockWaveCollisionHandler : MonoBehaviour
    {
        [SerializeField] Sprite burnedBear;
        [SerializeField] Sprite burnedTree;
        [SerializeField] float shrinkSpeed = 1;

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                ChangeSprite(collision, burnedBear);
                StartCoroutine(ShrinkSprite(collision, shrinkSpeed));

                GameManager.LastWaveCounter--;
            }
            else if (collision.gameObject.CompareTag("Tree"))
            {
                ChangeSprite(collision, burnedTree);
                StartCoroutine(ShrinkSprite(collision, shrinkSpeed));
            }
        }

        private void ChangeSprite(Collider collision, Sprite newSprite)
        {
            collision.gameObject.GetComponentInChildren<SpriteRenderer>().sprite = newSprite;
        }

        IEnumerator ShrinkSprite(Collider collision, float shrinkSpeed)
        {
            while(collision.transform.localScale.y >= 0.01f)
            {
                collision.transform.localScale = new Vector3(collision.transform.localScale.x - shrinkSpeed * Time.deltaTime, collision.transform.localScale.y - shrinkSpeed * Time.deltaTime, collision.transform.localScale.z - shrinkSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            collision.gameObject.SetActive(false);     
        }
    }
}