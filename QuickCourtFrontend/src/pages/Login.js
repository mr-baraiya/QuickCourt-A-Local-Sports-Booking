import React, { useState } from "react";
import ApiService from "../api/ApiService";
import { useNavigate, Link } from "react-router-dom";
import "../styles/login.css";

export default function Login() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [role, setRole] = useState("User");
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const data = await ApiService.login(email, password);

            // Check if credentials & role match
            if (data && data.token && data.role === role) {
                // Save token & role
                localStorage.setItem("authToken", data.token);
                localStorage.setItem("userRole", data.role);

                // Redirect based on role
                if (data.role === "Admin") {
                    navigate("/admin-dashboard");
                } else if (data.role === "FacilityOwner") {
                    navigate("/owner-dashboard");
                } else if (data.role === "User") {
                    navigate("/user-dashboard");
                }
            } else {
                alert("Invalid role or credentials. Please try again.");
            }
        } catch (error) {
            alert("Login failed: " + error.message);
        }
    };

    return (
        <form onSubmit={handleLogin}>
            <h2>Login</h2>
            
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

            {/* Role Dropdown */}
            <select value={role} onChange={(e) => setRole(e.target.value)} required>
                <option value="User">User</option>
                <option value="Admin">Admin</option>
                <option value="FacilityOwner">Facility Owner</option>
            </select>

            <button type="submit">Login</button>

            <p style={{ marginTop: "10px" }}>
                Don't have an account? <Link to="/register">Register here</Link>
            </p>
        </form>
    );
}
