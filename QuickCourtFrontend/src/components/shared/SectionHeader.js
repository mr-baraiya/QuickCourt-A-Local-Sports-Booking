import React from 'react';

const SectionHeader = ({ title, linkText, href }) => {
  return (
    <div className="section-header">
      <h2>{title}</h2>
      {linkText && <a href={href}>{linkText}</a>}
    </div>
  );
};

export default SectionHeader;