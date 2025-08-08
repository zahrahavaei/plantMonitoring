import { useContext, useState,useEffect } from "react";
import { AuthContext } from "../AuthContext";
import SensorSlider from "./SensorSlider";
import axios from "axios";

const GreenHouseSensor = ({ greenHouseName }) => {

    const { token } = useContext(AuthContext);
   const[sensors,setSensors]=useState([]);
   
    useEffect( () => {
    const fetchData=async()=>{
       try {
            const rsp = await axios.get(`https://localhost:7245/api/sensor/greenhouse?GreenHouseName=${greenHouseName}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            });
            console.log("sensor list for green house", rsp.data);
            setSensors(rsp.data);
        }
        catch (error) {
            console.log("error while fetching sensor for greenhouse", error);
        }
    }
    fetchData();
    }, [greenHouseName])
   
    return (
        <>
         {sensors.map(sensor=>(
            <div key={sensor.id} style={{ marginTop: "40px" }}>
               {<SensorSlider
                plantId={sensor.plantId} 
               sensorId={sensor.id}
                plantName={sensor.plantName}
                sensorType={sensor.type}/>}
            </div>
         ))}
        </>
    )
}
export default GreenHouseSensor;