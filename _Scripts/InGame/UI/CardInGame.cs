using ComponentUtilitys;

public class CardInGame : CardBase
{
    private TargetInGame _type;
    
    public void SetTargetType(TargetInGame type)
    {
        _type = type;
    }
    
    public override void OnClick()
    {
        switch (_type)
        {
            case TargetInGame.Background:
                EventManager.selectedBackgroundInGame?.Invoke(_cardID);
                break;
            case TargetInGame.ObjectSimulation:
                EventManager.selectedObjectIndex?.Invoke(_cardID);
                break;
            //-- Vape And Pod 
            case TargetInGame.Tank:
                EventManager.selectedTankIndex?.Invoke(_cardID);
                break;
            case TargetInGame.Juice:
                EventManager.selectedJuiceIndex?.Invoke(_cardID);
                break;
        }
    }
}