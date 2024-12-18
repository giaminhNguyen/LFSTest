using UnityEngine;
using UnityEngine.Serialization;

namespace _GameAssets._Scripts.Data
{
    [CreateAssetMenu(fileName = "ObjectSimulationDataSO", menuName = "DataSO/ObjectSimulationDataSO", order = 0)]
    public class ObjectSimulationDataSO : ScriptableObject
    {
        public VapeAndPodSO       vapeAndPodSo;
        [FormerlySerializedAs("machineGunSO")]
        public GunSO gunSo;
        public GunSO   scifiGunSO;
        public LightSaberSO lightSaberSO;
        public BackgroundSO backgroundSO;
    }
}