using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Player
{
    public class PlayerCollisionHandler : MonoBehaviour
    {
        [SerializeField] GameObject shopPanel;
        [SerializeField] TMP_Text outbreakCounter;
        [SerializeField] int requiredShrooms = 100;
        [SerializeField] GameObject nextDayFirst;

        private void Start()
        {
            if (GameManager.IsUltimateLifeForm)
            {
                transform.GetChild(1).gameObject.SetActive(true);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Shroom"))
            {
                CollectShroom(other.gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!GameManager.IsUltimateLifeForm)
            {
                if (collision.gameObject.CompareTag("House"))
                {
                    Time.timeScale = 0;
                    outbreakCounter.text = "Break out of the Loop\n" + GameManager.ShroomCount + "/" + requiredShrooms;
                    shopPanel.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(nextDayFirst);

                    if (GameManager.ShroomCount >= requiredShrooms)
                    {
                        nextDayFirst.GetComponentInChildren<TMP_Text>().text = "Do it.";
                        GameManager.IsUltimateLifeForm = true;
                    }
                }
                else if (collision.gameObject.CompareTag("Enemy"))
                {
                    GameManager.IsGameOver = true;
                }
            }
        }

        private void CollectShroom(GameObject shroom)
        {
            shroom.SetActive(false);
            GameManager.ShroomCount++;
        }
    }
}