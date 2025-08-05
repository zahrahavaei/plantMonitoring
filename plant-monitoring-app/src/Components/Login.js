import "./Login.css";


const Login=()=>{

    
    return(
        <>
        <div className="Row-Login">
            <div className="Column-Login">
                <input 
                className="txt-Login"
                placeholder="Enter User Name :"
                >
                
                </input>
            </div>
            <div className="Column-Login">
                <input 
                className="txt-Login"
                type="password"
                placeholder="Enter Password :"
                >
                
                </input>
            </div>
            <div className="Column-Login">
                <button className="btnLogin">
                    SignIn
                </button>
            </div>
        </div>
        </>
    )
}
export default Login;