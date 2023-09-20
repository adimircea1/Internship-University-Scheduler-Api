import { FetchStudentDataAsync } from "./StudentEntity.js";
import { FetchFilteredStudentDataAsync } from "./StudentEntity.js";
import { DeleteStudentByIdAsync } from "./StudentEntity.js";
import { GenerateUpdateEntitySidebar } from "./GenerateUpdateMenuSidebar.js";
import { FetchFilteredAndOrderedStudentDataAsync } from "./StudentEntity.js";
import { DeleteProfessorByIdAsync } from "./ProfessorEntity.js";
import { FetchFilteredAndOrderedProfessorDataAsync } from "./ProfessorEntity.js";
import { FetchFilteredProfessorDataAsync } from "./ProfessorEntity.js";
import { FetchProfessorDataAsync } from "./ProfessorEntity.js";
import { DeleteAdminByIdAsync, FetchFilteredAdminDataAsync, FetchFilteredAndOrderedAdminDataAsync } from "./AdminEntity.js";
import { DeleteUniversityGroupAsync, FetchUniversityGroupDataAsync } from "./UniversityGroupEntity.js";
import { DisplayInitialManageSidebar, GenerateManageEntitySidebar } from "./ManageGroupMenu.js";

let descending = false;
let orderCriterion = "";
let filterActive = false;
let filterByElements = {};
let tableHeaders = [];
let tableData = [];

export async function DisplayInitialTable(){
    let addButtonText = document.querySelector('.add-button-text');
    const activeOptionElement = document.querySelector('.option-active');
    switch(activeOptionElement.textContent){
        case 'Students':
            tableHeaders = ["Id", "StudyYear", "FullName", "Email", "PhoneNumber", "BirthdayDate", "UniversityGroupId", "PersonalEmail"];
            tableData = ["id", "studyYear", "fullName", "email", "phoneNumber", "birthdayDate", "universityGroupId", "personalEmail"];
            //MaxEntities, CurrentPageNumber
            const databaseFeedbackForStudents = await FetchStudentDataAsync(5, 1, "", 1);
            addButtonText.textContent = " Add a new student";
            await GenerateEntityTable(tableHeaders, tableData, databaseFeedbackForStudents.entities, activeOptionElement.textContent);
            let maxPages = databaseFeedbackForStudents.numberOfEntities / 5 > 1 ? databaseFeedbackForStudents.numberOfEntities / 5 : 1;
            await GenerateTablePagination(5, Math.ceil(maxPages), 1);

        break;
            
        case 'Professors':
            tableHeaders = ["Id", "FirstName", "LastName", "Email", "PhoneNumber", "Speciality", "BirthdayDate"];
            tableData = ["id", "firstName", "lastName", "email", "phoneNumber", "speciality", "birthdayDate"];
            //MaxEntities, CurrentPageNumber
            const databaseFeedbackForProfessors = await FetchProfessorDataAsync(5, 1, "", 1);
            addButtonText.textContent = " Add a new professor";
            await GenerateEntityTable(tableHeaders, tableData, databaseFeedbackForProfessors.entities, activeOptionElement.textContent);
            let maxPages1 = databaseFeedbackForProfessors.numberOfEntities / 5 > 1 ? databaseFeedbackForProfessors.numberOfEntities / 5 : 1;
            await GenerateTablePagination(5, Math.ceil(maxPages1), 1);
        
        break;

        case 'Admins':
            tableHeaders = ["Id", "Username", "Email", "VerifiedEmail"];
            tableData = ["id", "userName", "email", "verifiedEmail"];
            //MaxEntities, CurrentPageNumber
            let filterBy = {
                "Role" : "Admin"
            };

            const databaseFeedbackForAdmins = await FetchFilteredAdminDataAsync(5, 1, filterBy);
            addButtonText.textContent = " Add a new admin";
            await GenerateEntityTable(tableHeaders, tableData, databaseFeedbackForAdmins.entities, activeOptionElement.textContent);
            let maxPages2 = databaseFeedbackForAdmins.numberOfEntities / 5 > 1 ? databaseFeedbackForAdmins.numberOfEntities / 5 : 1;
            await GenerateTablePagination(5, Math.ceil(maxPages2), 1);
        
        break;

        case 'Groups':
            tableHeaders = ["Id", "NumberOfMembers", "MaxSize", "Name", "Specialization"];
            tableData = ["id", "numberOfMembers", "maxSize", "name", "specialization"];
            //MaxEntities, CurrentPageNumber
            const databaseFeedbackForGroups = await FetchUniversityGroupDataAsync(5, 1, "", 1);
            addButtonText.textContent = " Add a new university group";
            await GenerateEntityTable(tableHeaders, tableData, databaseFeedbackForGroups.entities, activeOptionElement.textContent);
            let maxPages3 = databaseFeedbackForGroups.numberOfEntities / 5 > 1 ? databaseFeedbackForGroups.numberOfEntities / 5 : 1;
            await GenerateTablePagination(5, Math.ceil(maxPages3), 1);

        default:
            break;
    }
}

export async function DisplayOrderedTable(numberOfEntitiesToShow, currentPage, orderBy, orderDirection){
    let addButtonText = document.querySelector('.add-button-text');
    const activeOptionElement = document.querySelector('.option-active');
    switch(activeOptionElement.textContent){
        case 'Students':
            tableHeaders = ["Id", "StudyYear", "FullName", "Email", "PhoneNumber", "BirthdayDate", "UniversityGroupId", "PersonalEmail"];
            tableData = ["id", "studyYear", "fullName", "email", "phoneNumber", "birthdayDate", "universityGroupId", "personalEmail"];
            //MaxEntities, CurrentPageNumber
            const databaseFeedbackForStudents = await FetchStudentDataAsync(numberOfEntitiesToShow, currentPage, orderBy, orderDirection);
            addButtonText.textContent = " Add a new student";
            await GenerateEntityTable(tableHeaders, tableData, databaseFeedbackForStudents.entities, activeOptionElement.textContent);
            let maxPages = databaseFeedbackForStudents.numberOfEntities / numberOfEntitiesToShow > 1 ? databaseFeedbackForStudents.numberOfEntities / numberOfEntitiesToShow : 1;
            await GenerateTablePagination(numberOfEntitiesToShow, Math.ceil(maxPages), currentPage);
        break;
            
        case 'Professors':
            tableHeaders = ["Id", "FirstName", "LastName", "Email", "PhoneNumber", "Speciality", "BirthdayDate"];
            tableData = ["id", "firstName", "lastName", "email", "phoneNumber", "speciality", "birthdayDate"];
            //MaxEntities, CurrentPageNumber
            const databaseFeedbackForProfessors = await FetchProfessorDataAsync(numberOfEntitiesToShow, currentPage, orderBy, orderDirection);
            addButtonText.textContent = " Add a new professor";
            await GenerateEntityTable(tableHeaders, tableData, databaseFeedbackForProfessors.entities, activeOptionElement.textContent);
            let maxPages1 = databaseFeedbackForProfessors.numberOfEntities / numberOfEntitiesToShow > 1 ? databaseFeedbackForProfessors.numberOfEntities / numberOfEntitiesToShow : 1;
            GenerateTablePagination(numberOfEntitiesToShow, Math.ceil(maxPages1), currentPage);
        
        break;

        case 'Admins':
            tableHeaders = ["Id", "Username", "Email", "VerifiedEmail"];
            tableData = ["id", "userName", "email", "verifiedEmail"];
            //MaxEntities, CurrentPageNumber
            let filterBy = {
                "Role" : "Admin"
            };

            const databaseFeedbackForAdmins = await FetchFilteredAndOrderedAdminDataAsync(numberOfEntitiesToShow, currentPage, filterBy, orderBy, orderDirection);
            addButtonText.textContent = " Add a new admin";
            await GenerateEntityTable(tableHeaders, tableData, databaseFeedbackForAdmins.entities, activeOptionElement.textContent);
            let maxPages2 = databaseFeedbackForAdmins.numberOfEntities / numberOfEntitiesToShow > 1 ? databaseFeedbackForAdmins.numberOfEntities / numberOfEntitiesToShow : 1;
            await GenerateTablePagination(numberOfEntitiesToShow, Math.ceil(maxPages2), currentPage);
        
        break;

        default:
            break;
    }
}

export async function DisplayFilteredTable(numberOfEntitiesToShow, currentPage, filterBy){
    let addButtonText = document.querySelector('.add-button-text');
    const activeOptionElement = document.querySelector('.option-active');
    switch(activeOptionElement.textContent){
        case 'Students':
            tableHeaders = ["Id", "StudyYear", "FullName", "Email", "PhoneNumber", "BirthdayDate", "UniversityGroupId", "PersonalEmail"];
            tableData = ["id", "studyYear", "fullName", "email", "phoneNumber", "birthdayDate", "universityGroupId", "personalEmail"];
            //MaxEntities, CurrentPageNumber
            const databaseFeedbackForStudents = await FetchFilteredStudentDataAsync(numberOfEntitiesToShow, currentPage, filterBy);
            addButtonText.textContent = " Add a new student";
            await GenerateEntityTable(tableHeaders, tableData, databaseFeedbackForStudents.entities, activeOptionElement.textContent);
            let maxPages = databaseFeedbackForStudents.numberOfEntities / numberOfEntitiesToShow > 1 ? databaseFeedbackForStudents.numberOfEntities / numberOfEntitiesToShow : 1;
            await GenerateTablePagination(numberOfEntitiesToShow, Math.ceil(maxPages), currentPage);
        break;
            
        case 'Professors':
            tableHeaders = ["Id", "FirstName", "LastName", "Email", "PhoneNumber", "Speciality", "BirthdayDate"];
            tableData = ["id", "firstName", "lastName", "email", "phoneNumber", "speciality", "birthdayDate"];
            //MaxEntities, CurrentPageNumber
            const databaseFeedbackForProfessors = await FetchFilteredProfessorDataAsync(numberOfEntitiesToShow, currentPage, filterBy);
            addButtonText.textContent = " Add a new professor";
            await GenerateEntityTable(tableHeaders, tableData, databaseFeedbackForProfessors.entities, activeOptionElement.textContent);
            let maxPages1 = databaseFeedbackForProfessors.numberOfEntities / numberOfEntitiesToShow > 1 ? databaseFeedbackForProfessors.numberOfEntities / numberOfEntitiesToShow : 1;
            await GenerateTablePagination(numberOfEntitiesToShow, Math.ceil(maxPages1), currentPage);

        break;

        case 'Admins':
            tableHeaders = ["Id", "Username", "Email", "VerifiedEmail"];
            tableData = ["id", "userName", "email", "verifiedEmail"];
            //MaxEntities, CurrentPageNumber
            filterBy["Role"] = "Admin";
            const databaseFeedbackForAdmins = await FetchFilteredAdminDataAsync(numberOfEntitiesToShow, currentPage, filterBy);
            addButtonText.textContent = " Add a new admin";
            await GenerateEntityTable(tableHeaders, tableData, databaseFeedbackForAdmins.entities, activeOptionElement.textContent);
            let maxPages2 = databaseFeedbackForAdmins.numberOfEntities / numberOfEntitiesToShow > 1 ? databaseFeedbackForAdmins.numberOfEntities / numberOfEntitiesToShow : 1;
            await GenerateTablePagination(numberOfEntitiesToShow, Math.ceil(maxPages2), currentPage);
        
        break;

        default:
            break;
    }
}

export async function DisplayFilteredAndOrderedTable(numberOfEntitiesToShow, currentPage, filterBy, orderBy, orderDirection){
    let addButtonText = document.querySelector('.add-button-text');
    const activeOptionElement = document.querySelector('.option-active');
    switch(activeOptionElement.textContent){
        case 'Students':
            tableHeaders = ["Id", "StudyYear", "FullName", "Email", "PhoneNumber", "BirthdayDate", "UniversityGroupId", "PersonalEmail"];
            tableData = ["id", "studyYear", "fullName", "email", "phoneNumber", "birthdayDate", "universityGroupId", "personalEmail"];
            //MaxEntities, CurrentPageNumber
            const databaseFeedbackForStudents = await FetchFilteredAndOrderedStudentDataAsync(numberOfEntitiesToShow, currentPage, filterBy, orderBy, orderDirection);
            addButtonText.textContent = " Add a new student";
            await GenerateEntityTable(tableHeaders, tableData, databaseFeedbackForStudents.entities, activeOptionElement.textContent);
            let maxPages = databaseFeedbackForStudents.numberOfEntities / numberOfEntitiesToShow > 1 ? databaseFeedbackForStudents.numberOfEntities / numberOfEntitiesToShow : 1;
            await GenerateTablePagination(numberOfEntitiesToShow, Math.ceil(maxPages), currentPage);
        break;
            
        case 'Professors':
            tableHeaders = ["Id", "FirstName", "LastName", "Email", "PhoneNumber", "Speciality", "BirthdayDate"];
            tableData = ["id", "firstName", "lastName", "email", "phoneNumber", "speciality", "birthdayDate"];
            //MaxEntities, CurrentPageNumber
            const databaseFeedbackForProfessors = await FetchFilteredAndOrderedProfessorDataAsync(numberOfEntitiesToShow, currentPage, filterBy, orderBy, orderDirection);
            addButtonText.textContent = " Add a new professor";
            await GenerateEntityTable(tableHeaders, tableData, databaseFeedbackForProfessors.entities, activeOptionElement.textContent);
            let maxPages1 = databaseFeedbackForProfessors.numberOfEntities / numberOfEntitiesToShow > 1 ? databaseFeedbackForProfessors.numberOfEntities / numberOfEntitiesToShow : 1;
            GenerateTablePagination(numberOfEntitiesToShow, Math.ceil(maxPages1), currentPage);
        break;

        case 'Admins':
            tableHeaders = ["Id", "Username", "Email", "VerifiedEmail"];
            tableData = ["id", "userName", "email", "verifiedEmail"];
            //MaxEntities, CurrentPageNumber
            filterBy["Role"] = "Admin";
            const databaseFeedbackForAdmins = await FetchFilteredAndOrderedAdminDataAsync(numberOfEntitiesToShow, currentPage, filterBy, orderBy, orderDirection);
            addButtonText.textContent = " Add a new admin";
            await GenerateEntityTable(tableHeaders, tableData, databaseFeedbackForAdmins.entities, activeOptionElement.textContent);
            let maxPages2 = databaseFeedbackForAdmins.numberOfEntities / numberOfEntitiesToShow > 1 ? databaseFeedbackForAdmins.numberOfEntities / numberOfEntitiesToShow : 1;
            await GenerateTablePagination(numberOfEntitiesToShow, Math.ceil(maxPages2), currentPage);
        break;

        default:
            break;
    }
}


async function GenerateEntityTable(tableHeaders, tableData, entities, entitiesType) {
    let tableContent = document.querySelector('.table-content');
    tableContent.innerHTML = '';

    const innerTableDiv = document.createElement('div');
    innerTableDiv.classList.add('inner-table-div');

    const table = document.createElement("table");
    table.classList.add('entity-table');

    const tableHead = document.createElement("thead");
    tableHead.classList.add('entity-table-head');

    const tableHeadRow = document.createElement("tr");
    tableHeadRow.classList.add('entity-table-head-row');

    const searchRow = document.createElement("tr");
    searchRow.classList.add('search-row');
    tableHeaders.forEach(entity => {
        const searchCell = document.createElement('td');
        const searchBox = document.createElement('input');
        searchBox.classList.add(`${entity}-search-box`, 'search-box');
        searchBox.setAttribute('data-filter-by', entity);
        searchBox.type = 'text';
        searchBox.placeholder = 'Search';
        searchCell.appendChild(searchBox);
        searchRow.appendChild(searchCell);
    });

    const iconsRow = document.createElement("tr");
    iconsRow.classList.add('icons-row');

    tableHeaders.forEach(element => {
        const tableItem = document.createElement("th");
        tableItem.textContent = element; 
        tableItem.classList.add(`entity-table-head-row-item-${element}`);
        tableHeadRow.appendChild(tableItem); 

        const orderIconsCell = document.createElement('td');
        const orderDescIcon = document.createElement('i');
        orderDescIcon.classList.add('fas', 'fa-sort-amount-down', 'order-icon', 'order-desc');
        orderDescIcon.setAttribute('data-order-desc-by', element);
        orderDescIcon.addEventListener('click', async () => {
           const pageIndicator = document.querySelector('.page-indicator');
           const numberOfEntitiesSelectMenu = document.querySelector('.entities-per-page-select');

           const currentPage = parseInt(pageIndicator.getAttribute("data-current-page"));
           const orderBy = orderDescIcon.getAttribute('data-order-desc-by');

           descending = true;

           orderCriterion = orderBy;

           //2 = descendent
           if(filterActive === true){
                await DisplayFilteredAndOrderedTable(numberOfEntitiesSelectMenu.value, currentPage, filterByElements, orderBy, 2);
           } else {
                await DisplayOrderedTable(numberOfEntitiesSelectMenu.value, currentPage, orderBy, 2);
           }
        });

        const orderAscIcon = document.createElement('i');
        orderAscIcon.classList.add('fas', 'fa-sort-amount-up', 'order-icon', 'order-asc');
        orderAscIcon.setAttribute('data-order-asc-by', element);
        orderAscIcon.addEventListener('click', async () => {
            const pageIndicator = document.querySelector('.page-indicator');
            const numberOfEntitiesSelectMenu = document.querySelector('.entities-per-page-select');
 
            const currentPage = parseInt(pageIndicator.getAttribute("data-current-page"));
            const orderBy = orderAscIcon.getAttribute('data-order-asc-by');
 
            descending = false;

            orderCriterion = orderBy;

            if(filterActive === true){
                await DisplayFilteredAndOrderedTable(numberOfEntitiesSelectMenu.value, currentPage, filterByElements, orderBy, 1);
           } else {
                await DisplayOrderedTable(numberOfEntitiesSelectMenu.value, currentPage, orderBy, 1);
           }
        });

        orderIconsCell.appendChild(orderDescIcon);
        orderIconsCell.appendChild(orderAscIcon);
        iconsRow.appendChild(orderIconsCell);
    });

    const iconRowEmptyHeader = document.createElement('td');
    iconsRow.appendChild(iconRowEmptyHeader);

    
    // Add the "Actions" header
    const actionsHeader = document.createElement("th");
    actionsHeader.textContent = "Actions";
    tableHeadRow.appendChild(actionsHeader);
    
    tableHead.appendChild(tableHeadRow);
    tableHead.appendChild(iconsRow);

    const filterIcon = document.createElement('i');
    filterIcon.classList.add('fas', 'fa-filter', 'filter-icon');
    filterIcon.addEventListener('click', async () =>{
        const searchBoxes = searchRow.querySelectorAll('.search-box');
        const nonEmptySearchBoxes = Array.from(searchBoxes).filter(searchBox => searchBox.value.trim() !== '');

        const numberOfEntitiesSelect = document.querySelector('.entities-per-page-select');
        const pageIndicator = document.querySelector('.page-indicator');

        const currentPage = parseInt(pageIndicator.getAttribute("data-current-page"));

        if(nonEmptySearchBoxes.length === 0){
            descending === true ? await DisplayOrderedTable(numberOfEntitiesSelect.value, currentPage, orderCriterion, 2) : await DisplayOrderedTable(numberOfEntitiesSelect.value, currentPage, orderCriterion, 1); 
            filterByElements = {};
            filterActive = false;
        } else {
            filterActive = true;

            for (const element of nonEmptySearchBoxes) {
                const filterByAttribute = element.getAttribute("data-filter-by");
                const filterValue = element.value;
                filterByElements[filterByAttribute] = filterValue;
            }

            await DisplayFilteredTable(numberOfEntitiesSelect.value, 1, filterByElements);
        }
    });
    searchRow.appendChild(filterIcon);
    tableHead.appendChild(searchRow);

    table.appendChild(tableHead); 
    

    const tableBody = document.createElement('tbody');
    tableBody.classList.add('entity-table-body');

    entities.forEach(entity => {
        const row = document.createElement('tr');
        row.classList.add('body-row-element');

        tableData.forEach(key => {
            const cell = document.createElement('td');
            cell.classList.add(`entity-cell-${key}`, 'row-body-entity-cell');
            if(entity[key] === null)
            {
                cell.textContent = "Null";
            } else {
                cell.textContent = entity[key];
            }
            row.appendChild(cell);
        });
        
        // Create actions cell with delete and update icons
        const actionsCell = document.createElement('td');
        const deleteIcon = document.createElement('i');
        deleteIcon.classList.add('fas', 'fa-trash-alt', 'action-icon', 'delete-action');
        deleteIcon.addEventListener('click', async () => {
            const row = deleteIcon.closest('tr');
            if (row) {
                row.remove();
                switch(entitiesType){
                    case 'Students':
                        await DeleteStudentByIdAsync(entity.id);
                        break;
                    case 'Professors':
                        await DeleteProfessorByIdAsync(entity.id);
                        break;
                    case 'Admins':
                        await DeleteAdminByIdAsync(entity.id);
                        break;
                    case 'Groups':
                        await DeleteUniversityGroupAsync(entity.id);
                        break;
                    default:
                        break;
                }
            }
        });
            
        const updateIcon = document.createElement('i');
        updateIcon.classList.add('fas', 'fa-edit', 'action-icon', 'update-action');
        updateIcon.addEventListener('click', () => {
            let message;
            let rowsData;
            switch(entitiesType){
                case 'Students':
                    message = `Update student with id ${entity.id}`;
                    rowsData = {
                        "StudyYear" : entity.studyYear, 
                        "FirstName" : entity.firstName, 
                        "LastName"  : entity.lastName, 
                        "PhoneNumber" : entity.phoneNumber, 
                        "BirthdayDate" : entity.birthdayDate,
                        "PersonalEmail" : entity.personalEmail
                    };
                    GenerateUpdateEntitySidebar(message, rowsData, entity.id);
                    break;
                
                case 'Professors':
                    message = `Update professor with id ${entity.id}`;
                    rowsData = {
                        "Speciality" : entity.speciality, 
                        "FirstName" : entity.firstName, 
                        "LastName"  : entity.lastName, 
                        "PhoneNumber" : entity.phoneNumber, 
                        "BirthdayDate" : entity.birthdayDate
                    };
                    GenerateUpdateEntitySidebar(message, rowsData, entity.id);
                    break;

                case 'Admins':
                    message = `Update admin with id ${entity.id}`;
                    rowsData = {
                        "Role" : entity.role
                    };
                    GenerateUpdateEntitySidebar(message, rowsData, entity.id);
                    break;    

                case 'Groups':
                    message = `Update group with id ${entity.id}`;
                    rowsData = {
                        "Name" : entity.name, 
                        "Specialization"  : entity.specialization
                    };
                    GenerateUpdateEntitySidebar(message, rowsData, entity.id);
                    break;
                default:
                    break;
            }

            let updateEntitySidebar = document.querySelector('.update-entity-sidebar');
            if(updateEntitySidebar !== null && updateEntitySidebar !== undefined){
                const pageContent = document.querySelector('.page-content');
                updateEntitySidebar.style.right = "-50%";
                pageContent.removeChild(updateEntitySidebar);
            }
            GenerateUpdateEntitySidebar(message, rowsData, entity.id);
        });
    
        actionsCell.appendChild(deleteIcon);
        actionsCell.appendChild(updateIcon);

        if(entitiesType === "Groups"){
            const manageIcon = document.createElement('i');
            manageIcon.classList.add('fas', 'fa-cog', 'action-icon', 'manage-action');
            manageIcon.addEventListener('click', async () => {
                localStorage.setItem("GroupId", entity.id);
                await DisplayInitialManageSidebar();
            });

            actionsCell.appendChild(manageIcon);
        }

        row.appendChild(actionsCell);
    
        tableBody.appendChild(row);
    });
 
    table.appendChild(tableBody);

    innerTableDiv.appendChild(table);

    tableContent.appendChild(innerTableDiv);

    tableContent.classList.add('loaded');
}

async function GenerateTablePagination(numberOfEntitiesToShow, maxPages, currentPage) {
    if(currentPage > maxPages)
    {
        currentPage = maxPages;
    }

    let tableContent = document.querySelector('.table-content');
    const paginationDiv = document.createElement('div');
    paginationDiv.classList.add('pagination-div');
    
    // Create previous page arrow
    const prevPageArrow = document.createElement("i");
    prevPageArrow.classList.add("fas", "fa-arrow-left", "pagination-arrow");
    prevPageArrow.addEventListener('click', async () => {
        if (currentPage > 1) {
            currentPage--;
            tableContent.removeChild(paginationDiv);
            if(filterActive === true){
                descending === true ? await DisplayFilteredAndOrderedTable(numberOfEntitiesToShow, currentPage, filterByElements, orderCriterion, 2) : await DisplayFilteredAndOrderedTable(numberOfEntitiesToShow, currentPage, filterByElements, orderCriterion, 1); 
            } else {
                descending === true ? await DisplayOrderedTable(numberOfEntitiesToShow, currentPage, orderCriterion, 2) : await DisplayOrderedTable(numberOfEntitiesToShow, currentPage, orderCriterion, 1); 
            }
        }
    });

    // Create next page arrow
    const nextPageArrow = document.createElement("i");
    nextPageArrow.classList.add("fas", "fa-arrow-right", "pagination-arrow");
    nextPageArrow.addEventListener('click', async () => {
        if (currentPage < maxPages) {
            currentPage++;
            tableContent.removeChild(paginationDiv);
            if(filterActive === true){
                descending === true ? await DisplayFilteredAndOrderedTable(numberOfEntitiesToShow, currentPage, filterByElements, orderCriterion, 2) : await DisplayFilteredAndOrderedTable(numberOfEntitiesToShow, currentPage, filterByElements, orderCriterion, 1); 
            } else {
                descending === true ? await DisplayOrderedTable(numberOfEntitiesToShow, currentPage, orderCriterion, 2) : await DisplayOrderedTable(numberOfEntitiesToShow, currentPage, orderCriterion, 1); 
            }
        }
    });

    const entitiesPerPageSelect = document.createElement("select");
    entitiesPerPageSelect.classList.add("entities-per-page-select");
    
    const entitiesPerPageOptions = [5, 10, 20, 50, 100];
    entitiesPerPageOptions.forEach(option => {
        const optionElement = document.createElement("option");
        optionElement.value = option;
        optionElement.textContent = option;
        entitiesPerPageSelect.appendChild(optionElement);
    });

    entitiesPerPageSelect.value = numberOfEntitiesToShow;

    entitiesPerPageSelect.addEventListener('change', async () => {
        numberOfEntitiesToShow = entitiesPerPageSelect.value
        tableContent.removeChild(paginationDiv);
        if(filterActive === true){
            descending === true ? await DisplayFilteredAndOrderedTable(numberOfEntitiesToShow, 1, filterByElements, orderCriterion, 2) : await DisplayFilteredAndOrderedTable(numberOfEntitiesToShow, 1, filterByElements, orderCriterion, 1); 

        } else {
            descending === true ? await DisplayOrderedTable(numberOfEntitiesToShow, 1, orderCriterion, 2) : await DisplayOrderedTable(numberOfEntitiesToShow, 1, orderCriterion, 1); 
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

    tableContent.appendChild(paginationDiv);
}
