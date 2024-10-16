import React, { useEffect, useState } from 'react';
import SpectacleForm from '../components/Forms/SpectacleForm';
import SpectacleTable from '../components/Tables/SpectacleTable';
import TableGenerator from '../components/Helpers/TableGenerator';
import { Button, InputGroup, Input } from 'reactstrap';

const Spectacles = () => {
    const [spectacles, setSpectacles] = useState([]);
    const [filteredData, setFilteredData] = useState([]); 
    const [spectacleToEdit, setSpectacleToEdit] = useState(null);
    const [minAveragePrice, setMinAveragePrice] = useState('');
    const [spectacleId, setSpectacleId] = useState('');

    const fetchAllSpectacles = async () => {
        try {
            const response = await fetch('/api/spectacles/get-all');
            if (!response.ok) throw new Error('Failed to fetch spectacles');
            const data = await response.json();
            setSpectacles(data);
        } catch (error) {
            console.error(error);
        }
    };

    useEffect(() => {
        fetchAllSpectacles();
    }, []);

    const fetchSpectaclesWithMinPrice = async () => {
        try {
            const response = await fetch(`/api/spectacles/with-total-info/${minAveragePrice}`);
            if (!response.ok) throw new Error('Failed to fetch spectacles with minimum price');
            const data = await response.json();
            setFilteredData(data); 
        } catch (error) {
            console.error(error);
        }
    };

    const fetchSpectaclesWithAdditionalData = async () => {
        if (!spectacleId) {
            console.error('Spectacle ID is required');
            return;
        }
        try {
            const response = await fetch(`/api/spectacles/with-actor-agency-name/${spectacleId}`);
            if (!response.ok) throw new Error('Failed to fetch spectacles with additional data');
            const data = await response.json();
            setFilteredData(data); 
        } catch (error) {
            console.error(error);
        }
    };

    const deleteSpectacle = async (id) => {
        try {
            const response = await fetch(`/api/spectacles/delete/${id}`, { method: 'DELETE' });
            if (!response.ok) throw new Error('Failed to delete spectacle');
            setSpectacles(prev => prev.filter(spectacle => spectacle.id !== id)); 
        } catch (error) {
            console.error(error);
        }
    };

    const handleEditSpectacle = (spectacle) => {
        setSpectacleToEdit(spectacle);
    };

    return (
        <div className="container mt-4">
            <div className="row">
                <div className="col-md-4">
                    <SpectacleForm
                        onSpectacleCreated={fetchAllSpectacles} 
                        spectacleToEdit={spectacleToEdit}
                        onSpectacleUpdated={fetchAllSpectacles}
                    />
                </div>
                <div className="col-md-8">
                    <h4>Spectacles List</h4>
                    <SpectacleTable
                        spectacles={spectacles}
                        handleEditSpectacle={handleEditSpectacle}
                        deleteSpectacle={deleteSpectacle} 
                    />

                    <h4>Spectacles with Minimum Price</h4>
                    <InputGroup className="mb-3" size="sm">
                        <Input
                            placeholder="Min Average Price"
                            value={minAveragePrice}
                            onChange={e => setMinAveragePrice(e.target.value)}
                        />
                        <Button color="primary" onClick={fetchSpectaclesWithMinPrice}>
                            Load Spectacles with Minimum Price
                        </Button>
                    </InputGroup>

                    <h4>Spectacle ID</h4>
                    <InputGroup className="mb-3" size="sm">
                        <Input
                            placeholder="Spectacle ID"
                            value={spectacleId}
                            onChange={e => setSpectacleId(e.target.value)}
                        />
                        <Button color="secondary" onClick={fetchSpectaclesWithAdditionalData}>
                            Load Spectacles with Additional Data
                        </Button>
                    </InputGroup>

                    <h4>Filtered Data</h4>
                    <TableGenerator data={filteredData} /> 
                </div>
            </div>
        </div>
    );
};

export default Spectacles;
