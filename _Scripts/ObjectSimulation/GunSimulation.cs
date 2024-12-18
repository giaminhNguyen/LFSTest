using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityHelper;

namespace _GameAssets._Scripts.ObjectSimulation
{
    public class GunSimulation : ObjectSimulationBase
    {
        #region Properties
        
        [SerializeField]
        private Animator _animator;
        
        [SerializeField]
        private float _delayPlaySmokeEffect = 0.4f;
        //
        private readonly string      _shootingNameAnimation = "Shoot";
        private readonly string      _reloadNameAnimation   = "Reload";
        private          GunData _gunData;
        private          float       _currentShootingMode;
        private          float       _nextShootingMode;
        private          bool        _isShooting;
        private          bool        _isShootNextFrame;
        private          bool        _isReloading;
        private          int         _countShooting = 1;
        private          float       _coolDown;
        private          Coroutine   _delayPlaySmokeEffectCoroutine;
        private          int         _totalBullet;
        #endregion

        #region Implement
        
        private void OnValidate()
        {
            TryGetComponent(out _animator);
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            EventDispatcher.Instance.RegisterListener(EventID.ChangeMode,OnChangeMode);
            EventDispatcher.Instance.RegisterListener(EventID.Reload,PlayAnimReload);
            EventManager.onPointerDown   += OnPointerDown;
            EventManager.onShake         += OnShake;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            EventDispatcher.Instance.RemoveListener(EventID.ChangeMode,OnChangeMode);
            EventDispatcher.Instance.RemoveListener(EventID.Reload,PlayAnimReload);
            EventManager.onPointerDown   -= OnPointerDown;
            EventManager.onShake         -= OnShake;
        }
        
        protected override void GetObjectBase()
        {
            if(!_hasData) return;

            if (EventManager.getSelectedObjectType().Equals(ObjectSimulationType.MachineGun))
            {
                _gunData = DataGame.Instance.GetMachineData(EventManager.getSelectedObjectIndex());
            }
            else
            {
                 _gunData = DataGame.Instance.GetScifiData(EventManager.getSelectedObjectIndex());
            }
            
            _capacity    = _gunData.capacity;
            _totalBullet = (int)_capacity;
        }
        
        protected override void Start()
        {
            base.Start();
            UpdateEnergy(_totalBullet);
        }
        
        protected override void Update()
        {
            base.Update();
            _coolDown -= Time.deltaTime;
            CheckAndUpdateShootingMode();
            CheckAndReload();
            CheckAndAutoShootingMode();
            CheckShooting();
        }
        
        #endregion
        
        #region Func Animation Reference
        
        private async void CheckShooting()
        {
            if(!_isShootNextFrame || !CheckCanShooting()) return;
            _isShootNextFrame = false;
            PlayAnimation(_shootingNameAnimation);
            if(EventManager.isFlash())
            {
                await Task.Delay(100);
            }
            EventManager.playFlash(1);
        }
        
        public void OnShooting()
        {
            _countShooting--;
            if (!EventManager.infinityEnergy())
            {
                _totalBullet--;
                UpdateEnergy(_totalBullet);
            }
            EventDispatcher.Instance.PostEvent(EventID.Shooting);
        }
        
        public void OnShootingEnd()
        {
            _coolDown = _gunData.cooldown;
            if (_currentShootingMode - 1 == 0)
            {
                _coolDown = _gunData.burstCooldown;
            }
            
            if (_countShooting <= 0)
            {
                _isShooting                    = false;
                _delayPlaySmokeEffectCoroutine = StartCoroutine(DelayPlaySmokeEffect(_delayPlaySmokeEffect));
            }
            else
            {
                _isShootNextFrame = true;
            }
            
        }
        
        private async void PlayAnimReload(object obj)
        {
            await Task.Delay((int)(_delayReload * 1000));
            PlayAnimation(_reloadNameAnimation);
        }

        public override void OnReload()
        {
            _totalBullet = (int)_capacity;
            _isReloading = false;
            UpdateEnergy(_totalBullet);
        }
        

        #endregion

        private void CheckAndUpdateShootingMode()
        {
            if(_isShooting) return;
            if(_currentShootingMode - _nextShootingMode == 0) return;
            _currentShootingMode = _nextShootingMode;
        }

        private void CheckAndAutoShootingMode()
        {
            if(!CheckCanShooting()  || _isShooting) return;
            
            if (_currentShootingMode - 2 != 0 || !EventManager.onMouseInteract())
            {
                return;
            }
            _countShooting = 1;
            PlayShootingAnimation();
        }
        
        private void CheckAndReload()
        {
            if(_totalBullet > 0) return;
            if(_isShooting || _isReloading) return;
            _isReloading = true;
            this.PostEvent(EventID.NeedReload);
        }
        
        private void OnChangeMode(object obj)
        {
            _nextShootingMode = (int)obj;
        }

        private void OnShake()
        {
            if(!CheckCanShooting()  || _isShooting) return;
            if(_currentShootingMode - 3 != 0) return;
            _countShooting = 1;
            PlayShootingAnimation();
        }

        private void OnPointerDown(Vector2 obj)
        {
            if(!CheckCanShooting() || _isShooting) return;
            
            _countShooting = 0;

            if (_currentShootingMode == 0)
            {
                _countShooting = 1;
            }else if (_currentShootingMode - 1 == 0)
            {
                _countShooting = _gunData.burstCount;
            }
            
            if (_countShooting > 0)
            {
                PlayShootingAnimation();
            }
            
        }

        private void PlayShootingAnimation()
        {
            if(_delayPlaySmokeEffectCoroutine != null)
            {
                StopCoroutine(_delayPlaySmokeEffectCoroutine);
                _delayPlaySmokeEffectCoroutine = null;
            }
            _countShooting    = Mathf.Min(_countShooting, _totalBullet);
            _isShooting       = true;
            _isShootNextFrame = true;
        }
        
        private IEnumerator DelayPlaySmokeEffect(float delay)
        {
            yield return new WaitForSeconds(delay);
            EventDispatcher.Instance.PostEvent(EventID.PlaySmokeEffect);
        }
        
        private void PlayAnimation(string name)
        {
            if (!_animator)
            {
                Debug.LogError("Animator bị null");
            }
            _animator.Play(name);
        }

        private bool CheckCanShooting()
        {
            return !_onRotateMode && !_isReloading  && _totalBullet > 0 && _coolDown <= 0;
        }
        
        
        
    }
}