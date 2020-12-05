namespace Orders.Api.Models{ 

    public class OrderItemSummaryDto    {
        public string Product { get; set; } 
        public int Quantity { get; set; } 
        public double PriceEach { get; set; } 
    }

}