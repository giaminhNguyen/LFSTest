using DG.Tweening;
using UnityEngine;
using UnityHelper;

namespace _GameAssets._Scripts
{
    [RequireComponent(typeof(AudioAction))]
    public class BulletDropCtrl : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        private ParticleSystem _bulletDropParticle;
        [SerializeField]
        private float _delayAudio;
        [SerializeField]
        private AudioAction _audioAction;
        #endregion

        private void OnValidate()
        {
            _audioAction = GetComponent<AudioAction>();
        }

        private void OnEnable()
        {
            EventDispatcher.Instance.RegisterListener(EventID.Shooting,BulletDrop);
        }
        
        private void OnDisable()
        {
            EventDispatcher.Instance.RemoveListener(EventID.Shooting,BulletDrop);
        }

        private void BulletDrop(object obj)
        {
            _bulletDropParticle.Emit(1);
            DOVirtual.DelayedCall(_delayAudio, () =>
            {
                _audioAction.Play();
            });
        }
    }
}