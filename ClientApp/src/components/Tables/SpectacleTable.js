import React from 'react';
import { Dropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';

const SpectacleTable = ({ spectacles, handleEditSpectacle, deleteSpectacle }) => { 
    const [dropdownOpen, setDropdownOpen] = React.useState(null);

    const toggleDropdown = (id) => {
        setDropdownOpen(dropdownOpen === id ? null : id);
    };

    return (
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
    );
};

export default SpectacleTable;
