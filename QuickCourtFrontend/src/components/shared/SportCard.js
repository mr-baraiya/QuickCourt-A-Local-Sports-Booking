import React from 'react';

const SportCard = ({ sport }) => {
  const name = sport?.name ?? 'Unknown Sport';
  // Use a placeholder image if the API doesn't provide one
  const imageUrl = sport?.iconUrl ?? `https://via.placeholder.com/300x400.png/2c3e50/ffffff?text=${name}`;
  
  return (
    <a href={`#sport/${name.toLowerCase()}`} className="sport-card">
      <img src={imageUrl} alt={name} loading="lazy" />
      <div className="sport-card-name">{name}</div>
    </a>
  );
};

export default SportCard;