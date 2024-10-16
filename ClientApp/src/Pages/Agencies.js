import React, { useState, useEffect } from 'react';
import AgencyForm from '../components/Forms/AgencyForm';
import { Dropdown, DropdownToggle, DropdownMenu, DropdownItem, Table, Button, Input } from 'reactstrap';
import 'bootstrap/dist/css/bootstrap.min.css';

const Agencies = () => {
    const [agencies, setAgencies] = useState([]);
    const [agencyToEdit, setAgencyToEdit] = useState(null);
    const [dropdownOpen, setDropdownOpen] = useState(null);
    const [agencyId, setAgencyId] = useState(''); 
    const [actorGroups, setActorGroups] = useState([]); 

    const toggleDropdown = (agencyId) => {
        setDropdownOpen(prev => (prev === agencyId ? null : agencyId));
    };

    const fetchAgencies = async () => {
        try {
            const response = await fetch('/api/agencies/get-all', { method: 'GET' });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const data = await response.json();
            setAgencies(data);
        } catch (error) {
            console.error('Error fetching agencies:', error);
        }
    };

    useEffect(() => {
        fetchAgencies();
    }, []);

    const deleteAgency = async (id) => {
        try {
            const response = await fetch(`/api/agencies/delete/${id}`, { method: 'DELETE' });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            setAgencies(prevAgencies => prevAgencies.filter(agency => agency.id !== id));
        } catch (error) {
            console.error('Error deleting agency:', error);
        }
    };

    const handleEditAgency = (agency) => {
        setAgencyToEdit(agency);
    };

    const handleAgencyUpdated = () => {
        fetchAgencies();
        setAgencyToEdit(null);
    };

    const handleCreateAgency = () => {
        fetchAgencies();
    };

    const fetchActorGroups = async () => {
        if (!agencyId) {
            alert('Please enter an agency ID.');
            return;
        }

        try {
            const response = await fetch(`/api/agencies/with-actor-groups/${agencyId}`, { method: 'GET' });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const data = await response.json();
            setActorGroups(data);
        } catch (error) {
            console.error('Error fetching actor groups:', error);
        }
    };

    return (
        <div className="container mt-4">
            <div className="row">
                <div className="col-md-4">
                    <h1 className="mb-4">{agencyToEdit ? 'Update Agency' : 'Create a New Agency'}</h1>
                    <AgencyForm
                        onAgencyCreated={handleCreateAgency}
                        agencyToEdit={agencyToEdit}
                        onAgencyUpdated={handleAgencyUpdated}
                    />
                </div>

                <div className="col-md-8">
                    <h2>Agencies List</h2>
                    {agencies.length === 0 ? (
                        <p>No agencies available.</p>
                    ) : (
                        <Table striped bordered>
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Name</th>
                                    <th>Email</th>
                                    <th>Phone</th>
                                    <th>Address</th>
                                    <th>Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {agencies.map(agency => (
                                    <tr key={agency.id}>
                                        <td>{agency.id}</td>
                                        <td>{agency.name}</td>
                                        <td>{agency.email}</td>
                                        <td>{agency.phone}</td>
                                        <td>{agency.address}</td>
                                        <td>
                                            <Dropdown isOpen={dropdownOpen === agency.id} toggle={() => toggleDropdown(agency.id)}>
                                                <DropdownToggle caret>
                                                    Actions
                                                </DropdownToggle>
                                                <DropdownMenu>
                                                    <DropdownItem onClick={() => handleEditAgency(agency)}>Update</DropdownItem>
                                                    <DropdownItem onClick={() => deleteAgency(agency.id)}>Delete</DropdownItem>
                                                </DropdownMenu>
                                            </Dropdown>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </Table>
                    )}

                   <div className="mt-4">
                        <h3>Fetch Actor Groups</h3>
                        <Input 
                            type="number" 
                            placeholder="Enter Agency ID" 
                            value={agencyId} 
                            onChange={(e) => setAgencyId(e.target.value)} 
                        />
                        <Button color="primary" onClick={fetchActorGroups} className="mt-2">Fetch Actor Groups</Button>
                    </div>

                    {actorGroups.length > 0 && (
                        <div className="mt-4">
                            <h4>Actor Groups</h4>
                            <Table striped bordered>
                                <thead>
                                    <tr>
                                        <th>Rank</th>
                                        <th>Actor Count</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {actorGroups.map((group, index) => (
                                        <tr key={index}>
                                            <td>{group.rank}</td>
                                            <td>{group.actorCount}</td>
                                        </tr>
                                    ))}
                                </tbody>
                            </Table>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default Agencies;
