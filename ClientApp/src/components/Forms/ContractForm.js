import React, { useState, useEffect } from 'react';

const ContractForm = ({ onContractCreated, contractToEdit, onContractUpdated }) => {
    const [role, setRole] = useState('');
    const [annualContractPrice, setAnnualContractPrice] = useState(null);
    const [actorId, setActorId] = useState(null);
    const [spectacleId, setSpectacleId] = useState(null);
    const [errors, setErrors] = useState({}); 

    useEffect(() => {
        if (contractToEdit) {
            setRole(contractToEdit.role);
            setAnnualContractPrice(contractToEdit.annualContractPrice);
            setActorId(contractToEdit.actorId);
            setSpectacleId(contractToEdit.spectacleId);
        } else {
            resetForm();
        }
    }, [contractToEdit]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        const contract = {
            actorId,
            spectacleId,
            role,
            annualContractPrice: parseFloat(annualContractPrice),
        };

        try {
            const response = contractToEdit 
                ? await fetch(`/api/contracts/update/${contractToEdit.id}`, {
                    method: 'PUT',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(contract),
                })
                : await fetch('/api/contracts/create', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(contract),
                });

            if (!response.ok) {
                const errorData = await response.json();
                console.log("Validation Errors:", errorData.errors);
                if (errorData.errors) {
                    setErrors(errorData.errors);
                }
                return;
            }

            resetForm();
            contractToEdit ? onContractUpdated() : onContractCreated();

        } catch (error) {
            console.error('Error saving contract:', error);
        }
    };

    const resetForm = () => {
        setRole('');
        setAnnualContractPrice(null);
        setActorId(null);
        setSpectacleId(null);
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
                <label className="form-label">Role</label>
                <input
                    type="text"
                    className="form-control"
                    value={role}
                    onChange={(e) => setRole(e.target.value)}
                />
                {errors['Role'] && (
                    <div className="text-danger">{errors['Role'][0]}</div>
                )}
            </div>

            <div className="mb-3">
                <label className="form-label">Annual Contract Price</label>
                <input
                    className="form-control"
                    value={annualContractPrice}
                    onChange={(e) => setAnnualContractPrice(e.target.value)}
                />
                {errors['AnnualContractPrice'] && (
                    <div className="text-danger">{errors['AnnualContractPrice'][0]}</div>
                )}
            </div>

            <div className="mb-3">
                <label className="form-label">Actor ID</label>
                <input
                    type="text"
                    className="form-control"
                    value={actorId || ''}
                    onChange={(e) => setActorId(e.target.value)}
                />
                {errors['ActorId'] && (
                    <div className="text-danger">{errors['ActorId'][0]}</div>
                )}
            </div>

            <div className="mb-3">
                <label className="form-label">Spectacle ID</label>
                <input
                    type="text"
                    className="form-control"
                    value={spectacleId || ''}
                    onChange={(e) => setSpectacleId(e.target.value)}
                />
                {errors['SpectacleId'] && (
                    <div className="text-danger">{errors['SpectacleId'][0]}</div>
                )}
            </div>

            <button type="submit" className="btn btn-primary w-100">
                {contractToEdit ? 'Update Contract' : 'Create Contract'}
            </button>
        </form>
    );
};

export default ContractForm;
