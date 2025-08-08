import { useState,useContext,useEffect } from "react"
import { AuthContext } from "../AuthContext";
import axios from "axios";
import SensorSlider from "./SensorSlider";
import DataChart from "./DataChart";

const GreenHousePlants=({location})=>{

    const{token}=useContext(AuthContext);
 const[plants,setPlants]=useState([]);
 
 useEffect(()=>{

    const fetchPlants=async ()=>{
      try{
        const rsp=await axios.get(`https://localhost:7245/api/plant/location/${location}`,{
        headers:{
            "Content-Type":"application/json",
            Authorization:`Bearer ${token}`
        }})
        setPlants(rsp.data);
        console.log(`plants assosiated to ${location}`,rsp.data);
    }
    catch(error)
    {
    console.log("error for fetching a greenHouse plants");
    }
    }
    fetchPlants();
 },[location,token])

 if (plants!==null & plants.length>0)
return(
    <>
    {plants.map(plant=>(
        <div key={`${plant.Id}-${plant.name}`} >
            {plant.name} <br/>
            <img src={plant.imageUrl}  alt="image plant" style={{width:"200px",height:"200px"}}/>
            {plant.sensors.map(sensor=>(
                <div key={`${sensor.id}`}>
                    <SensorSlider plantId={plant.id} 
                      sensorId={sensor.id} 
                       plantName={plant.name} 
                      sensorType={sensor.type}
                      style={{margintop:"40px"}} />
                 </div>
            ))}
            {plant.plantSensorsData.map(data=>(
                 <div key={`${data.id}`}>
                   <DataChart plantName={plant.name} sensorType={data.sensorType} data={data}  />
                </div>
            ))}
        </div>
    ))}
    </>
)

}
export default GreenHousePlants