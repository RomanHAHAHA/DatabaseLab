import React, { useEffect, useState } from 'react';
import ActorForm from '../components/Forms/ActorForm';
import ActorsSearchForm from '../components/Forms/ActorSearchForm';
import ActorTable from '../components/Tables/ActorTable';
import ActorsWithContractInfoTable from '../components/Tables/ActorsWithContractInfoTable';
import ActorsWithPrivateDataTable from '../components/Tables/ActorsWithPrivateDataTable';
import { Button, InputGroup, InputGroupText, Input } from 'reactstrap';

const Actors = () => {
    const [actors, setActors] = useState([]);
    const [actorsWithContracts, setActorsWithContracts] = useState([]);
    const [actorsWithPrivateData, setActorsWithPrivateData] = useState([]);
    const [actorToEdit, setActorToEdit] = useState(null);
    const [dropdownOpen, setDropdownOpen] = useState(null);
    const [searchPrefix, setSearchPrefix] = useState('');
    const [searchExperience, setSearchExperience] = useState('');
    const [searchRank, setSearchRank] = useState('');
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

    const fetchActorsByPrefix = async () => {
        if (!searchPrefix) return;
        try {
            const response = await fetch(`/api/actors/get-by-prefix/${searchPrefix}`);
            if (!response.ok) throw new Error('Failed to fetch actors by prefix');
            const data = await response.json();
            setActors(data);
        } catch (error) {
            console.error(error);
        }
    };

    const fetchActorsByExperience = async () => {
        if (!searchExperience) return;
        try {
            const response = await fetch(`/api/actors/get-by-experience/${searchExperience}`);
            if (!response.ok) throw new Error('Failed to fetch actors by experience');
            const data = await response.json();
            setActors(data);
        } catch (error) {
            console.error(error);
        }
    };

    const fetchActorsByRank = async () => {
        if (!searchRank) return;
        try {
            const response = await fetch(`/api/actors/get-by-rank/${searchRank}`);
            if (!response.ok) throw new Error('Failed to fetch actors by rank');
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
            setActorsWithContracts(data);
        } catch (error) {
            console.error(error);
        }
    };

    const fetchActorsWithPrivateData = async () => {
        try {
            const response = await fetch('/api/actors/with-private-data');
            if (!response.ok) throw new Error('Failed to fetch actors with private data');
            const data = await response.json();
            setActorsWithPrivateData(data);
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
        fetchActorsWithPrivateData();
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
                    <ActorsSearchForm
                        searchPrefix={searchPrefix}
                        setSearchPrefix={setSearchPrefix}
                        searchExperience={searchExperience}
                        setSearchExperience={setSearchExperience}
                        searchRank={searchRank}
                        setSearchRank={setSearchRank}
                        fetchActorsByPrefix={fetchActorsByPrefix}
                        fetchActorsByExperience={fetchActorsByExperience}
                        fetchActorsByRank={fetchActorsByRank}
                    />

                    <h4>Actors List</h4>
                    <ActorTable
                        actors={actors}
                        handleEditActor={handleEditActor}
                        deleteActor={deleteActor}
                        toggleDropdown={toggleDropdown}
                        dropdownOpen={dropdownOpen}
                    />

                    <h4>Actors with Contract Data</h4>
                    <InputGroup className="mb-3" size="sm">
                        <Input 
                            placeholder="Min Contract Price" 
                            value={minAverageContractPrice} 
                            onChange={e => setMinAverageContractPrice(e.target.value)} 
                        />
                        <InputGroupText>
                            <Button color="primary" onClick={() => fetchActorsWithContracts(minAverageContractPrice)}>Load</Button>
                        </InputGroupText>
                    </InputGroup>
                    <ActorsWithContractInfoTable actorsWithContracts={actorsWithContracts} />

                    <h4>Actors with Private Data</h4>
                    <ActorsWithPrivateDataTable actorsWithPrivateData={actorsWithPrivateData} />
                </div>
            </div>
        </div>
    );
};

export default Actors;
