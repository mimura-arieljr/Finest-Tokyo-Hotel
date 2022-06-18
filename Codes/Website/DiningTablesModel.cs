public class DiningTablesModel
{
    public int? Id {get; set;}

    public int? DiningId {get; set;}
    public DiningModel? Dining {get;set;}
    public int? TableCapacity {get;set;}
    public string? DiningStatus {get;set;}   
}