import { createContext, useEffect, useState } from "react";

export const  AuthContext=createContext();

export const AuthProvider=({children})=>{

    const [token,setToken]=useState(null);

    useEffect(()=>{

        const storedToken=localStorage.getItem("token");
        if(storedToken){
            setToken(storedToken)
        }
    },[])

    const saveToken=(newToken)=>{
        localStorage.setItem("token",newToken);
        setToken(newToken);
    }

    const logOut=()=>{
        localStorage.removeItem("token");
        setToken(null);
    }
    return(
        <AuthContext.Provider value={{token,saveToken,logOut}}>
            {children}
        </AuthContext.Provider>
    )
}
