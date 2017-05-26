using Mistong.Services.OrderService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.ThriftClient
{
    public class OrderServiceImplement : OrderService.Iface
    {
        private List<OrderInfo> orders;

        public OrderServiceImplement()
        {
            orders = new List<OrderInfo>();
            orders.Add(new OrderInfo { OrderID = 10, OrderNo = "订单00001", OrderAmount = 9800.00 });
        }

        public OrderInfo Get(int orderId)
        {
            return orders.SingleOrDefault(tmp => tmp.OrderID == orderId);
        }
    }
}