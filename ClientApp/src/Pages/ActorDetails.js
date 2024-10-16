import React, { useEffect, useState } from 'react';
import ActorDetailForm from '../components/Forms/ActorDetailForm';
import ActorDetailsTable from '../components/Tables/ActorDetailsTable';
import TableGenerator from '../components/Helpers/TableGenerator';
import { Button, Input } from 'reactstrap';
import 'bootstrap/dist/css/bootstrap.min.css';

const ActorDetails = () => {
    const [actorDetails, setActorDetails] = useState([]);
    const [detailToEdit, setDetailToEdit] = useState(null);
    const [agencyId, setAgencyId] = useState('');
    const [filteredData, setFilteredData] = useState([]);

    const fetchAllActorDetails = async () => {
        try {
            const response = await fetch('/api/actor-details/get-all', { method: 'GET' });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const data = await response.json();
            setActorDetails(data);
        } catch (error) {
            console.error('Error fetching actor details:', error);
        }
    };

    const fetchActorsByAgencyId = async (id) => {
        try {
            const response = await fetch(`/api/actor-details/by-agency/${id}`, { method: 'GET' });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const data = await response.json();
            setFilteredData(data); 
        } catch (error) {
            console.error('Error fetching actors by agency ID:', error);
        }
    };

    useEffect(() => {
        fetchAllActorDetails();
    }, []);

    const deleteDetail = async (id) => {
        try {
            const response = await fetch(`/api/actor-details/delete/${id}`, { method: 'DELETE' });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            setActorDetails(prevDetails => prevDetails.filter(detail => detail.actorId !== id));
        } catch (error) {
            console.error('Error deleting actor detail:', error);
        }
    };

    const handleEditDetail = (detail) => {
        setDetailToEdit(detail);
    };

    const handleFetchActorsByAgency = () => {
        fetchActorsByAgencyId(agencyId);
    };

    const handleFormReset = () => {
        setDetailToEdit(null); 
    };

    return (
        <div className="container mt-4">
            <div className="row">
                <div className="col-md-4">
                    <ActorDetailForm
                        detailToUpdate={detailToEdit}
                        onDetailCreated={fetchAllActorDetails}
                        onDetailUpdated={() => {
                            fetchAllActorDetails();
                            handleFormReset(); 
                        }}
                    />
                </div>
                <div className="col-md-8">
                    <h4>Actor Details List</h4>
                    <ActorDetailsTable
                        actorDetails={actorDetails}
                        onEdit={handleEditDetail}
                        onDelete={deleteDetail}
                    />
                </div>
            </div>
            <div className="mt-4">
                <h4>Actors by Agency</h4>
                <Input 
                    type="number" 
                    value={agencyId} 
                    onChange={(e) => setAgencyId(e.target.value)} 
                    placeholder="Enter Agency ID" 
                />
                <Button onClick={handleFetchActorsByAgency} color="primary" className="mt-2">Fetch Actors</Button>
                
                <TableGenerator data={filteredData} /> 
            </div>
        </div>
    );
};

export default ActorDetails;
