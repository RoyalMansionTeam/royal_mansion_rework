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
        AwaitingGuests = 3,
        ApartmentStayTimer = 4,
        Collectable = 5,
        AwaitingCleaning = 6,
        CleaningTimer = 7
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
}