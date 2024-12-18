using System.Threading.Tasks;
using UnityEngine;
using UnityHelper;
using Random = UnityEngine.Random;

namespace _GameAssets._Scripts.ObjectSimulation
{
    public class VapeAndPodSimulationContainer : MonoBehaviour
    {
        #region Properties

        [SerializeField]
        private float _delayReload;
        
        [SerializeField]
        private Transform _tipPosition;
    
        [SerializeField]
        private Transform _tankPosition;
    
        [SerializeField]
        private Transform _vapePosition;
        
        [SerializeField]
        private Material _juiceMaterial;

        [SerializeField]
        private float _lungCapacityTime = 5;

        [SerializeField]
        private float _warningTime = 1;

        [SerializeField]
        private float _juiceCapacityTime = 20;

        [SerializeField]
        private int _flashEffectCount = 6;
        
        [SerializeField]
        private GameObject _smokeEffectGObj;
        
        //
        private int   _currentTipIndex;
        private int   _currentTankIndex;
        private int   _currentVapeIndex;
        private int   _currentJuiceIndex = -1;
        private float _lungTimeFake;
        private float _lungTimeFakeDelta;
        private float _juiceTimeDelta;
        private bool  _isWarming;
        private bool  _lungActive;
        private bool  _juiceActive;
        private int   _tipLength;
        private bool _isRotate;
        private bool _curSmokingState;
        private bool _nextSmokingState;
        private bool _isBreathless;
        //
        private GameObject _currentTip;
        private GameObject _currentTank;
        private GameObject _currentVape;
        
        // KEY
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    
        #endregion

        private void OnEnable()
        {
            _smokeEffectGObj.SetActive(true);
            _lungTimeFake                       =  _lungCapacityTime - _warningTime;
            _lungTimeFakeDelta                  =  _lungTimeFake;
            _juiceTimeDelta                     =  _juiceCapacityTime;
            _lungActive                         =  true;
            _juiceActive                        =  true;
            EventManager.changeObjectSimulation += ChangeObjectSimulation;
            EventManager.changeTank             += ChangeTank;
            EventManager.changeJuice            += ChangeJuice;
            EventDispatcher.Instance.RegisterListener(EventID.Reload,OnReload);
            EventDispatcher.Instance.RegisterListener(EventID.FinishUpdateCapacity,FinishUpdateCapacity);
            EventDispatcher.Instance.RegisterListener(EventID.FinishUpdateCapacityJuice,FinishUpdateCapacityJuice);
            EventDispatcher.Instance.RegisterListener(EventID.RotateMode,OnRotateMode);
            EventDispatcher.Instance.RegisterListener(EventID.DefaultMode,OnDefaultMode);
        }

        private void OnDisable()
        {
            EventManager.changeObjectSimulation -= ChangeObjectSimulation;
            EventManager.changeTank             -= ChangeTank;
            EventManager.changeJuice            -= ChangeJuice;
            EventDispatcher.Instance.RemoveListener(EventID.Reload,OnReload);
            EventDispatcher.Instance.RemoveListener(EventID.FinishUpdateCapacity,FinishUpdateCapacity);
            EventDispatcher.Instance.RemoveListener(EventID.FinishUpdateCapacityJuice,FinishUpdateCapacityJuice);
            EventDispatcher.Instance.RemoveListener(EventID.RotateMode,OnRotateMode);
            EventDispatcher.Instance.RemoveListener(EventID.DefaultMode,OnDefaultMode);
        }

        private void OnDefaultMode(object obj)
        {
            _isRotate = false;
        }

        private void OnRotateMode(object obj)
        {
            _isRotate = true;
        }


        private void Start()
        {
            _currentVapeIndex = EventManager.getSelectedObjectIndex();

            if (DataGame.Instance)
            {
                _tipLength         = DataGame.Instance.DripTips.Length;   
                var vapeData = DataGame.Instance.GetVapeData(_currentVapeIndex);
                _currentTankIndex = vapeData?.defaultTank ?? 0;
            }

            BuildVapeAndPod();
            PostEventUpdateLung(noAnim:true);
            PostEventUpdateJuice(noAnim:true);
        }

        private void Update()
        {
            UpdateSmokingState();
            UpdateLungAndJuice();
        }

        private void UpdateSmokingState()
        {
            _nextSmokingState = EventManager.onMouseInteract() && _lungActive && _juiceActive && !_isRotate && !_isBreathless;
            if (_curSmokingState == _nextSmokingState) return;
            _curSmokingState = _nextSmokingState;
            if (_curSmokingState)
            {
                PlayAudioVape();
            }
            else
            {
                PlayAudioVape(2);
                PlayEffect();
            }

        }

        private async void PlayAudioVape()
        {
            PlayAudioVape(0);
            await Task.Yield();
            PlayAudioVape(1);
        }

        private void PlayAudioVape(int vape)
        {
            switch (vape)
            {
                case -1:
                    AudioManager.Instance.StopSFX();
                    break;
                case 0:
                    AudioManager.Instance.PlayOneShotSFX(KeySound.VapeStart);
                    break;
                case 1:
                    AudioManager.Instance.PlaySFX(KeySound.VapingLoop,true);
                    break;
                case 2:
                    PlayAudioVape(-1);
                    AudioManager.Instance.PlayOneShotSFX(KeySound.VapeStop);
                    break;
                case 3:
                    AudioManager.Instance.PlaySFX(KeySound.VapeRefill,true);
                    break;
            }
        }

        private void UpdateLungAndJuice()
        {
            if (_curSmokingState)
            {
                _lungTimeFakeDelta -= Time.deltaTime;
                _juiceTimeDelta    -= Time.deltaTime;
                PostEventUpdateJuice();
                if (_juiceTimeDelta <= 0)
                {
                    _juiceActive    = false;
                    EventDispatcher.Instance.PostEvent(EventID.NeedReload);
                }

                if (_lungTimeFakeDelta + _warningTime <= 0)
                {
                    _isBreathless = true;
                }
                else
                {
                    PostEventUpdateLung();
                }
            }
            else if(_lungActive && (_lungTimeFakeDelta - _lungTimeFake != 0))
            {
                _lungTimeFakeDelta = _lungTimeFake;
                _lungActive        = false;
                PostEventUpdateLung(true);
            }
        }

        private void PlayEffect()
        {
            if (_lungTimeFakeDelta + _warningTime <= 0)
            {
                EventManager.playSmokeEffectFail?.Invoke();
                AudioManager.Instance.PlayOneShotSFX(KeySound.Coughing_Female_2);
            }else
            {
                var ratio = _lungTimeFakeDelta / _lungTimeFake;

                if (ratio < 0.9f)
                {
                    EventManager.playSmokeEffect15.Invoke();
                    AudioManager.Instance.PlayOneShotSFX(KeySound.Blow15);
                    EventManager.playFlash?.Invoke((int)(_flashEffectCount/1.5f));
                }
                else if(ratio < 0.5f)
                {
                    EventManager.playSmokeEffect25.Invoke();
                    AudioManager.Instance.PlayOneShotSFX(KeySound.Blow25);
                    EventManager.playFlash?.Invoke(_flashEffectCount);
                }
                
            }
            

        }

        private void PostEventUpdateLung(bool noti = false,bool noAnim = false)
        {
            var     progress = Mathf.Clamp01(_lungTimeFakeDelta / _lungTimeFake);
            EventID id;

            if (noAnim)
            {
                id = EventID.UpdateEnergyNoAnim;
            }
            else
            {
                id = noti ? EventID.UpdateEnergyNoti : EventID.UpdateEnergy;
            }
        
            EventDispatcher.Instance.PostEvent( id,progress);
        
            switch (progress)
            {
                case 0 when !_isWarming:
                    _isWarming = true;
                    EventDispatcher.Instance.PostEvent(EventID.PlayWarning,_isWarming);

                    break;
                case > 0 when _isWarming:
                    _isWarming = false;
                    EventDispatcher.Instance.PostEvent(EventID.PlayWarning,_isWarming);

                    break;
            }
        }

        private void PostEventUpdateJuice(bool noti = false,bool noAnim = false)
        {
            var     progress = Mathf.Clamp01(_juiceTimeDelta / _juiceCapacityTime);
            EventID id;
            if (noAnim)
            {
                id = EventID.UpdateJuiceNoAnim;
            }
            else
            {
                id = noti ? EventID.UpdateJuiceNoti : EventID.UpdateJuice;
            }
        
            EventDispatcher.Instance.PostEvent(id,progress);
        }

        //Event {
    
        private void FinishUpdateCapacity(object obj)
        {
            _lungActive   = true;
            _isBreathless = false;
        }

        private void FinishUpdateCapacityJuice(object obj)
        {
            _juiceActive = true;
            PlayAudioVape(-1);
        }

        private async void OnReload(object obj)
        {
            await Task.Delay((int)(_delayReload * 1000));
            _juiceTimeDelta = _juiceCapacityTime;
            PlayAudioVape(3);
            PostEventUpdateJuice(true);
        }
    
        private void ChangeTank()
        {
            var index = EventManager.getSelectedTankIndex();
            if (index == _currentTankIndex) return;
            _currentTankIndex = index;
            _juiceTimeDelta   = _juiceCapacityTime;
            BuildTank();
            BuildRandomTip();
            PostEventUpdateJuice(noAnim:true);
        }
    
        private void ChangeJuice()
        {
            _currentJuiceIndex = EventManager.getSelectedJuiceIndex();
            BuildJuice();
        }

        private void ChangeObjectSimulation()
        {
            _juiceTimeDelta   = _juiceCapacityTime;
            _currentVapeIndex = EventManager.getSelectedObjectIndex();
            BuildBox();
            BuildRandomTip();
            PostEventUpdateJuice(noAnim:true);
        }
    
        // Event }
    
        // Build {

        private void BuildVapeAndPod()
        {
            BuildRandomTip();
            BuildTank();
            BuildJuice(true);
            BuildBox();
        }
    
        private void BuildTip()
        {
            if(!DataGame.Instance) return;
            if(_currentTip) Destroy(_currentTip);
            var tips = DataGame.Instance.DripTips;
            _currentTip = Instantiate(tips[_currentTipIndex].prefab, _tipPosition);
            _currentTip.ResetLocalTransformation();
        }

        private void BuildTank()
        {
            if(!DataGame.Instance) return;
            if(_currentTank) Destroy(_currentTank);
            var tankData = DataGame.Instance.GetTank(_currentTankIndex);
            _currentTank = Instantiate(tankData.prefab, _tankPosition);
            _currentTank.ResetLocalTransformation();
        }
    
        private void BuildJuice(bool isDefault = false)
        {
            if(!DataGame.Instance) return;
            Color color;

            if (isDefault)
            {
                color = DataGame.Instance.JuiceDefaultColor;
            }
            else
            {
                var juiceData = DataGame.Instance.Juices[_currentJuiceIndex];
                color = juiceData.color;
            }
            _juiceMaterial.SetColor(BaseColor, color);
        }
    
        private void BuildBox()
        {
            if(!DataGame.Instance) return;
            if(_currentVape) Destroy(_currentVape);
            var vapeData = DataGame.Instance.GetVapeData(_currentVapeIndex);
            _currentVape = Instantiate(vapeData.prefab, _vapePosition);
            _currentVape.ResetLocalTransformation();
        }
        private void BuildRandomTip()
        {
            if(!DataGame.Instance || _tipLength == 0) return;
        
            var index = Random.Range(0, _tipLength);
            if (_tipLength > 1 && index == _currentTipIndex)
            {
                BuildRandomTip();
                return;
            }

            _currentTipIndex = index;
            BuildTip();
        }
        // Build }
    
    }
}