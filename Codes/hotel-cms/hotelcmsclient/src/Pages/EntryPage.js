import React, { Component } from "react";
import { Routes, Route, Link, useNavigate } from "react-router-dom";
import "./EntryPage.css";

function EntryPage() {
  let navigate = useNavigate();

  async function handleSubmit(event) {
    event.preventDefault();
    var user = document.getElementById("UserName").value;
    var password = document.getElementById("Password").value;
    const response = await fetch("http://localhost:5000/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        "X-SessionID": "2cc9499a-6b8a-4f66-a92a-00b9e6e4e27e",
        "X-ApiKey": "chock",
      },
      body: JSON.stringify({ Username: `${user}`, Password: `${password}` }),
    });

    if (response.status == 401) {
      alert("Unauthorized access!");
    } else if (response.status == 200) {
      var json = await response.json();
      if (json.userType == "admin") {
        alert("Welcome, " + json.userName + "!");
        sessionStorage.setItem("sessionId", json.id)
        navigate("/AdminMainView", { replace: true });
      } else if (json.userType == "employee") {
        alert("Welcome, " + json.userName + "!");
        sessionStorage.setItem("sessionId", json.id)
        navigate("/EmployeeMainView", { replace: true });
      } else alert("User type not specified.");
    } else {
      alert(response.status);
    }
  }

  return (
    <div class="user">
      <p class="welcome">FINEST TOKYO HOTEL MEMBER LOG-IN</p>
      <div>
        <form id="loginform" onSubmit={handleSubmit}>
          <p>
            Username: <input type="text" name="UserName" id="UserName" />
          </p>
          <p>
            Password: <input type="password" name="Password" id="Password" />
          </p>
          <br />
          <input type="submit" name="submit" value="LOGIN" />
        </form>
      </div>
      <div class="register">
        <p>
          Not a member? <button id="registerbutton" onClick={() =>{
          navigate("/RegistrationPage");}}>Register Here!</button></p>
      </div>
    </div>
  );
}

export default EntryPage;
