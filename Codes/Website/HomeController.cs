using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System;
using System.Text;

public class HomeController : Controller
{
    [Route("/")]
    [Route("/EntryPage")]
    [Route("/home")]
    public IActionResult doIndexAction()
    {
        return View("Views/EntryPage.cshtml");
    }

    [Route("/LandingPage")]
    public IActionResult doLandAction()
    {
        return View("Views/LandingPage.cshtml");
    }

    [Route("/AboutPage")]
    public IActionResult doAboutAction()
    {
        return View("Views/AboutPage.cshtml");
    }

    [Route("/StayWithUsPage")]
    public IActionResult doStayAction()
    {
        var roomtypes = Database.GetRoomTypes();
        return View("Views/StayWithUsPage.cshtml", roomtypes);
    }

    [Route("/DineWithUsPage")]
    public IActionResult doDineAction()
    {
        var diningrestau = Database.GetDining();
        return View("Views/DineWithUsPage.cshtml", diningrestau);
    }

    [Route("/OffersPage")]
    public IActionResult doOffersAction()
    {
        var voucher = Database.GetVouchers();
        return View("Views/OffersPage.cshtml", voucher);
    }

    [Route("/ReachUsPage")]
    public IActionResult doReachAction()
    {
        return View("Views/ReachUsPage.cshtml");
    }

    [Route("/ReservationFormPage")]
    public IActionResult doStayFormAction()
    {
        var rooms = Database.GetRooms();
        return View("Views/ReservationFormPage.cshtml", rooms);
    }

    [Route("/DiningReservationFormPage")]
    public IActionResult doDiningFormAction()
    {
        var dining = Database.GetDining();
        return View("Views/DiningReservationFormPage.cshtml", dining);
    }

    [Route("/ConfirmationPage")]
    public IActionResult doConfirmationAction()
    {
        return View("Views/ConfirmationPage.cshtml");
    }

    [Route("/RoomReservationPage")]
    public IActionResult doRoomReservationAction()
    {
        return View("Views/RoomReservationPage.cshtml");
    }


    [HttpPost]
    [Route("/configure")]
    public IActionResult ConfigTable([FromBody] ConfigureActionModel param, [FromHeader(Name = "X-ApiKey")] string ApiKey)
    {
        var CorrectApiKey = Environment.GetEnvironmentVariable("API_KEY");
        var act = param.Action;
        if (ApiKey != CorrectApiKey)
        {
            return Unauthorized();
        }
        else
        {
            if (act.Equals("CreateTables"))
            {
                Database.CreateTables();
                return Ok("Tables have been created!");
            }
            if (act.Equals("CreateWebTables"))
            {
                Database.CreateWebTables();
                return Ok("Web Tables have been created!");
            }
            if (act.Equals("CreateBookingTables"))
            {
                Database.CreateBookingTables();
                return Ok("Booking Tables have been created!");
            }
        }
        return Ok("Error: Invalid parameters.");
    }

    [HttpPost]
    [Route("/users")]
    public IActionResult AddUser([FromBody] UsersModel user, [FromHeader(Name = "X-ApiKey")] string ApiKey)
    {
        var CorrectApiKey = Environment.GetEnvironmentVariable("API_KEY");
        if (ApiKey != CorrectApiKey)
        {
            return Unauthorized();
        }
        else
        {
            Database.AddUser(user);
            return Ok("User successfully added!");
        }
    }

    [HttpPost]
    [Route("/roomtypes")]
    public IActionResult AddRoomType([FromBody] RoomTypesModel user, [FromHeader(Name = "X-ApiKey")] string ApiKey)
    {
        var CorrectApiKey = Environment.GetEnvironmentVariable("API_KEY");
        if (ApiKey != CorrectApiKey)
        {
            return Unauthorized();
        }
        else
        {
            Database.AddRoomTypes(user);
            return Ok("Room Type successfully added!");
        }
    }

    [HttpPost]
    [Route("/rooms")]
    public IActionResult AddRoom([FromBody] RoomsModel user, [FromHeader(Name = "X-ApiKey")] string ApiKey)
    {
        var CorrectApiKey = Environment.GetEnvironmentVariable("API_KEY");
        if (ApiKey != CorrectApiKey)
        {
            return Unauthorized();
        }
        else
        {
            Database.AddRooms(user);
            return Ok("Room successfully added!");
        }
    }

    [HttpPost]
    [Route("/dining")]
    public IActionResult AddDiningTable([FromBody] DiningModel user, [FromHeader(Name = "X-ApiKey")] string ApiKey)
    {
        var CorrectApiKey = Environment.GetEnvironmentVariable("API_KEY");
        if (ApiKey != CorrectApiKey)
        {
            return Unauthorized();
        }
        else
        {
            Database.AddDining(user);
            return Ok("Dining Restaurant successfully added!");
        }
    }

    [HttpPost]
    [Route("/diningtable")]
    public IActionResult AddDiningTable([FromBody] DiningTablesModel user, [FromHeader(Name = "X-ApiKey")] string ApiKey)
    {
        var CorrectApiKey = Environment.GetEnvironmentVariable("API_KEY");
        if (ApiKey != CorrectApiKey)
        {
            return Unauthorized();
        }
        else
        {
            Database.AddDiningTables(user);
            return Ok("Dining Table successfully added!");
        }
    }

    [HttpPost]
    [Route("/vouchers")]
    public IActionResult AddVoucher([FromBody] VouchersModel user, [FromHeader(Name = "X-ApiKey")] string ApiKey)
    {
        var CorrectApiKey = Environment.GetEnvironmentVariable("API_KEY");
        if (ApiKey != CorrectApiKey)
        {
            return Unauthorized();
        }
        else
        {
            Database.AddVoucher(user);
            return Ok("Voucher successfully added!");
        }
    }

    [HttpPost]
    [Route("/hotelbookings")] //for postman use
    public IActionResult AddHotelBookings([FromBody] HotelBookingsModel user, [FromHeader(Name = "X-ApiKey")] string ApiKey)
    {
        var CorrectApiKey = Environment.GetEnvironmentVariable("API_KEY");
        if (ApiKey != CorrectApiKey)
        {
            return Unauthorized();
        }
        else
        {
            Database.AddHotelBooking(user);
            return Ok("Hotel Reservation successfully added!");
        }
    }

    [HttpPost]
    [Route("/bookaroom")] //Form to back end (step 1) then will return page that shows list of available rooms (page 2).
    public IActionResult doHotelBookingPost()
    {
        var checkin = HttpContext.Request.Form["checkin"];
        var checkout = HttpContext.Request.Form["checkout"];
        DateTime dateofCheckIn = DateTime.Parse(checkin); //convert string to datetime
        DateTime dateofCheckOut = DateTime.Parse(checkout);
        var CheckInDate = (long)((dateofCheckIn.Subtract(new System.DateTime(1970, 1, 1, 0, 0, 0, 0))).TotalSeconds); //convert datetime to unix time (long readable)
        var CheckOutDate = (long)((dateofCheckOut.Subtract(new System.DateTime(1970, 1, 1, 0, 0, 0, 0))).TotalSeconds);
        var guest = HttpContext.Request.Form["gname"];
        var noofguest = int.Parse(HttpContext.Request.Form["guests"]);
        var payment = HttpContext.Request.Form["payment"];
        var voucher = HttpContext.Request.Form["voucher"];
        var model = new HotelBookingsModel();
        model.CheckInDate = CheckInDate;
        model.CheckOutDate = CheckOutDate;
        model.GuestName = guest;
        model.NoOfGuests = noofguest;
        model.PaymentType = payment;
        model.VoucherCode = voucher;
        model.VoucherCodeDiscount = Database.GetVoucherSalePrice(voucher);
        var model2 = new RoomSelectionModel();
        model2.hotelbooking = model;
        model2.rooms = Database.GetRoomSelection(noofguest, CheckInDate, CheckOutDate);
        return View("/Views/RoomReservationPage.cshtml", model2);
    }

    [HttpPost]
    [Route("/roomselected")] // page 2 to back end (step 2) then will return confirmation page (that will add to database)
    public IActionResult doRoomSelectedPost()
    {
        var checkin = HttpContext.Request.Form["checkin"];
        var checkout = HttpContext.Request.Form["checkout"];
        var guest = HttpContext.Request.Form["gname"];
        var noofguest = int.Parse(HttpContext.Request.Form["guests"]);
        var payment = HttpContext.Request.Form["payment"];
        var roomType = int.Parse(HttpContext.Request.Form["roomType"]);
        var roomrate = int.Parse(HttpContext.Request.Form["roomrate"]);
        var room = int.Parse(HttpContext.Request.Form["room"]);
        var roomname = HttpContext.Request.Form["roomname"];
        var voucher = HttpContext.Request.Form["voucher"];
        var voucherSalePrice = int.Parse(HttpContext.Request.Form["voucherSalePrice"]);
        DateTime start = LongToDateTime(long.Parse(checkin));
        DateTime end = LongToDateTime(long.Parse(checkout));
        DateTime datenow = DateTime.Now;
        var bookingDate =  (long)((datenow.Subtract(new System.DateTime(1970, 1, 1, 0, 0, 0, 0))).TotalSeconds);
        var noOfNights = (end-start).TotalDays;
        var priceBeforeDiscount = Convert.ToInt32(noOfNights*roomrate);
        var totalprice = Convert.ToInt32(noOfNights*roomrate-voucherSalePrice);
        var model = new HotelBookingsModel();
        model.CheckInDateString = start.ToShortDateString();
        model.CheckOutDateString = end.ToShortDateString();
        model.CheckInDate = long.Parse(checkin);
        model.CheckOutDate = long.Parse(checkout);
        model.BookingDate = bookingDate;
        model.GuestName = guest;
        model.NoOfGuests = noofguest;
        model.PaymentType = payment;
        model.RoomTypeId = roomType;
        model.RoomId = room;
        model.PriceBeforeDiscount = priceBeforeDiscount;
        model.TotalPrice = totalprice;
        model.VoucherCode = voucher;
        model.VoucherCodeDiscount = voucherSalePrice;
        return View("/Views/ConfirmationPage.cshtml", model);
    }

    [HttpPost]
    [Route("/roombookings")] //for direct website use. Adding reservation to database. (step 3)
    public IActionResult AddRoomBookings()
    {
        var checkin = HttpContext.Request.Form["checkin"];
        var checkout = HttpContext.Request.Form["checkout"];
        var guest = HttpContext.Request.Form["gname"];
        var noofguest = int.Parse(HttpContext.Request.Form["guests"]);
        var payment = HttpContext.Request.Form["payment"];
        var roomType = int.Parse(HttpContext.Request.Form["roomType"]);
        var roomrate = int.Parse(HttpContext.Request.Form["roomrate"]);
        var room = int.Parse(HttpContext.Request.Form["room"]);
        var roomname =HttpContext.Request.Form["roomname"];
        DateTime start = LongToDateTime(long.Parse(checkin));
        DateTime end = LongToDateTime(long.Parse(checkout));
        DateTime datenow = DateTime.Now;
        var bookingDate =  (long)((datenow.Subtract(new System.DateTime(1970, 1, 1, 0, 0, 0, 0))).TotalSeconds);
        var noOfNights = (end-start).TotalDays;
        var totalprice = Convert.ToInt32(noOfNights*roomrate);
        var model = new HotelBookingsModel();
        model.CheckInDateString = start.ToShortDateString();
        model.CheckOutDateString = end.ToShortDateString();
        model.CheckInDate = long.Parse(checkin);
        model.CheckOutDate = long.Parse(checkout);
        model.BookingDate = bookingDate;
        model.GuestName = guest;
        model.NoOfGuests = noofguest;
        model.PaymentType = payment;
        model.RoomTypeId = roomType;
        model.RoomId = room;
        model.TotalPrice = totalprice;
        Database.AddHotelBooking(model);
        return View("/Views/ReservationSuccessful.cshtml");
    }

    [HttpPost]
    [Route("/bookadining")] //Form to back end (step 1) then will return page that shows list of available restaurants (page 2).
    public IActionResult doHotelDiningPost()
    {
        var checkin = HttpContext.Request.Form["checkin"];
        DateTime dateofCheckIn = DateTime.Parse(checkin); //convert string to datetime
        var CheckInDate = (long)((dateofCheckIn.Subtract(new System.DateTime(1970, 1, 1, 0, 0, 0, 0))).TotalSeconds); //convert datetime to unix time (long readable)
        var guest = HttpContext.Request.Form["gname"];
        var noofguest = int.Parse(HttpContext.Request.Form["guests"]);
        var payment = HttpContext.Request.Form["payment"];
        var model = new DiningBookingsModel();
        model.CheckInDate = CheckInDate;
        model.GuestName = guest;
        model.NoOfGuests = noofguest;
        model.PaymentType = payment;
        var model2 = new DiningSelectionModel();
        model2.diningbooking = model;
        model2.tables = Database.GetDiningSelection(noofguest);
        return View("/Views/DiningReservationPage.cshtml", model2);
    }

    [HttpPost]
    [Route("/tableselected")] //page 2 to back end (step 2) then will return confirmation page (that will add to database)
    public IActionResult doTableSelectedPost()
    {
        var checkin = HttpContext.Request.Form["checkin"];
        var guest = HttpContext.Request.Form["gname"];
        var noofguest = int.Parse(HttpContext.Request.Form["guests"]);
        var payment = HttpContext.Request.Form["payment"];
        var dining = int.Parse(HttpContext.Request.Form["dining"]);
        var table = int.Parse(HttpContext.Request.Form["table"]);
        DateTime start = LongToDateTime(long.Parse(checkin));
        DateTime datenow = DateTime.Now;
        var bookingDate =  (long)((datenow.Subtract(new System.DateTime(1970, 1, 1, 0, 0, 0, 0))).TotalSeconds);
        var model = new DiningBookingsModel();
        model.CheckInDateString = start.ToShortDateString();
        model.CheckInDate = long.Parse(checkin);
        model.BookingDate = bookingDate;
        model.GuestName = guest;
        model.NoOfGuests = noofguest;
        model.PaymentType = payment;
        model.DiningId = dining;
        model.TableId = table;
        return View("/Views/DiningConfirmationPage.cshtml", model);
    }

    [HttpPost]
    [Route("/restaubookings")] //for direct website use. Adding reservation to database. (step 3)
    public IActionResult AddRestaurantBookings()
    {
        var checkin = HttpContext.Request.Form["checkin"];
        var guest = HttpContext.Request.Form["gname"];
        var noofguest = int.Parse(HttpContext.Request.Form["guests"]);
        var payment = HttpContext.Request.Form["payment"];
        var dining = int.Parse(HttpContext.Request.Form["dining"]);
        var table = int.Parse(HttpContext.Request.Form["table"]);
        DateTime start = LongToDateTime(long.Parse(checkin));
        DateTime datenow = DateTime.Now;
        var bookingDate =  (long)((datenow.Subtract(new System.DateTime(1970, 1, 1, 0, 0, 0, 0))).TotalSeconds);
        var model = new DiningBookingsModel();
        model.CheckInDateString = start.ToShortDateString();
        model.CheckInDate = long.Parse(checkin);
        model.BookingDate = bookingDate;
        model.GuestName = guest;
        model.NoOfGuests = noofguest;
        model.PaymentType = payment;
        model.DiningId = dining;
        model.TableId = table;
        Database.AddDiningBooking(model);
        return View("/Views/ReservationSuccessful.cshtml");
    }


    [HttpPost]
    [Route("/diningbookings")]
    public IActionResult AddDiningBookings([FromBody] DiningBookingsModel user, [FromHeader(Name = "X-ApiKey")] string ApiKey)
    {
        var CorrectApiKey = Environment.GetEnvironmentVariable("API_KEY");
        if (ApiKey != CorrectApiKey)
        {
            return Unauthorized();
        }
        else
        {
            Database.AddDiningBooking(user);
            return Ok("Dining Reservation successfully added!");
        }
    }

    [HttpPost]
    [Route("/login")]
    public IActionResult AddSessions([FromBody] UserCredentialsModel session)
    {
        var holder = Database.AddSessionWithCredentials(session.UserName, session.Password);
        if (holder == null)
        {
            return Unauthorized();
        }
        else
        {
            Database.AddSession(holder);
            return (Json(holder));
        }
    }

    [HttpGet]
    [Route("/rooms")]
    public IActionResult ListRooms([FromHeader(Name = "X-ApiKey")] string ApiKey)
    {
        var CorrectApiKey = Environment.GetEnvironmentVariable("API_KEY");
        if (ApiKey != CorrectApiKey)
        {
            return Unauthorized();
        }
        else
        {
            var rooms = Database.GetRooms();
            var jsonderulo = JsonSerializer.Serialize(rooms, new JsonSerializerOptions { WriteIndented = true });
            return Ok(jsonderulo);
        }
    }

    [HttpGet]
    [Route("/roomtypes")]
    public IActionResult ListRoomTypes([FromHeader(Name = "X-ApiKey")] string ApiKey)
    {
        var CorrectApiKey = Environment.GetEnvironmentVariable("API_KEY");
        if (ApiKey != CorrectApiKey)
        {
            return Unauthorized();
        }
        else
        {
            var rooms = Database.GetRoomTypes();
            var jsonderulo = JsonSerializer.Serialize(rooms, new JsonSerializerOptions { WriteIndented = true });
            return Ok(jsonderulo);
        }
    }

    [HttpGet]
    [Route("/diningtables")]
    public IActionResult ListDiningTables([FromHeader(Name = "X-ApiKey")] string ApiKey)
    {
        var CorrectApiKey = Environment.GetEnvironmentVariable("API_KEY");
        if (ApiKey != CorrectApiKey)
        {
            return Unauthorized();
        }
        else
        {
            var dining = Database.GetDiningTable();
            var jsonderulo = JsonSerializer.Serialize(dining, new JsonSerializerOptions { WriteIndented = true });
            return Ok(jsonderulo);
        }
    }

    [HttpGet]
    [Route("/dining")]
    public IActionResult ListDining([FromHeader(Name = "X-ApiKey")] string ApiKey)
    {
        var CorrectApiKey = Environment.GetEnvironmentVariable("API_KEY");
        if (ApiKey != CorrectApiKey)
        {
            return Unauthorized();
        }
        else
        {
            var dining = Database.GetDining();
            var jsonderulo = JsonSerializer.Serialize(dining, new JsonSerializerOptions { WriteIndented = true });
            return Ok(jsonderulo);
        }
    }

    [HttpGet]
    [Route("/hotelbookings")]
    public IActionResult ListHotelBookings([FromHeader(Name = "X-ApiKey")] string ApiKey)
    {
        var CorrectApiKey = Environment.GetEnvironmentVariable("API_KEY");
        if (ApiKey != CorrectApiKey)
        {
            return Unauthorized();
        }
        else
        {
            var dining = Database.GetHotelBooking();
            var jsonderulo = JsonSerializer.Serialize(dining, new JsonSerializerOptions { WriteIndented = true });
            return Ok(jsonderulo);
        }
    }

    [HttpGet]
    [Route("/diningbookings")]
    public IActionResult ListDiningBookings([FromHeader(Name = "X-ApiKey")] string ApiKey)
    {
        var CorrectApiKey = Environment.GetEnvironmentVariable("API_KEY");
        if (ApiKey != CorrectApiKey)
        {
            return Unauthorized();
        }
        else
        {
            var dining = Database.GetDiningBooking();
            var jsonderulo = JsonSerializer.Serialize(dining, new JsonSerializerOptions { WriteIndented = true });
            return Ok(jsonderulo);
        }
    }


    [HttpPatch]
    [Route("/hotelbooking/{id}")]
    public IActionResult ModifyAHotelBooking(int Id, [FromBody] UpdateBookingModel model)
    {
        Database.ModifyHotelBooking(Id, model);
        return Ok("Hotel reservation has been successfully modified!");
    }

    [HttpPatch]
    [Route("/diningbooking/{Id}")]
    public IActionResult ModifyADiningBooking(int Id, [FromBody] UpdateBookingModel model)
    {
        Database.ModifyDiningBooking(Id, model);
        return Ok("Dining reservation has been successfully modified!");
    }

    [HttpPatch]
    [Route("/roomstatus")]
    public IActionResult ModifyARoom(int Id, [FromBody] UpdateRoomsModel model)
    {
        Database.ModifyRooms(Id, model);
        return Ok("Room has been successfully modified!");
    }

    [HttpPatch]
    [Route("/diningtatus")]
    public IActionResult ModifyARoom(int Id, [FromBody] UpdateDiningTableModel model)
    {
        Database.ModifyDiningTable(Id, model);
        return Ok("Dining tables have been successfully modified!");
    }

    [HttpPatch]
    [Route("/voucherstatus")]
    public IActionResult ModifyAVoucher(int Id, [FromBody] UpdateVoucherModel model)
    {
        Database.ModifyVoucher(Id, model);
        return Ok("Voucher has been successfully modified!");
    }

    [HttpDelete]
    [Route("/logout")]
    public IActionResult DeleteASession([FromBody] SessionsModel model)
    {
        Database.DeleteSession(model.Id);
        return Ok("Session has been successfully deleted!");
    }

    public static DateTime LongToDateTime(long unixTimeStamp) //converts long to DateTime
    {
        // Unix timestamp is seconds past epoch
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
    }
}