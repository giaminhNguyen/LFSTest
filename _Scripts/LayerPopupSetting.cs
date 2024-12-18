using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityHelper;

namespace _GameAssets._Scripts
{
    public class LayerPopupSetting : LayerPopup
    {
        #region Properties

        private bool _isVibration;
        private bool _isFlash;
        private bool _isScreenFlash;
        private bool _isAudioSfx;
        private bool _isAudioMusic;
        
        [SerializeField]
        private Toggle _toggleMusic;
        [SerializeField]
        private Toggle _toggleSfx;
        [SerializeField]
        private Toggle _toggleVibrate;
        [SerializeField]
        private Toggle _toggleFlash;
        [SerializeField]
        private Toggle _toggleScreenFlash;

        [SerializeField]
        private GameObject _vibrateGObj;
        
        //
        [SerializeField]
        private Image _screenFlashImage;



        private Color     _colorScreenFlash = Color.white;
        private Coroutine _coroutineFlash;
        
        #endregion

        protected override void Awake()
        {
            base.Awake();
            _isVibration             = DataSaveGame.ActiveVibrate;
            _isFlash                 = DataSaveGame.ActiveFlash;
            _isScreenFlash           = DataSaveGame.ActiveScreenFlash;
            _toggleMusic.isOn       = DataSaveGame.ActiveMusic;
            _toggleSfx.isOn         = DataSaveGame.ActiveSFX;
            _toggleVibrate.isOn     = _isVibration;
            _toggleFlash.isOn       = _isFlash;
            _toggleScreenFlash.isOn = _isScreenFlash;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _toggleMusic.onValueChanged.AddListener(ActiveMusicEventToggle);
            _toggleSfx.onValueChanged.AddListener(ActiveSfxEventToggle);
            _toggleVibrate.onValueChanged.AddListener(ActiveVibrateEventToggle);
            _toggleFlash.onValueChanged.AddListener(ActiveFlashEventToggle);
            _toggleScreenFlash.onValueChanged.AddListener(ActiveScreenFlashEventToggle);
            EventManager.enableScreenFlash += EnableScreenFlash;
            EventManager.playFlash         += PlayFlash;
            EventManager.enableFlash       += EnableFlash;
            EventManager.playVibrate       += PlayVibrate;
            EventManager.enableVibrate     += EnableVibrate;
            EventManager.isVibration       += IsVibration;
            EventManager.isFlash           += IsFlash;
            EventManager.isScreenFlash     += IsScreenFlash;
            EventManager.isAudioSfx        += IsAudioSfx;
            EventManager.isAudioMusic      += IsAudioMusic;
        }
        
        private void OnDisable()
        {
            _toggleMusic.onValueChanged.RemoveListener(ActiveMusicEventToggle);
            _toggleSfx.onValueChanged.RemoveListener(ActiveSfxEventToggle);
            _toggleVibrate.onValueChanged.RemoveListener(ActiveVibrateEventToggle);
            _toggleFlash.onValueChanged.RemoveListener(ActiveFlashEventToggle);
            _toggleScreenFlash.onValueChanged.RemoveListener(ActiveScreenFlashEventToggle);
            EventManager.enableScreenFlash -= EnableScreenFlash;
            EventManager.playFlash         -= PlayFlash;
            EventManager.enableFlash       -= EnableFlash;
            EventManager.playVibrate       -= PlayVibrate;
            EventManager.enableVibrate     -= EnableVibrate;
            EventManager.isVibration       -= IsVibration;
            EventManager.isFlash           -= IsFlash;
            EventManager.isScreenFlash     -= IsScreenFlash;
            EventManager.isAudioSfx        -= IsAudioSfx;
            EventManager.isAudioMusic      -= IsAudioMusic;
        }

        private bool IsVibration()
        {
            return _isVibration;
        }

        private bool IsFlash()
        {
            return _isFlash;
        }

        private bool IsScreenFlash()
        {
            return _isScreenFlash;
        }

        private bool IsAudioSfx()
        {
            return _isAudioSfx;
        }

        private bool IsAudioMusic()
        {
            return _isAudioMusic;
        }

        private void ActiveMusicEventToggle(bool arg0)
        {
            DataSaveGame.ActiveMusic = arg0;
            this.PostEvent(EventID.ActiveMusic, arg0);
            PlaySfxButtonClick();
            _isAudioMusic = arg0;
        }

        private void ActiveSfxEventToggle(bool arg0)
        {
            DataSaveGame.ActiveSFX = arg0;
            this.PostEvent(EventID.ActiveSound, arg0);
            PlaySfxButtonClick();
            _isAudioSfx = arg0;
        }

        private void ActiveVibrateEventToggle(bool arg0)
        {
            DataSaveGame.ActiveVibrate = arg0;
            _isVibration                = arg0;
            PlaySfxButtonClick();
        }

        private void ActiveFlashEventToggle(bool arg0)
        {
            DataSaveGame.ActiveFlash = arg0;
            _isFlash = arg0;
            PlaySfxButtonClick();
        }

        private void ActiveScreenFlashEventToggle(bool arg0)
        {
            DataSaveGame.ActiveScreenFlash = arg0;
            _isScreenFlash                  = arg0;
            PlaySfxButtonClick();
        }

        private void PlaySfxButtonClick()
        {
            AudioManager.Instance.PlayOneShotSFX(KeySound.Button_Click);
        }

        private void EnableVibrate(bool obj)
        {
            if(!_isVibration) return;
            if (obj)
            {
            }
        }

        private void PlayVibrate()
        {
            if(!_isVibration) return;
        }

        private void EnableFlash(bool obj)
        {
            if(!_isFlash) return;
        }

        private void PlayFlash(int count)
        {
            if (_isFlash && count > 1)
            {
                count /= 2;
            }
            var counts = count * 2;
            if (_coroutineFlash != null)
            {
                StopCoroutine(_coroutineFlash);
            }
            _coroutineFlash = StartCoroutine( PlayEffectFlash(counts));
            // PlayEffectFlash1(counts);
        }

        private IEnumerator PlayEffectFlash(int count)
        {
            EnableVibrate(true);
            var isEnable = true;
            
            for (var i = 0; i < count; i++)
            {
                EnableFlash(isEnable);
                EnableScreenFlash(isEnable);

                if (isEnable)
                {
                    yield return null;
                    yield return null;
                }
                else
                {
                    yield return null;
                }
                
                isEnable = !isEnable;
            }
            EnableFlash(false);
            EnableScreenFlash(false);
            EnableVibrate(false);
        }
        
        private async void PlayEffectFlash1(int count)
        {
            if (_isFlash && count > 1)
            {
                count /= 2;
            }
            EnableVibrate(true);
            var isEnable = true;
            var counts   = count * 2;
            for (var i = 0; i < counts; i++)
            {
                EnableFlash(isEnable);
                EnableScreenFlash(isEnable);
                
                await Task.Yield();
                
                // if (isEnable)
                // {
                //     await Task.Yield();
                // }
                //
                isEnable = !isEnable;
            }
            EnableVibrate(false);
        }

        private void EnableScreenFlash(bool enable)
        {
            if(!_isScreenFlash) return;
            _colorScreenFlash.a = enable ? 0.47f : 0;
            _screenFlashImage.color = _colorScreenFlash;
        }
        
    }
}