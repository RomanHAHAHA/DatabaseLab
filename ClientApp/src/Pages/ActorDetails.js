import React, { useEffect, useState } from 'react';
import ActorDetailForm from '../components/Forms/ActorDetailForm';
import { Table, Button } from 'reactstrap';
import 'bootstrap/dist/css/bootstrap.min.css';

const ActorDetails = () => {
    const [actorDetails, setActorDetails] = useState([]);
    const [detailToEdit, setDetailToEdit] = useState(null);

    const fetchAllActorDetails = async () => {
        try {
            const response = await fetch('/api/actor-details/get-all', { method: 'GET' });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const data = await response.json();
            setActorDetails(data);
        } catch (error) {
            console.error('Error fetching actor details:', error);
        }
    };

    useEffect(() => {
        fetchAllActorDetails();
    }, []);

    const deleteDetail = async (id) => {
        try {
            const response = await fetch(`/api/actor-details/delete/${id}`, { method: 'DELETE' });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            setActorDetails(prevDetails => prevDetails.filter(detail => detail.actorId !== id));
        } catch (error) {
            console.error('Error deleting actor detail:', error);
        }
    };

    const handleEditDetail = (detail) => {
        setDetailToEdit(detail);
    };

    return (
        <div className="container mt-4">
            <div className="row">
                <div className="col-md-4">
                    <ActorDetailForm
                        detailToUpdate={detailToEdit}
                        onDetailCreated={fetchAllActorDetails}
                        onDetailUpdated={fetchAllActorDetails}
                    />
                </div>
                <div className="col-md-8">
                    <h4>Actor Details List</h4>

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
                            {actorDetails.map(detail => (
                                <tr key={detail.actorId}>
                                    <td>{detail.actorId}</td>
                                    <td>{detail.email}</td>
                                    <td>{detail.phone}</td>
                                    <td>{new Date(detail.birthday).toLocaleDateString()}</td>
                                    <td>
                                        <Button onClick={() => handleEditDetail(detail)} color="info">Update</Button>
                                        <Button onClick={() => deleteDetail(detail.actorId)} color="danger">Delete</Button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </Table>
                </div>
            </div>
        </div>
    );
};

export default ActorDetails;
