using System;
using UnityEngine;

public class LayerControllerInGame : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private GameObject _vapeAndPodLayer;
    [SerializeField]
    private GameObject _machineGunLayer;

    [SerializeField]
    private GameObject _scifiGunLayer;

    [SerializeField]
    private GameObject _lightSaberLayer;
    
    [Space(10)]
    [SerializeField]
    private Transform _content;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        GameObject layer = null;
        switch (EventManager.getSelectedObjectType())
        {
            case ObjectSimulationType.VapeAndPod:
                layer = _vapeAndPodLayer;
                break;
            case ObjectSimulationType.MachineGun:
                layer = _machineGunLayer;
                break;
            case ObjectSimulationType.ScifiGun: 
                layer = _scifiGunLayer;
                break;
            default:
                layer = _lightSaberLayer;
                break;
        }

        Instantiate(layer, _content);

    }
}