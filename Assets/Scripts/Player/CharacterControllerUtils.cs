using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public static class CharacterControllerUtils
{
    public static Vector3 GetNormalWithSphereCast(CharacterController controller, out bool itHit,
        Vector3 defaultNormal, Vector3 rayDirection, LayerMask layerMask = default)
    {
        // same as normal, except returns a bool of the Raycast hitting (or not)

        Vector3 normal = defaultNormal;
        Vector3 center = controller.transform.position + controller.center;
        float distance = controller.height / 2f + controller.stepOffset + 0.01f;

        RaycastHit hit;

        itHit = Physics.SphereCast(center, controller.radius, rayDirection, out hit, distance, layerMask);
        if (itHit)
        {
            normal = hit.normal;
        }

        return normal;
    }

    public static Vector3 GetNormalWithSphereCast(CharacterController controller, Vector3 defaultNormal, Vector3 rayDirection, LayerMask layerMask = default)
    { 
        Vector3 normal = defaultNormal;
        Vector3 center = controller.transform.position + controller.center;
        float distance = controller.height / 2f + controller.stepOffset + 0.01f;

        RaycastHit hit;
        if (Physics.SphereCast(center, controller.radius, rayDirection, out hit, distance, layerMask))
        { 
            normal = hit.normal;
        }
         
        return normal;
    }

    public static Vector3 GetNormalWithSphereCast(CharacterController controller, LayerMask layerMask = default)
    {
        return GetNormalWithSphereCast(controller, Vector3.up, Vector3.down, layerMask);
    }
}
