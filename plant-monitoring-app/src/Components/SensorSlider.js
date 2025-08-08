
import React,{useContext, useState} from "react";
import{Slider,Button,Typography} from "@mui/material";
import {AuthContext} from "../AuthContext"
import {toast} from "react-toastify" ;
import axios from "axios";

const SensorSlider=({plantId,sensorId,plantName,sensorType})=>{

    const[value,setValue]=useState(50);
    const {token}=useContext(AuthContext);
   

    const handleChange=(event,newValue)=>{
        setValue(newValue);
    };

    const handleSendData=async()=>{

        const payload={
            plantId,
            sensorId,
            date:new Date().toISOString().split("T")[0]   ,
            time :new Date().toTimeString().split(" ")[0] ,
            value :value
        };
       /* try{
            const rsp=await fetch(`https://localhost:7245/api/sensordata`,{
                method:"POST",
                headers:{
                    "Content-Type":"application/json",
                    Authorization:`Bearer ${token}`
                },
                body:JSON.stringify(payLoad)
            });
            if(sp.ok){
                const result=await rsp.json();
                seValue(result.value);
                console.log("value of sensor",value);
                console.log(result);
            }else{
                const error=await rsp.text();
                console.log(error);
            }
        }
        catch(error){
            console.log(error);
        }
       */
      try{
            const rsp=await axios.post(`https://localhost:7245/api/sensordata`,
                payload,{
                headers: {
                    "Content-Type":"application/json",
                    Authorization:`Bearer ${token}`
                }
            }
            );
        console.log("response sensor data after post ",rsp.data);
      
        toast.success("Sensor Data was sent successfully");
      }
      catch(error)
      {
        console.log(error);
        toast.error("Failed to send sensor data ")
      }
    }
    return(
        <>
        <div>
           
              <Typography>plant Name:{plantName} <br/>
              sensorId:{sensorId} <br/> 
              sensor type: {sensorType}<br/>
              value:{value}
              </Typography>
          
            <Slider 
             value={value}
            onChange={handleChange} 
             min={0} 
            max={100}
            sx={{
            width:300,
            color:'green',
            }}
            />
            <Button
             onClick={handleSendData} 
              variant="contained" 
              sx={{
              backgroundColor: "#f5630d ",
              '&:hover': {
              backgroundColor: "#54554f"
             }
            }}
            >
                Send Sensor Data
            </Button>
        </div>
        </>
    )
}
export default SensorSlider ;