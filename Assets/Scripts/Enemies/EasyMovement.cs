using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EasyMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1.0f;
    Vector3 originalPos;

    private void Start()
    {
        originalPos = transform.position;
        originalPos.y = 1.75f;
    }

    void Update()
    {
        transform.position = originalPos + (Vector3.up * math.sin(Time.time * movementSpeed));
    }
}
