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
        Waiver
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