using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] TMP_Text shroomCountText;
        [SerializeField] GameObject gameOverPanel;
        [SerializeField] GameObject shroom;

        [SerializeField] GameObject gameOverMenuFirst;

        public static int ShroomCount { get; set; }
        public static bool IsGameOver { get; set; }
        private int lastShroomCount;

        void Start()
        {

            shroomCountText.text = "Shrooms: 0";
            SpawnShrooms(10);
        }

        void Update()
        {
            if (ShroomCount != lastShroomCount)
            {
                shroomCountText.text = "Shrooms: " + ShroomCount;
                lastShroomCount = ShroomCount;
            }

            if (IsGameOver)
            {
                Time.timeScale = 0;
                gameOverPanel.SetActive(true);

                if(EventSystem.current.currentSelectedGameObject == null)
                {
                    EventSystem.current.SetSelectedGameObject(gameOverMenuFirst);
                }         
            }
        }

        private void SpawnShrooms(int numOfShrooms)
        {
            for (int i = 0; i < numOfShrooms; i++)
            {
                Instantiate(shroom, new Vector3(Random.Range(-50, 51), 0, Random.Range(-50, 51)), shroom.transform.rotation);
            }
        }

        public void OnRetryGame()
        {
            IsGameOver = false;
            Time.timeScale = 1;
            EventSystem.current.SetSelectedGameObject(null);
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);   
        }

        public void OnExitGame()
        {
            #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            #endif

            Application.Quit();
        }
    }
}