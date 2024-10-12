import React from 'react';
import { Table, Dropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';

const getRankLabel = (rank) => {
    const rankLabels = {
        0: 'Junior',
        1: 'Senior',
        2: 'Lead',
        3: 'HonoredArtist',
    };
    
    return rankLabels[rank] || 'Unknown'; 
};

const ActorTable = ({ actors, handleEditActor, deleteActor, toggleDropdown, dropdownOpen }) => {
    return (
        <Table striped bordered>
            <thead>
                <tr>
                    <th>Id</th>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th>Experience</th>
                    <th>Rank</th>
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
                        <td>
                            <Dropdown isOpen={dropdownOpen === actor.id} toggle={() => toggleDropdown(actor.id)}>
                                <DropdownToggle caret>
                                    Actions
                                </DropdownToggle>
                                <DropdownMenu>
                                    <DropdownItem onClick={() => handleEditActor(actor)}>Update</DropdownItem>
                                    <DropdownItem onClick={() => deleteActor(actor.id)}>Delete</DropdownItem>
                                </DropdownMenu>
                            </Dropdown>
                        </td>
                    </tr>
                ))}
            </tbody>
        </Table>
    );
};

export default ActorTable;
