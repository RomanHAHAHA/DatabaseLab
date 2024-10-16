import React, { useEffect, useState } from 'react';

const SpectacleForm = ({ onSpectacleCreated, spectacleToEdit, onSpectacleUpdated }) => {
    const [id, setId] = useState(null);
    const [name, setName] = useState('');
    const [productionDate, setProductionDate] = useState('');
    const [budget, setBudget] = useState(null);
    const [errors, setErrors] = useState({});

    useEffect(() => {
        if (spectacleToEdit) {
            setId(spectacleToEdit.id || null);
            setName(spectacleToEdit.name || '');
            setProductionDate(spectacleToEdit.productionDate || '');
            setBudget(spectacleToEdit.budget || null);
        } else {
            resetForm();
        }
    }, [spectacleToEdit]);

    const handleSubmit = async (e) => {
        e.preventDefault();
    
        const spectacle = {
            name,
            productionDate: parseInt(productionDate, 10),
            budget: parseFloat(budget),
        };
    
        const response = id 
            ? await fetch(`/api/spectacles/update/${id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(spectacle),
            })
            : await fetch('/api/spectacles/create', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(spectacle),
            });
    
        if (response.ok) {
            if (id) {
                onSpectacleUpdated();
            } else {
                onSpectacleCreated();
            }
            resetForm();
        } else {
            const errorData = await response.json();
            if (errorData.errors) {
                setErrors(errorData.errors);
            }
        }
    };

    const resetForm = () => {
        setId(null);
        setName('');
        setProductionDate('');
        setBudget(0);
        setErrors({});
    };

    return (
        <form onSubmit={handleSubmit} className="p-4 bg-light shadow rounded">
            {errors.general && (
                <div className="text-danger mb-3">
                    {errors.general.map((error, index) => (
                        <div key={index}>{error}</div>
                    ))}
                </div>
            )}
            <div className="mb-3">
                <label htmlFor="name" className="form-label">Name</label>
                <input
                    type="text"
                    id="name"
                    className="form-control"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                />
                {errors['Name'] && (
                    <div className="text-danger">{errors['Name'][0]}</div>
                )}
            </div>
            <div className="mb-3">
                <label htmlFor="productionDate" className="form-label">Production Date</label>
                <input
                    type="number"
                    id="productionDate"
                    className="form-control"
                    value={productionDate}
                    onChange={(e) => setProductionDate(e.target.value)}
                />
                {errors['ProductionDate'] && (
                    <div className="text-danger">{errors['ProductionDate'][0]}</div>
                )}
            </div>
            <div className="mb-3">
                <label htmlFor="budget" className="form-label">Budget</label>
                <input
                    type="number"
                    id="budget"
                    className="form-control"
                    value={budget}
                    onChange={(e) => setBudget(e.target.value)}
                />
                {errors['Budget'] && (
                    <div className="text-danger">{errors['Budget'][0]}</div>
                )}
            </div>
            <button type="submit" className="btn btn-primary w-100">
                {id ? 'Update Spectacle' : 'Create Spectacle'}
            </button>
        </form>
    );
};

export default SpectacleForm;
