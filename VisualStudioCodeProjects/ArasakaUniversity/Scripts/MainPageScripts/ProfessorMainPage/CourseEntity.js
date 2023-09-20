export async function GetCourseByIdAsync(id){
    const url = new URL(`http://localhost:5150/courses/${id}`);
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