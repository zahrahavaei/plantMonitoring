import { useContext,useState } from "react";
import { AuthContext } from "../AuthContext";
import "./AdminDashboard.css"

import GreenHousePlants from "./GreenHousePlants.js";

const AdminDashboard=()=>{
const {token} =useContext(AuthContext);

const setLocationGreenAFunc=()=>{
 setLocationGreenA(!locationGreenA);
  setLocationGreenB(false);

}
const setLocationGreenBFunc=()=>{
 setLocationGreenB(!locationGreenB);
 setLocationGreenA(false);
}
const[locationGreenA,setLocationGreenA]=useState(false);
const[locationGreenB,setLocationGreenB]=useState(false);
return(
    <>
    <div className="main">
        <header>

        </header>
     <div className="col-md-12 row-dashoard">
         <div className="col-md-2 bg-left" >
           <ul>GreenHouses
              <li
               onClick={()=>setLocationGreenAFunc()}
                style={{cursor:"pointer"}}>
                GreenHouse A
                
                
              </li>
              <li
              onClick={()=>setLocationGreenBFunc()}
              style={{cursor:"pointer"}}
              >
                GreenHouse B
                
              </li>
           </ul>
         </div>

         <div className="col-md-10 bg-middle">
             <p>Monitor the Plant</p>
             <div className="row">
                <div className="col-md-5 middle-left">
               <div>
                 {locationGreenA && (
                  <GreenHousePlants location={"GreenHouseA"} />
                 )}
                  
               </div>
               
             </div>
             <div className="col-md-5 middle-right">
                <div>
                 
                 
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