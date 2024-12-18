using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityHelper;
using VInspector;

namespace _GameAssets._Scripts.ObjectSimulation
{
    public class LightSaberSimulation : ObjectSimulationBase
    {
        
        #region Properties

        [SerializeField]
        private LightSaberInfo[] _lightSaber;

        [SerializeField]
        private Material[] _saberMaterials;

        [SerializeField]
        private Material[] _lightningMaterials;
    
        [SerializeField]
        private float _activationTime = 0.25f;

        [SerializeField]
        private float _totalUsageTime = 8f;
    
        //Private
        private float          _progress;
        private LightSaberData _lightSaberData;
        private float          _energy;
        private bool           _isPlayVFX;
        private bool           _active;
        private bool           _isSliderRGB;
        private bool           _isRotaMode;
        private float          _audioState;
        
        //KEY
        private static readonly int BaseColor     = Shader.PropertyToID("_BaseColor");
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    
        #endregion

        #region Implement

        protected override void OnEnable()
        {
            base.OnEnable();
            _isSliderRGB = false;
            _progress    = 0;
            foreach (var lightSaber in _lightSaber)
            {
                UpdateLightSaber(lightSaber,_progress);
            }
            //
            EventDispatcher.Instance.RegisterListener(EventID.Reload,OnReload);
            EventDispatcher.Instance.RegisterListener(EventID.FinishUpdateCapacity,FinishUpdateCapacity);
            EventDispatcher.Instance.RegisterListener(EventID.SliderRGB,ActionSliderRGB);
            EventDispatcher.Instance.RegisterListener(EventID.RotateMode,ActionRotaMode);
            EventDispatcher.Instance.RegisterListener(EventID.DefaultMode,ActionDefaultMode);
            EventManager.onChangeRGBSlider += UpdateColor;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventDispatcher.Instance.RemoveListener(EventID.Reload,OnReload);
            EventDispatcher.Instance.RemoveListener(EventID.FinishUpdateCapacity,FinishUpdateCapacity);
            EventManager.onChangeRGBSlider -= UpdateColor;
            EventDispatcher.Instance.RegisterListener(EventID.SliderRGB,ActionSliderRGB);
            EventDispatcher.Instance.RegisterListener(EventID.SliderRGB,ActionRotaMode);
            EventDispatcher.Instance.RegisterListener(EventID.DefaultMode,ActionDefaultMode);
        }

        protected override void Start()
        {
            base.Start();
            UpdateEnergy(_energy,noAnim:true);
        }

        protected override void Update()
        {
            base.Update();
            UpdateLightSaber();
        }

        #endregion
        
        private void ActionSliderRGB(object obj)
        {
            _isSliderRGB = (bool)obj;
        }
        
        private void ActionDefaultMode(object obj)
        {
            _isRotaMode = false;

            if (_isSliderRGB)
            {
                EnableVFXLightning(true);
                _audioState = 1;
            }
        }

        private void ActionRotaMode(object obj)
        {
            _isRotaMode = true;
            EnableVFXLightning(false);

            if (_audioState > 0)
            {
                _audioState = 0;
                AudioManager.Instance.StopSFX();
            }
        }

        private void EnableVFXLightning(bool onActive)
        {
            if(_isPlayVFX == onActive) return;
            _isPlayVFX = onActive;
            EventDispatcher.Instance.PostEvent(EventID.PlayVFXLightning,_isPlayVFX);
        }
    
        private void UpdateLightSaber()
        {
            float nav = 0;

            if (EventManager.infinityEnergy())
            {
                _energy = 1;
            }
            
            var check = (EventManager.onMouseInteract() || _isRotaMode || _isSliderRGB) && _energy > 0 && _active;
            
            if (check)
            {
                nav = 1;
                if(_audioState - 1 == 0)
                {
                    AudioManager.Instance.PlaySFX(KeySound.Saber_Loop);
                    _audioState = 2;
                }
                if (!_isRotaMode)
                {
                    if (_audioState == 0)
                    {
                        EventManager.enableFlash?.Invoke(true);
                        AudioManager.Instance.PlaySFX(KeySound.Saber_On);
                        _audioState = 1;
                    }
                    EnableVFXLightning(true);
                    _energy = Mathf.Clamp01(_energy - Time.deltaTime / _totalUsageTime);
                }

                if (_energy == 0)
                {
                    _active = false;
                    EventDispatcher.Instance.PostEvent(EventID.NeedReload);
                }
                UpdateEnergy(_energy);
                if(_progress >= 1) return;
                
            }
            else
            {
                if(_progress <= 0) return;
                EnableVFXLightning(false);
                nav = -1;

                if (_audioState > 0)
                {
                    AudioManager.Instance.PlaySFX(KeySound.Saber_Off);
                    EventManager.enableFlash?.Invoke(false);
                    _audioState = 0;
                }
                
            }
            
            _progress = Mathf.Clamp01(_progress + nav *  Time.deltaTime / _activationTime);

            foreach (var lightSaber in _lightSaber)
            {
                UpdateLightSaber(lightSaber,_progress);
            }
        
        }

        private void UpdateLightSaber(LightSaberInfo lightSaber, float progress)
        {
            Vector3 vt;

            switch (lightSaber.axisScale)
            {
                case Axis.X: 
                    vt = new(progress, 1, 1);
                    break;
                case Axis.Y: 
                    vt = new(1, progress, 1);
                    break;
                default: 
                    vt = new(1, 1, progress);
                    break;
            }

            lightSaber.tf.localScale = vt;

        }
        
        protected override void GetObjectBase()
        {
            if(!_hasData) return;
            _lightSaberData = DataGame.Instance.GetLightSaberData(EventManager.getSelectedObjectIndex());
            _capacity       = 1;
            _energy         = 1;
            _active         = true;
            EventManager.onUpdateRGBSlider?.Invoke(_lightSaberData.defaultColor);
            UpdateColor(_lightSaberData.defaultColor);
        }
    
        private void FinishUpdateCapacity(object obj)
        {
            _active = true;
        }

        private async void OnReload(object obj)
        {
            await Task.Delay((int)(_delayReload * 1000));
            OnReload();
        }

        public override void OnReload()
        {
            _energy = _capacity;
            UpdateEnergy(_energy,true);
        }

        private void UpdateColor(Color color)
        {
            foreach (var saberLight in _saberMaterials)
            {
                // saberLight.SetColor(BaseColor,color);
                saberLight.SetColor(EmissionColor,color * Mathf.Pow(3.3f,2));
            }

            foreach (var lightning in _lightningMaterials)
            {
                lightning.SetColor(BaseColor,color);
            }
        
        }
    
        //struct
        [Serializable]
        private struct LightSaberInfo
        {
            public Transform tf;
            public Axis      axisScale;
        }
    }
}
