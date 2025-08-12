import React from 'react';

const SectionHeader = ({ title, linkText, href }) => {
  return (
    <div className="flex justify-between items-center mb-6">
      <h2 className="text-2xl font-bold text-gray-900">{title}</h2>
      {linkText && (
        <a 
          href={href} 
          className="text-accent hover:text-blue-700 font-medium transition-colors"
        >
          {linkText}
        </a>
      )}
    </div>
  );
};

export default SectionHeader;