import React, { useState, useEffect } from 'react';
// CORRECTED IMPORT PATH: Changed from ../../api/ApiService
import ApiService from '../api/ApiService';

export default function FacilityApproval() {
    const [pending, setPending] = useState([]);
    const [loading, setLoading] = useState(true);

    const fetchPending = async () => {
        setLoading(true);
        try {
            const data = await ApiService.getPendingFacilities();
            setPending(data);
        } catch (error) {
            console.error("Failed to fetch pending facilities:", error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchPending();
    }, []);

    const handleUpdateStatus = async (facilityId, newStatus) => {
        let comments = '';
        if (newStatus === 'rejected') {
            comments = prompt("Please provide a reason for rejection:");
            if (!comments) {
                alert("Rejection requires a reason.");
                return;
            }
        }

        try {
            await ApiService.updateFacilityStatus(facilityId, newStatus, comments);
            alert(`Facility successfully ${newStatus}!`);
            fetchPending(); // Refresh the list
        } catch (error) {
            alert(`Failed to update status: ${error.message}`);
        }
    };

    if (loading) return <p>Loading facilities for approval...</p>;

    return (
        <div>
            <h1>Facility Approval Queue</h1>
            {pending.length === 0 ? <p>No pending facilities to review.</p> : (
                <div style={{ display: 'flex', flexDirection: 'column', gap: '15px' }}>
                    {pending.map(facility => (
                        <div key={facility.facilityId} style={{ border: '1px solid #ddd', padding: '20px', borderRadius: '8px', backgroundColor: 'white' }}>
                            <h3>{facility.name}</h3>
                            <p><strong>Owner:</strong> {facility.ownerName}</p>
                            <p><strong>Location:</strong> {facility.address}, {facility.city}</p>
                            <p><strong>Submitted:</strong> {new Date(facility.createdAt).toLocaleString()}</p>
                            <div style={{ marginTop: '15px' }}>
                                <button onClick={() => handleUpdateStatus(facility.facilityId, 'approved')} style={{ marginRight: '10px', padding: '8px 12px', cursor: 'pointer', backgroundColor: '#28a745', color: 'white', border: 'none', borderRadius: '4px' }}>Approve ✅</button>
                                <button onClick={() => handleUpdateStatus(facility.facilityId, 'rejected')} style={{ padding: '8px 12px', cursor: 'pointer', backgroundColor: '#dc3545', color: 'white', border: 'none', borderRadius: '4px' }}>Reject ❌</button>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}
