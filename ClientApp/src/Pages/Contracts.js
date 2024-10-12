import React, { useState, useEffect } from 'react';
import ContractForm from '../components/Forms/ContractForm';
import { Dropdown, DropdownToggle, DropdownMenu, DropdownItem, Table, Button, Input } from 'reactstrap';
import 'bootstrap/dist/css/bootstrap.min.css';

const Contracts = () => {
    const [contracts, setContracts] = useState([]);
    const [contractToEdit, setContractToEdit] = useState(null);
    const [dropdownOpen, setDropdownOpen] = useState(null);
    const [filterPrice, setFilterPrice] = useState('');
    const [filterRole, setFilterRole] = useState('');
    const [filterActorId, setFilterActorId] = useState('');
    const [filterSpectacleId, setFilterSpectacleId] = useState('');

    const toggleDropdown = (contractId) => {
        setDropdownOpen(prev => (prev === contractId ? null : contractId));
    };

    const fetchContracts = async () => {
        try {
            const response = await fetch('/api/contracts/get-all', { method: 'GET' });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const data = await response.json();
            setContracts(data);
        } catch (error) {
            console.error('Error fetching contracts:', error);
        }
    };

    useEffect(() => {
        fetchContracts();
    }, []);

    const deleteContract = async (id) => {
        try {
            const response = await fetch(`/api/contracts/delete/${id}`, { method: 'DELETE' });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            setContracts(prevContracts => prevContracts.filter(contract => contract.id !== id));
        } catch (error) {
            console.error('Error deleting contract:', error);
        }
    };

    const handleEditContract = (contract) => {
        setContractToEdit(contract);
    };

    const handleContractUpdated = () => {
        fetchContracts();
        setContractToEdit(null);
    };

    const handleCreateContract = () => {
        fetchContracts();
    };

    const filterByPrice = async () => {
        try {
            const response = await fetch(`/api/contracts/with-price/${filterPrice}`, { method: 'GET' });
            if (response.ok) {
                const data = await response.json();
                setContracts(data);
            }
        } catch (error) {
            console.error('Error filtering contracts by price:', error);
        }
    };

    const filterByRole = async () => {
        try {
            const response = await fetch(`/api/contracts/with-role/${filterRole}`, { method: 'GET' });
            if (response.ok) {
                const data = await response.json();
                setContracts(data);
            }
        } catch (error) {
            console.error('Error filtering contracts by role:', error);
        }
    };

    const filterByActor = async () => {
        try {
            const response = await fetch(`/api/contracts/of-author/${filterActorId}`, { method: 'GET' });
            if (response.ok) {
                const data = await response.json();
                setContracts(data);
            }
        } catch (error) {
            console.error('Error filtering contracts by actor:', error);
        }
    };

    const filterBySpectacle = async () => {
        try {
            const response = await fetch(`/api/contracts/of-spectacle/${filterSpectacleId}`, { method: 'GET' });
            if (response.ok) {
                const data = await response.json();
                setContracts(data);
            }
        } catch (error) {
            console.error('Error filtering contracts by spectacle:', error);
        }
    };

    const handleKeyPress = (event, filterFunction) => {
        if (event.key === 'Enter') {
            filterFunction();
        }
    };

    return (
        <div className="container mt-4">
            <div className="row">
                <div className="col-md-4">
                    <h1 className="mb-4">{contractToEdit ? 'Update Contract' : 'Create a New Contract'}</h1>
                    <ContractForm
                        onContractCreated={handleCreateContract}
                        contractToEdit={contractToEdit}
                        onContractUpdated={handleContractUpdated}
                    />
                </div>

                <div className="col-md-8">
                    <h2>Contracts List</h2>
                    <div className="mb-3">
                        <h4>Filter Contracts</h4>
                        <div className="d-flex justify-content-between mb-2">
                            <div className="d-flex align-items-center me-2">
                                <Input
                                    type="number"
                                    placeholder="Price"
                                    value={filterPrice}
                                    onChange={(e) => setFilterPrice(e.target.value)}
                                    onKeyPress={(e) => handleKeyPress(e, filterByPrice)}
                                    style={{ width: '150px' }}
                                />
                                <Button className="ms-2" onClick={filterByPrice} color="primary">Filter</Button>
                            </div>
                            <div className="d-flex align-items-center me-2">
                                <Input
                                    type="text"
                                    placeholder="Role"
                                    value={filterRole}
                                    onChange={(e) => setFilterRole(e.target.value)}
                                    onKeyPress={(e) => handleKeyPress(e, filterByRole)}
                                    style={{ width: '150px' }}
                                />
                                <Button className="ms-2" onClick={filterByRole} color="primary">Filter</Button>
                            </div>
                        </div>
                        <div className="d-flex justify-content-between mb-2">
                            <div className="d-flex align-items-center me-2">
                                <Input
                                    type="number"
                                    placeholder="Actor ID"
                                    value={filterActorId}
                                    onChange={(e) => setFilterActorId(e.target.value)}
                                    onKeyPress={(e) => handleKeyPress(e, filterByActor)}
                                    style={{ width: '150px' }}
                                />
                                <Button className="ms-2" onClick={filterByActor} color="primary">Filter</Button>
                            </div>
                            <div className="d-flex align-items-center me-2">
                                <Input
                                    type="number"
                                    placeholder="Spectacle ID"
                                    value={filterSpectacleId}
                                    onChange={(e) => setFilterSpectacleId(e.target.value)}
                                    onKeyPress={(e) => handleKeyPress(e, filterBySpectacle)}
                                    style={{ width: '150px' }}
                                />
                                <Button className="ms-2" onClick={filterBySpectacle} color="primary">Filter</Button>
                            </div>
                        </div>
                    </div>
                    {contracts.length === 0 ? (
                        <p>No contracts available.</p>
                    ) : (
                        <Table striped bordered>
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Actor ID</th>
                                    <th>Spectacle ID</th>
                                    <th>Role</th>
                                    <th>Annual Contract Price</th>
                                    <th>Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {contracts.map(contract => (
                                    <tr key={contract.id}>
                                        <td>{contract.id}</td>
                                        <td>{contract.actorId}</td>
                                        <td>{contract.spectacleId}</td>
                                        <td>{contract.role}</td>
                                        <td>{contract.annualContractPrice}</td>
                                        <td>
                                            <Dropdown isOpen={dropdownOpen === contract.id} toggle={() => toggleDropdown(contract.id)}>
                                                <DropdownToggle caret>
                                                    Actions
                                                </DropdownToggle>
                                                <DropdownMenu>
                                                    <DropdownItem onClick={() => handleEditContract(contract)}>Update</DropdownItem>
                                                    <DropdownItem onClick={() => deleteContract(contract.id)}>Delete</DropdownItem>
                                                </DropdownMenu>
                                            </Dropdown>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </Table>
                    )}
                </div>
            </div>
        </div>
    );
};

export default Contracts;
