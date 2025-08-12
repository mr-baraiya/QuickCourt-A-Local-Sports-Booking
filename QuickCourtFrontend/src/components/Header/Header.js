import React from 'react';
import { Link } from 'react-router-dom';
// 1. Import the FiHome icon
import { FiUser, FiCalendar, FiMapPin, FiHome } from 'react-icons/fi';

const Header = () => {
  return (
    <header className="header">
      <div className="header-logo">
        <Link to="/" style={{ textDecoration: 'none', color: 'inherit' }}>
          QUICKCOURT
        </Link>
      </div>
      <nav className="header-nav">
        {/* 2. Add the new "Home" link here */}
        <Link to="/user-dashboard"><FiHome /> Home</Link>
        <Link to="/venues"><FiMapPin /> Venues</Link>
        <Link to="/book"><FiCalendar /> Book</Link>
        <Link to="/login"><FiUser /> Login / Sign Up</Link>
      </nav>
    </header>
  );
};

export default Header;