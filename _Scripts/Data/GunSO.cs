using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "MachineGunSO", menuName = "DataSO/MachineGunSO")]
public class GunSO : ScriptableObject
{
    [FormerlySerializedAs("machineData")]
    public GunData[] gunData;
}

[Serializable]
public class GunData : DataBase
{
    public GameObject prefab;
    [Header("Info")]
    public int   capacity;
    public int   burstCount;
    public float burstCooldown;
    public float cooldown;
    [Header("Mode")]
    public bool auto;
    public bool burst;
}