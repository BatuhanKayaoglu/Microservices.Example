using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages
{
    public class OrderItemMessage
    {
        public Guid ProductId { get; set; } // hangi product olduğunu anlamamız için
        public int Count  { get; set; }
    }
}
