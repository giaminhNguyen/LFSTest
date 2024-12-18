using _GameAssets._Scripts;
using UnityEngine;

public class CardListInGame : MonoBehaviour
{
    [SerializeField]
    private TargetInGame _type;
    [Space(10)]
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private CardInGame _cardPrefab;
    
    private void Start()
    {
        if (!_content)
            return;
        var objectDataBases = GetDataList();
        if(objectDataBases == null) return;
        SpawnCard(objectDataBases);
        
    }

    private DataBase[] GetDataList()
    {
        if (!DataGame.Instance || !GameState.Instance)
        {
            return null;
        }
        DataBase[] objectDataBases = null;

        switch (_type)
        {
            case TargetInGame.Background:
                objectDataBases = DataGame.Instance.BackgroundData;
                break;
            case TargetInGame.ObjectSimulation:
                objectDataBases = GetObjectSimulationData();
                break;
            case TargetInGame.Tank:
                objectDataBases = DataGame.Instance.Tanks;
                break;
            case TargetInGame.Juice: 
                objectDataBases = DataGame.Instance.Juices;
                break;
        }
        
        return objectDataBases;
    }

    private DataBase[] GetObjectSimulationData()
    {
        DataBase[] objectDataBases = null;
        switch (EventManager.getSelectedObjectType())
        {
            case ObjectSimulationType.VapeAndPod:
                objectDataBases = DataGame.Instance.VapeAndPodData;
                break;
            case ObjectSimulationType.ScifiGun:
                objectDataBases = DataGame.Instance.ScifiData;
                break;
            case ObjectSimulationType.LightSaber:
                objectDataBases = DataGame.Instance.LightSaberData;
                break;
            default:
                objectDataBases = DataGame.Instance.MachineData;
                break;
        }

        return objectDataBases;
    }

    private void SpawnCard(DataBase[] objectDataBases)
    {
        for(var i = 0; i < objectDataBases.Length; i++)
        {
            var card = Instantiate(_cardPrefab, _content);
            card.SetTargetType(_type);
            card.SetUp(i, objectDataBases[i].icon);
        }
    }
}