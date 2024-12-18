using System;



[Serializable]
public enum Axis
{
    X,
    Y,
    Z
}

[Serializable]
public enum ObjectSimulationType
{
    VapeAndPod,
    MachineGun,
    ScifiGun,
    LightSaber,
}

#region InGame

[Serializable]
public enum TargetInGame
{
    Background,
    ObjectSimulation,
    Tank,
    Juice,
}

#endregion