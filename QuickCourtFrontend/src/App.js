// src/App.js

import React, { useEffect, useState } from 'react';
// 1. Import routing components
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import ApiService from './api/ApiService';

// Import Layout Components
import Header from './components/Header/Header';
import Footer from './components/Footer/Footer';
import Loader from './components/shared/Loader';
import AdminSidebar from './pages/AdminSidebar';
// Import Page Components
import HomePage from './pages/HomePage';
import AllVenuesPage from './pages/AllVenuePage';
import SingleVenue from './pages/SingleVenue';
import AdminDashboard from './pages/AdminDashboard';
import FacilityApproval from './pages/FacilityApproval';
import Login from './pages/Login';
import Register from './pages/Register';

// Import styles
import './styles/App.css';
import './styles/components.css';

function UserDashboard() {
    return <h1>User Dashboard</h1>;
}

function OwnerDashboard() {
    return <h1>Facility Owner Dashboard</h1>;
}
function App() {
  // Data fetching is still done here so it can be passed to the homepage
  const [sports, setSports] = useState([]);
  const [venues, setVenues] = useState([]);
  const [venuePics, setVenuePics] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const loadData = async () => {
      try {
        setLoading(true);
        const [sportsData, venuesData, picsData] = await Promise.all([
          ApiService.fetchSports(),
          ApiService.fetchVenues(),
          ApiService.fetchVenuesPics(),
        ]);
        setSports(sportsData || []);
        setVenues(venuesData || []);
        setVenuePics(picsData || []);
        setError(null);
      } catch (err) {
        setError(err.message || 'Failed to load data.');
      } finally {
        setLoading(false);
      }
    };
    loadData();
  }, []);

  return (
    // 2. Wrap everything in a Router
    <Router>
      <div className="min-h-screen flex flex-col bg-gray-50">
        <Header />
        <main className="flex-1 max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-6">
          {loading ? (
            <Loader message="Loading awesome courts and sports..." />
          ) : error ? (
            <div className="text-center py-12 text-lg text-gray-600">{error}</div>
          ) : (
            // 3. Define the routes for your application
            <Routes>
              <Route
                path="/user-dashboard"
                element={
                  <HomePage
                    sports={sports}
                    venues={venues}
                    venuePics={venuePics}
                  />
                }
              />
              <Route path="/venues" element={<AllVenuesPage />} />
                <Route path="/venue/:facilityId" element={<SingleVenue />} />
                <Route path="/admin-dashboard" element={<AdminDashboard />} />
                <Route path="/owner-dashboard" element={<OwnerDashboard />} />
    <Route path="/admin/facilities" element={<FacilityApproval />} />
     <Route path="/" element={<Login />} />
                <Route path="/register" element={<Register />} />
              {/* You can add routes for /book and /login later */}
            </Routes>
          )}
        </main>
        <Footer />
      </div>
    </Router>
  );
}

export default App;