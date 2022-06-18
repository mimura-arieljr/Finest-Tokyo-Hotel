import logo from "./logo.svg";
import "./App.css";
import { Routes, Route, Link } from "react-router-dom";
import EntryPage from "./Pages/EntryPage";
import AdminMainView from "./Pages/AdminMainView";
import EmployeeMainView from "./Pages/EmployeeMainView"
import RegistrationPage from "./Pages/RegistrationPage";
import RoomTypesView from "./Pages/RoomTypesView"
import RoomView from "./Pages/RoomView";
import DiningsView from "./Pages/DiningsView";
import TablesView from "./Pages/TablesView";
import VouchersView from "./Pages/VouchersView";
import HotelBookingsView from "./Pages/HotelBookingsView";
import DiningBookingsView from "./Pages/DiningBookingsView";
import AddRoomTypeForm from "./Pages/AddRoomTypeForm";
import AddDiningForm from "./Pages/AddDiningForm";
import AddRoomForm from "./Pages/AddRoomForm";
import AddDiningTableForm from "./Pages/AddDiningTableForm";
import AddVoucherForm from "./Pages/AddVoucherForm";
import UsersView from "./Pages/UsersView";

function App() {
  return (
    <div className="App">
      <Routes>
        <Route path="/" element={<EntryPage/>} />
        <Route path="/AdminMainView" element={<AdminMainView/>} />
        <Route path="/EmployeeMainView" element={<EmployeeMainView/>} />
        <Route path="/RegistrationPage" element={<RegistrationPage/>}/>
        <Route path="/RoomTypesView" element={<RoomTypesView/>}/>
        <Route path="/RoomView" element={<RoomView/>}/>
        <Route path="/DiningsView" element={<DiningsView/>}/>
        <Route path="/TablesView" element={<TablesView/>}/>
        <Route path="/VouchersView" element={<VouchersView/>}/>
        <Route path="/HotelBookingsView" element={<HotelBookingsView/>}/>
        <Route path="/DiningBookingsView" element={<DiningBookingsView/>}/>
        <Route path="/AddRoomTypeForm" element={<AddRoomTypeForm/>}/>
        <Route path="/AddDiningForm" element={<AddDiningForm/>}/>
        <Route path="/AddRoomForm" element={<AddRoomForm/>}/>
        <Route path="/AddDiningTableForm" element={<AddDiningTableForm/>}/>
        <Route path="/AddVoucherForm" element={<AddVoucherForm/>}/>
        <Route path="/UsersView" element={<UsersView/>}/>
      </Routes>
    </div>
  );
}

export default App;
