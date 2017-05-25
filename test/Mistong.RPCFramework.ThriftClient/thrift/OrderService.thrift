namespace csharp Mistong.Services.OrderService

struct OrderInfo
{
	1:i32 OrderID
	2:string OrderNo
	3:double OrderAmount
}

service OrderService
{
	OrderInfo Get(1:i32 orderId)
}