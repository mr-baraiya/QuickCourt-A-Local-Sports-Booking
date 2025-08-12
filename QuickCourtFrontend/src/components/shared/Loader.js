import React from 'react';

const Loader = ({ message = 'Loading...' }) => {
  return (
    <div className="flex flex-col items-center justify-center py-12">
      <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-accent mb-4"></div>
      <p className="text-lg text-gray-600">{message}</p>
    </div>
  );
};

export default Loader;