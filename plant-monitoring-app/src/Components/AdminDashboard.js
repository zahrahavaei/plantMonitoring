import { useContext,useState } from "react";
import { AuthContext } from "../AuthContext";
import "./AdminDashboard.css"
import SensorSlider from "./SensorSlider.js";
import { Typography } from "@mui/material";
import GreenHouseSensor from "./GreenHouseSensor.js";
import SensorChart from "./SensorChart.js";

const AdminDashboard=()=>{
const {token} =useContext(AuthContext);
const[greenHouseName,setGreenHouseName]=useState("GreenHouseA");
return(
    <>
    <div className="main">
        <header>

        </header>
     <div className="col-md-12 row-dashoard">
         <div className="col-md-2 bg-left" >
           <ul>GreenHouses
              <li onClick={()=>setGreenHouseName("GreenHouseA")}>
                GreenHouse A
              </li>
              <li onClick={()=>setGreenHouseName("GreenHouseB")}>
                GreenHouse B
              </li>
           </ul>
         </div>

         <div className="col-md-10 bg-middle">
             <p>Monitor the Plant</p>
             <div className="row">
                <div className="col-md-5 middle-left">
               <div>
                 
                  {greenHouseName && (
                    <GreenHouseSensor greenHouseName={greenHouseName}  />
                  )}
               </div>
               
             </div>
             <div className="col-md-5 middle-right">
                <div>
                 {greenHouseName &&(
                  <SensorChart  greenHouseName={greenHouseName}  />
                 )}
                 
               </div>
             </div>
             </div>  
         </div>
         {/*<div className="col-md-2 bg-right">
          <p>Hello {userData?.name}{userData?.role}</p>
         </div>  */}

     </div>
    </div>
    </>
)
}
export default  AdminDashboard;