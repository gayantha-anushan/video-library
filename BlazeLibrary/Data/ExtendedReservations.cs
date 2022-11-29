namespace BlazeLibrary.Data
{
    public class ExtendedReservation
    {
        public int VideoId { get; set; }
        public int  MemberId { get; set; }
        public String VideoName { get; set; }
        public String MemberFirstName { get; set; }
        public String MemberLastName { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}
