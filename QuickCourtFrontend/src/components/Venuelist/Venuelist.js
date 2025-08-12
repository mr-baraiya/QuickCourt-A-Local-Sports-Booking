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
    <section style={{ marginTop: '40px' }}>
      <SectionHeader title="Book Venues" linkText="See all venues >" href="#all-venues" />
      <div className="venue-list-container">
        <button className="scroll-arrow left" onClick={() => scroll('left')} aria-label="Scroll Left">
          <FiChevronLeft />
        </button>
        <div className="venue-scroll-wrapper" ref={scrollRef}>
          {venues.map((venue) => (
            <VenueCard key={venue.venueId || venue.id} venue={venue} />
          ))}
        </div>
        <button className="scroll-arrow right" onClick={() => scroll('right')} aria-label="Scroll Right">
          <FiChevronRight />
        </button>
      </div>
    </section>
  );
};

export default VenueList;