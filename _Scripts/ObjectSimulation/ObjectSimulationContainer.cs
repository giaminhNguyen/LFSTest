using UnityEngine;
using UnityHelper;

namespace _GameAssets._Scripts.ObjectSimulation
{
    public class ObjectSimulationContainer : MonoBehaviour
    {
        #region Properties

        [Space(3)]
        [SerializeField]
        private VapeAndPodSimulationContainer _vapeAndPodSimulationContainer;
        [SerializeField]
        private GunSimulationContainer _gunSimulationContainer;
        [SerializeField]
        private LightSaberSimulationContainer _lightSaberSimulationContainer;
    
        #endregion
    
    
        private void Start()
        {
            var objectType = EventManager.getSelectedObjectType();
            switch (objectType)
            {
                case ObjectSimulationType.VapeAndPod:
                    _vapeAndPodSimulationContainer.enabled = true;
                    EventDispatcher.Instance.PostEvent(EventID.SetBloomIntensityLow);
                    break;
                case ObjectSimulationType.MachineGun:
                    _gunSimulationContainer.enabled = true;
                    EventDispatcher.Instance.PostEvent(EventID.SetBloomIntensityLow);
                    break;
                case ObjectSimulationType.ScifiGun: 
                    _gunSimulationContainer.enabled = true;
                    EventDispatcher.Instance.PostEvent(EventID.SetBloomIntensityLow);
                    break;
                case ObjectSimulationType.LightSaber: 
                    _lightSaberSimulationContainer.enabled = true;
                    EventDispatcher.Instance.PostEvent(EventID.SetBloomIntensityHigh);
                    break;
            }
            
            enabled = false;
        }
    }
}