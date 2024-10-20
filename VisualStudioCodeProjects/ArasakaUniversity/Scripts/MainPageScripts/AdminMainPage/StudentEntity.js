import { GetEntityInputData } from "./EntityToAddInputDataHandler.js";
import { DisplayInitialTable } from "./HomeEntityTableGenerator.js";
let accessToken = localStorage.getItem("AccessToken");


export async function FetchStudentDataAsync(maxEntities, currentPageNumber, orderBy, orderDirection){
    const paginationSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        OrderBy : orderBy,
        OrderDirection : orderDirection
    };

    const url = new URL("http://localhost:5150/students/ordered");
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

export async function FetchFilteredStudentDataAsync(maxEntities, currentPageNumber, filterBy){
    const filteringSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        FilterBy : filterBy
    };

    const url = new URL("http://localhost:5150/students/filtered");
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

export async function FetchFilteredAndOrderedStudentDataAsync(maxEntities, currentPageNumber, filterBy, orderBy, orderDirection){
    const filteringSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        FilterBy : filterBy,
        OrderBy : orderBy,
        OrderDirection : orderDirection
    };

    const url = new URL("http://localhost:5150/students/filtered-ordered");
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

export async function DeleteStudentByIdAsync(Id){
    const url = new URL(`http://localhost:5150/students/${Id}`);
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

export async function AddStudentAsync(){
    const url = new URL(`http://localhost:5150/students/student`);

    const studentData = GetEntityInputData();
    const studentToAdd = {
        FirstName : studentData.FirstName,
        LastName : studentData.LastName,
        Cnp : studentData.Cnp,
        PhoneNumber : studentData.PhoneNumber,
        Email : studentData.Email,
        BirthdayDate : studentData.BirthdayDate ? studentData.BirthdayDate : null,
        StudyYear : studentData.StudyYear,
        PersonalEmail: studentData.PersonalEmail
    };

    const requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
        body: JSON.stringify(studentToAdd)
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

export async function AddStudentFromRegisterRequestAsync(data){
    const url = new URL(`http://localhost:5150/students/student`);

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

export async function UpdateStudentById(updatedData, Id){
    const url = new URL(`http://localhost:5150/students/${Id}`);

    const studentUpdateJson = {};

    for(const key in updatedData){
        if(updatedData[key] !== ''){
            studentUpdateJson[key] = updatedData[key];
        }
    }

    const requestOptions = {
        method: 'PATCH',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
        body: JSON.stringify(studentUpdateJson)
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

export async function GetStudentByEmailAsync(email){
    console.log(email);
    const url = new URL(`http://localhost:5150/students/get-by-email`);
    let fetchedData;
    const requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${localStorage.getItem('AccessToken')}`
        },
        body: JSON.stringify(email)
    };

    await fetch(url, requestOptions)
    .then(async response => {
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return response.json();
    })
    .then(data => fetchedData = data)
    .catch(error => console.log("Fetch error:", error));

    return fetchedData;
}