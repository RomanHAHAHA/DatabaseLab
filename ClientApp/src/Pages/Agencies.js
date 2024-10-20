import React, { useEffect, useState } from 'react';
import AgencyForm from '../components/Forms/AgencyForm';
import TableGenerator from '../components/Helpers/TableGenerator'; // Ensure this import is correct
import { Button, InputGroup, Input } from 'reactstrap';

const Agencies = () => {
    const [agencies, setAgencies] = useState([]);
    const [filteredData, setFilteredData] = useState([]);
    const [agencyToEdit, setAgencyToEdit] = useState(null);
    const [agencyId, setAgencyId] = useState('');

    const fetchAgencies = async () => {
        try {
            const response = await fetch('/api/agencies/get-all');
            if (!response.ok) throw new Error('Failed to fetch agencies');
            const data = await response.json();
            setAgencies(data);
        } catch (error) {
            console.error(error);
        }
    };

    useEffect(() => {
        fetchAgencies();
    }, []);

    const deleteAgency = async (id) => {
        try {
            const response = await fetch(`/api/agencies/delete/${id}`, { method: 'DELETE' });
            if (!response.ok) throw new Error('Failed to delete agency');
            setAgencies(prev => prev.filter(agency => agency.id !== id));
        } catch (error) {
            console.error(error);
        }
    };

    const handleEditAgency = (agency) => {
        setAgencyToEdit(agency);
    };

    const fetchActorGroups = async () => {
        if (!agencyId) {
            console.error('Agency ID is required');
            return;
        }
        try {
            const response = await fetch(`/api/agencies/with-actor-groups/${agencyId}`);
            if (!response.ok) throw new Error('Failed to fetch actor groups');
            const data = await response.json();
            setFilteredData(data); // Set filtered data with actor groups
        } catch (error) {
            console.error(error);
        }
    };

    const fetchMaxMinSpectacleBudget = async () => {
        try {
            const response = await fetch('/api/agencies/with-max-min-spectacle-budget');
            if (!response.ok) throw new Error('Failed to fetch max/min spectacle budget');
            const data = await response.json();
            setFilteredData(data); // Set filtered data with max/min budget
        } catch (error) {
            console.error(error);
        }
    };

    return (
        <div className="container mt-4">
            <div className="row">
                <div className="col-md-4">
                    <h1 className="mb-4">{agencyToEdit ? 'Update Agency' : 'Create a New Agency'}</h1>
                    <AgencyForm
                        onAgencyCreated={fetchAgencies}
                        agencyToEdit={agencyToEdit}
                        onAgencyUpdated={fetchAgencies}
                    />
                </div>
                <div className="col-md-8">
                    <h4>Agencies List</h4>
                    <TableGenerator data={agencies} handleEdit={handleEditAgency} deleteAgency={deleteAgency} /> 

                    <h4>Fetch Actor Groups</h4>
                    <InputGroup className="mb-3">
                        <Input 
                            type="number" 
                            placeholder="Enter Agency ID" 
                            value={agencyId} 
                            onChange={(e) => setAgencyId(e.target.value)} 
                        />
                        <Button color="primary" onClick={fetchActorGroups}>Load</Button>
                    </InputGroup>

                    <h4>Max & Min Spectacle Budget</h4>
                    <Button color="success" onClick={fetchMaxMinSpectacleBudget}>Load</Button>

                    <h4>Filtered Data</h4>
                    <TableGenerator data={filteredData} /> 
                </div>
            </div>
        </div>
    );
};

export default Agencies;
