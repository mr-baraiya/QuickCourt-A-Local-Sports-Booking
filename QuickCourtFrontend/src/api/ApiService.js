// src/api/ApiService.js

const API_BASE_URL = "http://localhost:5085/api"; // Make sure this port matches your backend project

class ApiService {

    // Helper function to handle auth tokens if you implement them later
    _getAuthHeaders() {
        const token = localStorage.getItem('authToken'); // Example: getting token from storage
        return token ? { 'Authorization': `Bearer ${token}` } : {};
    }

    // =========================
    // Auth APIs
    // =========================
    async login(email, password) {
        try {
            const response = await fetch(`${API_BASE_URL}/Auth/login`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, password }),
            });
            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
            const data = await response.json();
            // Example: localStorage.setItem('authToken', data.token);
            return data;
        } catch (error) {
            console.error("Error during login:", error);
            throw error;
        }
    }

    async register(userData) {
        try {
            const response = await fetch(`${API_BASE_URL}/Auth/register`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(userData),
            });
            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
            return await response.json();
        } catch (error) {
            console.error("Error during registration:", error);
            throw error;
        }
    }


    // =========================
    // Sports APIs
    // =========================
    async fetchSports() {
        try {
            const response = await fetch(`${API_BASE_URL}/Sport/All`);
            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
            return await response.json();
        } catch (error) {
            console.error("Error fetching sports:", error);
            throw error;
        }
    }

    // =========================
    // Venues / Facilities APIs
    // =========================
    async fetchVenues() {
        try {
            const response = await fetch(`${API_BASE_URL}/Facilities`);
            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
            return await response.json();
        } catch (error) {
            console.error("Error fetching venues:", error);
            throw error;
        }
    }

    async fetchVenuesbyID(id) {
        try {
            const response = await fetch(`${API_BASE_URL}/Facilities/${id}`);
            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
            return await response.json();
        } catch (error) {
            console.error('Error fetching venue by ID:', error);
            throw error;
        }
    }

    async fetchVenuesPics() {
        try {
            const response = await fetch(`${API_BASE_URL}/FacilityPhotos`);
            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
            return await response.json();
        } catch (error) {
            console.error("Error fetching venue photos:", error);
            throw error;
        }
    }

    // =========================
    // Booking APIs
    // =========================
    async getAvailableSlots(facilityId, date) {
        try {
            const response = await fetch(`${API_BASE_URL}/Booking/available-slots?facilityId=${facilityId}&date=${date}`);
            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
            return await response.json();
        } catch (error) {
            console.error("Error fetching available slots:", error);
            throw error;
        }
    }

    async createBooking(bookingDetails) {
        try {
            const response = await fetch(`${API_BASE_URL}/Booking/create`, {
                method: 'POST',
                headers: { 
                    'Content-Type': 'application/json',
                    ...this._getAuthHeaders() 
                },
                body: JSON.stringify(bookingDetails),
            });
            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
            return await response.json();
        } catch (error) {
            console.error("Error creating booking:", error);
            throw error;
        }
    }
    
    async getMyBookings() {
        try {
            const response = await fetch(`${API_BASE_URL}/Bookings/MyBookings`, {
                headers: this._getAuthHeaders()
            });
            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
            return await response.json();
        } catch (error) {
            console.error("Error fetching user bookings:", error);
            throw error;
        }
    }


    // =========================
    // Admin Dashboard APIs
    // =========================
    async getDashboardStats() {
        try {
            const res = await fetch(`${API_BASE_URL}/Admin/dashboard/stats`, { headers: this._getAuthHeaders() });
            if (!res.ok) throw new Error(`HTTP error! status: ${res.status}`);
            return await res.json();
        } catch (error) {
            console.error("Error fetching dashboard stats:", error);
            throw error;
        }
    }

    async getBookingActivityData() {
        try {
            const res = await fetch(`${API_BASE_URL}/Admin/dashboard/charts/booking-activity`, { headers: this._getAuthHeaders() });
            if (!res.ok) throw new Error(`HTTP error! status: ${res.status}`);
            return await res.json();
        } catch (error) {
            console.error("Error fetching booking activity:", error);
            throw error;
        }
    }
    async GetMostActiveSports() {
    try {
        const res = await fetch(`${API_BASE_URL}/Admin/dashboard/charts/most-active-sports`, {
            headers: this._getAuthHeaders()
        });
        if (!res.ok) throw new Error(`HTTP error! status: ${res.status}`);
        return await res.json();
    } catch (error) {
        console.error("Error fetching active sports data:", error);
        throw error;
    }
}


    // =========================
    // Admin Facility Management APIs
    // =========================
    async getPendingFacilities() {
        try {
            const res = await fetch(`${API_BASE_URL}/Admin/facilities/pending`, { headers: this._getAuthHeaders() });
            if (!res.ok) throw new Error(`HTTP error! status: ${res.status}`);
            return await res.json();
        } catch (error) {
            console.error("Error fetching pending facilities:", error);
            throw error;
        }
    }

    async updateFacilityStatus(facilityId, newStatus, comments = '') {
        try {
            const res = await fetch(`${API_BASE_URL}/Admin/facilities/${facilityId}/status`, {
                method: "PUT",
                headers: { 
                    'Content-Type': 'application/json',
                    ...this._getAuthHeaders()
                },
                body: JSON.stringify({ newStatus, comments })
            });
            if (!res.ok) throw new Error(`HTTP error! status: ${res.status}`);
            return await res.json();
        } catch (error) {
            console.error("Error updating facility status:", error);
            throw error;
        }
    }
}

// Export a single instance of the class
export default new ApiService();
