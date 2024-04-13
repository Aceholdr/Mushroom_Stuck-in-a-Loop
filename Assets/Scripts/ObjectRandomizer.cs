using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectRandomizer : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var maxNum = transform.childCount;
            var displayedShroom = Random.Range(0, maxNum);

            transform.GetChild(displayedShroom).gameObject.SetActive(true);
        }
    }
}