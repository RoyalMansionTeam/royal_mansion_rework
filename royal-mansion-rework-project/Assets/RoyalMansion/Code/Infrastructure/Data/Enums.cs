namespace RoyalMasion.Code.Infrastructure.Data
{
    public enum WindowID
    {
        UiRoot = 0,
        MansionHUD = 1,
        Catalog = 2,
        MansionUnitsUI = 3,
        MansionUnitsUIRoot = 4,
        DailyMessagesPopUp = 5,
        DailyRewardsPopUp = 6,
        StaffRecruitment = 7
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