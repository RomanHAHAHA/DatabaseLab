import React from 'react';
import { Table } from 'reactstrap';

const ActorsWithPrivateDataTable = ({ actorsWithPrivateData }) => {
    return (
        <Table striped bordered className="mt-3">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th>Phone</th>
                    <th>Email</th>
                </tr>
            </thead>
            <tbody>
                {actorsWithPrivateData.map(actor => (
                    <tr key={actor.id}>
                        <td>{actor.id}</td>
                        <td>{actor.firstName}</td>
                        <td>{actor.lastName}</td>
                        <td>{actor.phone}</td>
                        <td>{actor.email}</td>
                    </tr>
                ))}
            </tbody>
        </Table>
    );
};

export default ActorsWithPrivateDataTable;
