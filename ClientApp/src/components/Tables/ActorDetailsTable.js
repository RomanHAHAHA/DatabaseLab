import React, { useState } from 'react';
import { Table, Dropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';

const ActorDetailsTable = ({ actorDetails, onEdit, onDelete }) => {
    const [openDropdowns, setOpenDropdowns] = useState({});

    const toggleDropdown = (actorId) => {
        setOpenDropdowns(prevState => ({
            ...prevState,
            [actorId]: !prevState[actorId] 
        }));
    };

    return (
        <Table striped bordered>
            <thead>
                <tr>
                    <th>ActorId</th>
                    <th>Email</th>
                    <th>Phone</th>
                    <th>Birthday</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                {actorDetails.map(detail => {
                    const isOpen = openDropdowns[detail.actorId]; 

                    return (
                        <tr key={detail.actorId}>
                            <td>{detail.actorId}</td>
                            <td>{detail.email}</td>
                            <td>{detail.phone}</td>
                            <td>{new Date(detail.birthday).toLocaleDateString()}</td>
                            <td>
                                <Dropdown isOpen={isOpen} toggle={() => toggleDropdown(detail.actorId)}>
                                    <DropdownToggle caret>
                                        Actions
                                    </DropdownToggle>
                                    <DropdownMenu>
                                        <DropdownItem onClick={() => onEdit(detail)}>Update</DropdownItem>
                                        <DropdownItem onClick={() => onDelete(detail.actorId)}>Delete</DropdownItem>
                                    </DropdownMenu>
                                </Dropdown>
                            </td>
                        </tr>
                    );
                })}
            </tbody>
        </Table>
    );
};

export default ActorDetailsTable;
