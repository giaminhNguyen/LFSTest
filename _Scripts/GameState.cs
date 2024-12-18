using UnityEngine;
using UnityHelper;
using VInspector;

namespace _GameAssets._Scripts
{
    public class GameState : Singleton<GameState>
    {
        #region Properties
        private int                  _objectSelectedIndex;
        private ObjectSimulationType _objectSimulationType;
        private int                  _humanSelectedIndex;
        //Vape And Pod
        private int                  _tankSelectedIndex;
        private int                  _juiceSelectedIndex;
        #endregion
        
        public int objectSelectedIndex;
        public ObjectSimulationType objectSimulationType;
        public int humanSelectedIndex;
        [Header("Vape And Pod")]
        public int tankSelectedIndex;

        [Header("Hack")]
        public bool infinityEnergy;
        public override void Awake()
        {
            _objectSelectedIndex = objectSelectedIndex;
            _objectSimulationType = objectSimulationType;
            _tankSelectedIndex = tankSelectedIndex;
            _humanSelectedIndex = humanSelectedIndex;
            //HACK

            EventManager.infinityEnergy += InfinityEnergy;
            
            //
            EventManager.selectedObjectIndex    += SetObjectSelectedIndex;
            EventManager.selectedObjectType     += SetObjectSimulationType;
            EventManager.getSelectedObjectIndex += GetObjectSelectedIndex;
            EventManager.getSelectedObjectType  += SetObjectSimulationType;
            EventManager.getSelectedHumanIndex  += GetHumanSelectedIndex;
            EventManager.selectedHumanIndex     += SetHumanSelectedIndex;
            //VApe And Pod
            EventManager.selectedTankIndex    += SetTankSelectedIndex;
            EventManager.getSelectedTankIndex += GetTankSelectedIndex;
            EventManager.selectedJuiceIndex   += SetJuiceSelectedIndex;
            EventManager.getSelectedJuiceIndex += GetJuiceSelectedIndex;
        }

        

        private void OnDestroy()
        {
            //HACK

            EventManager.infinityEnergy -= InfinityEnergy;
            
            //
            
            EventManager.selectedObjectIndex    -= SetObjectSelectedIndex;
            EventManager.selectedObjectType     -= SetObjectSimulationType;
            EventManager.getSelectedObjectIndex -= GetObjectSelectedIndex;
            EventManager.getSelectedObjectType  -= SetObjectSimulationType;
            EventManager.getSelectedHumanIndex  -= GetHumanSelectedIndex;
            EventManager.selectedHumanIndex    -= SetHumanSelectedIndex;
            //Vape And Pod
            EventManager.selectedTankIndex    -= SetTankSelectedIndex;
            EventManager.getSelectedTankIndex -= GetTankSelectedIndex;
            EventManager.selectedJuiceIndex   -= SetJuiceSelectedIndex;
            EventManager.getSelectedJuiceIndex -= GetJuiceSelectedIndex;
        }
        

        #region VapeAndPod Func

        private void SetTankSelectedIndex(int obj)
        {
            if(_tankSelectedIndex == obj) return;
            _tankSelectedIndex = obj;
            tankSelectedIndex  = obj;
            EventManager.changeTank?.Invoke();
            EventDispatcher.Instance.PostEvent(EventID.ChangeTank);
        }
        
        private int GetTankSelectedIndex()
        {
            return _tankSelectedIndex;
        }
        
        private void SetJuiceSelectedIndex(int obj)
        {
            if(_juiceSelectedIndex == obj) return;
            _juiceSelectedIndex = obj;
            EventManager.changeJuice?.Invoke();
            EventDispatcher.Instance.PostEvent(EventID.ChangeJuice);
        }
        
        private int GetJuiceSelectedIndex()
        {
            return _juiceSelectedIndex;
        }

        #endregion

        #region Hack

        public bool InfinityEnergy()
        {
            return infinityEnergy;
        }

        #endregion


        private void SetObjectSimulationType(ObjectSimulationType obj)
        {
            _objectSimulationType = obj;
            objectSimulationType  = obj;
        }

        private void SetObjectSelectedIndex(int obj)
        {
            if(_objectSelectedIndex == obj) return;
            _objectSelectedIndex = obj;
            objectSelectedIndex  = obj;
            EventManager.changeObjectSimulation?.Invoke();
            EventDispatcher.Instance.PostEvent(EventID.ChangeObjectSimulation);
        }

        private ObjectSimulationType SetObjectSimulationType()
        {
            return _objectSimulationType;
        }

        private int GetObjectSelectedIndex()
        {
            return _objectSelectedIndex;
        }
        
        private int GetHumanSelectedIndex()
        {
            return _humanSelectedIndex;
        }
        
        private void SetHumanSelectedIndex(int obj)
        {
            if(_humanSelectedIndex == obj) return;
            _humanSelectedIndex = obj;
            humanSelectedIndex  = obj;
            EventDispatcher.Instance.PostEvent(EventID.SelectedHuman);
        }
        
    }
}