import { GetCurrentProfessor } from "../AdminMainPage/ProfessorEntity.js";
import { GetUserByIdClaimAsync } from "../AdminMainPage/UserEntity.js";

const accessToken = localStorage.getItem("AccessToken");

export async function GenerateProfessorProfileAsync(){
    const professorFetchData = await GetCurrentProfessor();

    const pageContent = document.querySelector('.page-content')

    const profileHeaderContainer = document.createElement("div");
    profileHeaderContainer.classList.add("profile-header-container");

    const profileHeader = document.createElement("h2");
    profileHeader.className = "profile-header";
    profileHeader.textContent = "Profile";

    const profileDivider = document.createElement("hr");
    profileDivider.className = "profile-divider";

    profileHeaderContainer.appendChild(profileHeader);
    profileHeaderContainer.appendChild(profileDivider);

    const profileDataContainer = document.createElement("div");
    profileDataContainer.className = "profile-data-container";

    const profileInfo = document.createElement("div");
    profileInfo.className = "profile-info";

    const professorInfoHeading = document.createElement("h2");
    professorInfoHeading.textContent = "Professor info";

    const professorData = [
        { label: "First Name:", value: professorFetchData.firstName },
        { label: "Last Name:", value: professorFetchData.lastName },
        { label: "Phone Number:", value: professorFetchData.phoneNumber },
        { label: "Birthdate:", value: professorFetchData.birthdayDate }
    ];

    professorData.forEach(data => {
        const infoParagraph = document.createElement("p");
        infoParagraph.className = "professor-info";
        const strongElement = document.createElement("strong");
        strongElement.textContent = data.label;
        const spanElement = document.createElement("span");
        spanElement.className = "professor-info-data";
        spanElement.textContent = data.value;
        infoParagraph.appendChild(strongElement);
        infoParagraph.appendChild(spanElement);
        profileInfo.appendChild(infoParagraph);
    });

    const changePasswordSection = document.createElement("div");
    changePasswordSection.className = "change-password";

    const changePasswordHeading = document.createElement("h2");
    changePasswordHeading.textContent = "Change password";

    const oldPasswordInput = document.createElement("input");
    oldPasswordInput.className = "old-password";
    oldPasswordInput.type = "password"; 
    oldPasswordInput.placeholder = "Old password...";
    oldPasswordInput.required = true;

    const newPasswordInput = document.createElement("input");
    newPasswordInput.className = "new-password";
    newPasswordInput.type = "password"; 
    newPasswordInput.placeholder = "New password...";
    newPasswordInput.required = true;

    const confirmNewPassword = document.createElement("input");
    confirmNewPassword.className = "confirm-new-password";
    confirmNewPassword.type = "password"; 
    confirmNewPassword.placeholder = "Confirm password...";
    confirmNewPassword.required = true;

    const changePasswordButton = document.createElement("button");
    changePasswordButton.className = "change-password-btn";
    changePasswordButton.textContent = "Submit";
    changePasswordButton.addEventListener('click', async () => {
        const oldPassword = oldPasswordInput.value;
        const newPassword = newPasswordInput.value;
        const confirmPassword = confirmNewPassword.value;

        if (oldPassword.trim() === '' || newPassword.trim() === '' || confirmPassword.trim() === '') {
            alert("Before submitting, all fields must be filled!");
            oldPasswordInput.value = '';
            newPasswordInput.value = '';
            confirmNewPassword.value = '';
            return;
        }

        if (newPassword.trim() !== confirmPassword.trim()) {
            alert("The new password and confirm password do not match!");
            oldPasswordInput.value = '';
            newPasswordInput.value = '';
            confirmNewPassword.value = '';
            return;
        }

        const userId = (await GetUserByIdClaimAsync()).id;

        await ChangePasswordAsync(newPassword, oldPassword, userId);
        oldPasswordInput.value = '';
        newPasswordInput.value = '';
        confirmNewPassword.value = '';
    });

    profileDataContainer.appendChild(profileInfo);
    changePasswordSection.appendChild(changePasswordHeading);
    changePasswordSection.appendChild(oldPasswordInput);
    changePasswordSection.appendChild(newPasswordInput);
    changePasswordSection.appendChild(confirmNewPassword);
    changePasswordSection.appendChild(changePasswordButton);
    profileDataContainer.appendChild(changePasswordSection);

    pageContent.appendChild(profileHeaderContainer);
    pageContent.appendChild(profileDataContainer);
}

async function ChangePasswordAsync(newPassword, oldPassword, userId){
    const url = new URL(`http://localhost:5238/users/${userId}/new-password`);

    const contentToSend = {
        NewPassword : newPassword,
        OldPassword : oldPassword
    };

    const requestOptions = {
        method: "PATCH",
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
        body: JSON.stringify(contentToSend)
    };

    await fetch(url.href, requestOptions)  
    .then(async response => {
        if(response.ok){
            alert("Password change successful");
            return response.text();
        } else {
            const error = await response.json();
            const errors = error.errors;
            if(errors !== undefined){
                throw Error(response.status + " " + response.statusText + " --> " + errors);
            } else {
                throw Error(response.status + " " + response.statusText + " --> " + error);
            }
        }
    })
    .then(data => data)
    .catch(error => {
        alert(error);
    });
}

function camelCaseToWords(camelCase) {
    return camelCase.replace(/([a-z])([A-Z])/g, '$1 $2');
}
