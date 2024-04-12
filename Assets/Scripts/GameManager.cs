using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] TMP_Text shroomCountText;
        [SerializeField] GameObject gameOverPanel;
        [SerializeField] GameObject shroom;

        public static int ShroomCount { get; set; }
        private int lastShroomCount;

        void Start()
        {

            shroomCountText.text = "Shrooms: 0";
            SpawnShrooms(10);
        }

        void Update()
        {
            if(ShroomCount != lastShroomCount)
            {
                shroomCountText.text = "Shrooms: " + ShroomCount;
                lastShroomCount = ShroomCount;
            }
        }

        private void SpawnShrooms(int numOfShrooms)
        {
            for(int i = 0; i < numOfShrooms; i++)
            {
                Instantiate(shroom, new Vector3(Random.Range(-50, 51), 0, Random.Range(-50, 51)), shroom.transform.rotation);
            }
        }
    }
}