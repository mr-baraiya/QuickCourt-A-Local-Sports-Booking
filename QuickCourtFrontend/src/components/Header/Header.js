import React, { useState } from 'react';
import { Link } from 'react-router-dom';
// 1. Import the FiHome icon
import { FiUser, FiCalendar, FiMapPin, FiHome, FiMenu, FiX } from 'react-icons/fi';

const Header = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  return (
    <header className="bg-white border-b border-gray-200 sticky top-0 z-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between items-center h-16">
          {/* Logo */}
          <div className="flex-shrink-0">
            <Link 
              to="/" 
              className="text-xl font-bold text-primary hover:text-accent transition-colors"
            >
              QUICKCOURT
            </Link>
          </div>

          {/* Desktop Navigation */}
          <nav className="hidden md:flex space-x-8">
            <Link 
              to="/user-dashboard" 
              className="flex items-center space-x-2 text-gray-700 hover:text-accent font-medium transition-colors"
            >
              <FiHome className="w-4 h-4" />
              <span>Home</span>
            </Link>
            <Link 
              to="/venues" 
              className="flex items-center space-x-2 text-gray-700 hover:text-accent font-medium transition-colors"
            >
              <FiMapPin className="w-4 h-4" />
              <span>Venues</span>
            </Link>
            <Link 
              to="/book" 
              className="flex items-center space-x-2 text-gray-700 hover:text-accent font-medium transition-colors"
            >
              <FiCalendar className="w-4 h-4" />
              <span>Book</span>
            </Link>
            <Link 
              to="/login" 
              className="flex items-center space-x-2 text-gray-700 hover:text-accent font-medium transition-colors"
            >
              <FiUser className="w-4 h-4" />
              <span>Login / Sign Up</span>
            </Link>
          </nav>

          {/* Mobile menu button */}
          <div className="md:hidden">
            <button
              onClick={() => setIsMenuOpen(!isMenuOpen)}
              className="text-gray-700 hover:text-accent focus:outline-none focus:text-accent"
            >
              {isMenuOpen ? <FiX className="w-6 h-6" /> : <FiMenu className="w-6 h-6" />}
            </button>
          </div>
        </div>

        {/* Mobile Navigation */}
        {isMenuOpen && (
          <div className="md:hidden">
            <div className="px-2 pt-2 pb-3 space-y-1 sm:px-3 bg-white border-t border-gray-200">
              <Link 
                to="/user-dashboard" 
                className="flex items-center space-x-2 px-3 py-2 text-gray-700 hover:text-accent font-medium transition-colors"
                onClick={() => setIsMenuOpen(false)}
              >
                <FiHome className="w-4 h-4" />
                <span>Home</span>
              </Link>
              <Link 
                to="/venues" 
                className="flex items-center space-x-2 px-3 py-2 text-gray-700 hover:text-accent font-medium transition-colors"
                onClick={() => setIsMenuOpen(false)}
              >
                <FiMapPin className="w-4 h-4" />
                <span>Venues</span>
              </Link>
              <Link 
                to="/book" 
                className="flex items-center space-x-2 px-3 py-2 text-gray-700 hover:text-accent font-medium transition-colors"
                onClick={() => setIsMenuOpen(false)}
              >
                <FiCalendar className="w-4 h-4" />
                <span>Book</span>
              </Link>
              <Link 
                to="/login" 
                className="flex items-center space-x-2 px-3 py-2 text-gray-700 hover:text-accent font-medium transition-colors"
                onClick={() => setIsMenuOpen(false)}
              >
                <FiUser className="w-4 h-4" />
                <span>Login / Sign Up</span>
              </Link>
            </div>
          </div>
        )}
      </div>
    </header>
  );
};

export default Header;