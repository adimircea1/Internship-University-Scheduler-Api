import { CreateModal } from "./AddStudentsInGroupModal.js";
import { GetFilteredAndOrderedStudentsFromGroupWithId, RemoveStudentFromGroupAsync } from "./UniversityGroupEntity.js";

let filterActive = false;
let filterByElement = {};

export async function DisplayInitialManageSidebar(){
    const id = localStorage.getItem("GroupId");
    let filterBy = {
        "UniversityGroupId" : id.toString()
    };
    
    const databaseFeedback = await GetFilteredAndOrderedStudentsFromGroupWithId(5, 1, filterBy, "FullName", 1, id);
    GenerateManageEntitySidebar(databaseFeedback, 5, 1);
}

async function DisplayManageSidebar(numberOfEntities, currentPage){
    const id = localStorage.getItem("GroupId");
    let filterBy = {
        "UniversityGroupId" : id.toString()
    };
    
    const databaseFeedback = await GetFilteredAndOrderedStudentsFromGroupWithId(numberOfEntities, currentPage, filterBy, "FullName", 1, id);
    GenerateManageEntitySidebar(databaseFeedback, numberOfEntities, currentPage);
}

async function DisplaySidebarAfterFilter(numberOfEntities, currentPage, filterBy){
    const id = localStorage.getItem("GroupId");
    if(filterBy === ""){
        filterActive = false;
        filterByElement = {};
        await DisplayInitialManageSidebar();
    } else {
        filterActive = true;
        filterByElement = filterBy;
        filterBy["UniversityGroupId"] = id.toString();
        const databaseFeedback = await GetFilteredAndOrderedStudentsFromGroupWithId(numberOfEntities, currentPage, filterBy, "FullName", 1, id);
        GenerateManageEntitySidebar(databaseFeedback, numberOfEntities, currentPage);
    }
}

export async function GenerateManageEntitySidebar(databaseFeedback, numberOfEntitiesToShow, currentPage) {
    const id = localStorage.getItem("GroupId");
    const pageContent = document.querySelector('.page-content');

    const initialSidebar = document.querySelector('.manage-entity-sidebar');
    if(initialSidebar !== null){
        pageContent.removeChild(initialSidebar);
    }

    const manageEntitySidebar = document.createElement("div");
    manageEntitySidebar.classList.add("manage-entity-sidebar");

    const manageEntitySidebarContent = document.createElement("div");
    manageEntitySidebarContent.classList.add("manage-entity-sidebar-content");

    const manageEntitySidebarMessage = document.createElement("div");
    manageEntitySidebarMessage.classList.add("manage-entity-sidebar-message");

    const messageHeading = document.createElement("h3");
    messageHeading.textContent = `University group with id ${id}`;
    manageEntitySidebarMessage.appendChild(messageHeading);
    manageEntitySidebarContent.appendChild(manageEntitySidebarMessage);

    const manageEntitySidebarButtons = document.createElement("div");
    manageEntitySidebarButtons.classList.add("manage-entity-sidebar-buttons");

    const addStudentsButton = document.createElement("button");
    addStudentsButton.classList.add("manage-entity-sidebar-add-button", "manage-entity-sidebar-button");
    addStudentsButton.textContent = "Add students in group";

    addStudentsButton.addEventListener('click', async () => {
        CreateModal();
        const behindModal = document.querySelector('.behind-modal-overlay');
        behindModal.style.display = 'block';
    });

    const cancelButton = document.createElement("button");
    cancelButton.classList.add("manage-entity-sidebar-cancel-button", "manage-entity-sidebar-button");
    cancelButton.textContent = "Cancel";

    cancelButton.addEventListener('click', () => {
        manageEntitySidebar.style.right = "-50%";
        const overlay = document.querySelector('.overlay');
        overlay.style.display = 'none';
        pageContent.removeChild(manageEntitySidebar);
        localStorage.removeItem("GroupId");
    });

    manageEntitySidebarButtons.appendChild(addStudentsButton);
    manageEntitySidebarButtons.appendChild(cancelButton);

    const studentList = document.createElement('div');
    studentList.classList.add("student-list");

    const table = document.createElement("table");
    table.classList.add("student-table");
    const thead = document.createElement("thead");
    const tbody = document.createElement("tbody");
    tbody.classList.add("manage-tbody");

    const headerRow = document.createElement("tr");
    const idHeader = document.createElement("th");
    idHeader.textContent = "Id";
    const nameHeader = document.createElement("th");
    nameHeader.textContent = "Student Name";
    const actionHeader = document.createElement("th");
    actionHeader.textContent = "Action";

    headerRow.appendChild(idHeader);
    headerRow.appendChild(nameHeader);
    headerRow.appendChild(actionHeader);

    const searchRow = document.createElement("tr");
    const searchCell = document.createElement("td");
    searchCell.colSpan = 3;
    searchCell.classList.add('manage-group-search-row');
    const searchInput = document.createElement("input");
    searchInput.type = "text";
    searchInput.placeholder = "Search by Full Name";
    searchInput.classList.add('manage-group-fullname-filter');
    
    searchInput.addEventListener("keypress", async (event) => {
        if (event.key === "Enter") {

            if(searchInput.value === ''){
                filterActive = false;
                filterBy = "";
                await DisplaySidebarAfterFilter(numberOfEntitiesToShow, 1, "");
            } else {
                const filterBy = {
                    "FullName" : searchInput.value
                };
                await DisplaySidebarAfterFilter(numberOfEntitiesToShow, 1, filterBy);
            }

          
        }
    });
    searchCell.appendChild(searchInput);
    searchRow.appendChild(searchCell);

    thead.appendChild(headerRow);
    thead.appendChild(searchRow);

    table.appendChild(thead);

    for(const student of databaseFeedback.entities){
        const row = document.createElement("tr");

        const idCell = document.createElement("td");
        idCell.textContent = student.id;
        row.appendChild(idCell);

        const nameCell = document.createElement("td");
        nameCell.textContent = student.fullName;
        row.appendChild(nameCell);

        const actionCell = document.createElement("td");
        const removeIcon = document.createElement("i");
        removeIcon.classList.add("fas", "fa-user-minus", "remove-icon");

        removeIcon.addEventListener('click', async () => {
            await RemoveStudentFromGroupAsync(student.id, id);
        });

        actionCell.appendChild(removeIcon);
        row.appendChild(actionCell);

        tbody.appendChild(row);
    };

    table.appendChild(tbody);
    studentList.appendChild(table);

    manageEntitySidebarContent.appendChild(studentList);
    
    const maxPages = databaseFeedback.numberOfEntities / numberOfEntitiesToShow > 1 ? databaseFeedback.numberOfEntities / numberOfEntitiesToShow : 1;
    const paginationDiv = GenerateTablePagination(numberOfEntitiesToShow, Math.ceil(maxPages), currentPage);
    paginationDiv.classList.add("manage-group-pagination-div");
    manageEntitySidebarContent.appendChild(paginationDiv);
    manageEntitySidebarContent.appendChild(manageEntitySidebarButtons);
    manageEntitySidebar.appendChild(manageEntitySidebarContent);

    const overlay = document.querySelector('.overlay');
    overlay.style.display = "block";
    manageEntitySidebar.style.right = "0";

    pageContent.appendChild(manageEntitySidebar);
}


 function GenerateTablePagination(numberOfEntitiesToShow, maxPages, currentPage) {
    if(currentPage > maxPages)
    {
        currentPage = maxPages;
    }

    const paginationDiv = document.createElement('div');
    paginationDiv.classList.add('pagination-div');
    
    // Create previous page arrow
    const prevPageArrow = document.createElement("i");
    prevPageArrow.classList.add("fas", "fa-arrow-left", "pagination-arrow");
    prevPageArrow.addEventListener('click', async () => {
      
        if (currentPage > 1) {
            const manageEntitySidebarContent = document.querySelector('.manage-entity-sidebar-content');
            const numberOfEntitiesSelectMenu = document.querySelector('.entities-per-page-select'); 
            const studentList = document.querySelector('.student-list');
            currentPage--;
            manageEntitySidebarContent.removeChild(paginationDiv);
            studentList.innerHTML = '';
            await DisplayManageSidebar(numberOfEntitiesSelectMenu.value, currentPage);
        }
    });

    // Create next page arrow
    const nextPageArrow = document.createElement("i");
    nextPageArrow.classList.add("fas", "fa-arrow-right", "pagination-arrow");
    nextPageArrow.addEventListener('click', async () => {
     
        if (currentPage < maxPages) {
            const studentList = document.querySelector('.student-list');
            const numberOfEntitiesSelectMenu = document.querySelector('.entities-per-page-select'); 
            currentPage++;
            studentList.innerHTML = '';
            await DisplayManageSidebar(numberOfEntitiesSelectMenu.value, currentPage);
        }
    });

    const entitiesPerPageSelect = document.createElement("select");
    entitiesPerPageSelect.classList.add("entities-per-page-select");
    
    const entitiesPerPageOptions = [5, 10];
    entitiesPerPageOptions.forEach(option => {
        const optionElement = document.createElement("option");
        optionElement.value = option;
        optionElement.textContent = option;
        entitiesPerPageSelect.appendChild(optionElement);
    });

    entitiesPerPageSelect.value = numberOfEntitiesToShow;

    entitiesPerPageSelect.addEventListener('change', async () => {
        numberOfEntitiesToShow = entitiesPerPageSelect.value;
        const studentList = document.querySelector('.student-list');
        studentList.innerHTML = '';

        if(filterActive){
            await DisplaySidebarAfterFilter(numberOfEntitiesToShow, 1, filterByElement);
        } else {
            await DisplayManageSidebar(numberOfEntitiesToShow, 1);
        }
    });

    const pageIndicator = document.createElement("span");
    pageIndicator.textContent = `Page: ${currentPage} / ${maxPages}`;
    pageIndicator.classList.add("page-indicator");
    pageIndicator.setAttribute("data-current-page", currentPage);
    pageIndicator.setAttribute("data-max-pages", maxPages);

    paginationDiv.appendChild(prevPageArrow);
    paginationDiv.appendChild(entitiesPerPageSelect);
    paginationDiv.appendChild(pageIndicator);
    paginationDiv.appendChild(nextPageArrow);

    return paginationDiv;
}