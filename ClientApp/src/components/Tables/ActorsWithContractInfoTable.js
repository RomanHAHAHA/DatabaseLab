import React from 'react';
import { Table } from 'reactstrap';

const ActorsWithContractInfoTable = ({ actorsWithContracts }) => {
    return (
        <Table striped bordered className="mt-3">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th>Contract Count</th>
                    <th>Average Contract Price</th>
                </tr>
            </thead>
            <tbody>
                {actorsWithContracts.map(actor => (
                    <tr key={actor.id}>
                        <td>{actor.id}</td>
                        <td>{actor.firstName}</td>
                        <td>{actor.lastName}</td>
                        <td>{actor.contractCount}</td>
                        <td>{actor.averageContractPrice}</td>
                    </tr>
                ))}
            </tbody>
        </Table>
    );
};

export default ActorsWithContractInfoTable;
