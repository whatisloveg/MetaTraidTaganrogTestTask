namespace MetaTraidTaganrogTestTask.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsTaken { get; set; } = false;
        public int TakerClientId { get; set; } = 0;
    }
}
