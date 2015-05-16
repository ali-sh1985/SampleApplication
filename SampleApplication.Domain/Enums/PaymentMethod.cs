using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApplication.Domain.Enums
{
    public enum PaymentMethod : byte
    {
        Cash=1,
        Cheque=2,
        BankTransfer=3
    }
}
