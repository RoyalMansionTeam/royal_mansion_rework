namespace RoyalMasion.Code.Infrastructure.Data
{
    public enum UnitState
    {
        Locked = 0,
        Empty = 1,
        InUse = 2,
        Collectable = 3,
        Dirty = 4,
    }
    public enum InternalUnitStates
    {
        Locked = 0,
        Unlockable = 1,
        AwaitingFurniture = 2,
        AwaitingSeed = 3,
        AwaitingGuests = 4,
        ApartmentStayTimer = 5,
        GardenTimer = 6,
        CollectableApartment = 7,
        CollectableGarden = 8,
        AwaitingCleaning = 9,
        CleaningTimer = 10,
        KitchenReadyToOrder = 11,
        GuestReadyToOrder = 12,
        Pantry = 13
    }


    public enum UnitType
    {
        Apartment = 0,
        Garden = 1,
        Kitchen = 2,
        Pantry = 3
    }

    public enum ApartmentAreaType
    {
        None = 0,
        Bedroom = 1,
        Bathroom = 2
    }

    public enum CatalogSection
    {
        Bed = 0, 
        Bath = 1, 
        Toilet = 2, 
        Closet = 3, 
        Walls = 4, 
        Floors = 5, 
        Carpet = 6, 
        Mirror = 7, 
        Picture = 8, 
        Lamp = 9, 
        Tree = 10, 
        Seat = 11, 
        Table = 12, 
        None = 13
    }

    public enum NpcState
    {
        Resting = 0,
        PerformingTask = 1
    }
}