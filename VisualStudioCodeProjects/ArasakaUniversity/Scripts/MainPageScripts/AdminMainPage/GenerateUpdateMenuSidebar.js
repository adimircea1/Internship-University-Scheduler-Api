import { UpdateAdminById } from "./AdminEntity.js";
import { UpdateProfessorById } from "./ProfessorEntity.js";
import { UpdateStudentById } from "./StudentEntity.js";
import { UpdateUniversityGroupByIdAsync } from "./UniversityGroupEntity.js";

const pageContent = document.querySelector('.page-content');

export function GenerateUpdateEntitySidebar(message, rowsData, Id) {
    const updateEntitySidebar = document.createElement("div");
    updateEntitySidebar.classList.add("update-entity-sidebar");

    const updateEntitySidebarContent = document.createElement("div");
    updateEntitySidebarContent.classList.add("update-entity-sidebar-content");

    const updateEntitySidebarMessage = document.createElement("div");
    updateEntitySidebarMessage.classList.add("update-entity-sidebar-message");

    const messageHeading = document.createElement("h3");
    messageHeading.textContent = message;

    updateEntitySidebarMessage.appendChild(messageHeading);
    updateEntitySidebarContent.appendChild(updateEntitySidebarMessage);

    const itemsDiv = document.createElement('div');
    itemsDiv.classList.add('items-div');

    for (const propertyName in rowsData) {
        const entitySidebarItem = document.createElement("div");
        entitySidebarItem.classList.add("update-entity-sidebar-item")

        const inputTextBoxLabel = document.createElement('label');
        inputTextBoxLabel.textContent = propertyName;

        const inputTextBox = document.createElement('input');
        inputTextBox.type = "text";
        inputTextBox.classList.add("entityPropertyInputTextBox", `${propertyName}`);
        inputTextBox.value = rowsData[propertyName]; 

        entitySidebarItem.appendChild(inputTextBoxLabel);
        entitySidebarItem.appendChild(inputTextBox);
        itemsDiv.appendChild(entitySidebarItem);
    }

    updateEntitySidebarContent.appendChild(itemsDiv);

    const updatingStatusMessageDiv = document.createElement('div');
    updatingStatusMessageDiv.classList.add('updating-status-div');

    const updatingStatusMessage = document.createElement('p');
    updatingStatusMessage.classList.add('updating-status-message');

    updatingStatusMessageDiv.appendChild(updatingStatusMessage);
    updateEntitySidebarContent.appendChild(updatingStatusMessageDiv);

    const updateEntitySidebarButtons = document.createElement("div");
    updateEntitySidebarButtons.classList.add("update-entity-sidebar-buttons");

    const updateButton = document.createElement("button");
    updateButton.classList.add("update-entity-sidebar-update-button", "update-entity-sidebar-button");
    updateButton.textContent = "Update";
    updateButton.addEventListener('click', async () => {
        // Prepare the updated data based on user input
        const updatedData = {};
        for (const propertyName in rowsData) {
            const inputTextBox = updateEntitySidebar.querySelector(`.entityPropertyInputTextBox.${propertyName}`);
            updatedData[propertyName] = inputTextBox.value;
        }
        
        console.log(Id);

        if(message.includes("student")){
            await UpdateStudentById(updatedData, Id);
        } else 
        if(message.includes("professor")){
            await UpdateProfessorById(updatedData, Id);
        } else 
        if(message.includes("admin")){
            await UpdateAdminById(updatedData, Id);
          
        } else
        if(message.includes("group")){
            await UpdateUniversityGroupByIdAsync(updatedData, Id);
        }
        else {
            return;
        }

    });

    const cancelButton = document.createElement("button");
    cancelButton.classList.add("update-entity-sidebar-cancel-button", "update-entity-sidebar-button");
    cancelButton.textContent = "Cancel";
    cancelButton.addEventListener('click', () => {
        const overlay = document.querySelector('.overlay');
        updateEntitySidebar.style.right = "-50%";
        overlay.style.display = 'none';
        pageContent.removeChild(updateEntitySidebar);
    });

    updateEntitySidebarButtons.appendChild(updateButton);
    updateEntitySidebarButtons.appendChild(cancelButton);

    updateEntitySidebarContent.appendChild(updateEntitySidebarButtons);
    updateEntitySidebar.appendChild(updateEntitySidebarContent);

    const overlay = document.querySelector('.overlay');
    overlay.style.display = 'block';
    updateEntitySidebar.style.right = "0";
    pageContent.appendChild(updateEntitySidebar);
}
