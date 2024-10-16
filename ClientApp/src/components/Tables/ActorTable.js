import React, { useState } from 'react';
import { Table, Dropdown, DropdownToggle, DropdownMenu, DropdownItem, Modal, ModalHeader, ModalBody, ModalFooter, Button, Input } from 'reactstrap';
import Swal from 'sweetalert2';

const getRankLabel = (rank) => {
    const rankLabels = {
        0: 'Junior',
        1: 'Senior',
        2: 'Lead',
        3: 'HonoredArtist',
    };
    
    return rankLabels[rank] || 'Unknown'; 
};

const ActorTable = ({ actors, handleEditActor, deleteActor, toggleDropdown, dropdownOpen, fetchActors }) => {
    const [modalOpen, setModalOpen] = useState(false);
    const [selectedActor, setSelectedActor] = useState(null);
    const [agencyId, setAgencyId] = useState(0);

    const toggleModal = (actor = null) => {
        setSelectedActor(actor);
        setModalOpen(!modalOpen);
        setAgencyId('');  
    };

    const handleAddToAgency = async () => {
        const actorId = selectedActor.id;
        console.log(`/api/actors/add-to-agency/${actorId}/${agencyId}`);
        if (selectedActor && agencyId) {
            try {
                const response = await fetch(`/api/actors/add-to-agency/${actorId}/${agencyId}`, {
                    method: 'PATCH',
                });

                if (response.ok) {
                    Swal.fire({
                        title: 'Success!',
                        text: 'Actor successfully added to agency!',
                        icon: 'success',
                        confirmButtonText: 'OK'
                    });

                    await fetchActors();
                } else if (response.status === 404) {
                    Swal.fire({
                        title: 'Error',
                        text: 'Actor or agency not found.',
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                } else {
                    Swal.fire({
                        title: 'Error',
                        text: 'Failed to add actor to agency.',
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                }
            } catch (error) {
                Swal.fire({
                    title: 'Error',
                    text: 'An error occurred while adding actor to agency.',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }

            setModalOpen(false);
        } else {
            Swal.fire({
                title: 'Validation error',
                text: 'Please enter a valid agency ID.',
                icon: 'warning',
                confirmButtonText: 'OK'
            });
        }
    };

    return (
        <>
            <Table striped bordered>
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>First Name</th>
                        <th>Last Name</th>
                        <th>Experience</th>
                        <th>Rank</th>
                        <th>Agency Id</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {actors.map(actor => (
                        <tr key={actor.id}>
                            <td>{actor.id}</td>
                            <td>{actor.firstName}</td>
                            <td>{actor.lastName}</td>
                            <td>{actor.experience}</td>
                            <td>{getRankLabel(actor.rank)}</td>
                            <td>{actor.agencyId == null ? 'NULL' : actor.agencyId}</td>
                            <td>
                                <Dropdown isOpen={dropdownOpen === actor.id} toggle={() => toggleDropdown(actor.id)}>
                                    <DropdownToggle caret>
                                        Actions
                                    </DropdownToggle>
                                    <DropdownMenu>
                                        <DropdownItem onClick={() => handleEditActor(actor)}>Update</DropdownItem>
                                        <DropdownItem onClick={() => deleteActor(actor.id)}>Delete</DropdownItem>
                                        <DropdownItem onClick={() => toggleModal(actor)}>Add to Agency</DropdownItem>
                                    </DropdownMenu>
                                </Dropdown>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>

            <Modal isOpen={modalOpen} toggle={() => toggleModal(null)}>
                <ModalHeader toggle={() => toggleModal(null)}>Add Actor to Agency</ModalHeader>
                <ModalBody>
                    <Input 
                        type="number" 
                        value={agencyId} 
                        onChange={(e) => setAgencyId(e.target.value)} 
                        placeholder="Enter agency ID" 
                    />
                </ModalBody>
                <ModalFooter>
                    <Button color="primary" onClick={handleAddToAgency}>Submit</Button>
                    <Button color="secondary" onClick={() => toggleModal(null)}>Cancel</Button>
                </ModalFooter>
            </Modal>
        </>
    );
};

export default ActorTable;
