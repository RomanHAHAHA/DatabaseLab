import React, { useState, useEffect } from 'react';

const AgencyForm = ({ onAgencyCreated, agencyToEdit, onAgencyUpdated }) => {
    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [phone, setPhone] = useState('');
    const [address, setAddress] = useState('');
    const [errors, setErrors] = useState({});

    useEffect(() => {
        if (agencyToEdit) {
            setName(agencyToEdit.name);
            setEmail(agencyToEdit.email);
            setPhone(agencyToEdit.phone);
            setAddress(agencyToEdit.address);
        } else {
            resetForm();
        }
    }, [agencyToEdit]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        const agency = {
            name,
            email,
            phone,
            address
        };

        try {
            const response = agencyToEdit 
                ? await fetch(`/api/agencies/update/${agencyToEdit.id}`, {
                    method: 'PUT',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(agency),
                })
                : await fetch('/api/agencies/create', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(agency),
                });

            if (!response.ok) {
                const errorData = await response.json();
                if (errorData.errors) {
                    setErrors(errorData.errors);
                }
                return;
            }

            resetForm();
            agencyToEdit ? onAgencyUpdated() : onAgencyCreated();

        } catch (error) {
            console.error('Error saving agency:', error);
        }
    };

    const resetForm = () => {
        setName('');
        setEmail('');
        setPhone('');
        setAddress('');
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
                <label className="form-label">Name</label>
                <input
                    type="text"
                    className="form-control"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                />
                {errors['Name'] && (
                    <div className="text-danger">{errors['Name'][0]}</div>
                )}
            </div>

            <div className="mb-3">
                <label className="form-label">Email</label>
                <input
                    type="email"
                    className="form-control"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />
                {errors['Email'] && (
                    <div className="text-danger">{errors['Email'][0]}</div>
                )}
            </div>

            <div className="mb-3">
                <label className="form-label">Phone</label>
                <input
                    type="text"
                    className="form-control"
                    value={phone}
                    onChange={(e) => setPhone(e.target.value)}
                />
                {errors['Phone'] && (
                    <div className="text-danger">{errors['Phone'][0]}</div>
                )}
            </div>

            <div className="mb-3">
                <label className="form-label">Address</label>
                <input
                    type="text"
                    className="form-control"
                    value={address}
                    onChange={(e) => setAddress(e.target.value)}
                />
                {errors['Address'] && (
                    <div className="text-danger">{errors['Address'][0]}</div>
                )}
            </div>

            <button type="submit" className="btn btn-primary w-100">
                {agencyToEdit ? 'Update Agency' : 'Create Agency'}
            </button>
        </form>
    );
};

export default AgencyForm;
