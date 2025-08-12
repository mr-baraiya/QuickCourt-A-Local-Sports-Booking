import React, { useRef } from 'react';
import SectionHeader from '../shared/SectionHeader';
import VenueCard from '../shared/VenueCard';
import { FiChevronLeft, FiChevronRight } from 'react-icons/fi';

const VenueList = ({ venues }) => {
  const scrollRef = useRef(null);

  const scroll = (direction) => {
    const { current } = scrollRef;
    if (current) {
      const scrollAmount = direction === 'left' ? -300 : 300;
      current.scrollBy({ left: scrollAmount, behavior: 'smooth' });
    }
  };
  
  // Do not render the component if there are no venues
  if (!venues || venues.length === 0) {
    return null;
  }

  return (
    <section className="mt-12 mb-12">
      <SectionHeader title="Book Venues" linkText="See all venues >" href="#all-venues" />
      
      <div className="relative">
        {/* Left scroll button */}
        <button 
          className="absolute left-0 top-1/2 -translate-y-1/2 z-10 bg-white rounded-full shadow-lg p-2 hover:bg-gray-50 transition-colors hidden md:block"
          onClick={() => scroll('left')} 
          aria-label="Scroll Left"
        >
          <FiChevronLeft className="w-6 h-6 text-gray-600" />
        </button>

        {/* Scrollable container */}
        <div 
          className="flex gap-6 overflow-x-auto pb-4 px-2 md:px-12 scrollbar-hide"
          ref={scrollRef}
          style={{
            scrollbarWidth: 'none',
            msOverflowStyle: 'none',
          }}
        >
          {venues.map((venue) => (
            <VenueCard key={venue.venueId || venue.id} venue={venue} />
          ))}
        </div>

        {/* Right scroll button */}
        <button 
          className="absolute right-0 top-1/2 -translate-y-1/2 z-10 bg-white rounded-full shadow-lg p-2 hover:bg-gray-50 transition-colors hidden md:block"
          onClick={() => scroll('right')} 
          aria-label="Scroll Right"
        >
          <FiChevronRight className="w-6 h-6 text-gray-600" />
        </button>
      </div>

      <style jsx>{`
        .scrollbar-hide::-webkit-scrollbar {
          display: none;
        }
      `}</style>
    </section>
  );
};

export default VenueList;