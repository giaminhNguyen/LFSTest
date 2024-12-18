using System;
using _GameAssets._Scripts;
using UnityEngine;

public static class EventManager
{
    /// <summary>
    /// Hàm được gọi để kiểm tra xem có sự rung lắc hay không.
    /// </summary>
    public static Action onShake;
    
    /// <summary>
    /// Hàm được gọi để kiểm tra xem có sự tương tác của chuột hay không.
    /// </summary>
    public static Func<bool> onMouseInteract;

    /// <summary>
    /// Hành động được gọi khi sự kiện thả chuột xảy ra.
    /// </summary>
    public static Action<Vector2> onPointerUp;

    /// <summary>
    /// Hành động được gọi khi sự kiện nhấn chuột xảy ra.
    /// </summary>
    public static Action<Vector2> onPointerDown;

    /// <summary>
    /// Hành động được gọi khi sự kiện kéo chuột xảy ra.
    /// Nghĩa cuả biến là: startPosition, previousPosition, currentPosition
    /// </summary>
    public static Action<Vector2, Vector2, Vector2> onDrag;


    public static Action<ObjectSimulationType> selectedObjectType;

    public static Action<int> selectedObjectIndex;

    public static Action changeObjectSimulation;

    public static Func<ObjectSimulationType> getSelectedObjectType;

    public static Func<int> getSelectedObjectIndex;

    public static Func<int> getSelectedHumanIndex;
    
    public static Action<int> selectedHumanIndex;
    
    public static Action<Action,Action> loadLayer;

    public static Action goToGamePlayScene;
    
    public static Action goToMenuScene;

    public static Action<bool> enableScreenFlash;
    public static Action<bool> enableFlash;
    public static Action<int>       playFlash;
    public static Action<bool> enableVibrate;
    public static Action       playVibrate;
    
    #region HACK

    public static Func<bool> infinityEnergy;

    #endregion

    #region InGame

    public static Action<int> selectedBackgroundInGame;

    public static Action<Color> onChangeRGBSlider;

    public static Action<Color> onUpdateRGBSlider;

    public static Action playSmokeEffect15;
    public static Action playSmokeEffect25;
    public static Action playSmokeEffectFail;
    
    #endregion

    #region VapeAndPod

    public static Action<int> selectedTankIndex;
    public static Action      changeTank;
    public static Func<int>   getSelectedTankIndex;
    
    public static Action<int> selectedJuiceIndex;
    public static Action      changeJuice;
    public static Func<int>   getSelectedJuiceIndex;

    #endregion

    #region Setting

    public static Func<bool> isVibration;
    public static Func<bool> isFlash;
    public static Func<bool> isScreenFlash;
    public static Func<bool> isAudioSfx;
    public static Func<bool> isAudioMusic;

    #endregion
    
}