using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] Transform camTransform;

        void Update()
        {
            Vector3 awayDirection = transform.position - camTransform.position;
            Quaternion awayRotation = Quaternion.LookRotation(awayDirection);
            transform.rotation = awayRotation;
        }
    }
}