let accessToken = localStorage.getItem('AccessToken');

export async function GetExamByIdAsync(examId){
    const url = new URL(`http://localhost:5113/exams/${examId}`);
    let fetchedData;
    const requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
    };

    await fetch(url.href, requestOptions)
        .then(response => response.json())
        .then(data => fetchedData = data)
        .catch(error => console.log(error));

    return fetchedData;
}

export async function GetExamsAsync(maxEntities, currentPageNumber, orderBy, orderDirection){
    const paginationSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        OrderBy : orderBy,
        OrderDirection : orderDirection
    };

    const url = new URL("http://localhost:5113/exams/ordered");
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

export async function GetFilteredExamsAsync(maxEntities, currentPageNumber, filterBy){
    const filteringSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        FilterBy : filterBy
    };

    const url = new URL("http://localhost:5113/exams/filtered");
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

export async function GetFilteredAndOrderedExamsAsync(maxEntities, currentPageNumber, filterBy, orderBy, orderDirection){
    const filteringSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        FilterBy : filterBy,
        OrderBy : orderBy,
        OrderDirection : orderDirection
    };

    const url = new URL("http://localhost:5113/exams/filtered-ordered");
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

export async function GetAllExamsOfStudent(studentId){
    const url = new URL(`http://localhost:5113/exams/all/${studentId}`);
    let fetchedData;
    const requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
    };

    await fetch(url.href, requestOptions)
        .then(response => response.json())
        .then(data => fetchedData = data)
        .catch(error => console.log(error));

    return fetchedData;
}

export async function GetAvailableExamsOfStudent(studentId){
    const url = new URL(`http://localhost:5113/exams/available/${studentId}`);
    let fetchedData;
    const requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
    };

    await fetch(url.href, requestOptions)
        .then(response => response.json())
        .then(data => fetchedData = data)
        .catch(error => console.log(error));

    return fetchedData;
}

export async function GetUnavailableExamsOfStudent(studentId){
    const url = new URL(`http://localhost:5113/exams/unavailable/${studentId}`);
    let fetchedData;
    const requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
    };

    await fetch(url.href, requestOptions)
        .then(response => response.json())
        .then(data => fetchedData = data)
        .catch(error => console.log(error));

    return fetchedData;
}



