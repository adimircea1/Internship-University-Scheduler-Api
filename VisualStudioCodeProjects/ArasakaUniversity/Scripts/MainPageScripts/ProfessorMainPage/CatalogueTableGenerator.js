import { FetchFilteredAndOrderedCatalogueDataAsync, FetchFilteredCatalogueDataAsync } from "../ProfessorMainPage/CatalogueEntity.js";

let descending = false;
let orderCriterion = "";
let filterByElements = {};
let tableHeaders = [];
let tableData = [];
let filterActive = false;

export async function DisplayInitialCatalogueTable()
{
    tableHeaders = ["CatalogueId", "UniversityGroupId"];
    tableData = ["id", "universityGroupId"];

    let catalogueData = await FetchFilteredCatalogueDataAsync(5, 1);

    await GenerateEntityTable(tableHeaders, tableData, catalogueData.entities);
    const maxPages = catalogueData.numberOfEntities / 5 > 1 ? catalogueData.numberOfEntities / 5 : 1;
    await GenerateTablePagination(5, Math.ceil(maxPages), 1);
}

export async function DisplayFilteredCatalogueTable(numberOfEntitiesToShow, currentPage, filterBy) {
    tableHeaders = ["CatalogueId", "UniversityGroupId"];
    tableData = ["id", "universityGroupId"];

    let catalogueData = await FetchFilteredCatalogueDataAsync(numberOfEntitiesToShow, currentPage, filterBy);

    await GenerateEntityTable(tableHeaders, tableData, catalogueData.entities);

    const maxPages = catalogueData.numberOfEntities / numberOfEntitiesToShow > 1 ? catalogueData.numberOfEntities / numberOfEntitiesToShow : 1;
    await GenerateTablePagination(numberOfEntitiesToShow, Math.ceil(maxPages), currentPage);
}

export async function DisplayFilteredAndOrderedCatalogueTable(numberOfEntitiesToShow, currentPage, filterBy, orderBy, orderDirection) {
    tableHeaders = ["CatalogueId", "UniversityGroupId"];
    tableData = ["id", "universityGroupId"];

    let catalogueData = await FetchFilteredAndOrderedCatalogueDataAsync(numberOfEntitiesToShow, currentPage, filterBy, orderBy, orderDirection);

    await GenerateEntityTable(tableHeaders, tableData, catalogueData.entities);

    const maxPages = catalogueData.numberOfEntities / numberOfEntitiesToShow > 1 ? catalogueData.numberOfEntities / numberOfEntitiesToShow : 1;
    await GenerateTablePagination(numberOfEntitiesToShow, Math.ceil(maxPages), currentPage);
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
           await DisplayFilteredAndOrderedCatalogueTable(numberOfEntitiesSelectMenu.value, currentPage, filterByElements, orderBy, 2);

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

            await DisplayFilteredAndOrderedCatalogueTable(numberOfEntitiesSelectMenu.value, currentPage, filterByElements, orderBy, 1);
        });

        orderIconsCell.appendChild(orderDescIcon);
        orderIconsCell.appendChild(orderAscIcon);
        iconsRow.appendChild(orderIconsCell);
    });

    const iconRowEmptyHeader = document.createElement('td');
    iconsRow.appendChild(iconRowEmptyHeader);


    const actionsHeader = document.createElement("th");
    actionsHeader.textContent = "Actions";
    tableHeadRow.appendChild(actionsHeader);
    
    tableHead.appendChild(tableHeadRow);
    tableHead.appendChild(iconsRow);

    const filterIcon = document.createElement('i');
    filterIcon.classList.add('fas', 'fa-filter', 'catalogue-filter-icon');
    filterIcon.addEventListener('click', async () =>{
        const searchBoxes = searchRow.querySelectorAll('.search-box');
        const nonEmptySearchBoxes = Array.from(searchBoxes).filter(searchBox => searchBox.value.trim() !== '');

        const numberOfEntitiesSelect = document.querySelector('.entities-per-page-select');
        const pageIndicator = document.querySelector('.page-indicator');

        const currentPage = parseInt(pageIndicator.getAttribute("data-current-page"));

        if(nonEmptySearchBoxes.length === 0){
            await DisplayInitialCatalogueTable();
            filterByElements = filterBy;
            filterActive = false;
        } else {
            filterActive = true;

            for (const element of nonEmptySearchBoxes) {
                const filterByAttribute = element.getAttribute("data-filter-by");
                const filterValue = element.value;
                filterByElements[filterByAttribute] = filterValue;
            }

            await DisplayFilteredCatalogueTable(numberOfEntitiesSelect.value, 1, filterByElements);
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
    
    const prevPageArrow = document.createElement("i");
    prevPageArrow.classList.add("fas", "fa-arrow-left", "pagination-arrow");
    prevPageArrow.addEventListener('click', async () => {
        if (currentPage > 1) {
            currentPage--;
            tableContent.removeChild(paginationDiv);
            descending === true ? await DisplayFilteredAndOrderedCatalogueTable(numberOfEntitiesToShow, currentPage, filterByElements, orderCriterion, 2) : await DisplayFilteredAndOrdereCatalogueTable(numberOfEntitiesToShow, currentPage, filterByElements, orderCriterion, 1); 
        }
    });

    const nextPageArrow = document.createElement("i");
    nextPageArrow.classList.add("fas", "fa-arrow-right", "pagination-arrow");
    nextPageArrow.addEventListener('click', async () => {
        if (currentPage < maxPages) {
            currentPage++;
            tableContent.removeChild(paginationDiv);
            descending === true ? await DisplayFilteredAndOrderedCatalogueTable(numberOfEntitiesToShow, currentPage, filterByElements, orderCriterion, 2) : await DisplayFilteredAndOrderedCatalogueTable(numberOfEntitiesToShow, currentPage, filterByElements, orderCriterion, 1); 
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
        numberOfEntitiesToShow = entitiesPerPageSelect.value
        tableContent.removeChild(paginationDiv);
        descending === true ? await DisplayFilteredAndOrderedCatalogueTable(numberOfEntitiesToShow, 1, filterByElements, orderCriterion, 2) : await DisplayFilteredAndOrderedCatalogueTable(numberOfEntitiesToShow, 1, filterByElements, orderCriterion, 1); 
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