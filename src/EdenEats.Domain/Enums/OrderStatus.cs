using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Domain.Enums
{
    public enum OrderStatus
    {
        CREATED = 1,
        SAVED = 2,
        APPROVED = 3,
        VOIDED = 4,
        COMPLETED = 5
    }
}
