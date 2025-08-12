import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import ApiService from "../api/ApiService";

export default function SingleVenue() {
  const { facilityId } = useParams();
  const navigate = useNavigate();
  const [venue, setVenue] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const loadVenue = async () => {
      try {
        setLoading(true);
        const data = await ApiService.fetchVenuesbyID(facilityId);
        setVenue(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };
    loadVenue();
  }, [facilityId]);

  if (loading) return <p>Loading venue...</p>;
  if (error) return <p>Error: {error}</p>;
  if (!venue) return <p>No venue found</p>;

  return (
    <div style={{ padding: "20px" }}>
      <h1>{venue.name}</h1>
      <p><strong>Description:</strong> {venue.description}</p>
      <p><strong>Address:</strong> {venue.address}, {venue.city}, {venue.state}</p>
      <p><strong>Operating Hours:</strong> {venue.operatingHoursStart} - {venue.operatingHoursEnd}</p>

      {/* Amenities */}
      <section>
        <h2>Amenities</h2>
        <ul>
          <li>Parking</li>
          <li>Drinking Water</li>
          <li>Restrooms</li>
        </ul>
      </section>

      {/* About */}
      <section>
        <h2>About Venue</h2>
        <p>{venue.description}</p>
      </section>

      {/* Photo Gallery */}
      <section>
        <h2>Photo Gallery</h2>
        <div style={{ display: "flex", gap: "10px" }}>
          <img src="https://via.placeholder.com/150" alt="venue" />
          <img src="https://via.placeholder.com/150" alt="venue" />
          <img src="https://via.placeholder.com/150" alt="venue" />
        </div>
      </section>

      {/* Reviews */}
      <section>
        <h2>Reviews</h2>
        <p>No reviews yet.</p>
      </section>

      {/* Book Now */}
      <button
        style={{ padding: "10px 20px", marginTop: "20px" }}
        onClick={() => navigate("/book")}
      >
        Book Now
      </button>
    </div>
  );
}
