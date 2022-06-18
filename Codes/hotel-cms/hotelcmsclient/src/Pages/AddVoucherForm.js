import React from "react";
import { useNavigate, Link } from "react-router-dom";
import "./PagesView.css";
import { useState, useEffect } from "react";
import cms1 from "../Images/cms2.png";

function AddVoucherForm() {
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
  const [code, setCode] = useState('');
  const [salePrice, setSalePrice] = useState(0);
  const [voucherStatus, setVoucherStatus] = useState('');
  
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
        var code = document.getElementById("code").value;
        var saleprice = document.getElementById("saleprice").value;
        var voucherstatus = document.getElementById("voucherstatus").value;
        const response = await fetch("http://localhost:5000/vouchers", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "X-ApiKey": "chock",
        },
        body: JSON.stringify({ Code: `${code}`, SalePrice: `${saleprice}`, VoucherStatus: `${voucherstatus}`}),
        });

        if(response.status == 200){
            alert ("Voucher has been added succesfully!");
            navigate("/VouchersView", { replace: true });
        }
        else{
            alert ("Adding a voucher to the database was unsuccessful. Please try again.");
        }
    }

  return (
    <div class="main">
      <div class="menu">
        <p class="menu1">
          CONTENT MANAGEMENT SYSTEM || <b>FINEST TOKYO HOTEL©</b> || ADD A VOUCHER 
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
            <h1>Add a Voucher</h1>
            <form onSubmit={handleSubmit}>
            <div><input type="hidden" id="id" required value={id} onChange={(e)=>{setId(e.target.value)}}/></div>
            <div><label>Voucher Code:</label><input type="text" id="code" required value={code} onChange={(e)=>{setCode(e.target.value)}}/></div>
            <div><label>Discount Price:</label><input type="number" id="saleprice" required value={salePrice} onChange={(e)=>{setSalePrice(e.target.value)}} /></div>
            <div><input type="hidden" id="voucherstatus" required value={voucherStatus} onChange={(e)=>{setVoucherStatus(e.target.value)}} /></div>
            <input type="submit" name="submit" value="SAVE" />
            </form>
          </div>
        </div>
      </div>
    </div>
  );
}

export default AddVoucherForm;
