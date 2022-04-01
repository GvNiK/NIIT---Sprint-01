using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Player/Settings", order = 1)]
public class PlayerSettings : ScriptableObject
{
    [Header("Combat Settings")]
    public float shotCheckRadius = 15f;     //Default: 15
    public float shotAngleRadius = 60f;     //Default: 60
    public float playerMaxHP = 100f;

     public float swordDamage = 10f;

    [Header("Another Setting")]
    [Range(0f, 15f)]
    public float rangeSetting;

} 