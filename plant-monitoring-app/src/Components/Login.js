import { useState,useContext, } from "react";
import "./Login.css";
import { useNavigate } from "react-router-dom";
import {AuthContext} from "../AuthContext";

const Login=()=>{
 const[message,setMessage]=useState("");
const[userName,setUsername]=useState(null);
const[password,setPassword]=useState(null);

const{saveToken}=useContext(AuthContext);//consuming function of AuthContext
const navigate=useNavigate();

    const submitForm=async(e)=>{

        e.preventDefault();
        try{
        const rsp=await fetch(`https://localhost:7245/api/login`,{
            method:"Post",
            headers:{
                "Content-Type":"application/json",
            },
            body:JSON.stringify
            ({
                 UserName:userName,
                  Password:password
             } )
        });
        if (rsp.ok){
            const result=await rsp.json();
            const token=result.token;
             const userData={
                role:result.userRole,
                name:result.name,
                userName:result.userName,
            }
            console.log("token",token);
            console.log("user data",userData);
            saveToken(token,userData);
            
            if(userData.role=="Admin"){
                navigate(`/adminDashboard`);
            }
           else if(userData.role=="User")
            {
                navigate(`/userDashboard`);
            }
            else{
                console.log("login faile");

            }
        }
        else{
            const errorTxt=await rsp.text();
            console.log(errorTxt);
            setMessage(errorTxt);
        }
    }
    catch(error)
    {
        console.error("Error during login",error)
    }
    }
    
    return(
        <>
        <form onSubmit={submitForm}>
            <div className="Row-Login">
            <div className="Column-Login">
                <input 
                className="txt-Login"
                placeholder="Enter User Name :"
                onChange={e=>setUsername(e.target.value)}
                >
                </input>
            </div>
            <div className="Column-Login">
                <input 
                className="txt-Login"
                type="password"
                placeholder="Enter Password :"
                onChange={e=>setPassword(e.target.value)}
                >
                </input>
            </div>
            <div className="Column-Login">
                <button className="btnLogin"
                type="submit">
                    Login
                </button>
            </div>
            {message && (
                <div>
                    {message}
               </div>
            )}
        </div>
        </form>
        </>
    )
}
export default Login;