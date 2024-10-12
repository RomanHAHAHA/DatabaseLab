import React from 'react';
import { FormGroup, Label, Input, Button } from 'reactstrap';

const ActorRanks = [
    { value: 0, label: 'Junior' },
    { value: 1, label: 'Senior' },
    { value: 2, label: 'Lead' },
    { value: 3, label: 'HonoredArtist' },
];

const ActorsSearchForm = ({ 
    searchPrefix, 
    setSearchPrefix, 
    searchExperience, 
    setSearchExperience, 
    searchRank, 
    setSearchRank, 
    fetchActorsByPrefix, 
    fetchActorsByExperience, 
    fetchActorsByRank 
}) => {
    return (
        <div>
            <h4>Search Actors</h4>
            <FormGroup>
                <Label for="searchPrefix">Search by Surname Prefix:</Label>
                <Input
                    id="searchPrefix"
                    placeholder="Enter surname prefix"
                    value={searchPrefix}
                    onChange={e => setSearchPrefix(e.target.value)}
                />
                <Button color="primary" className="mt-2" onClick={fetchActorsByPrefix}>Search by Prefix</Button>
            </FormGroup>

            <FormGroup>
                <Label for="searchExperience">Search by Experience:</Label>
                <Input
                    id="searchExperience"
                    type="number"
                    placeholder="Enter experience in years"
                    value={searchExperience}
                    onChange={e => setSearchExperience(e.target.value)}
                />
                <Button color="primary" className="mt-2" onClick={fetchActorsByExperience}>Search by Experience</Button>
            </FormGroup>

            <FormGroup>
                <Label for="searchRank">Search by Rank:</Label>
                <Input
                    id="searchRank"
                    type="select"
                    value={searchRank}
                    onChange={e => setSearchRank(e.target.value)}
                >
                    <option value="">Select Rank</option>
                    {ActorRanks.map(rank => (
                        <option key={rank.value} value={rank.value}>{rank.label}</option>
                    ))}
                </Input>
                <Button color="primary" className="mt-2" onClick={fetchActorsByRank}>Search by Rank</Button>
            </FormGroup>
        </div>
    );
};

export default ActorsSearchForm;
