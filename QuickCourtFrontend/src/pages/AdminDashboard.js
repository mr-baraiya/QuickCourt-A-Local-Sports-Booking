import React, { useState, useEffect } from 'react';
import { Line, Bar } from 'react-chartjs-2';
import { Chart as ChartJS, CategoryScale, LinearScale, PointElement, LineElement, BarElement, Title, Tooltip, Legend } from 'chart.js';
// CORRECTED IMPORT PATH: Changed from ../../api/ApiService
import ApiService from '../api/ApiService';

// Register Chart.js components
ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, BarElement, Title, Tooltip, Legend);

export default function AdminDashboard() {
    const [stats, setStats] = useState(null);
    const [bookingData, setBookingData] = useState({});
    const [sportsData, setSportsData] = useState({});
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const [statsRes, bookingRes, sportsRes] = await Promise.all([
                    ApiService.getDashboardStats(),
                    ApiService.getBookingActivityData(),
                    ApiService.GetMostActiveSports() // Assuming this is the correct function name
                ]);

                setStats(statsRes);
                
                setBookingData({
                    labels: bookingRes.map(d => new Date(d.date).toLocaleDateString()),
                    datasets: [{
                        label: 'Bookings per Day',
                        data: bookingRes.map(d => d.count),
                        borderColor: 'rgb(75, 192, 192)',
                        tension: 0.1
                    }]
                });

                setSportsData({
                    labels: sportsRes.map(s => s.sportName),
                    datasets: [{
                        label: 'Total Bookings by Sport',
                        data: sportsRes.map(s => s.bookingCount),
                        backgroundColor: 'rgba(255, 99, 132, 0.5)',
                    }]
                });

            } catch (error) {
                console.error("Failed to fetch dashboard data:", error);
            } finally {
                setLoading(false);
            }
        };
        fetchData();
    }, []);

    if (loading) return <p>Loading dashboard...</p>;
    if (!stats) return <p>Could not load dashboard data.</p>;

    return (
        <div>
            <h1>Admin Dashboard</h1>
            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(4, 1fr)', gap: '20px', marginBottom: '40px' }}>
                <div style={cardStyle}><h3>{stats.totalUsers}</h3><p>Total Users</p></div>
                <div style={cardStyle}><h3>{stats.totalFacilityOwners}</h3><p>Facility Owners</p></div>
                <div style={cardStyle}><h3>{stats.totalBookings}</h3><p>Total Bookings</p></div>
                <div style={cardStyle}><h3>{stats.totalActiveCourts}</h3><p>Active Courts</p></div>
            </div>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '20px' }}>
                <div style={chartContainerStyle}>
                    <h2>Booking Activity</h2>
                    {bookingData.labels && <Line data={bookingData} />}
                </div>
                <div style={chartContainerStyle}>
                    <h2>Most Active Sports</h2>
                    {sportsData.labels && <Bar data={sportsData} />}
                </div>
            </div>
        </div>
    );
}

const cardStyle = {
    padding: '20px',
    borderRadius: '8px',
    boxShadow: '0 4px 6px rgba(0,0,0,0.1)',
    textAlign: 'center',
    backgroundColor: 'white'
};

const chartContainerStyle = {
    padding: '20px',
    borderRadius: '8px',
    boxShadow: '0 4px 6px rgba(0,0,0,0.1)',
    backgroundColor: 'white'
};
