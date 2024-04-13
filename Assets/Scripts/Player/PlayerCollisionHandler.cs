using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Player
{
    public class PlayerCollisionHandler : MonoBehaviour
    {
        [SerializeField] GameObject shopPanel;
        [SerializeField] GameObject nextDayFirst;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Shroom"))
            {
                CollectShroom(other.gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("House"))
            {
                Time.timeScale = 0;
                shopPanel.SetActive(true);
                EventSystem.current.SetSelectedGameObject(nextDayFirst);
            }
            else if (collision.gameObject.CompareTag("Enemy"))
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