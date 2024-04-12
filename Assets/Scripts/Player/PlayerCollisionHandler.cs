using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerCollisionHandler : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Shroom"))
            {
                CollectShroom(other.gameObject);
            }
            else if (other.gameObject.CompareTag("Enemy"))
            {
                GameManager.IsGameOver = true;
            }

        }

        private void CollectShroom(GameObject shroom)
        {
            shroom.SetActive(false);
            GameManager.ShroomCount++;
        }
    }
}