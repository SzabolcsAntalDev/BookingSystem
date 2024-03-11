namespace BookingSystemConsoleApp.entities
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int RoomId { get; set; }
        public decimal TotalPrice { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, check in date: {CheckInDate.ToShortDateString()}, check out date: {CheckOutDate.ToShortDateString()}, total price: {TotalPrice}.";
        }
    }
}
