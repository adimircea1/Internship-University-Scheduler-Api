import { RemoveTokenAsync } from "./Logout.js";

const usernameField = document.getElementById('username');
const passwordField = document.getElementById('password');
const loginUrl = "http://localhost:5238/authentication/login";
let errorMessage = document.getElementById('errorMessage');
const loginButton = document.getElementById('loginBtn');

document.addEventListener("DOMContentLoaded", async function () {
   localStorage.clear();
});


loginButton.addEventListener('click', async (event) => {
    event.preventDefault();
    await LoginUserAsync()
});

async function LoginUserAsync() {
    const loginForm = document.getElementById('login-form');
    const formContent = loginForm.querySelectorAll('input');

    if(CheckIfEmpty(formContent) === true){
        errorMessage.innerText = "Please fill in the given fields!";
        errorMessage.style.color = 'red';
        return;
    }

    const loginRequest = {
        Username: usernameField.value,
        Password: passwordField.value
    };
    
    const requestOptions = {

        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(loginRequest)
    };
    
    try {
        const response = await fetch(loginUrl, requestOptions)
        
        if(!response.ok){
            throw new Error("Login failed, try again!");
        }
        
        const data = await response.json();
        localStorage.setItem('AccessToken', data.accessToken);
        localStorage.setItem('RefreshToken', data.refreshToken);
        RedirectUser();
    }
    catch(error){
        console.log(error);
        console.log(error.message);
        errorMessage.innerText = error.message;
        errorMessage.style.color = 'red';
        usernameField.value = '';
        passwordField.value = '';
        await RemoveTokenAsync();
    }
}

async function RedirectUser(){
    const url = new URL("http://localhost:5238/users/role");

    const accessToken = localStorage.getItem('AccessToken');

    const requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        }    
    };

    fetch(url, requestOptions)
    .then(response => {
        if(response.ok){
            return response.text();
        }
    })
    .then(async (data) => {
        console.log(data);
        switch(data.toString()){
            case 'Role: Admin':
                window.location.href = "AdminMainPage.html";
            break;

            case 'Role: Student':
                window.location.href = "StudentMainPage.html";
            break;

            case 'Role: Professor':
                window.location.href = "ProfessorMainPage.html";
            break;

            default:
                await RemoveTokenAsync();
            return;
        };
    })
    .catch(async (error) => {
        console.log(error);
        await RemoveTokenAsync();
    });
}

function CheckIfEmpty(formContent){
    for(const element of formContent){
        if(element.value === ''){
            return true;
        }
    }
    return false;
}