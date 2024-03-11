namespace BookingSystemConsoleApp.entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal PricePerDay { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Address: {Address}, Price per day: {PricePerDay}";
        }
    }
}
