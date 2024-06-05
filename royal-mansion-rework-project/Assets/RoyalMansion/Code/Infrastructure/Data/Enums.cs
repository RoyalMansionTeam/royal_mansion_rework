namespace RoyalMasion.Code.Infrastructure.Data
{
    public enum WindowID
    {
        UiRoot = 0,
        MansionHUD = 1,
        Catalog = 2
    }

    public enum SceneID
    {
        BootstrapScene = 0,
        MansionScene = 1,
    }

    public enum ResourceType
    {
        None = 0,
        HardValue = 1,
        SoftVallue = 2,
        Fruit = 3,
    }
    public enum RewardState 
    { 
        Claimed = -1, 
        Claimable = 1, 
        Unclaimed = 0 
    };
}