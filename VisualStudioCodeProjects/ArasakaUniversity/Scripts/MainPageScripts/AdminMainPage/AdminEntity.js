import { GetEntityInputData } from "./EntityToAddInputDataHandler.js";
import { DisplayInitialTable } from "./HomeEntityTableGenerator.js";

let accessToken = localStorage.getItem("AccessToken");

export async function FetchAdminDataAsync(maxEntities, currentPageNumber, orderBy, orderDirection){
    const paginationSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        OrderBy : orderBy,
        OrderDirection : orderDirection
    };

    const url = new URL("http://localhost:5238/users/ordered");
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

export async function FetchFilteredAdminDataAsync(maxEntities, currentPageNumber, filterBy){
    const filteringSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        FilterBy : filterBy
    };

    const url = new URL("http://localhost:5238/users/filtered");
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

export async function FetchFilteredAndOrderedAdminDataAsync(maxEntities, currentPageNumber, filterBy, orderBy, orderDirection){
    const filteringSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        FilterBy : filterBy,
        OrderBy : orderBy,
        OrderDirection : orderDirection
    };

    const url = new URL("http://localhost:5238/users/filtered-ordered");
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

export async function DeleteAdminByIdAsync(Id){
    const url = new URL(`http://localhost:5238/users/${Id}`);
    const requestOptions = {
        method: 'DELETE',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        }
    };

    await fetch(url.href, requestOptions)  
        .then(async (response) => {
            if(response.ok){
                await DisplayInitialTable();
            }
        })
        .then(data => console.log(data))
        .catch(error => console.log(error)); 
}

export async function AddAdminAsync(){
    const url = new URL(`http://localhost:5238/users/user`);

    const adminData = GetEntityInputData();
    const adminToAdd = {
        Username : adminData.UserName,
        Email : adminData.Email,
        Password : adminData.Password,
        Role : "Admin"
    };

    const requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
        body: JSON.stringify(adminToAdd)
    };

    const message = document.querySelector('.adding-status-message');
    let noError = true;

    await fetch(url.href, requestOptions)  
        .then(async response => {
            if(response.ok){
                await DisplayInitialTable();
                return response.text();
            } else {
                noError = false;
                const error = await response.json()
                const errors = error.errors;
                if(errors !== undefined){
                    throw Error(response.status + " " + response.statusText + " --> " + errors);
                } else {
                    throw Error(response.status + " " + response.statusText + " --> " + error);
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

export async function UpdateAdminById(updatedData, Id){
    const url = new URL(`http://localhost:5238/users/${Id}`);

    const adminUpdateJson = {};

    for(const key in updatedData){
        if(updatedData[key] !== ''){
            adminUpdateJson[key] = updatedData[key];
        }
    }

    const requestOptions = {
        method: 'PATCH',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
        body: JSON.stringify(adminUpdateJson)
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
            await DisplayInitialTable();
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