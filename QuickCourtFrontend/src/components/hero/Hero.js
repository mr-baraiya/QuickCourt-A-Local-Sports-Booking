import React from 'react';
import { FiMapPin } from 'react-icons/fi';

const Hero = () => {
  return (
    <section className="py-12 lg:py-20">
      <div className="flex flex-col lg:flex-row gap-8 lg:gap-12 items-center">
        {/* Hero Content */}
        <div className="flex-1 text-center lg:text-left">
          <h1 className="text-3xl sm:text-4xl lg:text-5xl font-bold text-gray-900 leading-tight mb-4">
            FIND SPORTS & VENUES
          </h1>
          <p className="text-lg text-gray-600 mb-8 max-w-md mx-auto lg:mx-0">
            Seamlessly explore sports venues and play with sports enthusiasts just like you.
          </p>
          
          {/* Location Input */}
          <div className="flex items-center justify-center lg:justify-start">
            <div className="flex items-center border border-gray-300 rounded-lg px-4 py-3 bg-white shadow-sm max-w-xs w-full">
              <FiMapPin className="text-gray-500 w-5 h-5 mr-3" />
              <input 
                type="text" 
                defaultValue="Ahmedabad" 
                className="flex-1 outline-none text-gray-700 bg-transparent"
                placeholder="Enter location"
              />
            </div>
          </div>
        </div>

        {/* Hero Image */}
        <div className="flex-1 w-full">
          <div className="bg-gray-100 rounded-lg overflow-hidden shadow-lg">
            <img 
              src="https://media.istockphoto.com/id/1474907309/photo/badminton-court-on-3d-illustration.jpg?s=612x612&w=0&k=20&c=YsqiUVVpBnzvla9ksmzrMZ6TF1fk3DXchSRPhpUttqQ=" 
              alt="Sports venue" 
              className="w-full h-64 sm:h-80 lg:h-96 object-cover"
            />
          </div>
        </div>
      </div>
    </section>
  );
};

export default Hero;