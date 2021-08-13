namespace GIGLS.Core.Enums
{
    public enum ManifestStatus
    {
        Pending,
        Accepted,
        Rejected,
        Delivered
    }

    public enum MovementStatus
    {
        NoMovement,
        InProgress,
        EnRoute,
        ProcessEnded
    }

    public enum CargoStatus
    {
        Unprocessed,
        Cargoed
    }
}
