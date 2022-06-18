import React from "react";
import { useNavigate, Link } from "react-router-dom";
import "./PagesView.css";
import { useState, useEffect } from "react";
import cms1 from "../Images/cms2.png";

function AddDiningForm() {
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
  const [diningName, setDiningName] = useState('');
  const [about, setAbout] = useState('');
  const [imagePath, setImagePath] = useState('');
  const [diningCapacity, setDiningCapacity] = useState(0);
  const [tableQuantity, setTableQuantity] = useState(0);

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
        var diningname = document.getElementById("diningname").value;
        var about = document.getElementById("about").value;
        var imagepath = document.getElementById("imagepath").value;
        var diningcapacity = document.getElementById("diningcapacity").value;
        var tablequantity = document.getElementById("tableqty").value;
        const response = await fetch("http://localhost:5000/dining", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "X-ApiKey": "chock",
        },
        body: JSON.stringify({ DiningName: `${diningname}`, About: `${about}`, ImagePath: `${imagepath}`, DiningCapacity: `${diningcapacity}`, TableQuantity: `${tablequantity}`}),
        });

        if(response.status == 200){
            alert ("Dining added succesfully!");
            navigate("/DiningsView", { replace: true });
        }
        else{
            alert ("Adding a new dining to the database was unsuccessful. Please try again.");
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
            <h1>Add Dining</h1>
            <form onSubmit={handleSubmit}>
            <div><input type="hidden" id="id" required value={id} onChange={(e)=>{setId(e.target.value)}}/></div>
            <div><label>Dining Name:</label><input type="text" placeholder="Ex. Dohtonbori" id="diningname" required value={diningName} onChange={(e)=>{setDiningName(e.target.value)}}/></div>
            <div><label>About:</label><input type="text" id="about" placeholder="Restaurant description" required value={about} onChange={(e)=>{setAbout(e.target.value)}} /></div>
            <div><label>Image Path:</label><input type="text" id="imagepath" placeholder="Ex. ./images/dine2.jpg" required value={imagePath} onChange={(e)=>{setImagePath(e.target.value)}} /></div>
            <div><label>Dining Capacity:</label><input type="number" id="diningcapacity" required value={diningCapacity} onChange={(e)=>{setDiningCapacity(e.target.value)}} /></div>
            <div><label>TableQuantity:</label><input type="number" id="tableqty" required value={tableQuantity} onChange={(e)=>{setTableQuantity(e.target.value)}} /></div>
            <input type="submit" name="submit" value="SAVE" />
            </form>
          </div>
        </div>
      </div>
    </div>
  );
}

export default AddDiningForm;
