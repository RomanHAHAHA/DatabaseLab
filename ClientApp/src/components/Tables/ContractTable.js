import React from 'react';
import { Dropdown, DropdownToggle, DropdownMenu, DropdownItem, Table } from 'reactstrap';

const ContractTable = ({ contracts, onEdit, onDelete, dropdownOpen, toggleDropdown }) => {
    return (
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
                                    <DropdownItem onClick={() => onEdit(contract)}>Update</DropdownItem>
                                    <DropdownItem onClick={() => onDelete(contract.id)}>Delete</DropdownItem>
                                </DropdownMenu>
                            </Dropdown>
                        </td>
                    </tr>
                ))}
            </tbody>
        </Table>
    );
};

export default ContractTable;
