
import './App.css';
import {Routes,Route} from "react-router-dom";
import Login from "./Components/Login";
import hp2 from "./Images/hp2.png";
import AdminDashboard from './Components/AdminDashboard';

function App() {
  return (
    <>
    <div className="container-fluid">
      <header>

      </header>
      <div className="Row-HomePage">
        <Routes>
          <Route path="/adminDashboard" element={<AdminDashboard   />}   />
          
           <Route path="/" element={
            <>
             <div className="col-md-4 div-homepage-Left"> 
                <Login>

                </Login>
             </div>
             <div className="col-md-8 div-homepage-Right">
               <img src={hp2}   className="homepage-img"   alt="plant"/>
             </div>
            </>
           }>

           </Route>
        </Routes>
      </div>
    </div>
    </>
  );
}

export default App;
