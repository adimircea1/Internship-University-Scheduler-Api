const errorDiv = document.getElementById('errorDiv');
const errorMessage = errorDiv.querySelector('#errorMessage');
const roleSelectionContainer = document.querySelector('.role-selection-container');
const roleSelection = roleSelectionContainer.querySelector('.role-select');

document.addEventListener('DOMContentLoaded', async() =>{
    await GenerateRegisterPageAsync();
});

roleSelection.addEventListener('change', async () => {
    await GenerateRegisterPageAsync();
});

async function GenerateRegisterPageAsync(){
    let registerForm = document.getElementById('register-form')
    while (registerForm.firstChild) {
        registerForm.removeChild(registerForm.firstChild); 
    }
    const roleSelectionValue = roleSelection.value;
    switch(roleSelectionValue){
        case "student":
            await GenerateStudentRegistrationPageAsync();
            const phoneInputField = document.querySelector("#phone");
            const intlTelInputInstance = window.intlTelInput(phoneInputField, {
                utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
            });
            break;

        case "professor":
            await GenerateProfessorRegistrationPageAsync();
            const professorPhoneInputField = document.querySelector("#phone");
            const professorIntlTelInputInstance = window.intlTelInput(professorPhoneInputField, {
                utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
            });
            break;

        default:
            return;
    }
}

async function GenerateStudentRegistrationPageAsync(){
    const registerForm = document.getElementById('register-form')

    // Create form group for email and phone
    const emailPhoneGroup = document.createElement('div');
    emailPhoneGroup.classList.add('form-group', 'email-phone-group');

    // Create the email input
    const emailInput = document.createElement('input');
    emailInput.type = 'email';
    emailInput.id = 'email';
    emailInput.name = 'email';
    emailInput.placeholder = 'user@gmail.com';
    emailInput.required = true;

    // Create the phone input
    const phoneInput = document.createElement('input');
    phoneInput.id = 'phone';
    phoneInput.type = 'tel';
    phoneInput.name = 'phone';
    phoneInput.className = 'intl-tel-input';

    // Create split input div for email and phone
    const emailPhoneSplitInput = createSplitInput('Email:', emailInput);
    const phoneSplitInput = createSplitInput('Phone number:', phoneInput);

    emailPhoneGroup.appendChild(emailPhoneSplitInput);
    emailPhoneGroup.appendChild(phoneSplitInput);

    // Create form group for first name and last name
    const nameGroup = document.createElement('div');
    nameGroup.classList.add('form-group', 'name-group');

    // Create the first name input
    const firstNameInput = document.createElement('input');
    firstNameInput.type = 'text';
    firstNameInput.id = 'firstName';
    firstNameInput.name = 'firstName';
    firstNameInput.placeholder = 'John';
    firstNameInput.required = true;

    // Create the last name input
    const lastNameInput = document.createElement('input');
    lastNameInput.type = 'text';
    lastNameInput.id = 'lastName';
    lastNameInput.name = 'lastName';
    lastNameInput.placeholder = 'Doe';
    lastNameInput.required = true;

    // Create split input div for first name and last name
    const firstNameSplitInput = createSplitInput('First name:', firstNameInput);
    const lastNameSplitInput = createSplitInput('Last name:', lastNameInput);

    nameGroup.appendChild(firstNameSplitInput);
    nameGroup.appendChild(lastNameSplitInput);

    // Create form group for birthdate
    const birthdateGroup = document.createElement('div');
    birthdateGroup.className = 'form-group';

    // Create the birthdate input
    const birthdateInput = document.createElement('input');
    birthdateInput.type = 'date';
    birthdateInput.id = 'birthdate';
    birthdateInput.name = 'birthdate';
    birthdateInput.required = true;

    const splitBirthdate = createSplitInput('Birthdate:', birthdateInput);

    birthdateGroup.appendChild(splitBirthdate);

    // Create the Register button
    const registerButton = document.createElement('button');
    registerButton.id = 'registerBtn';
    registerButton.type = 'submit';
    registerButton.textContent = 'Register';
    registerButton.addEventListener('click', async (event) => {
        event.preventDefault();
        await RegisterStudentUserAsync()
    });

    // Append elements to the body or a container div
    registerForm.appendChild(emailPhoneGroup);
    registerForm.appendChild(nameGroup);
    registerForm.appendChild(birthdateGroup);
    registerForm.appendChild(registerButton);
}

async function GenerateProfessorRegistrationPageAsync(){
    const registerForm = document.getElementById('register-form')

    // Create form group for email and phone
    const emailPhoneGroup = document.createElement('div');
    emailPhoneGroup.classList.add('form-group', 'email-phone-group');

    // Create the email input
    const emailInput = document.createElement('input');
    emailInput.type = 'email';
    emailInput.id = 'email';
    emailInput.name = 'email';
    emailInput.placeholder = 'user@gmail.com';
    emailInput.required = true;

    // Create the phone input
    const phoneInput = document.createElement('input');
    phoneInput.id = 'phone';
    phoneInput.type = 'tel';
    phoneInput.name = 'phone';
    phoneInput.className = 'intl-tel-input';

    // Create split input div for email and phone
    const emailPhoneSplitInput = createSplitInput('Email:', emailInput);
    const phoneSplitInput = createSplitInput('Phone number:', phoneInput);

    emailPhoneGroup.appendChild(emailPhoneSplitInput);
    emailPhoneGroup.appendChild(phoneSplitInput);

    // Create form group for first name and last name
    const nameGroup = document.createElement('div');
    nameGroup.classList.add('form-group', 'name-group');

    // Create the first name input
    const firstNameInput = document.createElement('input');
    firstNameInput.type = 'text';
    firstNameInput.id = 'firstName';
    firstNameInput.name = 'firstName';
    firstNameInput.placeholder = 'John';
    firstNameInput.required = true;

    // Create the last name input
    const lastNameInput = document.createElement('input');
    lastNameInput.type = 'text';
    lastNameInput.id = 'lastName';
    lastNameInput.name = 'lastName';
    lastNameInput.placeholder = 'Doe';
    lastNameInput.required = true;

    // Create split input div for first name and last name
    const firstNameSplitInput = createSplitInput('First name:', firstNameInput);
    const lastNameSplitInput = createSplitInput('Last name:', lastNameInput);

    nameGroup.appendChild(firstNameSplitInput);
    nameGroup.appendChild(lastNameSplitInput);

    // Create form group for birthdate
    const birthdateGroup = document.createElement('div');
    birthdateGroup.className = 'form-group';

    // Create the birthdate input
    const birthdateInput = document.createElement('input');
    birthdateInput.type = 'date';
    birthdateInput.id = 'birthdate';
    birthdateInput.name = 'birthdate';
    birthdateInput.required = true;

    const splitBirthdate = createSplitInput('Birthdate:', birthdateInput);

    birthdateGroup.appendChild(splitBirthdate);

    // Create the Register button
    const registerButton = document.createElement('button');
    registerButton.id = 'registerBtn';
    registerButton.type = 'submit';
    registerButton.textContent = 'Register';
    registerButton.addEventListener('click', async (event) => {
        event.preventDefault();
        await RegisterProfessorUserAsync()
    });

    // Create form group for specialty
    const specialityContainer = document.createElement('div');
    specialityContainer.classList.add('form-group', 'speciality-group');

    // Create the specialty select input
    const specialitySelect = document.createElement('select');
    specialitySelect.id = 'speciality';
    specialitySelect.name = 'speciality';

    // Create options for the select input
    const specialities = ['Backend', 'ComputerScience', 'Dotnet', 'Frontend', 'Maths'];
    specialities.forEach(speciality => {
        const option = document.createElement('option');
        option.value = speciality;
        option.textContent = speciality;
        specialitySelect.appendChild(option);
    });

    const splitSpecialty = createSplitInput('Speciality:', specialitySelect);
    specialityContainer.appendChild(splitSpecialty);

    // Append elements to the body or a container div
    registerForm.appendChild(emailPhoneGroup);
    registerForm.appendChild(nameGroup);
    registerForm.appendChild(birthdateGroup);
    registerForm.appendChild(specialityContainer);
    registerForm.appendChild(registerButton);
}

// Function to create a split input div
function createSplitInput(labelText, inputElement) {
    const splitInputContainer = document.createElement('div');
    splitInputContainer.classList.add('split-input');

    const label = document.createElement('label');
    label.textContent = labelText;
    label.setAttribute('for', inputElement.id);

    splitInputContainer.appendChild(label);
    splitInputContainer.appendChild(inputElement);

    return splitInputContainer;
}


async function RegisterStudentUserAsync(){
    const registerForm = document.getElementById('register-form')
    const formContent = registerForm.querySelectorAll('input');
    const firstNameField = document.getElementById('firstName');
    const lastNameField = document.getElementById('lastName');
    const emailField = document.getElementById('email');
    const phoneNumberField = document.getElementById('phone');
    const birthdateField = document.getElementById('birthdate');

    if(CheckIfEmpty(formContent) === true){
        errorMessage.textContent = "Please fill in the given fields!";
        errorMessage.style.color = 'red';
        return;
    };


    if(validateGmail(emailField.value) === false){
        errorMessage.innerText = "Invalid email! Please use gmail!";
        errorMessage.style.color = 'red';
        return;
    }

    if (!ValidateText(firstNameField.value)) {
        errorMessage.innerText = "Invalid first name!";
        errorMessage.style.color = 'red';
        return;
    }

    if (!ValidateText(lastNameField.value)) {
        errorMessage.innerText = "Invalid last name!";
        errorMessage.style.color = 'red';
        return;
    }

    const registerRequest = {
        FirstName: firstNameField.value,
        LastName: lastNameField.value,
        Email: emailField.value,
        PhoneNumber: phoneNumberField.value,
        Birthdate: birthdateField.value
    };

    const registerUrl = new URL("http://localhost:5238/student-register-requests/register-request");

    const requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(registerRequest)
    };

    try {
        const response = await fetch(registerUrl.href, requestOptions);
        if(!response.ok){
        
            const error = await response.json()
            const errors = error.errors;
            if(errors !== undefined){
                throw Error(response.status + " " + response.statusText + " --> " + errors);
            } else {
                throw Error(response.status + " " + response.statusText + " --> " + error);
            }
        } 
    }
    catch (error) {
        console.log(error.message);
        errorMessage.textContent = error.message;
        errorMessage.style.color = 'red';
        ClearAllFields(formContent);
        return;
    }

    window.location.href = "Login.html";
}

async function RegisterProfessorUserAsync(){
    const registerForm = document.getElementById('register-form')
    const formContent = registerForm.querySelectorAll('input');
    const firstNameField = document.getElementById('firstName');
    const lastNameField = document.getElementById('lastName');
    const emailField = document.getElementById('email');
    const phoneNumberField = document.getElementById('phone');
    const birthdateField = document.getElementById('birthdate');
    const specialityField = document.getElementById('speciality');

    if(CheckIfEmpty(formContent) === true){
        errorMessage.textContent = "Please fill in the given fields!";
        errorMessage.style.color = 'red';
        return;
    };


    if(validateGmail(emailField.value) === false){
        errorMessage.innerText = "Invalid email! Please use gmail!";
        errorMessage.style.color = 'red';
        return;
    }

    if (!ValidateText(firstNameField.value)) {
        errorMessage.innerText = "Invalid first name!";
        errorMessage.style.color = 'red';
        return;
    }

    if (!ValidateText(lastNameField.value)) {
        errorMessage.innerText = "Invalid last name!";
        errorMessage.style.color = 'red';
        return;
    }

    const registerRequest = {
        FirstName: firstNameField.value,
        LastName: lastNameField.value,
        Email: emailField.value,
        PhoneNumber: phoneNumberField.value,
        Birthdate: birthdateField.value,
        Speciality: specialityField.value
    };

    const registerUrl = new URL("http://localhost:5238/professor-register-requests/register-request");

    const requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(registerRequest)
    };

    try {
        const response = await fetch(registerUrl.href, requestOptions);
        if(!response.ok){
        
            throw new Error("Register failed, try again!");
        } 
    }
    catch (error) {
        console.log(error.message);
        errorMessage.textContent = error.message;
        errorMessage.style.color = 'red';
        ClearAllFields(formContent);
        return;
    }

    window.location.href = "Login.html";
}

function CheckIfEmpty(formContent){
    for(const element of formContent){
        if(element.value === ''){
            return true;
        }
    }

    return false;
}

function ClearAllFields(formContent){
    for(const element of formContent){
        element.value = '';
    }
}

function ValidateEmail(email) {
    const pattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return pattern.test(email);
}

function ValidateText(input){
    const pattern = /^[A-Za-z\s]+$/;
    return pattern.test(input);
}

function validateGmail(email) {
    const gmailPattern = /^[a-zA-Z0-9._%+-]+@gmail\.com$/;
    return gmailPattern.test(email);
  }