public class DiningBookingsModel
{
    
    public int? Id {get;set;}
    public string? GuestName {get;set;}
    public int? DiningId {get;set;}
    public int? TableId {get;set;}

    public long BookingDate { get;set;}
    public long CheckInDate {get;set;}

    public int? NoOfGuests {get;set;}

    public int? TotalPrice {get;set;}
    public string? PaymentType {get;set;}
    public string? BookingStatus {get;set;}

    public string? CheckInDateString {get;set;}
}