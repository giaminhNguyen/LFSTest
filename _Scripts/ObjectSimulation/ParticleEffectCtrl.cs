using System;
using System.Collections;
using UnityEngine;
using VInspector;
using Random = UnityEngine.Random;

public class ParticleEffectCtrl : MonoBehaviour
{
    [Serializable]
    public class ParticleInfo
    {
        public Coroutine        coroutine;
        public float            delayTime;
        public ParticleSystem[] particleSystems;
    }

    [SerializeField]
    private ParticleInfo[] _particleInfos;
    

    [Button]
    public void Play()
    {
        foreach (var particleInfo in _particleInfos)
        {
            PlayEffect(particleInfo);
        }
       
    }

    public void Play(float size)
    {
        
        foreach (var particleInfo in _particleInfos)
        {
            foreach (var particle in particleInfo.particleSystems)
            {
                particle.transform.localScale = size * Vector3.one;
            }
        }
        
        Play();
    }
        
    //------

    private void PlayEffect(ParticleInfo particleInfo)
    {
        if (particleInfo.coroutine != null)
        {
            StopCoroutine(particleInfo.coroutine);
        }

        particleInfo.coroutine = StartCoroutine(PlayEffect(particleInfo.delayTime, particleInfo.particleSystems));
    }
        
    private IEnumerator PlayEffect(float delayTime,ParticleSystem[] particleSystems)
    {
        yield return new WaitForSeconds(delayTime);

        foreach (var particle in particleSystems)
        {
            particle.Stop();
            particle.Play();
        }
    }
}