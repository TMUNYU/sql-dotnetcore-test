namespace Orders.Api.Models{ 

    public class LatestOrderDto {
        public CustomerNamesDto Customer { get; set; } 
        public OrderSummaryDto Order { get; set; } 
    }

}