import {DisplayInitialRegisterRequestTable} from "./RegisterRequestTableGenerator.js"

export async function RegisterUserAsync(registerId){
    const activeOptionElement = document.querySelector('.option-active');
    let url;
    switch(activeOptionElement.textContent){
        case "Student registrations": 
            url = new URL(`http://localhost:5238/authentication/student/register/${registerId}`);
        break;

        case "Professor registrations":
            url = new URL(`http://localhost:5238/authentication/professor/register/${registerId}`);
        break;

        default:
            return;
    }

    const accessToken = localStorage.getItem('AccessToken');
    const requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        }
    };

    let receivedEmail = "";
    await fetch(url, requestOptions)
        .then(response => response.text())
        .then(data => receivedEmail = data)
        .catch(error => {
            console.log(error);
            return;
        });

    await DisplayInitialRegisterRequestTable();
    return receivedEmail;
}


export async function GetUserByIdClaimAsync(){
    const accessToken = localStorage.getItem('AccessToken');
    const url = new URL(`http://localhost:5238/users/user`);
    const requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        }
    };

    let user;

    await fetch(url, requestOptions)
        .then(response => response.json())
        .then(data => user = data)
        .catch(error => {
            console.log(error);
            return;
        });

    return user;
}
