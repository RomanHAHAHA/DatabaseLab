import React, { useState, useEffect } from 'react';

const ActorForm = ({ onActorCreated, actorToUpdate, onActorUpdated }) => {
    const [id, setId] = useState(null);
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [middleName, setMiddleName] = useState('');
    const [rank, setRank] = useState('Junior');
    const [experience, setExperience] = useState(0);
    const [errors, setErrors] = useState({});

    useEffect(() => {
        if (actorToUpdate) {
            setId(actorToUpdate.id);
            setFirstName(actorToUpdate.firstName);
            setLastName(actorToUpdate.lastName);
            setMiddleName(actorToUpdate.middleName || '');
            setRank(actorToUpdate.rank);
            setExperience(actorToUpdate.experience);
        }
    }, [actorToUpdate]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        
        const actor = {
            firstName,
            lastName,
            middleName,
            rank,
            experience: parseInt(experience, 10),
        };

        const response = id 
            ? await fetch(`/api/actors/update/${id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(actor),
            })
            : await fetch('/api/actors/create', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(actor),
            });

        if (response.ok) {    
            if (id) {
                onActorUpdated();
            } else {
                onActorCreated();
            }
            resetForm();
        } else {
            const errorData = await response.json();
            if (errorData.errors) {
                console.log(errorData.errors);
                setErrors(errorData.errors);
            }
        }
    };

    const resetForm = () => {
        setId(null);
        setFirstName('');
        setLastName('');
        setMiddleName('');
        setRank('Junior');
        setExperience(0);
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
                <label htmlFor="firstName" className="form-label">First Name</label>
                <input
                    type="text"
                    id="firstName"
                    className="form-control"
                    value={firstName}
                    onChange={(e) => setFirstName(e.target.value)}
                />
                {errors['FirstName'] && (
                    <div className="text-danger">{errors['FirstName'][0]}</div>
                )}
            </div>
            <div className="mb-3">
                <label htmlFor="lastName" className="form-label">Last Name</label>
                <input
                    type="text"
                    id="lastName"
                    className="form-control"
                    value={lastName}
                    onChange={(e) => setLastName(e.target.value)}
                />
                {errors['LastName'] && (
                    <div className="text-danger">{errors['LastName'][0]}</div>
                )}
            </div>
            <div className="mb-3">
                <label htmlFor="middleName" className="form-label">Middle Name</label>
                <input
                    type="text"
                    id="middleName"
                    className="form-control"
                    value={middleName}
                    onChange={(e) => setMiddleName(e.target.value)}
                />
                {errors['MiddleName'] && (
                    <div className="text-danger">{errors['MiddleName'][0]}</div>
                )}
            </div>
            <div className="mb-3">
                <label htmlFor="rank" className="form-label">Rank</label>
                <select
                    id="rank"
                    className="form-select"
                    value={rank}
                    onChange={(e) => setRank(e.target.value)}
                >
                    <option value="Junior">Junior</option>
                    <option value="Senior">Senior</option>
                    <option value="Lead">Lead</option>
                    <option value="HonoredArtist">Honored Artist</option>
                </select>
                {errors['Rank'] && (
                    <div className="text-danger">{errors['Rank'][0]}</div>
                )}
            </div>
            <div className="mb-3">
                <label htmlFor="experience" className="form-label">Experience (years)</label>
                <input
                    type="number"
                    id="experience"
                    className="form-control"
                    value={experience}
                    onChange={(e) => setExperience(e.target.value)}
                    required
                />
                {errors['Experience'] && (
                    <div className="text-danger">{errors['Experience'][0]}</div>
                )}
            </div>
            <button type="submit" className="btn btn-primary w-100">
                {id ? 'Update Actor' : 'Create Actor'}
            </button>
        </form>
    );
};

export default ActorForm;
