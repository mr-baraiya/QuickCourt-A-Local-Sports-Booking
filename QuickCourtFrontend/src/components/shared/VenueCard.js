import { useNavigate } from "react-router-dom";

export default function VenueCard({ venue, imageUrl }) {
  const navigate = useNavigate(); // ✅ Correct usage

  return (
    <div
      className="bg-white rounded-lg shadow-md overflow-hidden cursor-pointer transform transition-transform hover:scale-105 hover:shadow-lg min-w-[280px] flex-shrink-0"
      onClick={() => navigate(`/venue/${venue.facilityId}`)} // Navigate when clicked
    >
      <div className="aspect-video overflow-hidden">
        <img
          src={imageUrl || "https://via.placeholder.com/300x200"}
          alt={venue.name}
          className="w-full h-full object-cover"
        />
      </div>
      <div className="p-4">
        <h3 className="text-lg font-semibold text-gray-900 mb-1 truncate">{venue.name}</h3>
        <p className="text-gray-600">{venue.city}</p>
      </div>
    </div>
  );
}
