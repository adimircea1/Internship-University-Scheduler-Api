import { DisplayHomeTableMenu } from "./HomePageContentGenerator.js";

const upperBar = document.getElementById('upperBar');    
const upperBarEmail = upperBar.querySelector('.upperBarEmail');
const homeContent = document.getElementById('home-content');

let userData;

SetDirectory();
DisplayHomeTableMenu();

GetUserDataAsync();


async function GetUserDataAsync(){
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

    upperBarEmail.textContent = userData.email;
}

function SetDirectory(){
    const upperBarCurrentDirectory = document.querySelector('.upperBarCurrentDirectory');
    upperBarCurrentDirectory.innerHTML = `<i class="fas fa-home"></i> Home <span class="subdirectory"></span>`;
}