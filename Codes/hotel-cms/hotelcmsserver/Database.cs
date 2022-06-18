using System.Data.SqlClient;
public class Database
{
    public static void CreateTables()
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                @"IF NOT EXISTS (SELECT * FROM sysobjects 
                WHERE name='Users' and xtype='U')
                CREATE TABLE Users(
                Id INT NOT NULL IDENTITY PRIMARY KEY,
                UserName VARCHAR(255),
                Password VARCHAR(255),
                FirstName VARCHAR(255),
                LastName VARCHAR(255),
                UserType VARCHAR(255));";
                command.ExecuteNonQuery();
                command.CommandText =
                @"IF NOT EXISTS (SELECT * FROM sysobjects 
                WHERE name='Sessions' and xtype='U')
                CREATE TABLE Sessions(
                Id VARCHAR (255) NOT NULL PRIMARY KEY,
                UserName VARCHAR(255));";
                command.ExecuteNonQuery();
                Console.WriteLine("Table created successfully!");
            }
        }
    }

    public static void CreateWebTables()
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                @"IF NOT EXISTS (SELECT * FROM sysobjects 
                WHERE name='RoomTypes' and xtype='U')
                CREATE TABLE RoomTypes(
                Id INT NOT NULL IDENTITY PRIMARY KEY,
                RoomName VARCHAR(255),
                ImagePath VARCHAR(255),
                RoomSize VARCHAR(255),
                BedDetails VARCHAR(255),
                RoomCapacity INT,
                RoomRate INT,
                RoomQuantity INT);";
                command.ExecuteNonQuery();
                Console.WriteLine("RoomTypes table created successfully!");

                command.CommandText =
                @"IF NOT EXISTS (SELECT * FROM sysobjects 
                WHERE name='Rooms' and xtype='U')
                CREATE TABLE Rooms(
                Id INT NOT NULL IDENTITY PRIMARY KEY,
                RoomTypeId INT FOREIGN KEY REFERENCES RoomTypes(Id),
                RoomNumber INT,
                RoomStatus VARCHAR(255) DEFAULT 'Active');";
                command.ExecuteNonQuery();
                Console.WriteLine("Rooms table created successfully!");

                command.CommandText =
                @"IF NOT EXISTS (SELECT * FROM sysobjects 
                WHERE name='Dining' and xtype='U')
                CREATE TABLE Dining(
                Id INT NOT NULL IDENTITY PRIMARY KEY,
                DiningName VARCHAR(255),
                About VARCHAR(255),
                ImagePath VARCHAR(255),
                DiningCapacity INT,
                TableQuantity INT);";
                command.ExecuteNonQuery();
                Console.WriteLine("Dining table created successfully!");

                command.CommandText =
                @"IF NOT EXISTS (SELECT * FROM sysobjects 
                WHERE name='DiningTables' and xtype='U')
                CREATE TABLE DiningTables(
                Id INT NOT NULL IDENTITY PRIMARY KEY,
                DiningId INT FOREIGN KEY REFERENCES Dining(Id),
                TableCapacity INT,
                DiningStatus VARCHAR(255) DEFAULT 'Active');";
                command.ExecuteNonQuery();
                Console.WriteLine("DiningTables table created successfully!");

                command.CommandText =
                @"IF NOT EXISTS (SELECT * FROM sysobjects   
                WHERE name='Vouchers' and xtype='U')
                CREATE TABLE Vouchers(
                Id INT NOT NULL IDENTITY PRIMARY KEY,
                Code VARCHAR (255),
                SalePrice INT,
                VoucherStatus VARCHAR(255) DEFAULT 'Active');";
                command.ExecuteNonQuery();
                Console.WriteLine("Vouchers table created successfully!");
            }
        }
    }

    public static void CreateBookingTables()
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                @"IF NOT EXISTS (SELECT * FROM sysobjects 
                WHERE name='HotelBookings' and xtype='U')
                CREATE TABLE HotelBookings(
                Id INT NOT NULL IDENTITY PRIMARY KEY,
                GuestName VARCHAR(255) NOT NULL,
                NoOfGuests INT CHECK (NoOfGuests>=1),
                RoomTypeId INT FOREIGN KEY REFERENCES RoomTypes(Id),
                RoomId INT FOREIGN KEY REFERENCES Rooms(Id),
                BookingDate  BIGINT,
                CheckInDate BIGINT NOT NULL,
                CheckOutDate BIGINT NOT NULL,
                TotalPrice INT,
                PaymentType VARCHAR(255),
                BookingStatus VARCHAR(255));";
                command.ExecuteNonQuery();
                Console.WriteLine("HotelBookings table created successfully!");

                command.CommandText =
                @"IF NOT EXISTS (SELECT * FROM sysobjects 
                WHERE name='DiningBookings' and xtype='U')
                CREATE TABLE DiningBookings(
                Id INT NOT NULL IDENTITY PRIMARY KEY,
                GuestName VARCHAR(255) NOT NULL,
                NoOfGuests INT CHECK (NoOfGuests>=1),
                DiningId INT FOREIGN KEY REFERENCES Dining(Id),
                TableId INT FOREIGN KEY REFERENCES DiningTables(Id),
                BookingDate BIGINT,
                CheckInDate BIGINT NOT NULL,
                PaymentType VARCHAR(255),
                BookingStatus VARCHAR(255));";
                command.ExecuteNonQuery();
                Console.WriteLine("DiningBookings table created successfully!");
            }
        }
    }


    public static void AddUser(UsersModel user)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                @"INSERT INTO Users (UserName, Password, FirstName, LastName, UserType) VALUES (@Username, @Password, @FirstName, @LastName, @UserType);";
                command.Parameters.AddWithValue("@Username", user.UserName);
                var hashPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                command.Parameters.AddWithValue("@Password", hashPassword);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@UserType", user.UserType);
                command.ExecuteNonQuery();
            }
        }
    }

    public static List<UsersModel> GetUsers()
    {
        List<UsersModel> users = new List<UsersModel>();
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM Users;";
                var readme = command.ExecuteReader();
                while (readme.Read())
                {
                    users.Add(new UsersModel
                    {
                        Id = readme.GetInt32(0),
                        UserName = readme.GetString(1),
                        Password = readme.GetString(2),
                        FirstName = readme.GetString(3),
                        LastName = readme.GetString(4),
                        UserType = readme.GetString(5)
                    });
                }
            }
        }
        return users;
    }

    public static void AddSession(SessionsModel session)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                @"INSERT INTO Sessions (Id, UserName) VALUES (@Id, @UserName);";
                command.Parameters.AddWithValue("@Id", session.Id);
                command.Parameters.AddWithValue("@UserName", session.UserName); ;
                command.ExecuteNonQuery();
            }
        }
    }

    public static void DeleteUser(int Id)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"DELETE FROM Users where Id = {Id};";
                command.ExecuteNonQuery();
            }
        }
    }

    public static void DeleteSession(string Id)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"DELETE FROM Sessions where Id = '{Id}';";
                command.ExecuteNonQuery();
            }
        }
    }

    public static SessionsModel AddSessionForUser(string Username)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                var session = new SessionsModel();
                var randomId = Guid.NewGuid().ToString();
                session.Id = randomId;
                session.UserName = Username;
                return session;
            }
        }
    }

    public static SessionsModel AddSessionWithCredentials(string Username, string Password)
    {
        var checker = false;
        var adminOrEmp = "";
        var session = new SessionsModel();
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"Select Username, Password, UserType FROM Users where Username = '{Username}';";
                var readme = command.ExecuteReader();
                while (readme.Read())
                {
                    checker = BCrypt.Net.BCrypt.Verify(Password, readme.GetString(1));
                    adminOrEmp = (readme.GetString(2));
                }
            }
            if (checker)
            {
                session = AddSessionForUser(Username);
                session.UserType = adminOrEmp;
                return session;
            }
        }
        return null;
    }

    public static void AddRoomTypes(RoomTypesModel user)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                @"INSERT INTO RoomTypes  (RoomName, ImagePath, RoomSize, BedDetails, RoomCapacity, RoomRate, RoomQuantity) VALUES (@RoomName, @ImagePath, @RoomSize, @BedDetails, @RoomCapacity, @RoomRate, @RoomQuantity);";
                command.Parameters.AddWithValue("@RoomName", user.RoomName);
                command.Parameters.AddWithValue("@ImagePath", user.ImagePath);
                command.Parameters.AddWithValue("@RoomSize", user.RoomSize);
                command.Parameters.AddWithValue("@BedDetails", user.BedDetails);
                command.Parameters.AddWithValue("@RoomCapacity", user.RoomCapacity);
                command.Parameters.AddWithValue("@RoomRate", user.RoomRate);
                command.Parameters.AddWithValue("@RoomQuantity", user.RoomQuantity);
                command.ExecuteNonQuery();
            }
        }
    }

    public static void DeleteARoomType(int Id)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"DELETE FROM RoomTypes where Id = {Id};";
                command.ExecuteNonQuery();
            }
        }
    }

    public static void AddRooms(RoomsModel user)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                @"INSERT INTO Rooms  (RoomTypeId, RoomNumber, RoomStatus) VALUES (@RoomTypeId, @RoomNumber, @RoomStatus);";
                command.Parameters.AddWithValue("@RoomTypeId", user.RoomTypeId);
                command.Parameters.AddWithValue("@RoomNumber", user.RoomNumber);
                command.Parameters.AddWithValue("@RoomStatus", String.IsNullOrEmpty(user.RoomStatus) ? "Active" : user.RoomStatus); //Default value to "active"
                command.ExecuteNonQuery();
            }
        }
    }

    public static void DeleteARoom(int Id)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"DELETE FROM Rooms where Id = {Id};";
                command.ExecuteNonQuery();
            }
        }
    }

    public static void AddDining(DiningModel user)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                @"INSERT INTO Dining  (DiningName, About, ImagePath, DiningCapacity, TableQuantity) VALUES (@DiningName, @About, @ImagePath, @DiningCapacity, @TableQuantity);";
                command.Parameters.AddWithValue("@DiningName", user.DiningName);
                command.Parameters.AddWithValue("@About", user.About);
                command.Parameters.AddWithValue("@ImagePath", user.ImagePath);
                command.Parameters.AddWithValue("@DiningCapacity", user.DiningCapacity);
                command.Parameters.AddWithValue("@TableQuantity", user.TableQuantity);
                command.ExecuteNonQuery();
            }
        }
    }

    public static void DeleteADining(int Id)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"DELETE FROM Dining where Id = {Id};";
                command.ExecuteNonQuery();
            }
        }
    }

    public static void AddDiningTables(DiningTablesModel user)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                @"INSERT INTO DiningTables  (DiningId, TableCapacity, DiningStatus) VALUES (@DiningId, @TableCapacity, @DiningStatus);";
                command.Parameters.AddWithValue("@DiningId", user.DiningId);
                command.Parameters.AddWithValue("@TableCapacity", user.TableCapacity);
                command.Parameters.AddWithValue("@DiningStatus", String.IsNullOrEmpty(user.DiningStatus) ? "Active" : user.DiningStatus);
                command.ExecuteNonQuery();
            }
        }
    }

    public static void DeleteADiningTable(int Id)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"DELETE FROM DiningTables where Id = {Id};";
                command.ExecuteNonQuery();
            }
        }
    }

    public static void AddVoucher(VouchersModel user)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                @"INSERT INTO Vouchers  (Code, SalePrice, VoucherStatus) VALUES (@Code, @SalePrice, @VoucherStatus);";
                command.Parameters.AddWithValue("@Code", user.Code);
                command.Parameters.AddWithValue("@SalePrice", user.SalePrice);
                command.Parameters.AddWithValue("@VoucherStatus", String.IsNullOrEmpty(user.VoucherStatus) ? "Active" : user.VoucherStatus);
                command.ExecuteNonQuery();
            }
        }
    }

    public static void DeleteAVoucher(int Id)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"DELETE FROM Vouchers where Id = {Id};";
                command.ExecuteNonQuery();
            }
        }
    }

    public static void AddHotelBooking(HotelBookingsModel user)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                @"INSERT INTO HotelBookings  (GuestName, NoOfGuests, RoomTypeId, RoomId, BookingDate, CheckInDate, CheckOutDate, TotalPrice, PaymentType, BookingStatus) VALUES (@GuestName, @NoOfGuests, @RoomTypeId, @RoomId, @BookingDate, @CheckInDate, @CheckOutDate, @TotalPrice, @PaymentType, @BookingStatus);";
                command.Parameters.AddWithValue("@GuestName", user.GuestName);
                command.Parameters.AddWithValue("@NoOfGuests", user.NoOfGuests);
                command.Parameters.AddWithValue("@RoomTypeId", user.RoomTypeId);
                command.Parameters.AddWithValue("@RoomId", user.RoomId);
                command.Parameters.AddWithValue("@BookingDate", user.BookingDate);
                command.Parameters.AddWithValue("@CheckInDate", user.CheckInDate);
                command.Parameters.AddWithValue("@CheckOutDate", user.CheckOutDate);
                command.Parameters.AddWithValue("@TotalPrice", user.TotalPrice);
                command.Parameters.AddWithValue("@PaymentType", user.PaymentType);
                command.Parameters.AddWithValue("@BookingStatus", String.IsNullOrEmpty(user.BookingStatus) ? "Booked" : user.BookingStatus);
                command.ExecuteNonQuery();
            }
        }
    }

    public static void DeleteAHotelBooking(int Id)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"DELETE FROM HotelBookings where Id = {Id};";
                command.ExecuteNonQuery();
            }
        }
    }

    public static void AddDiningBooking(DiningBookingsModel user)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                @"INSERT INTO DiningBookings  (GuestName, NoOfGuests, DiningId, TableId, BookingDate, CheckInDate, PaymentType, BookingStatus) VALUES (@GuestName, @NoOfGuests, @DiningId, @TableId, @BookingDate, @CheckInDate, @PaymentType, @BookingStatus);";
                command.Parameters.AddWithValue("@GuestName", user.GuestName);
                command.Parameters.AddWithValue("@NoOfGuests", user.NoOfGuests);
                command.Parameters.AddWithValue("@DiningId", user.DiningId);
                command.Parameters.AddWithValue("@TableId", user.TableId);
                command.Parameters.AddWithValue("@BookingDate", user.BookingDate);
                command.Parameters.AddWithValue("@CheckInDate", user.CheckInDate);
                command.Parameters.AddWithValue("@PaymentType", user.PaymentType);
                command.Parameters.AddWithValue("@BookingStatus", String.IsNullOrEmpty(user.BookingStatus) ? "Booked" : user.BookingStatus);
                command.ExecuteNonQuery();
            }
        }
    }

    public static void DeleteADiningBooking(int Id)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"DELETE FROM DiningBookings where Id = {Id};";
                command.ExecuteNonQuery();
            }
        }
    }

    public static List<RoomTypesModel> GetRoomTypes()
    {
        List<RoomTypesModel> users = new List<RoomTypesModel>();
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM RoomTypes;";
                var readme = command.ExecuteReader();
                while (readme.Read())
                {
                    users.Add(new RoomTypesModel
                    {
                        Id = readme.GetInt32(0),
                        RoomName = readme.GetString(1),
                        ImagePath = readme.GetString(2),
                        RoomSize = readme.GetString(3),
                        BedDetails = readme.GetString(4),
                        RoomCapacity = readme.GetInt32(5),
                        RoomRate = readme.GetInt32(6),
                        RoomQuantity = readme.GetInt32(7)
                    });
                }
            }
        }
        return users;
    }

    public static List<RoomsAndRoomTypesModel> GetRooms()
    {
        List<RoomsAndRoomTypesModel> rooms = new List<RoomsAndRoomTypesModel>();
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT Rooms.Id, RoomTypes.RoomName, Rooms.RoomNumber, Rooms.RoomStatus FROM Rooms INNER JOIN RoomTypes ON Rooms.RoomTypeId=RoomTypes.Id;";
                var readme = command.ExecuteReader();
                while (readme.Read())
                {
                    rooms.Add(new RoomsAndRoomTypesModel
                    {
                        Id = readme.GetInt32(0),
                        RoomName = readme.GetString(1),
                        RoomNumber = readme.GetInt32(2),
                        RoomStatus = readme.GetString(3),
                    });
                }
            }
        }
        return rooms;
    }

    public static List<DiningModel> GetDining()
    {
        List<DiningModel> users = new List<DiningModel>();
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM Dining;";
                var readme = command.ExecuteReader();
                while (readme.Read())
                {
                    users.Add(new DiningModel
                    {
                        Id = readme.GetInt32(0),
                        DiningName = readme.GetString(1),
                        About = readme.GetString(2),
                        ImagePath = readme.GetString(3),
                        DiningCapacity = readme.GetInt32(4),
                        TableQuantity = readme.GetInt32(5)
                    });
                }
            }
        }
        return users;
    }

    public static List<DiningAndDiningTables> GetDiningTable()
    {
        List<DiningAndDiningTables> rooms = new List<DiningAndDiningTables>();
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT DiningTables.Id, Dining.DiningName, DiningTables.TableCapacity, DiningTables.DiningStatus FROM DiningTables INNER JOIN Dining ON DiningTables.DiningId=Dining.Id;";
                var readme = command.ExecuteReader();
                while (readme.Read())
                {
                    rooms.Add(new DiningAndDiningTables
                    {
                        Id = readme.GetInt32(0),
                        DiningName = readme.GetString(1),
                        TableCapacity = readme.GetInt32(2),
                        DiningStatus = readme.GetString(3),
                    });
                }
            }
        }
        return rooms;
    }

    public static List<VouchersModel> GetVouchers()
    {
        List<VouchersModel> users = new List<VouchersModel>();
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM Vouchers;";
                var readme = command.ExecuteReader();
                while (readme.Read())
                {
                    users.Add(new VouchersModel
                    {
                        Id = readme.GetInt32(readme.GetOrdinal("Id")),
                        Code = readme.GetString(readme.GetOrdinal("Code")),
                        SalePrice = readme.GetInt32(readme.GetOrdinal("SalePrice")),
                        VoucherStatus = readme.GetString(readme.GetOrdinal("VoucherStatus")),
                    });
                }
            }
        }
        return users;
    }

    public static List<HotelBookingsModel> GetHotelBooking()
    {
        List<HotelBookingsModel> users = new List<HotelBookingsModel>();
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM HotelBookings;";
                var readme = command.ExecuteReader();
                while (readme.Read())
                {
                    users.Add(new HotelBookingsModel
                    {
                        Id = readme.GetInt32(readme.GetOrdinal("Id")),
                        GuestName = readme.GetString(readme.GetOrdinal("GuestName")),
                        NoOfGuests = readme.GetInt32(readme.GetOrdinal("NoOfGuests")),
                        RoomTypeId = readme.GetInt32(readme.GetOrdinal("RoomTypeId")),
                        RoomId = readme.GetInt32(readme.GetOrdinal("RoomId")),
                        BookingDate = readme.GetInt64(readme.GetOrdinal("BookingDate")),
                        CheckInDate = readme.GetInt64(readme.GetOrdinal("CheckInDate")),
                        CheckOutDate = readme.GetInt64(readme.GetOrdinal("CheckOutDate")),
                        TotalPrice = readme.GetInt32(readme.GetOrdinal("TotalPrice")),
                        PaymentType = readme.GetString(readme.GetOrdinal("PaymentType")),
                        BookingStatus = readme.GetString(readme.GetOrdinal("BookingStatus"))
                    });
                }
            }
        }
        return users;
    }

    public static List<DiningBookingsModel> GetDiningBooking()
    {
        List<DiningBookingsModel> users = new List<DiningBookingsModel>();
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM DiningBookings;";
                var readme = command.ExecuteReader();
                while (readme.Read())
                {
                    users.Add(new DiningBookingsModel
                    {
                        Id = readme.GetInt32(readme.GetOrdinal("Id")),
                        GuestName = readme.GetString(readme.GetOrdinal("GuestName")),
                        DiningId = readme.GetInt32(readme.GetOrdinal("DiningId")),
                        TableId = readme.GetInt32(readme.GetOrdinal("TableId")),
                        BookingDate = readme.GetInt64(readme.GetOrdinal("BookingDate")),
                        CheckInDate = readme.GetInt64(readme.GetOrdinal("CheckInDate")),
                        PaymentType = readme.GetString(readme.GetOrdinal("PaymentType")),
                        BookingStatus = readme.GetString(readme.GetOrdinal("BookingStatus"))
                    });
                }
            }
        }
        return users;
    }

    public static void ModifyARoomType(int Id, RoomTypesModel model)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"UPDATE RoomTypes SET RoomName=@RoomName, ImagePath=@ImagePath, RoomSize=@RoomSize, BedDetails=@BedDetails, RoomCapacity=@RoomCapacity, RoomRate=@RoomRate, RoomQuantity=@RoomQuantity WHERE Id={Id};";
                if (model.RoomName != null)
                {
                    command.Parameters.AddWithValue("@RoomName", model.RoomName);
                }
                if (model.ImagePath != null)
                {
                    command.Parameters.AddWithValue("@ImagePath", model.ImagePath);
                }
                if (model.RoomSize != null)
                {
                    command.Parameters.AddWithValue("@RoomSize", model.RoomSize);
                }
                if (model.BedDetails != null)
                {
                    command.Parameters.AddWithValue("@BedDetails", model.BedDetails);
                }
                if (model.RoomCapacity != null)
                {
                    command.Parameters.AddWithValue("@RoomCapacity", model.RoomCapacity);
                }
                if (model.RoomRate != null)
                {
                    command.Parameters.AddWithValue("@RoomRate", model.RoomRate);
                }
                if (model.RoomQuantity != null)
                {
                    command.Parameters.AddWithValue("@RoomQuantity", model.RoomQuantity);
                }
                command.ExecuteNonQuery();
            }
        }
    }

    public static void ModifyRooms(int Id, UpdateRoomsModel model)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"UPDATE Rooms SET RoomStatus=@RoomStatus WHERE Id={Id};";
                if (model.RoomStatus != null)
                {
                    command.Parameters.AddWithValue("@RoomStatus", model.RoomStatus);
                }
                command.ExecuteNonQuery();
            }
        }
    }

    public static void ModifyDining(int Id, DiningModel model)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"UPDATE Dining SET DiningName=@DiningName, About=@About, ImagePath=@ImagePath, DiningCapacity=@DiningCapacity, TableQuantity=@TableQuantity WHERE Id={Id};";
                if (model.DiningName != null)
                {
                    command.Parameters.AddWithValue("@DiningName", model.DiningName);
                }
                if (model.About != null)
                {
                    command.Parameters.AddWithValue("@About", model.About);
                }
                if (model.ImagePath != null)
                {
                    command.Parameters.AddWithValue("@ImagePath", model.ImagePath);
                }
                if (model.DiningCapacity != null)
                {
                    command.Parameters.AddWithValue("@DiningCapacity", model.DiningCapacity);
                }
                if (model.TableQuantity != null)
                {
                    command.Parameters.AddWithValue("@TableQuantity", model.TableQuantity);
                }
                command.ExecuteNonQuery();
            }
        }
    }

    public static void ModifyDiningTable(int Id, UpdateDiningTableModel model)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"UPDATE DiningTable SET DiningStatus=@DiningStatus WHERE Id={Id};";
                if (model.DiningStatus != null)
                {
                    command.Parameters.AddWithValue("@DiningStatus", model.DiningStatus);
                }
                command.ExecuteNonQuery();
            }
        }
    }

    public static void ModifyVoucher(int Id, UpdateVoucherModel model)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"UPDATE Vouchers SET VoucherStatus=@VoucherStatus WHERE Id={Id};";
                if (model.VoucherStatus != null)
                {
                    command.Parameters.AddWithValue("@VoucherStatus", model.VoucherStatus);
                }
                command.ExecuteNonQuery();
            }
        }
    }

    public static void ModifyHotelBooking(int Id, UpdateBookingModel model)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"UPDATE HotelBookings SET TotalPrice=@TotalPrice, PaymentType=@PaymentType, BookingStatus=@BookingStatus WHERE Id={Id};";
                if (model.TotalPrice != null)
                {
                    command.Parameters.AddWithValue("@TotalPrice", model.TotalPrice);
                }
                if (model.PaymentType != null)
                {
                    command.Parameters.AddWithValue("@PaymentType", model.PaymentType);
                }
                if (model.BookingStatus != null)
                {
                    command.Parameters.AddWithValue("@BookingStatus", model.BookingStatus);
                }
                command.ExecuteNonQuery();
            }
        }
    }

    public static void ModifyDiningBooking(int Id, UpdateBookingModel model)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"UPDATE DiningBookings SET TotalPrice=@TotalPrice, PaymentType=@PaymentType, BookingStatus=@BookingStatus WHERE Id={Id};";
                if (model.TotalPrice != null)
                {
                    command.Parameters.AddWithValue("@TotalPrice", model.TotalPrice);
                }
                if (model.PaymentType != null)
                {
                    command.Parameters.AddWithValue("@PaymentType", model.PaymentType);
                }
                if (model.BookingStatus != null)
                {
                    command.Parameters.AddWithValue("@BookingStatus", model.BookingStatus);
                }
                command.ExecuteNonQuery();
            }
        }
    }

    public static Dictionary<int, RoomTypesModel> GetRoomSelection(int guestNo, long CheckInDate, long CheckOutDate)
    {
        Dictionary<int, RoomTypesModel> users = new Dictionary<int, RoomTypesModel>();
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT Rooms.Id AS RoomId,Rooms.RoomNumber,RoomTypes.* " + 
                    "FROM Rooms LEFT JOIN RoomTypes " +
                    " ON Rooms.RoomTypeId=RoomTypes.Id " + 
                    " LEFT JOIN HotelBookings ON Rooms.Id=HotelBookings.RoomId " + 
                   $" WHERE (HotelBookings.Id IS NULL AND RoomTypes.RoomCapacity>='{guestNo}') OR " +
                   $" (RoomTypes.RoomCapacity>='{guestNo}' AND " +
                    $" (NOT (HotelBookings.CheckInDate<='{CheckInDate}' AND HotelBookings.CheckoutDate>'{CheckInDate}') AND " + 
                    $" NOT(HotelBookings.CheckInDate<'{CheckOutDate}' AND HotelBookings.CheckoutDate>='{CheckOutDate}')));";
                var readme = command.ExecuteReader();
                List<int> existingRoomType = new List<int>();
                while (readme.Read())
                {
                    int roomTypeId = readme.GetInt32(readme.GetOrdinal("Id"));

                    if(!existingRoomType.Contains(roomTypeId))
                    {
                        var roomTypes = new RoomTypesModel();
                        roomTypes.Id = roomTypeId;
                        roomTypes.RoomName = readme.GetString(readme.GetOrdinal("RoomName"));
                        roomTypes.ImagePath = readme.GetString(readme.GetOrdinal("ImagePath"));
                        roomTypes.RoomSize = readme.GetString(readme.GetOrdinal("RoomSize"));
                        roomTypes.BedDetails = readme.GetString(readme.GetOrdinal("BedDetails"));
                        roomTypes.RoomCapacity = readme.GetInt32(readme.GetOrdinal("RoomCapacity"));
                        roomTypes.RoomRate = readme.GetInt32(readme.GetOrdinal("RoomRate"));
                        roomTypes.RoomQuantity = readme.GetInt32(readme.GetOrdinal("RoomQuantity"));

                        int roomId = readme.GetInt32(readme.GetOrdinal("RoomId"));
                        users.Add(roomId, roomTypes);

                        existingRoomType.Add(roomTypeId);
                    } 
                }
            }
        }
        return users;
    }

     public static int GetVoucherSalePrice(string voucher)
    {
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT SalePrice FROM Vouchers WHERE Code='{voucher}';";
                var readme = command.ExecuteReader();
                while(readme.Read())
                {
                   int salePrice = readme.GetInt32(readme.GetOrdinal("SalePrice"));
                   return salePrice;
                }
            }
        }
        return 0;
    }

    public static Dictionary<int, DiningModel> GetDiningSelection(int guestNo)
    {
        Dictionary<int, DiningModel> users = new Dictionary<int, DiningModel>();
        using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT DiningTables.Id AS TablesId, DiningTables.DiningId, DiningTables.TableCapacity, Dining.* FROM DiningTables LEFT JOIN Dining ON DiningTables.DiningId=Dining.Id LEFT JOIN DiningBookings ON DiningTables.Id=DiningBookings.TableId WHERE (DiningBookings.Id IS NULL AND Dining.DiningCapacity>='{guestNo}');";
                var readme = command.ExecuteReader();
                List<int> existingDining = new List<int>();
                while (readme.Read())
                {
                    int DiningId = readme.GetInt32(readme.GetOrdinal("Id"));    

                    if(!existingDining.Contains(DiningId))
                    {
                        var Dining = new DiningModel();
                        Dining.Id = DiningId;
                        Dining.DiningName = readme.GetString(readme.GetOrdinal("DiningName"));
                        Dining.ImagePath = readme.GetString(readme.GetOrdinal("ImagePath"));
                        Dining.About = readme.GetString(readme.GetOrdinal("About"));
                        Dining.DiningCapacity = readme.GetInt32(readme.GetOrdinal("DiningCapacity"));
                        Dining.TableQuantity = readme.GetInt32(readme.GetOrdinal("TableQuantity"));

                        int TableId = readme.GetInt32(readme.GetOrdinal("TablesId"));
                        users.Add(TableId, Dining);

                        existingDining.Add(DiningId);
                    } 
                }
            }
        }
        return users;
    }
}