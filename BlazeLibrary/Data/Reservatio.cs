namespace BlazeLibrary.Data
{
    public class Reservatio
    {
        public int VideoId { get; set; }
        public int UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
