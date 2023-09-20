import { GetAllExamsOfStudent, GetAvailableExamsOfStudent, GetFilteredAndOrderedExamsAsync, GetUnavailableExamsOfStudent } from "./ExamEntity.js";
import { GetFilteredExamsAsync } from "./ExamEntity.js";
import { GetExamAttendancesByStudentIdAsync } from "./ExamAttendanceEntity.js";
import { GetUserDataAsync } from "./StudentMainPage.js";
import { GetStudentByEmailAsync } from "../AdminMainPage/StudentEntity.js";
import { GetCourseByIdAsync } from "../ProfessorMainPage/CourseEntity.js";

let filterActive;
let filterByElement;
let descending;
let orderCriterion;
let studentId;
let userData = await GetUserDataAsync();

export async function InitialisePage(){
    pageContent.innerHTML = '';
    const studentEmail = userData.email;
    studentId = (await GetStudentByEmailAsync(studentEmail)).id;
    filterByElement = {};
    filterActive = false;
    descending = false;
    orderCriterion = "AvailableFrom";
    await GenerateTableExamMenu();
}

async function GenerateTableExamMenu(){
    const pageContent = document.querySelector('.page-content');

    const examMenu = document.createElement('div');
    examMenu.classList.add('exam-table-menu');

    const examOptions = document.createElement('div');
    examOptions.classList.add('exam-options');

    const showAvailableButton = GenerateButton('show-available', "Show available");
    showAvailableButton.addEventListener('click', async () => {
        const examsDiv = document.querySelector('.exams-div');
        const pageContent = document.querySelector('.page-content');
        pageContent.removeChild(examsDiv);
        const examData = await GetAvailableExamsOfStudent(studentId);
        await GenerateExamTable(examData);
    });

    const showUnavailableButton = GenerateButton('show-unavailable', "Show unavailable");
    showUnavailableButton.addEventListener('click', async () => {
        const examsDiv = document.querySelector('.exams-div');
        const pageContent = document.querySelector('.page-content');
        pageContent.removeChild(examsDiv);
        const examData = await GetUnavailableExamsOfStudent(studentId);
        console.log(examData);
        await GenerateExamTable(examData);
    });

    
    const showAllButton = GenerateButton('show-all', "Show all");
    showAllButton.addEventListener('click', async () => {
        const examsDiv = document.querySelector('.exams-div');
        const pageContent = document.querySelector('.page-content');
        pageContent.removeChild(examsDiv);
        const examData = await GetAllExamsOfStudent(studentId);
        await GenerateExamTable(examData);
    });

    const examSearch = document.createElement('div');
    examSearch.classList.add('exam-search');

    const searchInput = document.createElement('input');
    searchInput.placeholder = "Filter by...";
    searchInput.type = "text";
    searchInput.classList.add('search-input');

    examOptions.appendChild(showAvailableButton);
    examOptions.appendChild(showUnavailableButton);
    examOptions.appendChild(showAllButton);

    const examDivider = document.createElement("hr");
    examDivider.classList.add("exam-div-divider");

    examMenu.appendChild(examOptions);
    examMenu.appendChild(examDivider);
    pageContent.appendChild(examMenu);

    await GenerateInitialExamTable();
}

async function GenerateInitialExamTable(){
    const examsDiv = document.createElement('div');
    examsDiv.classList.add('exams-div');

    const examDivContent = document.createElement("div");
    examDivContent.classList.add("exam-div-content");

    const examList = document.createElement("ul");
    examList.className = "exam-list";

    const exams = await GetAllExamsOfStudent(studentId);

    for(const exam of exams){
        const examElement = await GenerateExamElement(exam);
        examList.appendChild(examElement);
    }

    examDivContent.appendChild(examList);

    examsDiv.appendChild(examDivContent);

    const pageContent = document.querySelector('.page-content');
    pageContent.appendChild(examsDiv);
}

async function GenerateExamTable(examData){
    const examsDiv = document.createElement('div');
    examsDiv.classList.add('exams-div');

    const examDivContent = document.createElement("div");
    examDivContent.classList.add("exam-div-content");

    const examList = document.createElement("ul");
    examList.className = "exam-list";

    for(const exam of examData){
        const examElement = await GenerateExamElement(exam);
        examList.appendChild(examElement);
    }

    examDivContent.appendChild(examList);

    examsDiv.appendChild(examDivContent);

    const pageContent = document.querySelector('.page-content');
    pageContent.appendChild(examsDiv);
}


async function GenerateExamElement(exam) {
    const li = document.createElement("li");
    li.className = "exam-element";

    const div = document.createElement("div");
    div.className = "exam-card";

    const examDetails = document.createElement("div");
    examDetails.className = "exam-details";

    const availableFromDate = new Date(exam.availableFrom).toLocaleString(); // Convert to local date and time format
    const availableUntilDate = new Date(exam.availableUntil).toLocaleString(); // Convert to local date and time format
    
    const p1 = CreateParagraph(`Available from: ${availableFromDate}`);
    const p2 = CreateParagraph(`Available until: ${availableUntilDate}`);
    const p3 = CreateParagraph(`Exam duration: ${exam.examDuration} minutes`);
    const p4 = CreateParagraph(`Final grade: ${exam.finalGrade}`);

    const course = await GetCourseByIdAsync(exam.courseId);
    const p5 = CreateParagraph(`Course domain: ${course.domain}`);

    examDetails.appendChild(p1);
    examDetails.appendChild(p2);
    examDetails.appendChild(p3);    
    examDetails.appendChild(p4);
    examDetails.appendChild(p5);

    const attendButton = GenerateButton("attend-button", "Attend exam");

    const now = new Date(); // Current date and time
    const availableFrom = new Date(exam.availableFrom);
    const availableUntil = new Date(exam.availableUntil);

    if (now < availableFrom || now > availableUntil || exam.finalGrade !== null) {
        attendButton.disabled = true; 
        attendButton.textContent = "Exam Not Available";
        attendButton.style.backgroundColor = "#9e9e9e"; 
        attendButton.style.pointerEvents = "none"; 
    } else {
        attendButton.addEventListener("click", () => {
            localStorage.setItem('examId', exam.id);
            localStorage.setItem('studentId', studentId);
            window.location.href = "ExamTemplate.html";
        });
    }

    div.appendChild(examDetails);
    div.appendChild(attendButton);

    li.appendChild(div);

    return li;
}

function CreateParagraph(text) {
    const p = document.createElement("p");
    p.textContent = text;
    return p;
}

function GenerateButton(className, text){
    const button = document.createElement('button');
    if(text === 'Attend exam')
    {
        button.classList.add(className);
    } else {
        button.classList.add('option-button', className);
    }
    button.textContent = text;
    return button;
}

// function GeneratePaginationDiv(numberOfEntitiesToShow, maxPages, currentPage) {
//     if(currentPage > maxPages)
//     {
//         currentPage = maxPages;
//     }

//     const paginationDiv = document.createElement('div');
//     paginationDiv.classList.add('pagination-div');
    
//     // Create previous page arrow
//     const prevPageArrow = document.createElement("i");
//     prevPageArrow.classList.add("fas", "fa-arrow-left", "pagination-arrow");
//     prevPageArrow.addEventListener('click', () => {
//         const numberOfEntitiesSelectMenu = document.querySelector('.entities-per-page-select');

//         if (currentPage > 1) {
//             currentPage--;
//             const examsDiv = document.querySelector('.exams-div');
//             const pageContent = document.querySelector('.page-content');
//             pageContent.removeChild(examsDiv);
//             if(filterActive === true){
//                 descending === true ? GenerateFilteredAndOrderedExamTable(numberOfEntitiesSelectMenu.value, currentPage, filterByElement, 2) : GenerateFilteredAndOrderedExamTable(numberOfEntitiesSelectMenu.value, currentPage, filterByElement, 1); 
//             } else {
//                 descending === true ? GenerateOrderedExamTable(numberOfEntitiesSelectMenu.value, currentPage, 2) : GenerateOrderedExamTable(numberOfEntitiesSelectMenu.value, currentPage, 1); 
//             }
//         }
//     });

//     // Create next page arrow
//     const nextPageArrow = document.createElement("i");
//     nextPageArrow.classList.add("fas", "fa-arrow-right", "pagination-arrow");
//     nextPageArrow.addEventListener('click', () => {
//         const numberOfEntitiesSelectMenu = document.querySelector('.entities-per-page-select');

//         if (currentPage < maxPages) {
//             currentPage++;
//             const examsDiv = document.querySelector('.exams-div');
//             const pageContent = document.querySelector('.page-content');
//             pageContent.removeChild(examsDiv);
//             if(filterActive === true){
//                 descending === true ? GenerateFilteredAndOrderedExamTable(numberOfEntitiesSelectMenu.value, currentPage, filterByElement, 2) : GenerateFilteredAndOrderedExamTable(numberOfEntitiesSelectMenu.value, currentPage, filterByElement, 1); 
//             } else {
//                 descending === true ? GenerateOrderedExamTable(numberOfEntitiesSelectMenu.value, currentPage, 2) : GenerateOrderedExamTable(numberOfEntitiesSelectMenu.value, currentPage, 1); 
//             }
//         }
//     });

//     const entitiesPerPageSelect = document.createElement("select");
//     entitiesPerPageSelect.classList.add("entities-per-page-select");
    
//     const entitiesPerPageOptions = [5, 10];
//     entitiesPerPageOptions.forEach(option => {
//         const optionElement = document.createElement("option");
//         optionElement.value = option;
//         optionElement.textContent = option;
//         entitiesPerPageSelect.appendChild(optionElement);
//     });

//     entitiesPerPageSelect.value = numberOfEntitiesToShow;

//     entitiesPerPageSelect.addEventListener('change', async () => {
//         numberOfEntitiesToShow = entitiesPerPageSelect.value;
//         if(filterActive === true){
//             descending === true ? GenerateFilteredAndOrderedExamTable(numberOfEntitiesToShow, currentPage, filterByElement, 2) : GenerateFilteredAndOrderedExamTable(numberOfEntitiesToShow, currentPage, filterByElement, 1); 
//         } else {
//             descending === true ? GenerateOrderedExamTable(numberOfEntitiesToShow, currentPage, 2) : GenerateOrderedExamTable(numberOfEntitiesToShow, currentPage, 1); 
//         }
//         const examsDiv = document.querySelector('.exams-div');
//         const pageContent = document.querySelector('.page-content');
//         pageContent.removeChild(examsDiv);
//     });


//     const pageIndicator = document.createElement("span");
//     pageIndicator.textContent = `Page: ${currentPage} / ${maxPages}`;
//     pageIndicator.classList.add("page-indicator");
//     pageIndicator.setAttribute("data-current-page", currentPage);
//     pageIndicator.setAttribute("data-max-pages", maxPages);

//     paginationDiv.appendChild(prevPageArrow);
//     paginationDiv.appendChild(entitiesPerPageSelect);
//     paginationDiv.appendChild(pageIndicator);
//     paginationDiv.appendChild(nextPageArrow);

//     return paginationDiv;
// }
