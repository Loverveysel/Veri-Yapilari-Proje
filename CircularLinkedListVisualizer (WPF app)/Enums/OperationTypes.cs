namespace CircularLinkedListVisualizer.Enums
{
    public enum OperationType { Insert, Delete, Utilities }

    public enum SubOperationType
    {
        InsertBack, InsertFront, InsertAt,
        Remove, RemoveAt, RemoveFront, RemoveBack,
        UpdateData, Clear, Search
    }
}