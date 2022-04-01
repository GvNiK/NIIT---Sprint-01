using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettingsHolder : MonoBehaviour
{
    public PlayerSettings playerSettings;

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, playerSettings.shotCheckRadius);
    }
}
