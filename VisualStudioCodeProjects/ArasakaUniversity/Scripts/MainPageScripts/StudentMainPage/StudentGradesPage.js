import { GetStudentByEmailAsync } from "../AdminMainPage/StudentEntity.js";

const upperBarEmailContent = document.querySelector('.upperBarEmail');
const studentEmail = upperBarEmailContent.textContent;
const accessToken = localStorage.getItem('AccessToken');
const pageContent = document.querySelector('.page-content');

let descendent = false;
let filterActive = false;
let filterByElement = {};

export async function GenerateInitialGradesTableAsync(){
    const databaseFeedback = await GetFilteredGradesByStudentIdAsync(5, 1, filterByElement);
    console.log(databaseFeedback);
    const maxPages = databaseFeedback.numberOfEntities / 5 > 1 ? databaseFeedback.numberOfEntities / 5 : 1;
    await  GenerateGradesSectionPageAsync(databaseFeedback.entities, 1, Math.ceil(maxPages), 5);
}

async function GenerateGradesSectionPageAsync(grades, currentPage, maxPages, numberOfEntitiesToShow){
    pageContent.innerHTML = '';

    const gradeTableContainer = document.createElement('div');
    gradeTableContainer.classList.add('grade-table-container');

    const gradeTableMenuContainer = document.createElement('div');
    gradeTableMenuContainer.classList.add('grade-table-menu-container');

    const orderAscButton = document.createElement('button');
    orderAscButton.textContent = 'OrderByValueAsc';
    orderAscButton.classList.add('order-grades-button');
    orderAscButton.addEventListener('click', async () => {
        const pageIndicator = document.querySelector('.page-indicator');
        const pageNumber = parseInt(pageIndicator.getAttribute("data-current-page"));

        const numberOfEntitiesToShow = document.querySelector('.entities-per-page-select');
        let databaseFeedback;
        databaseFeedback = await GetFilteredAndOrderedGradesByStudentIdAsync(numberOfEntitiesToShow.value, pageNumber, filterByElement, 1);
        const maxPages = databaseFeedback.numberOfEntities / numberOfEntitiesToShow.value > 1 ? databaseFeedback.numberOfEntities / numberOfEntitiesToShow.value : 1;
        descendent = false;

        await GenerateGradesSectionPageAsync(databaseFeedback.entities, pageNumber, Math.ceil(maxPages), numberOfEntitiesToShow.value); 
    });

    const orderDescButton = document.createElement('button');
    orderDescButton.textContent = 'OrderByValueDesc';
    orderDescButton.classList.add('order-grades-button');
    orderDescButton.addEventListener('click', async () => {
        const pageIndicator = document.querySelector('.page-indicator');
        const pageNumber = parseInt(pageIndicator.getAttribute("data-current-page"));

        const numberOfEntitiesToShow = document.querySelector('.entities-per-page-select');
        let databaseFeedback;
        databaseFeedback = await GetFilteredAndOrderedGradesByStudentIdAsync(numberOfEntitiesToShow.value, pageNumber, filterByElement, 2);
        const maxPages = databaseFeedback.numberOfEntities / numberOfEntitiesToShow.value > 1 ? databaseFeedback.numberOfEntities / numberOfEntitiesToShow.value : 1;
        descendent = true;

        await GenerateGradesSectionPageAsync(databaseFeedback.entities, pageNumber, Math.ceil(maxPages), numberOfEntitiesToShow.value); 
    });

    const searchBox = document.createElement('input');
    searchBox.classList.add("filter-grade-by-text-input");
    searchBox.placeholder = "Filter by course id...";

    const filteringIcon = document.createElement('i');
    filteringIcon.classList.add('fas', 'fa-filter'); 
    filteringIcon.classList.add('grade-filter-icon');
    filteringIcon.addEventListener('click', async() => {
        const searchBox = document.querySelector('.filter-grade-by-text-input');
        const numberOfEntitiesToShow = document.querySelector('.entities-per-page-select');
        let databaseFeedback;
        if(searchBox.value === ''){
            filterActive = false;
            filterByElement = {};
            descendent === true ? databaseFeedback = await GetFilteredAndOrderedGradesByStudentIdAsync(numberOfEntitiesToShow.value, 1, filterByElement, 2) : databaseFeedback = await GetFilteredAndOrderedGradesByStudentIdAsync(numberOfEntitiesToShow.value, 1, filterByElement, 1);
        } else {
            filterByElement = {
                "CourseId" :  searchBox.value
            };
            filterActive = true;
            databaseFeedback = await GetFilteredGradesByStudentIdAsync(numberOfEntitiesToShow.value, 1, filterByElement);
        }

        const maxPages = databaseFeedback.numberOfEntities / numberOfEntitiesToShow.value > 1 ? databaseFeedback.numberOfEntities / numberOfEntitiesToShow.value : 1;
        await GenerateGradesSectionPageAsync(databaseFeedback.entities, 1, Math.ceil(maxPages), numberOfEntitiesToShow.value); 

    });

    const searchBoxContainer = document.createElement('div');
    searchBoxContainer.classList.add('search-box-container');
    searchBoxContainer.appendChild(searchBox);
    searchBoxContainer.appendChild(filteringIcon);

    gradeTableMenuContainer.appendChild(orderAscButton);
    gradeTableMenuContainer.appendChild(orderDescButton);
    gradeTableMenuContainer.appendChild(searchBoxContainer);

    const divider = document.createElement('hr');
    divider.classList.add('grade-table-divider');

    const gradeTableInnerContainer = document.createElement('div');
    gradeTableInnerContainer.classList.add('grade-table-inner-container');

    const headerDiv = document.createElement('div');
    headerDiv.classList.add('grade-header');
    headerDiv.innerHTML = '<div class="header-cell">Value</div><div class="header-cell">Course Id</div><div class="header-cell">Course Domain</div>';

    const gradeTableBody = document.createElement('div');
    gradeTableBody.classList.add('grade-table-body');

    grades.forEach(async (grade) => {
        const valueCell = document.createElement('div');
        valueCell.classList.add('body-cell');
        valueCell.textContent = grade.value;

        const courseData = await GetCourseByIdAsync(grade.courseId);

        const idCell = document.createElement('div');
        idCell.classList.add('body-cell');
        idCell.textContent = courseData.id;

        const domainCell = document.createElement('div');
        domainCell.classList.add('body-cell');
        domainCell.textContent = courseData.domain;

        const row = document.createElement('div');
        row.classList.add('grade-row');
        row.appendChild(valueCell);
        row.appendChild(idCell);
        row.appendChild(domainCell);

        gradeTableBody.appendChild(row);
    });

    gradeTableInnerContainer.appendChild(headerDiv);
    gradeTableInnerContainer.appendChild(gradeTableBody);

    gradeTableContainer.appendChild(gradeTableMenuContainer);
    gradeTableContainer.appendChild(divider);
    gradeTableContainer.appendChild(gradeTableInnerContainer);

    const paginationContainer = GeneratePaginationContainer(numberOfEntitiesToShow, Math.ceil(maxPages), currentPage);

    gradeTableContainer.append(paginationContainer);

    pageContent.appendChild(gradeTableContainer);
}


function GeneratePaginationContainer(numberOfEntitiesToShow, maxPages, currentPage) {
    if (currentPage > maxPages) {
        currentPage = maxPages;
    }

    const paginationContainer = document.createElement('div');
    paginationContainer.classList.add('grade-table-pagination-container');

    // Create previous page arrow
    const prevPageArrow = document.createElement("i");
    prevPageArrow.classList.add("fas", "fa-arrow-left", "pagination-arrow");
    prevPageArrow.addEventListener('click', async () => {
        if (currentPage > 1) {
            currentPage--;
           
            const numberOfEntitiesToShow = document.querySelector('.entities-per-page-select');

            let databaseFeedback;

            descendent === true ? databaseFeedback = await GetFilteredAndOrderedGradesByStudentIdAsync(numberOfEntitiesToShow.value, currentPage, filterByElement, 2) : databaseFeedback = await GetFilteredAndOrderedGradesByStudentIdAsync(numberOfEntitiesToShow.value, currentPage, filterByElement, 1);

            const maxPages = databaseFeedback.numberOfEntities / numberOfEntitiesToShow.value > 1 ? databaseFeedback.numberOfEntities / numberOfEntitiesToShow.value : 1;
           
            await GenerateGradesSectionPageAsync(databaseFeedback.entities, currentPage, Math.ceil(maxPages), numberOfEntitiesToShow.value); 
        }
    });

    // Create next page arrow
    const nextPageArrow = document.createElement("i");
    nextPageArrow.classList.add("fas", "fa-arrow-right", "pagination-arrow");
    nextPageArrow.addEventListener('click', async () => {
        if (currentPage < maxPages) {
            currentPage++;

            const numberOfEntitiesToShow = document.querySelector('.entities-per-page-select');
          
            let databaseFeedback;

            descendent === true ? databaseFeedback = await GetFilteredAndOrderedGradesByStudentIdAsync(numberOfEntitiesToShow.value, currentPage, filterByElement, 2) : databaseFeedback = await GetFilteredAndOrderedGradesByStudentIdAsync(numberOfEntitiesToShow.value, currentPage, filterByElement, 1);

            const maxPages = databaseFeedback.numberOfEntities  / numberOfEntitiesToShow.value > 1 ? databaseFeedback.numberOfEntities / numberOfEntitiesToShow.value : 1;
            
            await GenerateGradesSectionPageAsync(databaseFeedback.entities, currentPage, Math.ceil(maxPages), numberOfEntitiesToShow.value); 
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
        let databaseFeedback;
        descendent === true ? databaseFeedback = await GetFilteredAndOrderedGradesByStudentIdAsync(numberOfEntitiesToShow, 1, filterByElement, 2) : databaseFeedback = await GetFilteredAndOrderedGradesByStudentIdAsync(numberOfEntitiesToShow, 1, filterByElement, 1); 
        const maxPages = databaseFeedback.numberOfEntities / numberOfEntitiesToShow > 1 ? databaseFeedback.numberOfEntities / numberOfEntitiesToShow : 1;
        await GenerateGradesSectionPageAsync(databaseFeedback.entities, 1, Math.ceil(maxPages), numberOfEntitiesToShow); 
    });

    const pageIndicator = document.createElement("span");
    pageIndicator.textContent = `Page: ${currentPage} / ${maxPages}`;
    pageIndicator.classList.add("page-indicator");
    pageIndicator.setAttribute("data-current-page", currentPage);
    pageIndicator.setAttribute("data-max-pages", maxPages);

    paginationContainer.appendChild(prevPageArrow);
    paginationContainer.appendChild(entitiesPerPageSelect);
    paginationContainer.appendChild(pageIndicator);
    paginationContainer.appendChild(nextPageArrow);

    return paginationContainer;
}


async function GetGradesByStudentIdAsync(){
    const studentId = (await GetStudentByEmailAsync(studentEmail)).id;
    const url = new URL(`http://localhost:5150/grades/student/${studentId}`);
    const requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        }
    };

    let gradesData;

    await fetch(url, requestOptions)
        .then(response => response.json())
        .then(data => gradesData = data)
        .catch(error => {
            console.log(error);
            return;
        });

    return gradesData;
}

async function GetFilteredAndOrderedGradesByStudentIdAsync(maxEntities, currentPageNumber, filterBy, orderDirection){
    const studentId = (await GetStudentByEmailAsync(studentEmail)).id;
    filterBy["StudentId"] = studentId.toString();
    const url = new URL(`http://localhost:5150/grades/filtered-ordered`);

    const filteringSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        FilterBy : filterBy,
        OrderBy : "Value",
        OrderDirection : orderDirection
    };

    const requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
        body: JSON.stringify(filteringSettings)
    };

    let gradesData;

    await fetch(url, requestOptions)
        .then(response => response.json())
        .then(data => gradesData = data)
        .catch(error => {
            console.log(error);
            return;
        });

    return gradesData;
}

async function GetFilteredGradesByStudentIdAsync(maxEntities, currentPageNumber, filterBy){
    const studentId = (await GetStudentByEmailAsync(studentEmail)).id;
    console.log(studentId);
    filterBy["StudentId"] = studentId.toString();

    const url = new URL(`http://localhost:5150/grades/filtered`);

    const filteringSettings = {
        PageSize : maxEntities,
        PageNumber : currentPageNumber,
        FilterBy : filterBy
    };

    const requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        },
        body: JSON.stringify(filteringSettings)
    };

    let gradesData;

    await fetch(url, requestOptions)
        .then(response => response.json())
        .then(data => gradesData = data)
        .catch(error => {
            console.log(error);
            return;
        });

    return gradesData;
}

async function GetCourseByIdAsync(courseId){
    const url = new URL(`http://localhost:5150/courses/${courseId}`);
    const requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type' : 'application/json',
            'Authorization' : `Bearer ${accessToken}`
        }
    };

    let courseData;

    await fetch(url, requestOptions)
        .then(response => response.json())
        .then(data => courseData = data)
        .catch(error => {
            console.log(error);
            return;
        });

    return courseData;
}