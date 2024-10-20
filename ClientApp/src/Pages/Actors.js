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
    const [birthday, setBirthday] = useState('');
    const [spectacleCount, setSpectacleCount] = useState('');
    const [month, setMonth] = useState('');

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

    const fetchActorsWithContracts = async () => {
        try {
            const response = await fetch('/api/actors/with-contract-data');
            if (!response.ok) throw new Error('Failed to fetch actors with contracts');
            const data = await response.json();
            setFilteredData(data);
        } catch (error) {
            console.error(error);
        }
    };

    const fetchActorsWithPrivateData = async () => {
        try {
            const response = await fetch(`/api/actors/with-private-data/${birthday}`);
            if (!response.ok) throw new Error('Failed to fetch actors with private data');
            const data = await response.json();
            setFilteredData(data);
        } catch (error) {
            console.error(error);
        }
    };

    const fetchActorsWithSpectaclesCount = async () => {
        try {
            const response = await fetch(`/api/actors/with-spectacles-count/${spectacleCount}`);
            if (!response.ok) throw new Error('Failed to fetch actors with spectacles count');
            const data = await response.json();
            setFilteredData(data);
        } catch (error) {
            console.error(error);
        }
    };

    const fetchActorsBornInMonth = async () => {
        try {
            const response = await fetch(`/api/actors/born-in-month/${month}`);
            if (!response.ok) throw new Error('Failed to fetch actors born in month');
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
                    <Button color="primary" onClick={fetchActorsWithContracts}>
                        Load
                    </Button>

                    <h4>Actors with born after</h4>
                    <InputGroup className="mb-3" size="sm">
                        <Input
                            type="date"
                            placeholder="Enter birthday"
                            value={birthday}
                            onChange={e => setBirthday(e.target.value)}
                        />
                        <InputGroupText>
                            <Button color="secondary" onClick={fetchActorsWithPrivateData}>
                                Load
                            </Button>
                        </InputGroupText>
                    </InputGroup>

                    <h4>Actors with Spectacles Count</h4>
                    <InputGroup className="mb-3" size="sm">
                        <Input
                            type="number"
                            placeholder="Enter spectacles count"
                            value={spectacleCount}
                            onChange={e => setSpectacleCount(e.target.value)}
                        />
                        <InputGroupText>
                            <Button color="secondary" onClick={fetchActorsWithSpectaclesCount}>
                                Load
                            </Button>
                        </InputGroupText>
                    </InputGroup>

                    <h4>Actors Born in Month</h4>
                    <InputGroup className="mb-3" size="sm">
                        <Input
                            type="number"
                            placeholder="Enter month (1-12)"
                            value={month}
                            onChange={e => setMonth(e.target.value)}
                        />
                        <InputGroupText>
                            <Button color="secondary" onClick={fetchActorsBornInMonth}>
                                Load
                            </Button>
                        </InputGroupText>
                    </InputGroup>

                    <h4>Filtered Data</h4>
                    <TableGenerator data={filteredData} />
                </div>
            </div>
        </div>
    );
};

export default Actors;
