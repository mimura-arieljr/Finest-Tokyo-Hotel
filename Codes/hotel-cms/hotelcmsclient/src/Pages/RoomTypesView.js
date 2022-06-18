import React from "react";
import { useNavigate, Link } from "react-router-dom";
import "./PagesView.css";
import { useState, useEffect } from "react";
import cms1 from "../Images/cms2.png";

function RoomView() {
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

  const roomTypeDelete = (Id) =>{
    fetch("http://localhost:5000/roomtype", {
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

  function roomTypeSelect(id) {
    let roomType = null;
    data.forEach(datum => { 
      if(datum.Id == id) {
        roomType = datum;
      }
    });
    setId(roomType.Id);
    setRoomName(roomType.RoomName);
    setImagePath(roomType.ImagePath);
    setRoomSize(roomType.RoomSize);
    setBedDetails(roomType.BedDetails);
    setRoomCapacity(roomType.RoomCapacity);
    setRoomRate(roomType.RoomRate);
    setRoomQuantity(roomType.RoomQuantity);
  }

  const roomTypeUpdate = (id, roomName, imagePath, roomSize, bedDetails, roomCapacity, roomRate, roomQuantity) => {
    fetch(`http://localhost:5000/roomtypeupdate/${id}` , {
          method: 'PUT',
          headers: {
              "Content-Type" : "application/json"
          },
          body: JSON.stringify({"RoomName" : roomName, "ImagePath" : imagePath, "RoomSize" : roomSize, "BedDetails": bedDetails, "RoomCapacity": roomCapacity, "RoomRate": roomRate, "RoomQuantity": roomQuantity}),
      })
      .then(() => {
        window.location.reload();
      });
    }

  return (
    <div class="main">
      <div class="menu">
        <p class="menu1">
          CONTENT MANAGEMENT SYSTEM || <b>FINEST TOKYO HOTEL©</b> || ROOM TYPES
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
          <table class="styled-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Room Type</th>
                <th>Image Path</th>
                <th>Room Size</th>
                <th>Bed Details</th>
                <th>Room Capacity</th>
                <th>Room Rate</th>
                <th>Room Quantity</th>
                <th>Action</th>
              </tr>
            </thead>
            <tbody>
              {data.map((datum, i) =>(
              <tr class="row" key={i}>
                <td>{datum.Id}</td>
                <td>{datum.RoomName}</td>
                <td>{datum.ImagePath}</td>
                <td>{datum.RoomSize}</td>
                <td>{datum.BedDetails}</td>
                <td>{datum.RoomCapacity}</td>
                <td>{datum.RoomRate}</td>
                <td>{datum.RoomQuantity}</td>
                <td>
                  <button class="buttonedit" onClick={() => roomTypeSelect(datum.Id)} >EDIT</button>
                  <button class="button" onClick={() => roomTypeDelete(datum.Id)}>DELETE</button>
                </td>
              </tr>
              ))}
            </tbody>
          </table>
          <p class="addbutton"><Link to="/AddRoomTypeForm"  class="button" style={{ textDecoration: 'none' }}><b>ADD ROOM TYPE</b></Link></p>

          <div class="editform" >
            <div><input type="hidden" name="name" value={id} onChange={(e)=>{setId(e.target.value)}}/></div>
            <div><label>Room Name:</label><input type="text" name="name" value={roomName} onChange={(e)=>{setRoomName(e.target.value)}}/></div>
            <div><label>Image Path:</label><input type="text" name="imagepath" value={imagePath} onChange={(e)=>{setImagePath(e.target.value)}} /></div>
            <div><label>Room Size:</label><input type="text" name="roomsize" value={roomSize} onChange={(e)=>{setRoomSize(e.target.value)}} /></div>
            <div><label>Bed Details:</label><input type="text" name="beddetails" value={bedDetails} onChange={(e)=>{setBedDetails(e.target.value)}} /></div>
            <div><label>Room Capacity:</label><input type="number" name="roomcapacity" value={roomCapacity} onChange={(e)=>{setRoomCapacity(e.target.value)}} /></div>
            <div><label>Room Rate:</label><input type="number" name="roomrate" value={roomRate} onChange={(e)=>{setRoomRate(e.target.value)}} /></div>
            <div><label>Room Quantity:</label><input type="number" name="roomqty" value={roomQuantity} onChange={(e)=>{setRoomQuantity(e.target.value)}} /></div>
            <div><button class="buttonedit" onClick={() => roomTypeUpdate(id,roomName, imagePath, roomSize, bedDetails, roomCapacity, roomRate, roomQuantity)}>UPDATE</button></div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default RoomView;
