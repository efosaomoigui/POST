namespace GIGLS.Core.Enums
{
    public enum PaymentType
    {
        Cash,
        Pos,
        Online,
        Wallet,
        Transfer,
        Partial,
        Waiver,
        USSD
    }

    public enum CODPaidOutStatus 
    {
        PaidOut,
        NotPaidOut 
    }

    public enum OnlinePaymentType
    {
        Paystack,
        TheTeller,
        Flutterwave,
        USSD
    }
}