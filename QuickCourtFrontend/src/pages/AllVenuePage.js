import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom'; // ✅ Add this
import ApiService from '../api/ApiService';
import VenueCard from '../components/shared/VenueCard';
import Loader from '../components/shared/Loader';

const AllVenuesPage = () => {
  const [venues, setVenues] = useState([]);
  const [venuePics, setVenuePics] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate(); // ✅ For navigation

  useEffect(() => {
    const loadData = async () => {
      try {
        setLoading(true);
        const [venuesData, picsData] = await Promise.all([
          ApiService.fetchVenues(),
          ApiService.fetchVenuesPics(),
        ]);
        setVenues(venuesData || []);
        setVenuePics(picsData || []);
        setError(null);
      } catch (err) {
        setError(err.message || 'Failed to load venues.');
      } finally {
        setLoading(false);
      }
    };
    loadData();
  }, []);

  return (
    <div style={{ padding: '40px 0' }}>
      <h1 style={{ marginBottom: '24px' }}>All Venues</h1>
      
      {loading && <Loader message="Loading all venues..." />}
      {error && <div className="status-message">{error}</div>}
      
      {!loading && !error && (
        <div className="sport-list">
          {venues.map((venue) => {
            const matchingPic = venuePics.find(pic => pic.facilityId === venue.id);
            const imageUrl = matchingPic?.photoUrl;
            return (
              <div key={venue.facilityId} style={{ textAlign: 'center' }}>
                <VenueCard
                  venue={venue}
                  imageUrl={imageUrl}
                />
                {/* View Details Button */}
                <button
                  style={{
                    marginTop: '10px',
                    padding: '8px 12px',
                    backgroundColor: '#007bff',
                    color: 'white',
                    border: 'none',
                    borderRadius: '4px',
                    cursor: 'pointer'
                  }}
                  onClick={() => navigate(`/venue/${venue.facilityId}`)}
                >
                  View Details
                </button>
              </div>
            );
          })}
        </div>
      )}
    </div>
  );
};

export default AllVenuesPage;
