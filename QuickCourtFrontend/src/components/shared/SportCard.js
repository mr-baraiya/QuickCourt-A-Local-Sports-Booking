import React from 'react';

const SportCard = ({ sport }) => {
  const name = sport?.name ?? 'Unknown Sport';
  // Use a placeholder image if the API doesn't provide one
  const imageUrl = sport?.iconUrl ?? `https://via.placeholder.com/300x400.png/2c3e50/ffffff?text=${name}`;
  
  return (
    <a 
      href={`#sport/${name.toLowerCase()}`} 
      className="group bg-white rounded-lg shadow-md overflow-hidden hover:shadow-lg transform transition-all duration-200 hover:-translate-y-1"
    >
      <div className="aspect-square overflow-hidden">
        <img 
          src={imageUrl} 
          alt={name} 
          loading="lazy" 
          className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-200"
        />
      </div>
      <div className="p-4">
        <h3 className="text-center font-semibold text-gray-900 group-hover:text-accent transition-colors">
          {name}
        </h3>
      </div>
    </a>
  );
};

export default SportCard;