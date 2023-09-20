import { DisplayInitialRegisterRequestTable } from "./RegisterRequestTableGenerator.js";

let accessToken = localStorage.getItem("AccessToken");

export async function FetchRegisterRequestDataAsync(maxEntities, currentPageNumber, orderBy, orderDirection){
    const paginationSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        OrderBy : orderBy,
        OrderDirection : orderDirection
    };

    let fetchedData, url;
    const activeOptionElement = document.querySelector('.option-active');
    switch(activeOptionElement.textContent){
        case "Student registrations" : 
            url = new URL("http://localhost:5238/student-register-requests/ordered");
        break;

        case "Professor registrations" : 
             url = new URL("http://localhost:5238/professor-register-requests/ordered");
        break;

        default:
        return;
    }

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

export async function FetchFilteredRegisterRequestDataAsync(maxEntities, currentPageNumber, filterBy){
    const filteringSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        FilterBy : filterBy
    };

    let fetchedData, url;
    const activeOptionElement = document.querySelector('.option-active');
    switch(activeOptionElement.textContent){
        case "Student registrations" : 
            url = new URL("http://localhost:5238/student-register-requests/filtered");
        break;

        case "Professor registrations" :
            url = new URL("http://localhost:5238/professor-register-requests/filtered");
        break;

    }

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

export async function FetchFilteredAndOrderedRegisterRequestDataAsync(maxEntities, currentPageNumber, filterBy, orderBy, orderDirection){
    const filteringSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        FilterBy : filterBy,
        OrderBy : orderBy,
        OrderDirection : orderDirection
    };

    let fetchedData, url;
    const activeOptionElement = document.querySelector('.option-active');
    switch(activeOptionElement.textContent){
        case "Student registrations" : 
            url = new URL("http://localhost:5238/student-register-requests/filtered-ordered");
        break;

        case "Professor registrations" : 
            url = new URL("http://localhost:5238/professor-register-requests/filtered-ordered");
        break;

        default:
        return;
    }

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

export async function DeleteRegisterRequestByIdAsync(Id){
    let url;
    const activeOptionElement = document.querySelector('.option-active');
    switch(activeOptionElement.textContent){
        case "Student registrations" : 
            url = new URL(`http://localhost:5238/student-register-requests/${Id}`);
        break;

        case "Professor registrations" : 
            url =  new URL(`http://localhost:5238/professor-register-requests/${Id}`);
        break;

        default:
        return;
    }

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
        .catch(error => {
            console.log(error);
            return;
        }); 

    await DisplayInitialRegisterRequestTable();
}
