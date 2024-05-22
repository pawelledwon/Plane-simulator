using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform plane;
    float storedShadowDistance;

    private void LateUpdate()
    {
        if (plane == null)
            return;
        Vector3 newPosition = plane.position + Vector3.up * 10f;

        // Update position
        transform.position = newPosition;

        // Keep the minimap camera always facing downwards
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }


    private void OnPreRender()
    {
        storedShadowDistance = QualitySettings.shadowDistance;
        QualitySettings.shadowDistance = 0;
    }

    private void OnPostRender()
    {
        QualitySettings.shadowDistance = storedShadowDistance;
    }
}
