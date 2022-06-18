public class RoomsModel
{
    public int Id {get; set;}

    public int? RoomTypeId {get; set;}
    public RoomTypesModel? RoomType {get; set;}
    public int? RoomNumber {get;set;}
    public string? RoomStatus {get;set;}   
}