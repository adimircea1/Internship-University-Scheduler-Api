let accessToken = localStorage.getItem("AccessToken");

export async function GetCatalogueById(id){
    const url = new URL(`http://localhost:5150/catalogues/${id}`);
    const options = {
        method : "GET",
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${localStorage.getItem('AccessToken')}`
        }
    };

    let fetchedData;
    await fetch(url.href, options)
        .then(response => response.json())
        .then(data => fetchedData = data)
        .catch(error => console.log(error));

    return fetchedData;
}

export async function FetchFilteredAndOrderedCatalogueDataAsync(maxEntities, currentPageNumber, filterBy, orderBy, orderDirection){
    const filteringSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        FilterBy : filterBy,
        OrderBy : orderBy,
        OrderDirection : orderDirection
    };

    const url = new URL("http://localhost:5150/catalogues/filtered-ordered");
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

export async function FetchCatalogueDataAsync(maxEntities, currentPageNumber, orderBy, orderDirection){
    const paginationSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        OrderBy : orderBy,
        OrderDirection : orderDirection
    };

    const url = new URL("http://localhost:5150/catalogues/ordered");
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

export async function FetchFilteredCatalogueDataAsync(maxEntities, currentPageNumber, filterBy){
    const filteringSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        FilterBy : filterBy
    };

    const url = new URL("http://localhost:5150/catalogues/filtered");
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

