import React, { useEffect, useState } from 'react';
import ActorForm from '../components/Forms/ActorForm';
import ActorTable from '../components/Tables/ActorTable';
import TableGenerator from '../components/Helpers/TableGenerator';
import { Button, InputGroup, InputGroupText, Input } from 'reactstrap';

const Actors = () => {
    const [actors, setActors] = useState([]);
    const [filteredData, setFilteredData] = useState([]); 
    const [actorToEdit, setActorToEdit] = useState(null);
    const [dropdownOpen, setDropdownOpen] = useState(null);
    const [minAverageContractPrice, setMinAverageContractPrice] = useState('');

    const toggleDropdown = (actorId) => {
        setDropdownOpen(prev => (prev === actorId ? null : actorId));
    };

    const fetchAllActors = async () => {
        try {
            const response = await fetch('/api/actors/get-all');
            if (!response.ok) throw new Error('Failed to fetch actors');
            const data = await response.json();
            setActors(data);
        } catch (error) {
            console.error(error);
        }
    };

    const fetchActorsWithContracts = async (minPrice) => {
        try {
            const response = await fetch(`/api/actors/with-contract-data/${minPrice}`);
            if (!response.ok) throw new Error('Failed to fetch actors with contracts');
            const data = await response.json();
            setFilteredData(data); 
        } catch (error) {
            console.error(error);
        }
    };

    const fetchActorsWithPrivateData = async () => {
        try {
            const response = await fetch('/api/actors/with-private-data');
            if (!response.ok) throw new Error('Failed to fetch actors with private data');
            const data = await response.json();
            setFilteredData(data); 
        } catch (error) {
            console.error(error);
        }
    };

    const deleteActor = async (id) => {
        try {
            const response = await fetch(`/api/actors/delete/${id}`, { method: 'DELETE' });
            if (!response.ok) throw new Error('Failed to delete actor');
            fetchAllActors();
        } catch (error) {
            console.error(error);
        }
    };

    const handleEditActor = (actor) => {
        setActorToEdit(actor);
    };

    useEffect(() => {
        fetchAllActors();
    }, []);

    return (
        <div className="container mt-4">
            <div className="row">
                <div className="col-md-4">
                    <ActorForm
                        actorToUpdate={actorToEdit}
                        onActorCreated={fetchAllActors}
                        onActorUpdated={fetchAllActors}
                    />
                </div>
                <div className="col-md-8">
                    <h4>Actors List</h4>
                    <ActorTable
                        actors={actors}
                        handleEditActor={handleEditActor}
                        deleteActor={deleteActor}
                        toggleDropdown={toggleDropdown}
                        dropdownOpen={dropdownOpen}
                        fetchActors={fetchAllActors}
                    />

                    <h4>Actors with Contract Data</h4>
                    <InputGroup className="mb-3" size="sm">
                        <Input
                            placeholder="Min Contract Price"
                            value={minAverageContractPrice}
                            onChange={e => setMinAverageContractPrice(e.target.value)}
                        />
                        <InputGroupText>
                            <Button color="primary" onClick={() => fetchActorsWithContracts(minAverageContractPrice)}>
                                Load Actors with Contracts
                            </Button>
                        </InputGroupText>
                    </InputGroup>

                    <h4>Actors with Private Data</h4>
                    <Button color="secondary" onClick={fetchActorsWithPrivateData}>
                        Load Actors with Private Data
                    </Button>

                    <h4>Filtered Data</h4>
                    <TableGenerator data={filteredData} /> 
                </div>
            </div>
        </div>
    );
};

export default Actors;
