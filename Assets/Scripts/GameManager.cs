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
        [SerializeField] GameObject nextDayPanel;
        [SerializeField] GameObject shroom;
        [SerializeField] GameObject tree;

        [SerializeField] GameObject gameOverMenuFirst;
        [SerializeField] Transform[] spawnPoints;
        [SerializeField] GameObject enemy;
        [SerializeField] GameObject player;

        [SerializeField] int treeCalculator = 80;
        [SerializeField] int shroomCalculator = 1;

        [SerializeField] float spawnTimeDecrease = 0.1f;
        [SerializeField] float gameStartTime = 4f;
        [SerializeField] float currentSpawnTime;

        public static float StartSpawnTime { get; set; }
        public static int ShroomCount { get; set; }
        public static int DayCounter { get; set; }
        public static bool IsGameOver { get; set; }
        private int lastShroomCount;

        void Start()
        {
            if(StartSpawnTime <= 0)
            {
                StartSpawnTime = gameStartTime;
            }
            else if(StartSpawnTime >= 1f)
            {
                StartSpawnTime -= 0.5f;
            }

            currentSpawnTime = StartSpawnTime;

            StartCoroutine(StartNewDay());
            StartCoroutine(SpawnEnemy());

            shroomCountText.text = "Shrooms: 0";
            SpawnShrooms(10);
        }

        IEnumerator SpawnEnemy()
        {
            for(int i = 0; i < spawnPoints.Length; i++)
            {
                if (!IsVisible(spawnPoints[i].position, new Vector3(1,1,1), Camera.main))
                {
                    Instantiate(enemy, spawnPoints[i].position, enemy.transform.rotation);
                }
            }

            yield return new WaitForSeconds(currentSpawnTime);

            if(currentSpawnTime > 0.1f)
            {
                currentSpawnTime -= spawnTimeDecrease;
            }

            StartCoroutine(SpawnEnemy());
        }

        bool IsVisible(Vector3 pos, Vector3 boundSize, Camera camera)
        {
            var bounds = new Bounds(pos, boundSize);
            var planes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(planes, bounds);
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

                if (EventSystem.current.currentSelectedGameObject == null)
                {
                    EventSystem.current.SetSelectedGameObject(gameOverMenuFirst);
                }
            }
        }

        private void SpawnShrooms(int numOfShrooms)
        {
            for (int j = -23; j < 24; j++)
            {
                for (int k = -49; k < 49; k++)
                {
                    if(j < 20 || k < 40)
                    {
                        int random = Random.Range(0, 500);

                        if (random < shroomCalculator)
                        {
                            Instantiate(shroom, new Vector3(j, 0, k), shroom.transform.rotation);
                        }
                        else if (random >= treeCalculator)
                        {
                            Instantiate(tree, new Vector3(j * 2, 0, k), shroom.transform.rotation);
                        }
                    }
                }
            }
        }

        public void OnRetryGame()
        {
            IsGameOver = false;
            DayCounter = 0;
            ShroomCount = 0;
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

        public void OnNextDay()
        {
            EventSystem.current.SetSelectedGameObject(null);
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
        }

        IEnumerator StartNewDay()
        {
            Time.timeScale = 0;
            DayCounter++;   
            nextDayPanel.SetActive(true);
            nextDayPanel.GetComponentInChildren<TMP_Text>().text = "Day\n" + DayCounter;
            yield return new WaitForSecondsRealtime(2);

            nextDayPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }
}