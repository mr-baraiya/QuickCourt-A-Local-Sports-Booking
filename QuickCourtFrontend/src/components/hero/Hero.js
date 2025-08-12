import React from 'react';
import { FiMapPin } from 'react-icons/fi';

const Hero = () => {
  return (
    <section className="hero">
      <div className="hero-content"> 
        <h1>FIND SPORTS & VENUES </h1>
        <p>Seamlessly explore sports venues and play with sports enthusiasts just like you.</p>
        <div className="location-input">
          <FiMapPin color="#757575" />
          <input type="text" defaultValue="Ahmedabad" />
        </div>
      </div>
      <div className="hero-image">
        <span><img src="https://media.istockphoto.com/id/1474907309/photo/badminton-court-on-3d-illustration.jpg?s=612x612&w=0&k=20&c=YsqiUVVpBnzvla9ksmzrMZ6TF1fk3DXchSRPhpUttqQ=" alt="Hero" /></span>
      </div>
    </section>
  );
};

export default Hero;