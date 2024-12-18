using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityHelper;

public class LoadingLayer : LayerBase
{
    #region Properties

    [Header("Reference")]
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [Header("Data")]
    [SerializeField]
    private string _menuSceneName = "Menu";

    [SerializeField]
    private string _gamePlaySceneName = "GamePlay";

    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();
        EventManager.loadLayer         += LoadLayer;
        EventManager.goToGamePlayScene += GoToGamePlayScene;
        EventManager.goToMenuScene     += GoToMenuScene;
        EventDispatcher.Instance.RegisterListener(EventID.GoToGamePlayScene,GoToGamePlayScene);
        EventDispatcher.Instance.RegisterListener(EventID.GoToMenuScene,GoToMenuScene);
    }

    private void OnDisable()
    {
        EventManager.loadLayer         -= LoadLayer;
        EventManager.goToGamePlayScene -= GoToGamePlayScene;
        EventManager.goToMenuScene     -= GoToMenuScene;
    }
    
    private void GoToMenuScene(object obj)
    {
        GoToMenuScene();
    }
    
    private void GoToGamePlayScene(object obj)
    {
        GoToGamePlayScene();
    }
    
    private void GoToMenuScene()
    {
        LoadingScene(_menuSceneName);
    }

    private void GoToGamePlayScene()
    {
        LoadingScene(_gamePlaySceneName);
    }
    
    private async void LoadingScene(string sceneName)
    {
        var sfxActive = AudioManager.Instance.ActiveSFX;
        Open();
        _canvasGroup.DOFade(1f, 0.3f);
        await Task.Delay(300);
        AudioManager.Instance.ActiveSfx(false);
        this.LoadScene_LoadAsync(sceneName,false);
        while (!this.LoadScene_CanDone())
        {
            await Task.Yield();
        }
        await Task.Delay(1000);
        this.LoadScene_Activate();
        AudioManager.Instance.ActiveSfx(sfxActive);
        _canvasGroup.DOFade(0f, 0.2f).OnComplete(() =>
        {
            Close();
        });
    }
    

    private void LoadLayer(Action arg1, Action arg2)
    {
        Open(arg1);
        DOVirtual.DelayedCall(1f, () =>
        {
            Close(arg2);
        });
    }

    protected override void InitAwake()
    {
    }

    protected override void InitOnEnable()
    {
    }

    protected override void InitStart()
    {
    }

    public override void Init()
    {
    }

    private void Open(Action action)
    {
        Open();
        _canvasGroup.DOFade(1f, 0.3f).OnComplete(() =>
        {
            action?.Invoke();
        });
    }

    private void Close(Action action)
    {
        action?.Invoke();
        _canvasGroup.DOFade(0f, 0.3f).OnComplete(() =>
        {
            Close();
        });
    }

    public override void Open()
    {
        base.Open();
        _content.SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        _content.SetActive(false);
    }
}