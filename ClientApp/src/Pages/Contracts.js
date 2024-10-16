import React, { useState, useEffect } from 'react';
import ContractForm from '../components/Forms/ContractForm';
import ContractTable from '../components/Tables/ContractTable';
import TableGenerator from '../components/Helpers/TableGenerator';
import { Button, Input } from 'reactstrap';
import 'bootstrap/dist/css/bootstrap.min.css';

const Contracts = () => {
    const [contracts, setContracts] = useState([]);
    const [contractToEdit, setContractToEdit] = useState(null);
    const [dropdownOpen, setDropdownOpen] = useState(null);
    const [year, setYear] = useState('');
    const [filteredData, setFilteredData] = useState([]);

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

    const fetchFilteredContracts = async (year) => {
        try {
            const response = await fetch(`/api/contracts/in-each-agency/${year}`, { method: 'GET' });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const data = await response.json();
            setFilteredData(data);
        } catch (error) {
            console.error('Error fetching filtered contracts:', error);
        }
    };

    useEffect(() => {
        fetchContracts();
    }, []);

    const handleYearChange = (event) => {
        setYear(event.target.value);
    };

    const handleFetchFilteredContracts = () => {
        if (year) {
            fetchFilteredContracts(year);
        }
    };

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
                    <div className="mb-4">
                        <h2>Contracts List</h2>
                        {contracts.length === 0 ? (
                            <p>No contracts available.</p>
                        ) : (
                            <ContractTable
                                contracts={contracts}
                                onEdit={handleEditContract}
                                onDelete={deleteContract}
                                dropdownOpen={dropdownOpen}
                                toggleDropdown={toggleDropdown}
                            />
                        )}
                    </div>

                    <div>
                        <h2>Get Contract Counts by Agency for a Year</h2>
                        <Input
                            type="number"
                            placeholder="Enter Year"
                            value={year}
                            onChange={handleYearChange}
                        />
                        <Button color="primary" onClick={handleFetchFilteredContracts} className="mt-2">
                            Fetch Contracts
                        </Button>

                        <TableGenerator data={filteredData} />
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Contracts;
