namespace ARS.API.Models
{
    public class Strand
    {
        public int Id { get; set; }
        public string StrandName { get; set; }
        public string? StrandAbbrev { get; set; }
        public int TrackId { get; set; }
    }
}
