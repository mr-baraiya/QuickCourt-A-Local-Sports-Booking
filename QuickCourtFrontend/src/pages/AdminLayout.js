import React from 'react';
import { Outlet } from 'react-router-dom';
import AdminSidebar from './AdminSidebar';

export default function AdminLayout() {
  return (
    <div style={{ display: 'flex', minHeight: '100vh' }}>
      <AdminSidebar />
      <main style={{ flexGrow: 1, padding: '20px', backgroundColor: '#f4f7f6' }}>
        {/* The nested admin routes will be rendered here */}
        <Outlet />
      </main>
    </div>
  );
}
