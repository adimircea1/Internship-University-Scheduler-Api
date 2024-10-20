import { GetEntityInputData } from "./EntityToAddInputDataHandler.js";
import { DisplayInitialTable } from "./HomeEntityTableGenerator.js";

let accessToken = localStorage.getItem("AccessToken");

export async function FetchProfessorDataAsync(maxEntities, currentPageNumber, orderBy, orderDirection){
    const paginationSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        OrderBy : orderBy,
        OrderDirection : orderDirection
    };

    const url = new URL("http://localhost:5150/professors/ordered");
    let fetchedData;
    const requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
        body: JSON.stringify(paginationSettings)
    };

    await fetch(url.href, requestOptions)
        .then(response => response.json())
        .then(data => fetchedData = data)
        .catch(error => console.log(error));

    return fetchedData;
}

export async function GetCurrentProfessor(){
    const url = new URL("http://localhost:5150/professors/professor");
    let fetchedData;
    const requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        }
    };

    await fetch(url.href, requestOptions)
        .then(response => response.json())
        .then(data => fetchedData = data)
        .catch(error => console.log(error));

    return fetchedData;
}

export async function GetCurrentProfessorByEmail(email){
    const url = new URL(`http://localhost:5150/professors/get-by-email?email=${(email)}`);

    let fetchedData;
    const requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        }
    };

    await fetch(url, requestOptions)
    .then(async response => {
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const json = await response.json();
        return json;
    })
    .then(data => fetchedData = data)
    .catch(error => console.log("Error:", error));

    return fetchedData;
}

export async function AddProfessorFromRegisterRequestAsync(data){
    const url = new URL(`http://localhost:5150/professors/professor`);

    const requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
        body: JSON.stringify(data)
    };

    await fetch(url.href, requestOptions)  
        .then(response => response)
        .then(data => data)
        .catch(error => {
            console.log(error);
            return;
        }); 
}

export async function FetchFilteredProfessorDataAsync(maxEntities, currentPageNumber, filterBy){
    const filteringSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        FilterBy : filterBy
    };

    const url = new URL("http://localhost:5150/professors/filtered");
    let fetchedData;
    const requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
        body: JSON.stringify(filteringSettings)
    };

    await fetch(url.href, requestOptions)
        .then(response => response.json())
        .then(data => fetchedData = data)
        .catch(error => console.log(error));

    return fetchedData;
}

export async function FetchFilteredAndOrderedProfessorDataAsync(maxEntities, currentPageNumber, filterBy, orderBy, orderDirection){
    const filteringSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        FilterBy : filterBy,
        OrderBy : orderBy,
        OrderDirection : orderDirection
    };

    const url = new URL("http://localhost:5150/professors/filtered-ordered");
    let fetchedData;
    const requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
        body: JSON.stringify(filteringSettings)
    };


    await fetch(url.href, requestOptions)
        .then(response => response.json())
        .then(data => fetchedData = data)
        .catch(error => console.log(error));

    return fetchedData;
}

export async function DeleteProfessorByIdAsync(Id){
    const url = new URL(`http://localhost:5150/professors/${Id}`);
    const requestOptions = {
        method: 'DELETE',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        }
    };

    await fetch(url.href, requestOptions)  
        .then(response => response)
        .then(data => console.log(data))
        .catch(error => console.log(error)); 
}

export async function AddProfessorAsync(){
    const url = new URL(`http://localhost:5150/professors/professor`);

    const professorData = GetEntityInputData();
    const professorToAdd = {
        FirstName : professorData.FirstName,
        LastName : professorData.LastName,
        PhoneNumber : professorData.PhoneNumber,
        Email : professorData.Email,
        BirthdayDate : professorData.BirthdayDate ? professorData.BirthdayDate : null,
        Speciality : professorData.Speciality
    };

    const requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
        body: JSON.stringify(professorToAdd)
    };

    const message = document.querySelector('.adding-status-message');
    let noError = true;

    await fetch(url.href, requestOptions)  
        .then(async response => {
            if(response.ok){
                DisplayInitialTable();
                return response.text();
            } else {
                noError = false;
                const error = await response.json()
                const errors = error.errors;
                if(errors !== undefined){
                    throw Error(response.status + " " + response.statusText + " " + errors);
                } else {
                    throw Error(response.status + " " + response.statusText + " " + error);
                }
            }
        })
        .then(data => {
            if(noError === true){
                message.textContent = data;
                message.style.color = "green";
        }})
        .catch(error => {
            console.log(error);
            message.textContent = error;
            message.style.color = "red";
        }); 
}

export async function UpdateProfessorById(updatedData, id){
    console.log(id);
    const url = new URL(`http://localhost:5150/professors/${id}`);

    const professorUpdateJson = {};

    for(const key in updatedData){
        if(updatedData[key] !== ''){
            professorUpdateJson[key] = updatedData[key];
        }
    }

    const requestOptions = {
        method: 'PATCH',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
        body: JSON.stringify(professorUpdateJson)
    };

    const message = document.querySelector('.updating-status-message');
    let noError = true;
    await fetch(url.href, requestOptions)  
    .then(async response => {
        if(!response.ok){
            const error = await response.json()
            const errors = error.errors;
            noError = true;
            if(errors !== undefined){
                throw Error(response.status + " " + response.statusText + " --> " + errors);
            } else {
                throw Error(response.status + " " + response.statusText + " --> " + error);
            }
        } else {
            DisplayInitialTable();
            return response.text();
        }
    })
    .then(data => {
        if(noError === true){
            message.textContent = data;
            message.style.color = "green";
        }
    })
    .catch(error => {
        console.log(error);
        message.textContent = error;
        message.style.color = "red";
    }); 
}
