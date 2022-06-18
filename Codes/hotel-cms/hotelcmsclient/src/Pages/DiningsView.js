import React from "react";
import { useNavigate, Link } from "react-router-dom";
import "./PagesView.css";
import { useState, useEffect } from "react";
import cms1 from "../Images/cms2.png";

function DiningsView() {
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
    fetch("http://localhost:5000/dining", {
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

  const diningDelete = (Id) =>{
    fetch("http://localhost:5000/dining", {
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

  function diningSelect(id) {
    let diner = null;
    data.forEach(datum => { 
      if(datum.Id == id) {
        diner = datum;
      }
    });
    setId(diner.Id);
    setDiningName(diner.DiningName);
    setAbout(diner.About);
    setImagePath(diner.ImagePath);
    setDiningCapacity(diner.DiningCapacity);
    setTableQuantity(diner.TableQuantity);
  }

  const diningUpdate = (id, diningName, about, imagePath, diningCapacity, tableQuantity) => {
    fetch(`http://localhost:5000/diningupdate/${id}` , {
          method: 'PUT',
          headers: {
              "Content-Type" : "application/json"
          },
          body: JSON.stringify({"DiningName" : diningName, "About" : about, "ImagePath" : imagePath, "DiningCapacity" : diningCapacity, "TableQuantity": tableQuantity}),
      })
      .then(() => {
        window.location.reload();
      });
    }

  return (
    <div class="main">
      <div class="menu">
        <p class="menu1">
          CONTENT MANAGEMENT SYSTEM || <b>FINEST TOKYO HOTEL©</b> || DININGS
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
                <th>Dining Id</th>
                <th>Dining Name</th>
                <th>About</th>
                <th>Image Path</th>
                <th>Dining Capacity</th>
                <th>Table Quantity</th>
                <th>Action</th>
              </tr>
            </thead>
            <tbody>
              {data.map((datum, i) =>(
              <tr class="row" key={i}>
                <td>{datum.Id}</td>
                <td>{datum.DiningName}</td>
                <td>{datum.About}</td>
                <td>{datum.ImagePath}</td>
                <td>{datum.DiningCapacity}</td>
                <td>{datum.TableQuantity}</td>
                <td>
                  <button class="buttonedit" onClick={() => diningSelect(datum.Id)} >EDIT</button>
                  <button class="button" onClick={() => diningDelete(datum.Id)}>DELETE</button>                
                </td>
              </tr>
              ))}
            </tbody>
          </table>
          <p class="addbutton"><Link to="/AddDiningForm"  class="button" style={{ textDecoration: 'none' }}><b>ADD DINING</b></Link></p>
          
          <div class="editform" >
            <div><input type="hidden" name="id" value={id} onChange={(e)=>{setId(e.target.value)}}/></div>
            <div><label>Dining Name:</label><input type="text" name="name" value={diningName} onChange={(e)=>{setDiningName(e.target.value)}}/></div>
            <div><label>About:</label><input type="text" name="about" value={about} onChange={(e)=>{setAbout(e.target.value)}} /></div>
            <div><label>Image Path:</label><input type="text" name="imagepath" value={imagePath} onChange={(e)=>{setImagePath(e.target.value)}} /></div>
            <div><label>Dining Capacity:</label><input type="number" name="number" value={diningCapacity} onChange={(e)=>{setDiningCapacity(e.target.value)}} /></div>
            <div><label>TableQuantity:</label><input type="number" name="tableqty" value={tableQuantity} onChange={(e)=>{setTableQuantity(e.target.value)}} /></div>
            <div><button class="buttonedit" onClick={() => diningUpdate(id,diningName, about, imagePath, diningCapacity, tableQuantity)}>UPDATE</button></div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default DiningsView;
