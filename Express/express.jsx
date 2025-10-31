import React from "react";
import ReactDOM from "react-dom/client";

function ExpressApp() {
    return (
        <div style={{ textAlign: "center", backgroundColor: "#e0f7fa", padding: "30px" }}>
            <h2>Panel de Alimentos con React + Express.jsx 🚀</h2>
            <p>Esta vista está renderizada dentro de ASP.NET MVC.</p>
        </div>
    );
}

const root = ReactDOM.createRoot(document.getElementById("react-root"));
root.render(<ExpressApp />);
