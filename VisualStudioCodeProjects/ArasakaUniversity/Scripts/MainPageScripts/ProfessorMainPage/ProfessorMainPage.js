const upperBar = document.getElementById('upperBar');    
const upperBarEmail = upperBar.querySelector('.upperBarEmail');

SetDirectory();
await SetUserCredentials();

export async function GetUserDataAsync(){
    let userData;
    const accessToken = localStorage.getItem('AccessToken');
    const url = new URL('http://localhost:5238/users/user');
    const requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization': `Bearer ${accessToken}`
        }
    };

    await fetch(url.href, requestOptions)
    .then(response => response.json())
    .then(data => userData = data)
    .catch(error => console.log(error));

   return userData;
}

function SetDirectory(){
    const upperBarCurrentDirectory = document.querySelector('.upperBarCurrentDirectory');
    upperBarCurrentDirectory.innerHTML = `<i class="fas fa-home"></i> Home <span class="subdirectory"></span>`;
}

async function SetUserCredentials(){
    let userData = await GetUserDataAsync();
    upperBarEmail.textContent = userData.email;
}