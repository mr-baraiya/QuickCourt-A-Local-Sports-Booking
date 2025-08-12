// src/pages/HomePage.js

import React from 'react';
import Hero from '../components/hero/Hero';
import VenueList from '../components/Venuelist/Venuelist';
import SportList from '../components/SportList/SportList';

// Note: We pass the data down from App.js as props
const HomePage = ({ sports, venues, venuePics }) => {
  return (
    <>
      <Hero />
      <VenueList venues={venues} venuePics={venuePics} />
      <SportList sports={sports} />
    </>
  );
};

export default HomePage;