import React, { useState } from "react";
import ApiService from "../api/ApiService";
import { useNavigate, Link } from "react-router-dom";

export default function Login() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [role, setRole] = useState("User");
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        setLoading(true);
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
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
            <div className="max-w-md w-full space-y-8">
                <div className="text-center">
                    <h2 className="text-3xl font-bold text-gray-900 mb-2">Welcome Back</h2>
                    <p className="text-gray-600">Sign in to your account</p>
                </div>

                <div className="bg-white rounded-lg shadow-md p-6 sm:p-8">
                    <form onSubmit={handleLogin} className="space-y-6">
                        <div>
                            <label htmlFor="email" className="block text-sm font-medium text-gray-700 mb-2">
                                Email Address
                            </label>
                            <input
                                id="email"
                                type="email"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                                placeholder="Enter your email"
                                required
                                className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-accent focus:border-accent transition-colors"
                            />
                        </div>

                        <div>
                            <label htmlFor="password" className="block text-sm font-medium text-gray-700 mb-2">
                                Password
                            </label>
                            <input
                                id="password"
                                type="password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                placeholder="Enter your password"
                                required
                                className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-accent focus:border-accent transition-colors"
                            />
                        </div>

                        <div>
                            <label htmlFor="role" className="block text-sm font-medium text-gray-700 mb-2">
                                Role
                            </label>
                            <select 
                                id="role"
                                value={role} 
                                onChange={(e) => setRole(e.target.value)} 
                                required
                                className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-accent focus:border-accent transition-colors"
                            >
                                <option value="User">User</option>
                                <option value="Admin">Admin</option>
                                <option value="FacilityOwner">Facility Owner</option>
                            </select>
                        </div>

                        <button 
                            type="submit" 
                            disabled={loading}
                            className="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-accent hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-accent disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
                        >
                            {loading ? "Signing in..." : "Sign In"}
                        </button>

                        <div className="text-center">
                            <p className="text-sm text-gray-600">
                                Don't have an account?{" "}
                                <Link 
                                    to="/register" 
                                    className="font-medium text-accent hover:text-blue-500 transition-colors"
                                >
                                    Register here
                                </Link>
                            </p>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}
