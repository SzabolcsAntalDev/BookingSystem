using BookingSystemConsoleApp.entities;

namespace BookingSystemConsoleApp.interfaces
{
    public interface IBookingSystem
    {
        public string City { get; }

        public bool AddUser(User user);

        public bool RemoveUser(User user);

        public IList<Room> GetAvailableRooms(DateTime checkInDate, DateTime checkOutDate, decimal maxTotalPrice);

        public int BookRoom(User user, int roomId, DateTime checkInDate, DateTime checkOutDate);

        public bool CancelBooking(User user, int bookingId);

        public IList<Booking> GetBookingHistory(User user);

        public IList<Booking> FilterBookingsByPeriod(User user, DateTime checkInDate, DateTime checkOutDate);

        public IList<Booking> FilterBookingsByPrice(User user, decimal minPrice, decimal maxPrice);

        public IList<Booking> FilterBookingsByPeriodAndPrice(User user, DateTime checkInDate, DateTime checkOutDate, decimal minPrice, decimal maxPrice);
    }
}
