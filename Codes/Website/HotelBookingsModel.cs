public class HotelBookingsModel
{
    public int? Id {get;set;}
    public string? GuestName {get;set;}
    public int? NoOfGuests {get;set;}
    public int? RoomTypeId {get;set;}
    public int? RoomId {get;set;}

    public long BookingDate { get; set; } 

    public long CheckInDate {get;set;}

    public long CheckOutDate {get;set;}

    public int? TotalPrice {get;set;}
    public string? PaymentType {get;set;}
    public string? BookingStatus {get;set;}

    public string? CheckInDateString {get;set;}
    public string? CheckOutDateString {get;set;}

    public string? VoucherCode {get;set;}
    public int? VoucherCodeDiscount {get;set;}

    public int? PriceBeforeDiscount {get;set;}
}