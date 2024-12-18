using System;
using UnityEngine;
using UnityHelper;

public class LayerControllerInMenu : MonoBehaviour
{

    #region Properties

    [Header("Reference")]
    
    [SerializeField]
    private LayerBase _typeObjectMenu;
    
    [SerializeField]
    private LayerBase _objectMenu;
    
    [SerializeField]
    private LayerBase _changeHumanLayer;
    
    #endregion
    
    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.GoToObjectMenu, GoToObjectMenu);
        EventDispatcher.Instance.RegisterListener(EventID.GoToTypeObjectMenu, GoToTypeObjectMenu);
        EventDispatcher.Instance.RegisterListener(EventID.GoToChangeHumanLayer, GoToChangeHumanLayer);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.GoToObjectMenu, GoToObjectMenu);
        EventDispatcher.Instance.RemoveListener(EventID.GoToTypeObjectMenu, GoToTypeObjectMenu);
        EventDispatcher.Instance.RemoveListener(EventID.GoToChangeHumanLayer, GoToChangeHumanLayer);
    }

    private void GoToChangeHumanLayer(object obj)
    {
        _changeHumanLayer.Init();
        OpenLayer(_changeHumanLayer.Open);
    }

    private void GoToTypeObjectMenu(object obj)
    {
        _typeObjectMenu.Init();
        OpenLayer(_typeObjectMenu.Open);
    }

    private void GoToObjectMenu(object obj)
    {
        if(obj == null) return;
        var type = (ObjectSimulationType)obj;
        EventManager.selectedObjectType?.Invoke(type);
        _objectMenu.Init();
        OpenLayer(_objectMenu.Open);
    }
    
    private void OpenLayer(Action action)
    {
        EventManager.loadLayer?.Invoke(CloseAllLayer, action);
    }
    
    private void CloseAllLayer()
    {
        _typeObjectMenu.Close();
        _objectMenu.Close();
        _changeHumanLayer.Close();
    }
}