using UnityEngine;
using UnityHelper;

public class BackgroundInGame : MonoBehaviour
{
    [SerializeField]
    private ImageInGame _imageInGame;
    private void Awake()
    {
        EventManager.selectedBackgroundInGame += SelectedBackgroundInGame;
    }
    
    private void OnDisable()
    {
        EventManager.selectedBackgroundInGame -= SelectedBackgroundInGame;
    }

    private void SelectedBackgroundInGame(int obj)
    {
        if(DataGame.Instance == null || !_imageInGame) return;
        var image     = DataGame.Instance.GetBackgroundData(obj);
        var imageInfo = new ImageInfo{sprite = image.sprite,resolution = image.resolution};
        _imageInGame.ApplyBackground(imageInfo);
    }
}