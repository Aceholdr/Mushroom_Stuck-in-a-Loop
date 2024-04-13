using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SunRotation : MonoBehaviour
    {
        [SerializeField] float rotationSpeed = 50f;

        void Update()
        {
            transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
        }
    }
}