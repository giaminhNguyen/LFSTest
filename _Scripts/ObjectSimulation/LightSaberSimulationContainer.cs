using UnityEngine;
using UnityHelper;

namespace _GameAssets._Scripts.ObjectSimulation
{
    public class LightSaberSimulationContainer : MonoBehaviour
    {
        #region Properties
    
        [SerializeField]
        private Transform _content;
    
        //Private
        private GameObject _currentObj;
        private bool       _hasDataGame;
    
        #endregion

        private void OnEnable()
        {
            EventManager.changeObjectSimulation += ChangeObjectSimulation;
        }

        private void OnDisable()
        {
            EventManager.changeObjectSimulation -= ChangeObjectSimulation;
        }

        private void Start()
        {
            _hasDataGame = DataGame.Instance;
            InstanceObjectSelected();
        }

        private void ChangeObjectSimulation()
        {
            InstanceObjectSelected();
        }

        private void InstanceObjectSelected()
        {
            if (_currentObj)
            {
                DestroyImmediate(_currentObj);
                _currentObj = null;
            }
        
            if(!_hasDataGame) return;
        
            var index = EventManager.getSelectedObjectIndex();
            var data  = DataGame.Instance.GetLightSaberData(index);
            if(data.Equals(default) || !data.prefab) return;
            _currentObj = Instantiate(data.prefab, _content);
            _currentObj.ResetLocalTransformation();

        }
    }
}