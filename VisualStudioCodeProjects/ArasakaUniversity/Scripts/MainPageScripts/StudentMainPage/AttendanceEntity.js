let accessToken = localStorage.getItem("AccessToken");

export async function FetchAttendanceDataAsync(maxEntities, currentPageNumber, orderBy, orderDirection) {
    const paginationSettings = {
        PageSize: maxEntities,
        PageNumber: currentPageNumber,
        OrderBy: orderBy,
        OrderDirection: orderDirection
    };

    const url = new URL("http://localhost:5150/attendances/ordered");
    let fetchedData;
    const requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${accessToken}`
        },
        body: JSON.stringify(paginationSettings)
    };

    await fetch(url.href, requestOptions)
        .then(response => response.json())
        .then(data => fetchedData = data)
        .catch(error => console.log(error));

    return fetchedData;
}

export async function FetchFilteredAttendanceDataAsync(maxEntities, currentPageNumber, filterBy) {
    const filteringSettings = {
        PageSize: maxEntities,
        PageNumber: currentPageNumber,
        FilterBy: filterBy
    };

    const url = new URL("http://localhost:5150/attendances/filtered");
    let fetchedData;
    const requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${accessToken}`
        },
        body: JSON.stringify(filteringSettings)
    };

    await fetch(url.href, requestOptions)
        .then(response => response.json())
        .then(data => fetchedData = data)
        .catch(error => console.log(error));

    return fetchedData;
}

export async function FetchFilteredAndOrderedAttendanceDataAsync(maxEntities, currentPageNumber, filterBy, orderBy, orderDirection) {
    const filteringSettings = {
        PageSize: maxEntities,
        PageNumber: currentPageNumber,
        FilterBy: filterBy,
        OrderBy: orderBy,
        OrderDirection: orderDirection
    };

    const url = new URL("http://localhost:5150/attendances/filtered-ordered");
    let fetchedData;
    const requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${accessToken}`
        },
        body: JSON.stringify(filteringSettings)
    };

    await fetch(url.href, requestOptions)
        .then(response => response.json())
        .then(data => fetchedData = data)
        .catch(error => console.log(error));

    return fetchedData;
}
