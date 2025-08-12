import React from 'react';

const Loader = ({ message = 'Loading...' }) => {
  return (
    <div className="status-message">
      {message}
    </div>
  );
};

export default Loader;