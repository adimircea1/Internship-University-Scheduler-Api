export async function RemoveTokenAsync(){
    const url = new URL("http://localhost:5238/authentication/logout");

    const accessToken = localStorage.getItem("AccessToken");

    const requestOptions = {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        }    
    };

    await fetch(url, requestOptions)
    .then(response => response)
    .then(data => data)
    .catch(error => console.log(error));

    localStorage.clear();
}