using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
        [SerializeField] GameObject house;

        [SerializeField] GameObject instructionPanel;
        [SerializeField] GameObject instructionMenuFirst;

        [SerializeField] GameObject resultsPanel;
        [SerializeField] GameObject resultsPanelFirst;

        [SerializeField] TMP_Text gameOverText;
        [SerializeField] GameObject gameOverMenuFirst;
        [SerializeField] Transform[] spawnPoints;
        [SerializeField] GameObject enemy;
        [SerializeField] GameObject player;

        [SerializeField] int treeCalculator = 80;
        [SerializeField] int shroomCalculator = 1;

        [SerializeField] float spawnTimeDecrease = 0.1f;
        [SerializeField] float gameStartTime = 4f;
        [SerializeField] float currentSpawnTime;

        public static bool IsFirstTime { get; private set; } = true;

        public static bool IsUltimateLifeForm { get; set; }
        public static float StartSpawnTime { get; set; }
        public static int ShroomCount { get; set; }
        public static int DayCounter { get; set; }
        [SerializeField] public static int LastWaveCounter { get; set; }
        public static bool IsGameOver { get; set; }
        public bool IsFinished { get; private set; }

        private int lastShroomCount;
        [SerializeField] private int waveCount;

        void Start()
        {
            Time.timeScale = 0;

            if (StartSpawnTime <= 0)
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
            SpawnShrooms();
        }

        IEnumerator SpawnEnemy()
        {
            for(int i = 0; i < spawnPoints.Length; i++)
            {
                if (!IsVisible(spawnPoints[i].position, new Vector3(1,1,1), Camera.main))
                {
                    if(currentSpawnTime <= 30)
                    {
                        Instantiate(enemy, spawnPoints[i].position, enemy.transform.rotation);

                        if (IsUltimateLifeForm)
                        {
                            LastWaveCounter++;
                        }
                    }
                }
            }

            if (IsUltimateLifeForm)
            {
                yield return new WaitForSeconds(spawnTimeDecrease);
                spawnTimeDecrease += 0.01f;
            }
            else
            {
                yield return new WaitForSeconds(currentSpawnTime);
            }


            if(currentSpawnTime > 0.1f && !IsUltimateLifeForm)
            {
                currentSpawnTime -= spawnTimeDecrease;
            }
            else if (IsUltimateLifeForm)
            {
                currentSpawnTime += spawnTimeDecrease;
            }

            if(LastWaveCounter <= 0 && IsUltimateLifeForm)
            {
                IsFinished = true;
            }
            else if (spawnTimeDecrease <= 3f)
            {
                StartCoroutine(SpawnEnemy());
            }
        }

        bool IsVisible(Vector3 pos, Vector3 boundSize, Camera camera)
        {
            var bounds = new Bounds(pos, boundSize);
            var planes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(planes, bounds);
        }

        void Update()
        {
            waveCount = LastWaveCounter;

            if (ShroomCount != lastShroomCount)
            {
                shroomCountText.text = "Shrooms: " + ShroomCount;
                lastShroomCount = ShroomCount;
            }

            if (IsGameOver)
            {
                Time.timeScale = 0;
                gameOverPanel.SetActive(true);
                gameOverText.text = "You can not flee from the eternal loop.\r\nCollected mushrooms: " + ShroomCount;

                if (EventSystem.current.currentSelectedGameObject == null)
                {
                    EventSystem.current.SetSelectedGameObject(gameOverMenuFirst);
                }
            }

            if (IsFinished)
            {
                Time.timeScale = 0;
                resultsPanel.SetActive(true);
                if (EventSystem.current.currentSelectedGameObject == null)
                {
                    EventSystem.current.SetSelectedGameObject(resultsPanelFirst);
                }
            }

            if (IsFirstTime && EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(instructionMenuFirst);
            }
        }

        private void SpawnShrooms()
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
            IsFinished = false;
            StartSpawnTime = gameStartTime;
            IsUltimateLifeForm = false;
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
            if (IsFirstTime)
            {
                instructionPanel.SetActive(true);
            }

            yield return new WaitWhile(() => IsFirstTime);

            DayCounter++;   
            nextDayPanel.SetActive(true);

            if (IsUltimateLifeForm)
            {
                nextDayPanel.GetComponentInChildren<TMP_Text>().text = "Final Day";
                house.SetActive(false);
                currentSpawnTime = 0.1f;
            }
            else
            {
                nextDayPanel.GetComponentInChildren<TMP_Text>().text = "Day\n" + DayCounter;
            }
            
            yield return new WaitForSecondsRealtime(2);
            nextDayPanel.SetActive(false);

            Time.timeScale = 1;
        }

        public void OnStartNewGame()
        {
            StartSpawnTime = gameStartTime;
            Time.timeScale = 1;
            IsFirstTime = false;
            instructionPanel.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}