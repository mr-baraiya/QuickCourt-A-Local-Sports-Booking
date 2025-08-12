import React from 'react';
import SectionHeader from '../shared/SectionHeader';
import SportCard from '../shared/SportCard';

const SportList = ({ sports }) => {
  // Do not render the component if there are no sports
  if (!sports || sports.length === 0) {
    return null;
  }

  return (
    <section className="mt-16 mb-12">
      <SectionHeader title="Popular Sports" />
      <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6 gap-4 sm:gap-6">
        {sports.map((sport) => (
          <SportCard key={sport.sportId || sport.id} sport={sport} />
        ))}
      </div>
    </section>
  );
};

export default SportList;