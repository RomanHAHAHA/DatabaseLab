import React, { useState, useEffect } from 'react';

const ActorDetailForm = ({ onDetailCreated, detailToUpdate, onDetailUpdated }) => {
    const [actorId, setActorId] = useState('');
    const [phone, setPhone] = useState('');
    const [email, setEmail] = useState('');
    const [birthday, setBirthday] = useState('');
    const [errors, setErrors] = useState({});

    useEffect(() => {
        if (detailToUpdate) {
            setActorId(detailToUpdate.actorId);
            setPhone(detailToUpdate.phone);
            setEmail(detailToUpdate.email);
            setBirthday(new Date(detailToUpdate.birthday).toISOString().split('T')[0]);
        } else {
            resetForm();
        }
    }, [detailToUpdate]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        const actorDetail = {
            actorId: parseInt(actorId),  
            phone,
            email,
            birthday,
        };

        const response = detailToUpdate 
            ? await fetch(`/api/actor-details/update/${actorId}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(actorDetail),
            })
            : await fetch('/api/actor-details/create', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(actorDetail),
            });

        if (response.ok) {
            if (detailToUpdate) {
                onDetailUpdated();
            } else {
                onDetailCreated();
            }
            resetForm();
        } else {
            const errorData = await response.json();
            console.log("Validation Errors:", errorData.errors);
            if (errorData.errors) {
                setErrors(errorData.errors);
            }
        }
    };

    const resetForm = () => {
        setActorId('');
        setPhone('');
        setEmail('');
        setBirthday('');
        setErrors({});
    };

    return (
        <form onSubmit={handleSubmit} className="p-3 bg-light shadow-sm rounded">
            {errors.general && (
                <div className="text-danger mb-3">
                    {errors.general.map((error, index) => (
                        <div key={index}>{error}</div>
                    ))}
                </div>
            )}
            <div className="mb-3">
                <label htmlFor="actorId" className="form-label">Actor ID</label>
                <input
                    type="number"
                    id="actorId"
                    className="form-control"
                    value={actorId || ''}
                    onChange={(e) => setActorId(e.target.value)}
                />
                {errors['ActorId'] && (
                    <div className="text-danger">{errors['ActorId'][0]}</div>
                )}
            </div>
            <div className="mb-3">
                <label htmlFor="phone" className="form-label">Phone</label>
                <input
                    type="text"
                    id="phone"
                    className="form-control"
                    value={phone}
                    onChange={(e) => setPhone(e.target.value)}
                />
                {errors['Phone'] && (
                    <div className="text-danger">{errors['Phone'][0]}</div>
                )}
            </div>
            <div className="mb-3">
                <label htmlFor="email" className="form-label">Email</label>
                <input
                    id="email"
                    className="form-control"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />
                {errors['Email'] && (
                    <div className="text-danger">{errors['Email'][0]}</div>
                )}
            </div>
            <div className="mb-3">
                <label htmlFor="birthday" className="form-label">Birthday</label>
                <input
                    id="birthday"
                    className="form-control"
                    value={birthday}
                    onChange={(e) => setBirthday(e.target.value)}
                />
                {errors['Birthday'] && (
                    <div className="text-danger">{errors['Birthday'][0]}</div>
                )}
            </div>
            <button type="submit" className="btn btn-primary w-100">
                {detailToUpdate ? 'Update Details' : 'Create Details'}
            </button>
        </form>
    );
};

export default ActorDetailForm;
