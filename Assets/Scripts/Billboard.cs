using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    [SerializeField] float offsetY = 0f;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, Camera.main.transform.eulerAngles.y + offsetY, transform.rotation.z));
        transform.position = followTarget.position;
    }
}
