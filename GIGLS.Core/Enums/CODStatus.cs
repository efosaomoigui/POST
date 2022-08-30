namespace GIGLS.Core.Enums
{
    public enum CODStatus
    {
        Unprocessed,
        Pending,
        Processed
    }

    public enum CODStatushistory
    {
        Created = 1,
        CollectedByDispatch,
        RecievedAtServiceCenter, 
        Banked
    }

    public enum DepositStatus
    {
        Unprocessed = 0,
        Pending, 
        Deposited,
        Verified
    }

    public enum CODMobileStatus
    {
        Initiated,
        Collected,
        Paid,
        All //this is just for reporting purpose 
    }
}
