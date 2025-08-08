import { useContext, useState,useEffect} from "react";
import { AuthContext } from "../AuthContext";

import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import { Typography } from "@mui/material";

const DataChart=(plantName,sensorType,data)=>{

    const {token}=useContext(AuthContext);
    
  
   
  return(
  <>
        <div  style={{ marginTop: "40px" }}>
          <Typography>
            <strong>Plant Name:</strong> {plantName}
            <br />
            <strong>Sensor Type:</strong> {sensorType}
          </Typography>

          <ResponsiveContainer width="100%" height={300}>
            <LineChart data={data}>
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
  </>
  )
}
export default DataChart;