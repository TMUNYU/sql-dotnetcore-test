namespace Orders.Api.Models.Response
{ 

    public class LatestOrderDto {
        public CustomerNamesDto Customer { get; set; } 
        public OrderSummaryDto Order { get; set; } 
    }

}