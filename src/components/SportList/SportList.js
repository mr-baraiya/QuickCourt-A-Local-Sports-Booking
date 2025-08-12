import React from 'react';
import SectionHeader from '../shared/SectionHeader';
import SportCard from '../shared/SportCard';

const SportList = ({ sports }) => {
  // Do not render the component if there are no sports
  if (!sports || sports.length === 0) {
    return null;
  }

  return (
    <section style={{ marginTop: '60px' }}>
      <SectionHeader title="Popular Sports" />
      <div className="sport-list">
        {sports.map((sport) => (
          <SportCard key={sport.sportId || sport.id} sport={sport} />
        ))}
      </div>
    </section>
  );
};

export default SportList;