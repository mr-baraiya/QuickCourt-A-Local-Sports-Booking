import React, { useState } from "react";
import ApiService from "../api/ApiService";
import { useNavigate } from "react-router-dom"; // for redirection

export default function Register() {
    const [name, setName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate(); // hook to navigate

    const handleRegister = async (e) => {
        e.preventDefault();
        try {
            await ApiService.register({ name, email, password });
            alert("Registration successful! You can now log in.");
            navigate("/"); // redirect to login page
        } catch (error) {
            alert("Registration failed: " + error.message);
        }
    };

    return (
        <form onSubmit={handleRegister}>
            <h2>Register</h2>
            <input 
                type="text" 
                value={name} 
                onChange={(e) => setName(e.target.value)} 
                placeholder="Name" 
                required 
            />
            <input 
                type="email" 
                value={email} 
                onChange={(e) => setEmail(e.target.value)} 
                placeholder="Email" 
                required 
            />
            <input 
                type="password" 
                value={password} 
                onChange={(e) => setPassword(e.target.value)} 
                placeholder="Password" 
                required 
            />
            <button type="submit">Register</button>
        </form>
    );
}
