import { createContext, useEffect, useState } from "react";

export const  AuthContext=createContext();

export const AuthProvider=({children})=>{

    const [token,setToken]=useState(null);
    const[userData,setUserData]=useState(null);

    useEffect(()=>{

        const storedToken=localStorage.getItem("token");
        const storedUser=localStorage.getItem("user");
        if(storedToken){
            setToken(storedToken)
        }
        if(storedUser){
            setUserData(JSON.parse(storedUser));
        }
    },[])

    const saveToken=(newToken,newuserData)=>{
        localStorage.setItem("token",newToken);
        localStorage.setItem("user",JSON.stringify(userData));
        setToken(newToken);
        setUserData(newuserData);
    }

    const logOut=()=>{
        localStorage.removeItem("token");
         localStorage.removeItem("user");
        setToken(null);
        setUserData(null);
    }
    return(
        <AuthContext.Provider value={{token,saveToken,logOut,userData}}>
            {children}
        </AuthContext.Provider>
    )
}
