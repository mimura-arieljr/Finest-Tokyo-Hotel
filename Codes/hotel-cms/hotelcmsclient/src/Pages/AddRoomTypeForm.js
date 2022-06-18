import React from "react";
import { useNavigate, Link } from "react-router-dom";
import "./PagesView.css";
import { useState, useEffect } from "react";
import cms1 from "../Images/cms2.png";

function AddRoomTypeForm() {
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
  const [id, setId] = useState(0);
  const [roomName, setRoomName] = useState('');
  const [imagePath, setImagePath] = useState('');
  const [roomSize, setRoomSize] = useState('');
  const [bedDetails, setBedDetails] = useState('');
  const [roomCapacity, setRoomCapacity] = useState(0);
  const [roomRate, setRoomRate] = useState(0);
  const [roomQuantity, setRoomQuantity] = useState(0);

  useEffect(() =>{
    roomsGet();
  }, []);


  const roomsGet = () =>{
    fetch("http://localhost:5000/roomtypes", {
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

    async function handleSubmit(event) {
        event.preventDefault();
        var roomname = document.getElementById("roomname").value;
        var imagepath = document.getElementById("imagepath").value;
        var roomsize = document.getElementById("roomsize").value;
        var beddetails = document.getElementById("beddetails").value;
        var roomcapacity = document.getElementById("roomcapacity").value;
        var roomrate = document.getElementById("roomrate").value;
        var roomquantity = document.getElementById("roomqty").value;
        const response = await fetch("http://localhost:5000/roomtypes", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "X-ApiKey": "chock",
        },
        body: JSON.stringify({ RoomName: `${roomname}`, ImagePath: `${imagepath}`, RoomSize: `${roomsize}`, BedDetails: `${beddetails}`, RoomCapacity: `${roomcapacity}`, RoomRate: `${roomrate}`, RoomQuantity: `${roomquantity}`}),
        });

        if(response.status == 200){
            alert ("Room Type added succesfully!");
            navigate("/RoomTypesView", { replace: true });
        }
        else{
            alert ("Adding room type to the database was unsuccessful. Please try again.");
        }
    }

  return (
    <div class="main">
      <div class="menu">
        <p class="menu1">
          CONTENT MANAGEMENT SYSTEM || <b>FINEST TOKYO HOTEL©</b> || ADD ROOM TYPE
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
          <div class="addform" >
            <h1>Add Room Type</h1>
            <form onSubmit={handleSubmit}>
            <div><input type="hidden" id="id" required value={id} onChange={(e)=>{setId(e.target.value)}}/></div>
            <div><label>Room Name:</label><input type="text" placeholder="Ex. Finest Suite" id="roomname" required value={roomName} onChange={(e)=>{setRoomName(e.target.value)}}/></div>
            <div><label>Image Path:</label><input type="text" placeholder="Ex. ./images/japan3.jpg" id="imagepath" required value={imagePath} onChange={(e)=>{setImagePath(e.target.value)}} /></div>
            <div><label>Room Size:</label><input type="text" placeholder="Ex. 30sqm" id="roomsize" required value={roomSize} onChange={(e)=>{setRoomSize(e.target.value)}} /></div>
            <div><label>Bed Details:</label><input type="text" placeholder="Ex. One 2300x3000mm bed" id="beddetails" required value={bedDetails} onChange={(e)=>{setBedDetails(e.target.value)}} /></div>
            <div><label>Room Capacity:</label><input type="number" id="roomcapacity" required value={roomCapacity} onChange={(e)=>{setRoomCapacity(e.target.value)}} /></div>
            <div><label>Room Rate:</label><input type="number" id="roomrate" required value={roomRate} onChange={(e)=>{setRoomRate(e.target.value)}} /></div>
            <div><label>Room Quantity:</label><input type="number" id="roomqty" required value={roomQuantity} onChange={(e)=>{setRoomQuantity(e.target.value)}} /></div>
            <input type="submit" name="submit" value="SAVE" />
            </form>
          </div>
        </div>
      </div>
    </div>
  );
}

export default AddRoomTypeForm;
