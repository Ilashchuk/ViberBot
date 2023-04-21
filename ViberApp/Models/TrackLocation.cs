namespace ViberApp.Models
{
    public class TrackLocation
    {
        public int Id { get; set; }
        public string IMEI { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime DateEvent { get; set; }
        public DateTime date_track { get; set; }
        public int TypeSource { get; set; } = 1;
    }
}
