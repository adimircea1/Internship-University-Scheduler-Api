import { GetEntityInputData } from "./EntityToAddInputDataHandler.js";
import { DisplayInitialTable } from "./HomeEntityTableGenerator.js";
import { DisplayInitialModalTable } from "./AddStudentsInGroupModal.js";
import { DisplayInitialManageSidebar } from "./ManageGroupMenu.js";
let accessToken = localStorage.getItem("AccessToken");

// Fetch university group data
export async function FetchUniversityGroupDataAsync(maxEntities, currentPageNumber, orderBy, orderDirection){
    const paginationSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        OrderBy : orderBy,
        OrderDirection : orderDirection
    };

    const url = new URL("http://localhost:5150/groups/ordered");
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

// Fetch filtered university group data
export async function FetchFilteredUniversityGroupDataAsync(maxEntities, currentPageNumber, filterBy){
    const filteringSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        FilterBy : filterBy
    };

    const url = new URL("http://localhost:5150/groups/filtered");
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

// Fetch filtered and ordered university group data
export async function FetchFilteredAndOrderedUniversityGroupDataAsync(maxEntities, currentPageNumber, filterBy, orderBy, orderDirection){
    const filteringSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        FilterBy : filterBy,
        OrderBy : orderBy,
        OrderDirection : orderDirection
    };

    const url = new URL("http://localhost:5150/groups/filtered-ordered");
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

export async function DeleteUniversityGroupAsync(Id){
    const url = new URL(`http://localhost:5150/groups/${Id}`);
    const requestOptions = {
        method: 'DELETE',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        }
    };

    await fetch(url.href, requestOptions)  
        .then(response => {
            if(response.ok){
                DisplayInitialTable();
            }
        })
        .then(data => console.log(data))
        .catch(error => console.log(error)); 
}

export async function AddUniversityGroupAsync(){
    const url = new URL(`http://localhost:5150/groups/group`);

    const groupdata = GetEntityInputData();
    const groupToAdd = {
        MaxSize : groupdata.MaxSize,
        Name : groupdata.Name,
        Specialization : groupdata.Specialization
    };

    const requestOptions = {
        method: "POST",
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
        body: JSON.stringify(groupToAdd)
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

export async function AddStudentAsync(){
    const url = new URL(`http://localhost:5150/groups/group`);

    const universityGroupData = GetEntityInputData();
    const universityGroupToAdd = {
       NumberOfMembers : "0",
       MaxSize : universityGroupData.maxSize,
       Name : universityGroupData.name,
       DiscordLink: universityGroupData.discordLink,
       Specialization: universityGroupData.specialization
    };

    const requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
        body: JSON.stringify(universityGroupToAdd)
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

export async function UpdateUniversityGroupByIdAsync(updatedData, Id){
    const url = new URL(`http://localhost:5150/groups/${Id}`);
    const groupUpdateJson = {};

    for(const key in updatedData){
        if(updatedData[key] !== ''){
            groupUpdateJson[key] = updatedData[key];
        }
    }

    const requestOptions = {
        method: 'PATCH',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
        body: JSON.stringify(groupUpdateJson)
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

export async function GetFilteredAndOrderedStudentsFromGroupWithId(maxEntities, currentPageNumber, filterBy, orderBy, orderDirection, groupId){
    const filteringSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        FilterBy : filterBy,
        OrderBy : orderBy,
        OrderDirection : orderDirection
    };

    const url = new URL(`http://localhost:5150/groups/${groupId}/students`);
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

export async function RemoveStudentFromGroupAsync(studentId, groupId){
    const url = new URL(`http://localhost:5150/groups/${groupId}/student/${studentId}`);
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
                await DisplayInitialManageSidebar();
                await DisplayInitialTable();
            }
        })
        .then(data => data)
        .catch(error => console.log(error));
}

export async function AddStudentsInGroupAsync(studentIds, groupId){
    const url = new URL(`http://localhost:5150/groups/${groupId}/add-students`);
    const requestOptions = {
        method: 'PATCH',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
        body: JSON.stringify(studentIds)
    };

    await fetch(url.href, requestOptions)
        .then(async (response) => {
            if(response.ok){
                const modal = document.querySelector('.middle-modal-container');
                await DisplayInitialModalTable(modal);
                await DisplayInitialManageSidebar();
                await DisplayInitialTable();
            }
        })
        .then(data => data)
        .catch(error => console.log(error));

}