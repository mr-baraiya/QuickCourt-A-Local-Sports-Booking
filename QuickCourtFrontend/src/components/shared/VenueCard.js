import { useNavigate } from "react-router-dom";

export default function VenueCard({ venue, imageUrl }) {
  const navigate = useNavigate(); // ✅ Correct usage

  return (
    <div
      className="venue-card"
      onClick={() => navigate(`/venue/${venue.facilityId}`)} // Navigate when clicked
      style={{ cursor: "pointer" }}
    >
      {/* <img
        src={imageUrl || "https://via.placeholder.com/150"}
        alt={venue.name}
        style={{ width: "100%", height: "auto" }}
      /> */}
      <h3>{venue.name}</h3>
      <p>{venue.city}</p>
    </div>
  );
}
