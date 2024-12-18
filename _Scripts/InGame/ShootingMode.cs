using System;
using UnityEngine;

public class ShootingMode : MonoBehaviour
{
    [SerializeField]
    private GameObject _autoMode;
    [SerializeField]
    private GameObject _burstMode;
    
    private void Awake()
    {
        EventManager.selectedObjectIndex += selectedObjectIndex;
    }
    
    private void OnDisable()
    {
        EventManager.selectedObjectIndex -= selectedObjectIndex;
    }

    private void Start()
    {
        LoadMode(EventManager.getSelectedObjectIndex());
    }

    private void selectedObjectIndex(int obj)
    {
        LoadMode(obj);
    }

    private void LoadMode(int index)
    {
        switch (EventManager.getSelectedObjectType())
        {
            case ObjectSimulationType.MachineGun:
                var objMachineGun = DataGame.Instance.GetMachineData(index);
                SetUpMode(objMachineGun.burst,objMachineGun.auto);
                break;
            case ObjectSimulationType.ScifiGun:
                var objScifiGun = DataGame.Instance.GetScifiData(index);
                SetUpMode(objScifiGun.burst,objScifiGun.auto);
                break;
        }
    }

    private void SetUpMode(bool burstMode, bool autoMode)
    {
        if (_autoMode)
        {
            _autoMode.SetActive(autoMode);
        }
        if (_burstMode)
        {
            _burstMode.SetActive(burstMode);
        }
    }
}