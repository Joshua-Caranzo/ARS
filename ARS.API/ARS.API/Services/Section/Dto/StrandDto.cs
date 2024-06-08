namespace ARS.API.Services.Section.Dto
{
    public class StrandDto
    {
        public int Id { get; set; }
        public string StrandName { get; set; }
        public string? StrandAbbrev { get; set; }
        public int TrackId { get; set; }
        public string TrackName { get; set; }
    }
}
