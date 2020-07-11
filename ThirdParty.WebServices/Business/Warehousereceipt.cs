using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdParty.WebServices.Magaya.Business.New
{
    public partial class WarehouseReceipt
    {
        private string actualAmountCollected;
        private string expectedAmountToCollect;
        private string magayaPaymentOption;
        private string magayaPaymentType; 

        public string ActualAmountCollected
        {
            get { return actualAmountCollected; }
            set { actualAmountCollected = value; }
        }

        public string ExpectedAmountToCollect
        {
            get { return expectedAmountToCollect; }
            set { expectedAmountToCollect = value; }
        }

        public string MagayaPaymentOption
        {
            get { return magayaPaymentOption; }
            set { magayaPaymentOption = value; }
        }

        public string MagayaPaymentType
        {
            get { return magayaPaymentType; }
            set { magayaPaymentType = value; }
        }

    }
}
