using BookingSystemConsoleApp.entities;
using BookingSystemConsoleApp.interfaces;

namespace BookingSystemConsoleApp.implementations
{
    public class BudapestBookingSystem : IBookingSystem
    {
        protected readonly IList<Room> _rooms = new List<Room>();
        protected readonly IList<User> _users = new List<User>();
        protected readonly IDictionary<User, IList<Booking>> _userBookings = new Dictionary<User, IList<Booking>>();

        private IEnumerable<Booking> AllBookings => _userBookings.Values.SelectMany(b => b);

        public string City => "Budaptest";

        public BudapestBookingSystem()
        {
            AddInitialData();
        }

        private void AddInitialData()
        {
            AddRooms();
            AddBookings();
        }

        private void AddRooms()
        {
            _rooms.Add(
                new Room()
                {
                    Id = 1,
                    Name = "Room on Szent Istvan Street",
                    Address = "Szent Istvan Street 8A., Budapest",
                    PricePerDay = 1.3m,
                });
            _rooms.Add(
                new Room()
                {
                    Id = 2,
                    Name = "Room on Matyas Kiraly Street",
                    Address = "Matyas Kiraly Street 3., Budapest",
                    PricePerDay = 2.3m
                });
            _rooms.Add(
                new Room()
                {
                    Id = 3,
                    Name = "Room on Vaci Street",
                    Address = "Vaci Street 47., Budapest",
                    PricePerDay = 1.6m
                });
        }

        private void AddBookings()
        {
            var szabolcsAntal = new User() { Name = "Szabolcs Antal" };
            _users.Add(szabolcsAntal);

            var bookingsOfSzabolcsAntal =
                new List<Booking>()
                    {
                        new Booking()
                        {
                            Id = 1,
                            CheckInDate = new DateTime(2024, 3, 1),
                            CheckOutDate = new DateTime(2024, 3, 3),
                            RoomId = 1,
                            TotalPrice = 2.6m
                        },
                        new Booking()
                        {
                            Id = 2,
                            CheckInDate = new DateTime(2024, 3, 6),
                            CheckOutDate = new DateTime(2024, 3, 8),
                            RoomId = 2,
                            TotalPrice = 4.6m
                        },
                        new Booking()
                        {
                            Id = 1,
                            CheckInDate = new DateTime(2024, 3, 15),
                            CheckOutDate = new DateTime(2024, 3, 17),
                            RoomId = 1,
                            TotalPrice = 2.6m
                        },
                        new Booking()
                        {
                            Id = 2,
                            CheckInDate = new DateTime(2024, 3, 19),
                            CheckOutDate = new DateTime(2024, 3, 21),
                            RoomId = 2,
                            TotalPrice = 4.6m
                        },
                    };

            _userBookings.Add(szabolcsAntal, bookingsOfSzabolcsAntal);
        }

        public bool AddUser(User user)
        {
            if (_users.Contains(user))
            {
                Console.WriteLine($"User {user.Name} already exists in the system.");
                return false;
            }

            _users.Add(user);
            _userBookings[user] = new List<Booking>();
            return true;
        }

        public bool RemoveUser(User user)
        {
            if (_users.Contains(user))
            {
                _users.Remove(user);
                _userBookings.Remove(user);
                return true;
            }

            Console.WriteLine($"User {user.Name} does not exist in the system.");
            return false;
        }

        public IList<Room> GetAvailableRooms(DateTime checkInDate, DateTime checkOutDate, decimal maxTotalPrice)
        {
            var availableRooms = new List<Room>();

            foreach (var room in _rooms)
            {
                if (!IsRoomAvailableInInterval(room, checkInDate, checkOutDate))
                    continue;

                if (GetNumberOfDaysBetween(checkInDate, checkOutDate) * room.PricePerDay > maxTotalPrice)
                    continue;

                availableRooms.Add(room);
            }

            return availableRooms;
        }

        private bool IsRoomAvailableInInterval(Room room, DateTime checkInDate, DateTime checkOutDate)
        {
            var roomBookings = AllBookings.Where(b => b.RoomId == room.Id);
            return !roomBookings.Any(b => ArePeriodsInterfering(checkInDate, checkOutDate, b.CheckInDate, b.CheckOutDate));
        }

        private static bool ArePeriodsInterfering(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            return !(end1 <= start2 || start1 >= end2);
        }

        public int BookRoom(User user, int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            var room = _rooms.FirstOrDefault(b => b.Id == roomId);
            if (room == null)
            {
                Console.WriteLine($"Could not find room with id {roomId}.");
                return -1;
            }

            if (!IsRoomAvailableInInterval(room, checkInDate, checkOutDate))
            {
                Console.WriteLine($"Room is not available in the interval {checkInDate.ToShortDateString()} - {checkOutDate.ToShortDateString()}.");
                return -1;
            }

            var totalPrice = GetNumberOfDaysBetween(checkInDate, checkOutDate) * room.PricePerDay;

            var booking = new Booking()
            {
                Id = AllBookings.Count() + 1,
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate,
                TotalPrice = totalPrice
            };

            _userBookings[user].Add(booking);

            return booking.Id;
        }

        public bool CancelBooking(User user, int bookingId)
        {
            if (_userBookings.ContainsKey(user))
            {
                var userBookings = _userBookings[user];
                var booking = userBookings.FirstOrDefault(b => b.Id == bookingId);
                if (booking != null)
                {
                    userBookings.Remove(booking);
                    return true;
                }
                else
                {
                    Console.WriteLine($"Could not find booking with id {bookingId} for the user {user.Name}.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine($"User {user.Name} has no bookings.");
                return false;
            }
        }

        public IList<Booking> GetBookingHistory(User user)
        {
            if (_userBookings.ContainsKey(user))
                return _userBookings[user];
            else
                return new List<Booking>();
        }

        public void PrintBookingHistory(User user)
        {
            var bookingHistory = GetBookingHistory(user);
            if (bookingHistory.Any())
                foreach (var booking in bookingHistory)
                    Console.WriteLine(booking);
            else
                Console.WriteLine($"User {user.Name} has no booking history.");
        }

        public IList<Booking> FilterBookingsByPeriod(User user, DateTime checkInDate, DateTime checkOutDate)
        {
            var filteredBookings = new List<Booking>();

            if (_userBookings.ContainsKey(user))
            {
                foreach (var booking in _userBookings[user])
                {
                    if (ArePeriodsInterfering(booking.CheckInDate, booking.CheckOutDate, checkInDate, checkOutDate))
                        filteredBookings.Add(booking);
                }
            }

            return filteredBookings;
        }

        public IList<Booking> FilterBookingsByPrice(User user, decimal minPrice, decimal maxPrice)
        {
            var filteredBookings = new List<Booking>();

            if (_userBookings.ContainsKey(user))
            {
                foreach (var booking in _userBookings[user])
                {
                    if (IsPriceBetween(booking.TotalPrice, minPrice, maxPrice))
                        filteredBookings.Add(booking);
                }
            }

            return filteredBookings;
        }

        public IList<Booking> FilterBookingsByPeriodAndPrice(User user, DateTime checkInDate, DateTime checkOutDate, decimal minPrice, decimal maxPrice)
        {
            var filteredBookings = new List<Booking>();

            if (_userBookings.ContainsKey(user))
            {
                foreach (var booking in _userBookings[user])
                {
                    if (ArePeriodsInterfering(booking.CheckInDate, booking.CheckOutDate, checkInDate, checkOutDate) && IsPriceBetween(booking.TotalPrice, minPrice, maxPrice))
                        filteredBookings.Add(booking);
                }
            }
            return filteredBookings;
        }

        private static int GetNumberOfDaysBetween(DateTime start, DateTime end)
        {
            TimeSpan duration = end - start;
            return duration.Days;
        }

        private static bool IsPriceBetween(decimal price, decimal minPrice, decimal maxPrice)
        {
            return price >= minPrice && price <= maxPrice;
        }
    }
}
