import { AddAdminAsync } from "./AdminEntity.js";
import { DisplayInitialTable } from "./HomeEntityTableGenerator.js";
import { AddProfessorAsync } from "./ProfessorEntity.js";
import { AddStudentAsync } from "./StudentEntity.js";
import { AddUniversityGroupAsync } from "./UniversityGroupEntity.js";

const pageContent = document.querySelector('.page-content');

export function GenerateAddEntitySidebar(message, tableHeaders, requirements) {
    const addEntitySidebar = document.createElement("div");
    addEntitySidebar.classList.add("add-entity-sidebar");

    const addEntitySidebarContent = document.createElement("div");
    addEntitySidebarContent.classList.add("add-entity-sidebar-content");

    const addEntitySidebarMessage = document.createElement("div");
    addEntitySidebarMessage.classList.add("add-entity-sidebar-message");

    const messageHeading = document.createElement("h3");

    messageHeading.textContent = message;
    addEntitySidebarMessage.appendChild(messageHeading);
    addEntitySidebarContent.appendChild(addEntitySidebarMessage);

    const itemsDiv = document.createElement('div');
    itemsDiv.classList.add('items-div');

    for(let i=0; i<tableHeaders.length; i++){
        const element = tableHeaders[i];
        const entitySidebarItem = document.createElement("div");
        entitySidebarItem.classList.add("add-entity-sidebar-item")

        const inputTextBoxLabel = document.createElement('label');
        inputTextBoxLabel.textContent = element;

        const inputTextBox = document.createElement('input'); 
        inputTextBox.type = "text";
        inputTextBox.classList.add("entityPropertyInputTextBox", `${element}`);
        if(requirements[i] === "required"){
            inputTextBox.required = 'true';
        }

        entitySidebarItem.appendChild(inputTextBoxLabel);
        entitySidebarItem.appendChild(inputTextBox);
        itemsDiv.appendChild(entitySidebarItem);
    }

    addEntitySidebarContent.appendChild(itemsDiv);

    const addEntityStatusMessageDiv = document.createElement('div');
    addEntityStatusMessageDiv.classList.add('adding-status-div');

    const addEntityStatusMessage = document.createElement('p');
    addEntityStatusMessage.classList.add('adding-status-message');

    addEntityStatusMessageDiv.appendChild(addEntityStatusMessage);
    addEntitySidebarContent.appendChild(addEntityStatusMessageDiv);

    const addEntitySidebarButtons = document.createElement("div");
    addEntitySidebarButtons.classList.add("add-entity-sidebar-buttons");
    const saveButton = document.createElement("button");
    saveButton.classList.add("add-entity-sidebar-save-button", "add-entity-sidebar-button");
    saveButton.textContent = "Save";
    saveButton.addEventListener('click', async () => {
        const message = document.querySelector('.adding-status-message');
        let ok = 1;
        switch(addEntitySidebarMessage.textContent)
        {
            case ' Add a new student':
                const studentFields = addEntitySidebar.querySelectorAll('.entityPropertyInputTextBox[required]');
                studentFields.forEach(field => {
                    if(field.value === ''){
                        field.style.border = '1px solid red';
                        field.placeholder = "This field is required!";           
                        ok = 0;
                    }
                });

                if(ok === 0){
                    message.textContent = "Please fill in all the required fields!";
                    message.style.color = "red";  
                    return;
                } else {
                    studentFields.forEach(field => {
                        field.style.border = 'none';
                    });
                }
                await AddStudentAsync();
                await DisplayInitialTable();
            break;
            
            case ' Add a new professor':
                const professorFields = addEntitySidebar.querySelectorAll('.entityPropertyInputTextBox[required]');
                professorFields.forEach(field => {
                    if(field.value === ''){
                        field.style.border = '1px solid red';
                        field.placeholder = "This field is required!";           
                        ok = 0;
                    }
                });
    
                if(ok === 0){
                    message.textContent = "Please fill in all the required fields!";
                    message.style.color = "red";  
                    return;
                } else {
                    professorFields.forEach(field => {
                        field.style.border = 'none';
                    });
                }
                await AddProfessorAsync();
                await DisplayInitialTable();
            break;

            case ' Add a new admin':
                const adminFields = addEntitySidebar.querySelectorAll('.entityPropertyInputTextBox[required]');
                adminFields.forEach(field => {
                    if(field.value === ''){
                        field.style.border = '1px solid red';
                        field.placeholder = "This field is required!";           
                        ok = 0;
                    }
                });
    
                if(ok === 0){
                    message.textContent = "Please fill in all the required fields!";
                    message.style.color = "red";  
                    return;
                } else {
                    adminFields.forEach(field => {
                        field.style.border = 'none';
                    });
                }
                await AddAdminAsync();
                await DisplayInitialTable();
            break;

            case ' Add a new university group':
                const groupFields = addEntitySidebar.querySelectorAll('.entityPropertyInputTextBox[required]');
                groupFields.forEach(field => {
                    if(field.value === ''){
                        field.style.border = '1px solid red';
                        field.placeholder = "This field is required!";           
                        ok = 0;
                    }
                });
    
                if(ok === 0){
                    message.textContent = "Please fill in all the required fields!";
                    message.style.color = "red";  
                    return;
                } else {
                    groupFields.forEach(field => {
                        field.style.border = 'none';
                    });
                }
                await AddUniversityGroupAsync();
                await DisplayInitialTable();
            break;

            default:
                return;
        }
    });

    const cancelButton = document.createElement("button");
    cancelButton.classList.add("add-entity-sidebar-cancel-button", "add-entity-sidebar-button");
    cancelButton.textContent = "Cancel";
    cancelButton.addEventListener('click', () => {
        addEntitySidebar.style.right = "-50%";
        const overlay = document.querySelector('.overlay');
        overlay.style.display = 'none';
        pageContent.removeChild(addEntitySidebar);
    });

    addEntitySidebarButtons.appendChild(saveButton);
    addEntitySidebarButtons.appendChild(cancelButton);

    addEntitySidebarContent.appendChild(addEntitySidebarButtons);
    addEntitySidebar.appendChild(addEntitySidebarContent);

    console.log(addEntitySidebar);

    const overlay = document.querySelector('.overlay');
    overlay.style.display = "block";
    addEntitySidebar.style.right = "0";

    pageContent.appendChild(addEntitySidebar);
}

