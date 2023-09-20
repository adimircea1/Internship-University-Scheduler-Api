import { DeleteRegisterRequestByIdAsync, FetchFilteredAndOrderedRegisterRequestDataAsync, FetchFilteredRegisterRequestDataAsync, FetchRegisterRequestDataAsync } from "./RegisterRequestEntity.js";
import { AddStudentFromRegisterRequestAsync } from "./StudentEntity.js";
import { RegisterUserAsync } from "./UserEntity.js";
import { AddProfessorFromRegisterRequestAsync } from "./ProfessorEntity.js";

let descending = false;
let orderCriterion = "";
let filterActive = false;
let filterByElements = {};
let tableHeaders = [];
let tableData = [];

export async function DisplayInitialRegisterRequestTable(){
    const activeOptionElement = document.querySelector('.option-active');
    switch(activeOptionElement.textContent){
        case "Student registrations" : 
            tableHeaders = ["Id", "FirstName", "LastName", "Email", "PhoneNumber", "Birthdate"];
            tableData = ["id", "firstName", "lastName", "email", "phoneNumber", "birthdate"];
            //MaxEntities, CurrentPageNumber
            let databaseFeedbackForStudentRegisterRequests = await FetchRegisterRequestDataAsync(5, 1, "", 1);
            await GenerateEntityTable(tableHeaders, tableData, databaseFeedbackForStudentRegisterRequests.entities);
            const maxPages1 = databaseFeedbackForStudentRegisterRequests.numberOfEntities / 5 > 1 ? databaseFeedbackForStudentRegisterRequests.numberOfEntities / 5 : 1;
            await GenerateTablePagination(5, Math.ceil(maxPages1), 1);
        break;

        case "Professor registrations" :
            tableHeaders = ["Id", "FirstName", "LastName", "Email", "PhoneNumber", "Birthdate", "Speciality"];
            tableData = ["id", "firstName", "lastName", "email", "phoneNumber", "birthdate", "speciality"];
            //MaxEntities, CurrentPageNumber
            let databaseFeedbackForProfessorRegisterRequests = await FetchRegisterRequestDataAsync(5, 1, "", 1);
            await GenerateEntityTable(tableHeaders, tableData, databaseFeedbackForProfessorRegisterRequests.entities);
            const maxPages2 = databaseFeedbackForProfessorRegisterRequests.numberOfEntities / 5 > 1 ? databaseFeedbackForProfessorRegisterRequests.numberOfEntities / 5 : 1;
            await GenerateTablePagination(5, Math.ceil(maxPages2), 1);
        break;
    }
   
}

 async function DisplayOrderedTable(numberOfEntitiesToShow, currentPage, orderBy, orderDirection){

    const activeOptionElement = document.querySelector('.option-active');
    switch(activeOptionElement.textContent){
        case "Student registrations" : 
            tableHeaders = ["Id", "FirstName", "LastName", "Email", "PhoneNumber", "Birthdate"];
            tableData = ["id", "firstName", "lastName", "email", "phoneNumber", "birthdate"];
            //MaxEntities, CurrentPageNumber
            let databaseFeedbackForStudentRegisterRequests = await FetchRegisterRequestDataAsync(numberOfEntitiesToShow, currentPage, orderBy, orderDirection);
            await GenerateEntityTable(tableHeaders, tableData, databaseFeedbackForStudentRegisterRequests.entities);
            const maxPages1 = databaseFeedbackForStudentRegisterRequests.numberOfEntities / 5 > 1 ? databaseFeedbackForStudentRegisterRequests.numberOfEntities / 5 : 1;
            await GenerateTablePagination(numberOfEntitiesToShow, Math.ceil(maxPages1), currentPage);
        break;

        case "Professor registrations" :
            tableHeaders = ["Id", "FirstName", "LastName", "Email", "PhoneNumber", "Birthdate", "Speciality"];
            tableData = ["id", "firstName", "lastName", "email", "phoneNumber", "birthdate", "speciality"];
            //MaxEntities, CurrentPageNumber
            let databaseFeedbackForProfessorRegisterRequests = await FetchRegisterRequestDataAsync(numberOfEntitiesToShow, currentPage, orderBy, orderDirection);
            await GenerateEntityTable(tableHeaders, tableData, databaseFeedbackForProfessorRegisterRequests.entities);
            const maxPages2 = databaseFeedbackForProfessorRegisterRequests.numberOfEntities / 5 > 1 ? databaseFeedbackForProfessorRegisterRequests.numberOfEntities / 5 : 1;
            await GenerateTablePagination(numberOfEntitiesToShow, Math.ceil(maxPages2), currentPage);
        break;  
    }
}

 async function DisplayFilteredTable(numberOfEntitiesToShow, currentPage, filterBy){
    const activeOptionElement = document.querySelector('.option-active');
    switch(activeOptionElement.textContent){
        case "Student registrations" : 
            tableHeaders = ["Id", "FirstName", "LastName", "Email", "PhoneNumber", "Birthdate"];
            tableData = ["id", "firstName", "lastName", "email", "phoneNumber", "birthdate"];
            //MaxEntities, CurrentPageNumber
            let databaseFeedbackForStudentRegisterRequests = await FetchFilteredRegisterRequestDataAsync(numberOfEntitiesToShow, currentPage, filterBy);
            await GenerateEntityTable(tableHeaders, tableData, databaseFeedbackForStudentRegisterRequests.entities);
            const maxPages1 = databaseFeedbackForStudentRegisterRequests.numberOfEntities / 5 > 1 ? databaseFeedbackForStudentRegisterRequests.numberOfEntities / 5 : 1;
            await GenerateTablePagination(numberOfEntitiesToShow, Math.ceil(maxPages1), currentPage);
        break;

        case "Professor registrations" :
            tableHeaders = ["Id", "FirstName", "LastName", "Email", "PhoneNumber", "Birthdate", "Speciality"];
            tableData = ["id", "firstName", "lastName", "email", "phoneNumber", "birthdate", "speciality"];
            //MaxEntities, CurrentPageNumber
            let databaseFeedbackForProfessorRegisterRequests = await FetchFilteredRegisterRequestDataAsync(numberOfEntitiesToShow, currentPage, filterBy);
            await GenerateEntityTable(tableHeaders, tableData, databaseFeedbackForProfessorRegisterRequests.entities);
            const maxPages2 = databaseFeedbackForProfessorRegisterRequests.numberOfEntities / 5 > 1 ? databaseFeedbackForProfessorRegisterRequests.numberOfEntities / 5 : 1;
            await GenerateTablePagination(numberOfEntitiesToShow, Math.ceil(maxPages2), currentPage);
        break;  
    }
}

 async function DisplayFilteredAndOrderedTable(numberOfEntitiesToShow, currentPage, filterBy, orderBy, orderDirection){
    const activeOptionElement = document.querySelector('.option-active');
    switch(activeOptionElement.textContent){
        case "Student registrations" : 
            tableHeaders = ["Id", "FirstName", "LastName", "Email", "PhoneNumber", "Birthdate", "PersonalEmail"];
            tableData = ["id", "firstName", "lastName", "email", "phoneNumber", "birthdate", "personalEmail"];
            //MaxEntities, CurrentPageNumber
            let databaseFeedbackForStudentRegisterRequests = await FetchFilteredAndOrderedRegisterRequestDataAsync(numberOfEntitiesToShow, currentPage, filterBy, orderBy, orderDirection);
            await GenerateEntityTable(tableHeaders, tableData, databaseFeedbackForStudentRegisterRequests.entities);
            const maxPages1 = databaseFeedbackForStudentRegisterRequests.numberOfEntities / 5 > 1 ? databaseFeedbackForStudentRegisterRequests.numberOfEntities / 5 : 1;
            await GenerateTablePagination(numberOfEntitiesToShow, Math.ceil(maxPages1), currentPage);
        break;

        case "Professor registrations" :
            tableHeaders = ["Id", "FirstName", "LastName", "Email", "PhoneNumber", "Birthdate", "Speciality"];
            tableData = ["id", "firstName", "lastName", "email", "phoneNumber", "birthdate", "speciality"];
            //MaxEntities, CurrentPageNumber
            let databaseFeedbackForProfessorRegisterRequests = await FetchFilteredAndOrderedRegisterRequestDataAsync(numberOfEntitiesToShow, currentPage, filterBy, orderBy, orderDirection);
            await GenerateEntityTable(tableHeaders, tableData, databaseFeedbackForProfessorRegisterRequests.entities);
            const maxPages2 = databaseFeedbackForProfessorRegisterRequests.numberOfEntities / 5 > 1 ? databaseFeedbackForProfessorRegisterRequests.numberOfEntities / 5 : 1;
            await GenerateTablePagination(numberOfEntitiesToShow, Math.ceil(maxPages2), currentPage);
        break;
    }
}

async function GenerateEntityTable(tableHeaders, tableData, entities) {
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
            for (const element of nonEmptySearchBoxes) {
                const filterByAttribute = element.getAttribute("data-filter-by");
                const filterValue = element.value;
                filterByElements[filterByAttribute] = filterValue;
            }

            await DisplayFilteredTable(numberOfEntitiesSelect.value, 1, filterByElements);
            filterByElements = {};
            filterActive = true;
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
        
        const actionsCell = document.createElement('td');
        const approveIcon = document.createElement('i');
        approveIcon.classList.add('fas', 'fa-check', 'action-icon', 'approve-action');
        approveIcon.addEventListener('click', async () => {
            
            var response = await RegisterUserAsync(entity.id); //this will also delete the request
            console.log(response);
            if(response === undefined){
                console.log("Something went wrong while sending the credentials!");
                return;
            }
        });

        const deleteIcon = document.createElement('i');
        deleteIcon.classList.add('fas', 'fa-trash-alt', 'action-icon', 'delete-action');
        deleteIcon.addEventListener('click', async () => {
            await DeleteRegisterRequestByIdAsync(entity.id);
        });
            
        actionsCell.appendChild(approveIcon);
        actionsCell.appendChild(deleteIcon);
        row.appendChild(actionsCell);
    
        tableBody.appendChild(row);
    });
 
    table.appendChild(tableBody);

    innerTableDiv.appendChild(table);

    tableContent.appendChild(innerTableDiv);

    tableContent.classList.add('loaded');

    pageContent.appendChild(tableContent);
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
