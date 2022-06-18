import React from "react";
import { useNavigate, Link } from "react-router-dom";
import "./PagesView.css";
import { useState, useEffect } from "react";
import cms1 from "../Images/cms2.png";

function AddDiningTableForm() {
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
  const [diningId, setDiningId] = useState(0);
  const [tableCapacity, setTableCapacity] = useState(0);
  const [diningStatus, setDiningStatus] = useState('');

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
        var diningid = document.getElementById("diningid").value;
        var tablecapacity = document.getElementById("tablecapacity").value;
        var diningstatus = document.getElementById("diningstatus").value;
        const response = await fetch("http://localhost:5000/diningtable", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "X-ApiKey": "chock",
        },
        body: JSON.stringify({ DiningId: `${diningid}`, TableCapacity: `${tablecapacity}`, DiningStatus: `${diningstatus}`}),
        });

        if(response.status == 200){
            alert ("Dining table added succesfully!");
            navigate("/TablesView", { replace: true });
        }
        else{
            alert ("Adding a new dining table to the database was unsuccessful. Please try again.");
        }
    }

  return (
    <div class="main">
      <div class="menu">
        <p class="menu1">
          CONTENT MANAGEMENT SYSTEM || <b>FINEST TOKYO HOTEL©</b> || ADD A DINING TABLE
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
            <h1>Add Dining Table</h1>
            <form onSubmit={handleSubmit}>
            <div><input type="hidden" id="id" required value={id} onChange={(e)=>{setId(e.target.value)}}/></div>
            <div><label>Dining Id:</label><input type="number" id="diningid" required value={diningId} onChange={(e)=>{setDiningId(e.target.value)}}/></div>
            <div><label>Table Capacity:</label><input type="number" id="tablecapacity" required value={tableCapacity} onChange={(e)=>{setTableCapacity(e.target.value)}} /></div>
            <div><label>Dining Status:</label><input type="hidden" id="diningstatus" required value={diningStatus} onChange={(e)=>{setDiningStatus(e.target.value)}} /></div>
            <input type="submit" name="submit" value="SAVE" />
            </form>
          </div>
        </div>
      </div>
    </div>
  );
}

export default AddDiningTableForm;
