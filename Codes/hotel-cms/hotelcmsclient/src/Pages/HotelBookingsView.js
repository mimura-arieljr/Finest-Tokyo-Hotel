import React from "react";
import { useNavigate, Link } from "react-router-dom";
import "./PagesView.css";
import { useState, useEffect } from "react";
import cms1 from "../Images/cms2.png";

function HotelBookingsView() {
  let navigate = useNavigate();

  async function logout(event) {
    event.preventDefault();
    let sessionId = sessionStorage.getItem("sessionId");
    const response = await fetch("http://localhost:5000/logout", {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ Id: `${sessionId}` }),
    });
    sessionStorage.removeItem("sessionId");
    navigate("/", { replace: true });
  }
  
  const [data, setData] = useState([]);
  const [searchWord, setSearchWord] = useState('');

  useEffect(() =>{
    roomsGet();
  }, []);


  const roomsGet = () =>{
    fetch("http://localhost:5000/hotelbookings", {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        "X-SessionID": "2cc9499a-6b8a-4f66-a92a-00b9e6e4e27e",
        "X-ApiKey": "chock",
      }
    })
    .then((response) => response.json())
    .then((json) => {
      console.log(json);
      setData(json);
  });
};

  const hotelBookingDelete = (Id) =>{
    fetch("http://localhost:5000/hotelbooking", {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({"Id" : Id})
    })
    .then(() => {
      window.location.reload();
    });
  };

  return (
    <div class="main">
      <div class="menu">
        <p class="menu1">
          CONTENT MANAGEMENT SYSTEM || <b>FINEST TOKYO HOTEL©</b> || HOTEL BOOKINGS
        </p>
        <p class="finest">FINEST TOKYO HOTEL</p>
      </div>
      <div class="secondlayer">
        <div class="container">
          <div class="sidebar">
            <img src={cms1} class="icon"></img>
            <p class="content"><Link to="/AdminMainView"  class="button" style={{ textDecoration: 'none' }}><b>HOME (家)</b></Link></p>
            <p class="content"><Link to="/RoomTypesView" class="button" style={{ textDecoration: 'none' }}><b>ROOM TYPES</b><br></br>(部屋のタイプ)</Link></p>
            <p class="content"><Link to="/RoomView" class="button" style={{ textDecoration: 'none' }}><b>ROOMS (部屋)</b></Link></p>
            <p class="content"><Link to="/DiningsView"  class="button" style={{ textDecoration: 'none' }}><b>DININGS (飲食店)</b></Link></p>
            <p class="content"><Link to="/TablesView" class="button" style={{ textDecoration: 'none' }}><b>DINING TABLES<br></br> (テーブル)</b></Link></p>
            <p class="content"><Link to="/VouchersView" class="button" style={{ textDecoration: 'none' }}><b>VOUCHERS<br></br> (バウチャー)</b></Link></p>
            <p class="content"><Link to="/HotelBookingsView" class="button" style={{ textDecoration: 'none' }}><b>HOTEL BOOKINGS<br></br>(ホテルの予約)</b></Link></p>
            <p class="content"><Link to="/DiningBookingsView" class="button" style={{ textDecoration: 'none' }}><b>DINING BOOKINGS<br></br>(レストラン予約)</b></Link></p>
            <p class="content"><Link to="/UsersView" class="button" style={{ textDecoration: 'none' }}><b>USERS<br></br>(スタッフ)</b></Link></p>
            <form id="button" onSubmit={logout}>
              <input type="submit" name="submit" value="LOGOUT" />
            </form>
          </div>
        </div>
        <div class="tablelayout">
        <input type="text" name="searchbar" placeholder="Search by guest name..." onChange={(e)=>{setSearchWord(e.target.value)}}/>
          <table class="styled-table">
            <thead>
              <tr>
                <th>Booking ID</th>
                <th>Guest Name</th>
                <th>Number of Guests</th>
                <th>RoomType ID</th>
                <th>Room ID</th>
                <th>Booking Date</th>
                <th>Check In Date</th>
                <th>Check Out Date</th>
                <th>Total Price (¥)</th>
                <th>Payment Type</th>
                <th>Booking Status</th>
                <th>Action</th>
              </tr>
            </thead>
            <tbody>
              {data.filter((datum) => {
                if (searchWord == ""){
                  return datum
                }
                else if(datum.GuestName.toLowerCase().includes(searchWord.toLowerCase())){
                  return datum
                }
              }).map((datum, i) =>(
              <tr class="row" key={i}>
                <td>{datum.Id}</td>
                <td>{datum.GuestName}</td>
                <td>{datum.NoOfGuests}</td>
                <td>{datum.RoomTypeId}</td>
                <td>{datum.RoomId}</td>
                <td>{datum.BookingDate}</td>
                <td>{datum.CheckInDate}</td>
                <td>{datum.CheckOutDate}</td>
                <td>{datum.TotalPrice}</td>
                <td>{datum.PaymentType}</td>
                <td>{datum.BookingStatus}</td>
                <td><button class="button" onClick={() => hotelBookingDelete(datum.Id)}>DELETE</button></td>
              </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}

export default HotelBookingsView;
