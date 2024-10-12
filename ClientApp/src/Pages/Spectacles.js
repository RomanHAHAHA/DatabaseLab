import React, { useEffect, useState } from 'react';
import SpectacleForm from '../components/Forms/SpectacleForm';
import { Dropdown, DropdownToggle, DropdownMenu, DropdownItem, Input, Button, Row, Col } from 'reactstrap';
import 'bootstrap/dist/css/bootstrap.min.css';

const Spectacles = () => {
    const [spectacles, setSpectacles] = useState([]);
    const [spectacleToEdit, setSpectacleToEdit] = useState(null);
    const [dropdownOpen, setDropdownOpen] = useState(null);
    const [budget, setBudget] = useState('');
    const [productionYear, setProductionYear] = useState('');
    const [namePrefix, setNamePrefix] = useState('');

    const fetchSpectacles = async () => {
        try {
            const response = await fetch('/api/spectacles/get-all', { method: 'GET' });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const data = await response.json();
            setSpectacles(data);
        } catch (error) {
            console.error('Error fetching spectacles:', error);
        }
    };

    useEffect(() => {
        fetchSpectacles();
    }, []);

    const deleteSpectacle = async (id) => {
        try {
            const response = await fetch(`/api/spectacles/delete/${id}`, { method: 'DELETE' });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            setSpectacles(prevSpectacles => prevSpectacles.filter(spectacle => spectacle.id !== id));
        } catch (error) {
            console.error('Error deleting spectacle:', error);
        }
    };

    const handleEditSpectacle = (spectacle) => {
        setSpectacleToEdit(spectacle);
    };

    const handleSpectacleUpdated = () => {
        fetchSpectacles();
        setSpectacleToEdit(null);
    };

    const handleCreateSpectacle = () => {
        fetchSpectacles();
    };

    const toggleDropdown = (id) => {
        setDropdownOpen(dropdownOpen === id ? null : id);
    };

    const searchByBudget = async () => {
        const response = await fetch(`/api/spectacles/with-budget/${budget}`);
        const data = await response.json();
        setSpectacles(data);
    };

    const searchByProductionYear = async () => {
        const response = await fetch(`/api/spectacles/with-production-year/${productionYear}`);
        const data = await response.json();
        setSpectacles(data);
    };

    const searchByNamePrefix = async () => {
        const response = await fetch(`/api/spectacles/with-prefix/${namePrefix}`);
        const data = await response.json();
        setSpectacles(data);
    };

    const handleKeyPress = (e, searchFn) => {
        if (e.key === 'Enter') {
            searchFn();
        }
    };

    return (
        <div className="container mt-4">
            <div className="row">
                <div className="col-md-4">
                    <h1 className="mb-4">{spectacleToEdit ? 'Update Spectacle' : 'Create a New Spectacle'}</h1>
                    <SpectacleForm
                        onSpectacleCreated={handleCreateSpectacle}
                        spectacleToEdit={spectacleToEdit}
                        onSpectacleUpdated={handleSpectacleUpdated}
                    />
                </div>

                <div className="col-md-8">
                    <h2>Spectacles List</h2>

                    <div className="mb-3">
                        <h5>Search by Budget</h5>
                        <Row>
                            <Col xs="8">
                                <Input
                                    type="number"
                                    placeholder="Enter budget"
                                    value={budget}
                                    onChange={(e) => setBudget(e.target.value)}
                                    onKeyPress={(e) => handleKeyPress(e, searchByBudget)}
                                />
                            </Col>
                            <Col xs="4">
                                <Button onClick={searchByBudget} color="primary">Search</Button>
                            </Col>
                        </Row>
                    </div>

                    <div className="mb-3">
                        <h5>Search by Production Year</h5>
                        <Row>
                            <Col xs="8">
                                <Input
                                    type="number"
                                    placeholder="Enter production year"
                                    value={productionYear}
                                    onChange={(e) => setProductionYear(e.target.value)}
                                    onKeyPress={(e) => handleKeyPress(e, searchByProductionYear)}
                                />
                            </Col>
                            <Col xs="4">
                                <Button onClick={searchByProductionYear} color="primary">Search</Button>
                            </Col>
                        </Row>
                    </div>

                    <div className="mb-3">
                        <h5>Search by Name Prefix</h5>
                        <Row>
                            <Col xs="8">
                                <Input
                                    type="text"
                                    placeholder="Enter name prefix"
                                    value={namePrefix}
                                    onChange={(e) => setNamePrefix(e.target.value)}
                                    onKeyPress={(e) => handleKeyPress(e, searchByNamePrefix)}
                                />
                            </Col>
                            <Col xs="4">
                                <Button onClick={searchByNamePrefix} color="primary">Search</Button>
                            </Col>
                        </Row>
                    </div>

                    <div className="table-responsive">
                        <table className="table table-striped table-bordered">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Name</th>
                                    <th>Production Date</th>
                                    <th>Budget</th>
                                    <th>Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {spectacles.length === 0 ? (
                                    <tr>
                                        <td colSpan="5" className="text-center">No spectacles available.</td>
                                    </tr>
                                ) : (
                                    spectacles.map(spectacle => (
                                        <tr key={spectacle.id}>
                                            <td>{spectacle.id}</td>
                                            <td>{spectacle.name}</td>
                                            <td>{spectacle.productionDate}</td>
                                            <td>{spectacle.budget}</td>
                                            <td>
                                                <Dropdown isOpen={dropdownOpen === spectacle.id} toggle={() => toggleDropdown(spectacle.id)}>
                                                    <DropdownToggle caret>
                                                        Actions
                                                    </DropdownToggle>
                                                    <DropdownMenu>
                                                        <DropdownItem onClick={() => handleEditSpectacle(spectacle)}>
                                                            Update
                                                        </DropdownItem>
                                                        <DropdownItem onClick={() => deleteSpectacle(spectacle.id)}>
                                                            Delete
                                                        </DropdownItem>
                                                    </DropdownMenu>
                                                </Dropdown>
                                            </td>
                                        </tr>
                                    ))
                                )}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Spectacles;
