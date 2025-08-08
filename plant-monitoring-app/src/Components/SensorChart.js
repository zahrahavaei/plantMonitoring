import { useContext, useState,useEffect} from "react";
import { AuthContext } from "../AuthContext";
import axios from "axios";
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import { Typography } from "@mui/material";

const SensorChart=({greenHouseName})=>{

    const {token}=useContext(AuthContext);
    const[sensorsData,setSensorsData]=useState([]);
  useEffect(()=>{
   
    const fetchData=async()=>{
        try{
        const rsp=await axios.get(`https://localhost:7245/api/sensordata/greenhouse/${greenHouseName}`,
            {
                headers:{
                    "Content-Type":"application/json",
                    Authorization:`Bearer ${token}`
                }
            });
            setSensorsData(rsp.data);
            console.log("sensorData base green House",rsp.data);
        }
        catch(error)
        {
            console.log("error fetching ")
        }
    }
   fetchData();

  },[greenHouseName,token])

  return(
  <>
    {sensorsData.map((sensorGroup) => (
        <div key={sensorGroup.sensorId} style={{ marginTop: "40px" }}>
          <Typography>
            <strong>Plant Name:</strong> {sensorGroup.plantName}
            <br />
            <strong>Sensor Type:</strong> {sensorGroup.sensorType}
          </Typography>

          <ResponsiveContainer width="100%" height={300}>
            <LineChart data={sensorGroup.data}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="time" />
              <YAxis />
              <Tooltip />
              <Legend />
              <Line
                type="monotone"
                dataKey="value"
                stroke="#8884d8"
                name="Sensor Value"
              />
            </LineChart>
          </ResponsiveContainer>
        </div>
      ))}
  </>
  )
}
export default SensorChart;