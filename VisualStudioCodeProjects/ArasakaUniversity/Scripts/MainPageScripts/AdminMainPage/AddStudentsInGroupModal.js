import { FetchFilteredAndOrderedStudentDataAsync } from "../AdminMainPage/StudentEntity.js"
import { AddStudentsInGroupAsync } from "./UniversityGroupEntity.js";

let filterActive = false;
let globalFilter = {};

export async function CreateModal() {
    const pageContent = document.querySelector('.page-content');
    const initialModalContainer = document.querySelector('.middle-modal-container');
    if (initialModalContainer !== null) {
        pageContent.removeChild(initialModalContainer);
    }

    const modalContainer = document.createElement("div");
    modalContainer.classList.add("middle-modal-container");

    const messageContainer = document.createElement("div");
    messageContainer.classList.add("modal-message-container");
    messageContainer.textContent = "Available students to add";

    const buttonContainer = document.createElement("div");
    buttonContainer.classList.add("modal-button-container");

    const addButton = document.createElement("button");
    addButton.addEventListener('click', async () => {
        const checkedBoxes = modalContainer.querySelectorAll(`input[type="checkbox"]:checked`);
        const studentIds = [];

        for(const checkBox of checkedBoxes){
            studentIds.push(checkBox.value);
            checkBox.checked = false;
        }

        await AddStudentsInGroupAsync(studentIds, localStorage.getItem('GroupId'));
    });

    addButton.classList.add("modal-button", "add-button");
    addButton.textContent = "Add";

    const cancelButton = document.createElement("button");
    cancelButton.classList.add("modal-button", "cancel-button");

    cancelButton.addEventListener('click', () => {
        const pageContent = document.querySelector('.page-content');
        const initialModalContainer = document.querySelector('.middle-modal-container');
        pageContent.removeChild(initialModalContainer);
        const behindModal = document.querySelector('.behind-modal-overlay');
        behindModal.style.display = 'none';
    });

    cancelButton.classList.add("modal-button", "cancel-button");
    cancelButton.textContent = "Cancel";

    buttonContainer.appendChild(addButton);
    buttonContainer.appendChild(cancelButton);

    modalContainer.appendChild(messageContainer);

    await DisplayInitialModalTable(modalContainer);

    modalContainer.appendChild(buttonContainer);

    pageContent.appendChild(modalContainer);
}

export async function DisplayInitialModalTable(modalContainer){
    let filterBy = {
        "UniversityGroupId" : "null"
    };
    
    const databaseFeedback = await FetchFilteredAndOrderedStudentDataAsync(5, 1, filterBy, "FullName", 1);
    GenerateModalStudentTable(modalContainer, databaseFeedback, 5, 1);
}

async function DisplayModalTable(modalContainer, numberOfEntities, currentPage){
    let filterBy = {
        "UniversityGroupId" : "null"
    };
    
    const databaseFeedback = await FetchFilteredAndOrderedStudentDataAsync(numberOfEntities, currentPage, filterBy, "FullName", 1);
    GenerateModalStudentTable(modalContainer, databaseFeedback, numberOfEntities, currentPage);
}

async function DisplayFilteredModalTable(modalContainer, numberOfEntities, currentPage, filterBy){
    filterBy["UniversityGroupId"] = "null";

    const databaseFeedback = await FetchFilteredAndOrderedStudentDataAsync(numberOfEntities, currentPage, filterBy, "FullName", 1);
    GenerateModalStudentTable(modalContainer, databaseFeedback, numberOfEntities, currentPage);
}

function GenerateModalStudentTable(modalContainer, databaseFeedback, numberOfEntitiesToShow, currentPage){
    const initialStudentList = modalContainer.querySelector('.middle-modal-student-list-container');
    const initialPaginationDiv =  modalContainer.querySelector('.middle-modal-group-pagination-div');
    if(initialStudentList !== null){
        modalContainer.removeChild(initialStudentList);
        modalContainer.removeChild(initialPaginationDiv);
    }

    const studentList = document.createElement('div');
    studentList.classList.add("middle-modal-student-list-container");

    const table = document.createElement("table");
    table.classList.add("middle-modal-student-table");
    const thead = document.createElement("thead");
    thead.classList.add("middle-modal-manage-thead");
    const tbody = document.createElement("tbody");
    tbody.classList.add("middle-modal-manage-tbody");

    const headerRow = document.createElement("tr");
    const idHeader = document.createElement("th");
    idHeader.textContent = "Id";
    const nameHeader = document.createElement("th");
    nameHeader.textContent = "Student Name";
    const yearHeader = document.createElement("th");
    yearHeader.textContent = "Study Year";
    const actionHeader = document.createElement("th");
    actionHeader.textContent = "Actions";
    
    headerRow.appendChild(idHeader);
    headerRow.appendChild(nameHeader);
    headerRow.appendChild(yearHeader);
    headerRow.appendChild(actionHeader);

    const searchRow = document.createElement("tr");

    const emptySearchCell = document.createElement("td");
    emptySearchCell.classList.add('middle-modal-manage-group-search-row');

    const emptySearchCellInput = document.createElement("input");
    emptySearchCellInput.type = "text";
    emptySearchCellInput.placeholder = "";
    emptySearchCellInput.classList.add('middle-modal-manage-group-none', 'middle-modal-manage-group-filter');
    
    emptySearchCell.appendChild(emptySearchCellInput);

    emptySearchCell.style.opacity = "0";
    emptySearchCell.style.pointerEvents = "none";
    emptySearchCellInput.readOnly = true;


    searchRow.appendChild(emptySearchCell);

    const searchCell = document.createElement("td");
    searchCell.colSpan;
    searchCell.classList.add('middle-modal-manage-group-search-row');

    const searchInput = document.createElement("input");
    searchInput.type = "text";
    searchInput.placeholder = "Search by Student Name";
    searchInput.classList.add('middle-modal-manage-group-fullname-filter', 'middle-modal-manage-group-filter');
    searchInput.addEventListener("keydown", async (event) => {
        
        if(event.key === "Enter"){
            filterActive = true;

            const entitiesPerPageSelect = document.querySelector('.entities-per-page-select');
            const modalContainer = document.querySelector('.middle-modal-container');
            const yearFilter = document.querySelector('.middle-modal-manage-group-year-filter');
            let filterBy;
        
            if(yearFilter.value === '' && searchInput.value === ''){
                filterActive = false;
                globalFilter = {};
                await DisplayModalTable(modalContainer, entitiesPerPageSelect.value, currentPage);
                return;
            }
        
            if(yearFilter.value === ''){
                filterBy = {
                    Fullname : searchInput.value,
                };
            } else {
                filterBy = {
                    Fullname : searchInput.value,
                    StudyYear : yearFilter.value
                };
            }
            
            globalFilter = filterBy;

            await DisplayFilteredModalTable(modalContainer, entitiesPerPageSelect.value, 1, filterBy);
        }


       
    });

    searchCell.appendChild(searchInput);
    searchRow.appendChild(searchCell);

    const searchCellYear = document.createElement("td");
    searchCellYear.classList.add('middle-modal-manage-group-search-row')

    const searchInputYear = document.createElement("input");
    searchInputYear.type = "text";
    searchInputYear.placeholder = "Search by Study Year";
    searchInputYear.classList.add('middle-modal-manage-group-year-filter', 'middle-modal-manage-group-filter');

    searchInputYear.addEventListener("keydown", async (event) => {
        
        if(event.key === "Enter"){
            filterActive = true;

            const entitiesPerPageSelect = document.querySelector('.entities-per-page-select');
            const modalContainer = document.querySelector('.middle-modal-container');
            const yearFilter = document.querySelector('.middle-modal-manage-group-year-filter');
            let filterBy;

            if(yearFilter.value === '' && searchInput.value === ''){
                filterActive = false;
                await DisplayModalTable(modalContainer, entitiesPerPageSelect.value, currentPage);
                globalFilter = {};
                return;
            }

            if(yearFilter.value === ''){
                filterBy = {
                    StudyYear : yearFilter.value
                };
            } else {
                filterBy = {
                    Fullname : searchInput.value,
                    StudyYear : yearFilter.value
                };
            }

            globalFilter = filterBy;
    
            await DisplayFilteredModalTable(modalContainer, entitiesPerPageSelect.value, 1, filterBy);
        }
    });

    searchCellYear.appendChild(searchInputYear);
    searchRow.appendChild(searchCellYear);

    thead.appendChild(headerRow);
    thead.appendChild(searchRow);

    table.appendChild(thead);

    for (const student of databaseFeedback.entities) {
        const row = document.createElement("tr");
    
        const idCell = document.createElement("td");
        idCell.textContent = student.id;
        row.appendChild(idCell);
    
        const nameCell = document.createElement("td");
        nameCell.textContent = student.fullName;
        row.appendChild(nameCell);
    
        const yearCell = document.createElement("td");
        yearCell.textContent = student.studyYear;
        row.appendChild(yearCell);
    
        const actionCell = document.createElement("td");
        const checkBox = document.createElement("input");
        checkBox.type = "checkbox";
        checkBox.classList.add("student-checkbox");
        checkBox.value = student.id;
    
        actionCell.appendChild(checkBox);
        row.appendChild(actionCell);
    
        tbody.appendChild(row);
    }
    

    table.appendChild(tbody);
    studentList.appendChild(table);
    
    modalContainer.appendChild(studentList);

    const maxPages = databaseFeedback.numberOfEntities / numberOfEntitiesToShow > 1 ? databaseFeedback.numberOfEntities / numberOfEntitiesToShow : 1;
    const paginationDiv = GenerateTablePagination(numberOfEntitiesToShow, Math.ceil(maxPages), currentPage);
    paginationDiv.classList.add("middle-modal-group-pagination-div");
    modalContainer.appendChild(paginationDiv);
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
            currentPage--;
            const modalContainer = document.querySelector('.middle-modal-container');
            await DisplayModalTable(modalContainer, numberOfEntitiesToShow, currentPage);
        }
    });

    // Create next page arrow
    const nextPageArrow = document.createElement("i");
    nextPageArrow.classList.add("fas", "fa-arrow-right", "pagination-arrow");
    nextPageArrow.addEventListener('click', async () => {
        if (currentPage < maxPages) {   
            currentPage++;
            const modalContainer = document.querySelector('.middle-modal-container');
            await DisplayModalTable(modalContainer, numberOfEntitiesToShow, currentPage);
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
        const modalContainer = document.querySelector('.middle-modal-container');
        const pageIndicator = document.querySelector('.page-indicator');
        const currentPage = pageIndicator.getAttribute('data-current-page');
        filterActive ? await DisplayFilteredModalTable(modalContainer, numberOfEntitiesToShow, currentPage, globalFilter) : 
        await DisplayModalTable(modalContainer, numberOfEntitiesToShow, currentPage);
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