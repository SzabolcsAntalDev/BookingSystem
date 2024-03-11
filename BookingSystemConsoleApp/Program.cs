using BookingSystemConsoleApp.entities;
using BookingSystemConsoleApp.implementations;

namespace BookingSystemConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var bookingSystem = new BudapestBookingSystem();

            Console.WriteLine($"Rooms available between {new DateTime(2024, 3, 14).ToShortDateString()} and {new DateTime(2024, 3, 17).ToShortDateString()} with a maximum total price of 50:");
            var availableRooms = bookingSystem.GetAvailableRooms(new DateTime(2024, 3, 14), new DateTime(2024, 3, 17), 50);
            foreach (var room in availableRooms)
                Console.WriteLine(room);

            Console.WriteLine();
            Console.WriteLine("Adding user Marton Toth.");
            var martonToth = new User() { Name = "Marton Toth" };
            bookingSystem.AddUser(martonToth);

            Console.WriteLine();
            Console.WriteLine($"Booking the room for Marton Toth on Matyas Kiraly Street between {new DateTime(2024, 3, 14).ToShortDateString()} and {new DateTime(2024, 3, 17).ToShortDateString()}.");
            bookingSystem.BookRoom(martonToth, 2, new DateTime(2024, 3, 14), new DateTime(2024, 3, 17));

            Console.WriteLine();
            Console.WriteLine($"Trying to book the room for Marton Toth on Matyas Kiraly Street between {new DateTime(2024, 3, 19).ToShortDateString()} and {new DateTime(2024, 3, 21).ToShortDateString()}.");
            bookingSystem.BookRoom(martonToth, 2, new DateTime(2024, 3, 19), new DateTime(2024, 3, 21));

            Console.WriteLine();
            Console.WriteLine($"Booking the room for Marton Toth on Matyas Kiraly Street between {new DateTime(2024, 3, 23).ToShortDateString()} and {new DateTime(2024, 3, 29).ToShortDateString()}.");
            int book3Id = bookingSystem.BookRoom(martonToth, 3, new DateTime(2024, 3, 23), new DateTime(2024, 3, 29));

            Console.WriteLine();
            Console.WriteLine($"Booking the room for Marton Toth on Matyas Kiraly Street between {new DateTime(2024, 4, 4).ToShortDateString()} and {new DateTime(2024, 4, 6).ToShortDateString()}.");
            bookingSystem.BookRoom(martonToth, 3, new DateTime(2024, 4, 4), new DateTime(2024, 4, 6));

            Console.WriteLine();
            Console.WriteLine("Bookings of Marton Toth:");
            bookingSystem.PrintBookingHistory(martonToth);

            Console.WriteLine();
            Console.WriteLine("Filtering bookings of Marton Toth by total price between 6 and 10:");
            var filteredBookingsByPrice = bookingSystem.FilterBookingsByPrice(martonToth, 6, 10);
            foreach (var booking in filteredBookingsByPrice)
                Console.WriteLine(booking);

            Console.WriteLine();
            Console.WriteLine($"Filtering bookings of Marton Toth between {new DateTime(2024, 3, 22).ToShortDateString()} - {new DateTime(2024, 4, 1).ToShortDateString()}");
            var filteredBookingsByPeriod = bookingSystem.FilterBookingsByPeriod(martonToth, new DateTime(2024, 3, 22), new DateTime(2024, 4, 1));
            foreach (var booking in filteredBookingsByPeriod)
                Console.WriteLine(booking);

            Console.WriteLine();
            Console.WriteLine("Cancelling the second booking of Marton Toth, on Matyas Kiraly Street.");
            bookingSystem.CancelBooking(martonToth, book3Id);

            Console.WriteLine();
            Console.WriteLine("Bookings of Marton Toth after cancelling the second one:");
            bookingSystem.PrintBookingHistory(martonToth);

            Console.WriteLine();
            Console.WriteLine($"Filtering bookings of Marton Toth between {new DateTime(2024, 3, 1).ToShortDateString()} - {new DateTime(2024, 5, 1).ToShortDateString()} with the price between 1 and 5:");
            var filteredBookingsByPeriodAndPrice = bookingSystem.FilterBookingsByPeriodAndPrice(martonToth, new DateTime(2024, 3, 1), new DateTime(2024, 5, 1), 1, 5);
            foreach (var booking in filteredBookingsByPeriodAndPrice)
                Console.WriteLine(booking);

            Console.ReadLine();
        }
    }
}