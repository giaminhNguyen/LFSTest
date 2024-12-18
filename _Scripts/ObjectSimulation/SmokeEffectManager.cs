using System;
using UnityEngine;
using VInspector;
using Random = UnityEngine.Random;

public class SmokeEffectManager : MonoBehaviour
{
    [SerializeField]
    private ParticleEffectCtrl[] _particleEffectCtrls15;
    [SerializeField]
    private ParticleEffectCtrl[] _particleEffectCtrls25;

    [SerializeField]
    private ParticleEffectCtrl _particleEffectCtrlFail;

    private int _length15;
    private int _length2;
    private int _curIndex;
    
    private void OnEnable()
    {
        EventManager.playSmokeEffect15   += Play15;
        EventManager.playSmokeEffect25    += Play25;
        EventManager.playSmokeEffectFail += PlayFail;
    }

    private void OnDisable()
    {
        EventManager.playSmokeEffect15     -= Play15;
        EventManager.playSmokeEffect25     -= Play25;
        EventManager.playSmokeEffectFail -= PlayFail;
    }

    private void Start()
    {
        _length15 = _particleEffectCtrls15.Length;
        _length2 = _particleEffectCtrls25.Length;
    }

    [Button]
    public void Play15()
    {
        _particleEffectCtrls15[GetIndex(max:_length15)].Play();
    }
    
    [Button]
    public void Play25()
    {
        _particleEffectCtrls25[GetIndex(max:_length2)].Play();
    }
    
    

    private int GetIndex(int min = 0,int max = 1)
    {
        var index = 0;

        do
        {
            index = Random.Range(min, max);
        }
        while (_curIndex == index);

        _curIndex = index;
        
        return _curIndex;
    }

    [Button]
    public void PlayFail()
    {
        _particleEffectCtrlFail.Play();
    }
    
}