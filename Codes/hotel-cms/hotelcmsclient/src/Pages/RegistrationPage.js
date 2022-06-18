import React, { Component } from "react";
import { Routes, Route, Link, useNavigate } from "react-router-dom";
import "./EntryPage.css";

function RegistrationPage() {
  let navigate = useNavigate();

  async function handleSubmit(event) {
    event.preventDefault();
    var user = document.getElementById("UserName").value;
    var password = document.getElementById("Password").value;
    var first = document.getElementById("FirstName").value;
    var last = document.getElementById("LastName").value;
    var type = document.getElementById("UserType").value;
    const response = await fetch("http://localhost:5000/users", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        "X-ApiKey": "chock",
      },
      body: JSON.stringify({ UserName: `${user}`, Password: `${password}`, FirstName: `${first}`, LastName: `${last}`, UserType: `${type}`}),
    });

    if(response.status == 200){
        alert ("Registration succesful!");
        navigate("/", { replace: true });
    }
    else{
        alert ("Registration failed. Please try again.");
    }
  }

    return (
      <div class="user">
        <p class="welcome">FINEST TOKYO MEMBER REGISTRATION</p>
        <div>
          <form id="loginform" onSubmit={handleSubmit}>
            <p>
              Username: <input type="text" name="UserName" id="UserName" placeholder="Ex. stephenstrange"/>
            </p>
            <p>
              Password: <input type="password" name="Password" id="Password" />
            </p>
            <p>
              Firstname: <input type="text" name="FirstName" id="FirstName" placeholder="Ex. Stephen"/>
            </p>
            <p>
              Lastname: <input type="text" name="LastName" id="LastName" placeholder="Ex. Strange"/>
            </p>
            <p>
              Position        : <select name="UserType" id="UserType"><option value="admin">Admin</option><option value="employee">Employee</option></select>
            </p>
            <br />
            <input type="submit" name="submit" value="REGISTER" />
          </form>
        </div>
      </div>
    );
}

export default RegistrationPage;
