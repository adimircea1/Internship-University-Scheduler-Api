import { GetAllExamsAsync } from "../StudentMainPage/ExamEntity.js";
import { GetAllExamProblems } from "../ProfessorMainPage/ProblemEntity.js"; 


const tableHeaders = ["Id", "CourseId", "Available From", "Available Until", "Exam Duration", "Actions"];
const tableData = ["id", "courseId", "availableFrom", "availableUntil", "examDuration"];

export async function FetchAndDisplayExams(numberOfEntitiesToShow, currentPage) {
    let examData = await GetAllExamsAsync(numberOfEntitiesToShow, currentPage, 1);
    await GenerateEntityTable(tableHeaders, tableData, examData);
}

async function GenerateEntityTable(headers, dataKeys, entities) {
    const tableContent = document.querySelector('.table-content');
    const table = document.createElement('table');

    table.style.width = '100%';
    table.style.borderCollapse = 'collapse';
    table.style.fontFamily = "'Arial', sans-serif";

    const headerRow = document.createElement('tr');
    headers.forEach(header => {
        const th = document.createElement('th');
        th.textContent = header;

        th.style.border = '1px solid #ddd';
        th.style.padding = '12px';
        th.style.backgroundColor = '#f2f2f2';
        th.style.textAlign = 'center';
        th.style.color = '#333';

        headerRow.appendChild(th);
    });
    table.appendChild(headerRow);

    entities.forEach(entity => {
        const row = document.createElement('tr');

        dataKeys.forEach(key => {
            const td = document.createElement('td');

            if (key === 'availableFrom' || key === 'availableUntil') {
                td.textContent = new Date(entity[key]).toLocaleString();
            } else {
                td.textContent = entity[key];
            }

            td.style.border = '1px solid #ddd';
            td.style.padding = '12px';
            td.style.textAlign = 'left';
            td.style.backgroundColor = '#fff';
            td.style.textAlign = 'center';
            row.appendChild(td);
        });

        const actionsTd = document.createElement('td');
        actionsTd.style.border = '1px solid #ddd';
        actionsTd.style.padding = '12px';
        actionsTd.style.textAlign = 'center';

        const updateIcon = document.createElement('i');
        updateIcon.classList.add('fas', 'fa-edit');
        updateIcon.style.cursor = 'pointer';
        updateIcon.style.color = '#4CAF50';
        updateIcon.style.margin = '0 5px';
        updateIcon.addEventListener('click', () => {
            console.log(`Update exam with Id: ${entity.id}`);
        });

        updateIcon.addEventListener('click', async () => {
            await handleUpdate(entity.id);  
        });
        updateIcon.addEventListener('mouseenter', () => {
            updateIcon.style.color = '#388E3C'; 
        });
        updateIcon.addEventListener('mouseleave', () => {
            updateIcon.style.color = '#4CAF50'; 
        });

        const deleteIcon = document.createElement('i');
        deleteIcon.classList.add('fas', 'fa-trash');
        deleteIcon.style.cursor = 'pointer';
        deleteIcon.style.color = '#f44336';
        deleteIcon.style.margin = '0 5px';
        deleteIcon.addEventListener('click', () => {
            console.log(`Delete exam with Id: ${entity.id}`);
        });

        deleteIcon.addEventListener('click', () => {
            handleDelete(entity.id);  
        });
        deleteIcon.addEventListener('mouseenter', () => {
            deleteIcon.style.color = '#d32f2f'; 
        });
        deleteIcon.addEventListener('mouseleave', () => {
            deleteIcon.style.color = '#f44336'; 
        });

        actionsTd.appendChild(updateIcon);
        actionsTd.appendChild(deleteIcon);
        row.appendChild(actionsTd);

        table.appendChild(row);
    });

    tableContent.innerHTML = '';
    tableContent.appendChild(table);
}


function handleDelete(examId) {
    console.log(`Delete exam with Id: ${examId}`);
}

async function handleUpdate(examId) {
    const modal = document.createElement('div');
    modal.style.position = 'fixed';
    modal.style.top = '50%';
    modal.style.left = '50%';
    modal.style.transform = 'translate(-50%, -50%)';
    modal.style.backgroundColor = '#fff';
    modal.style.padding = '40px'; 
    modal.style.boxShadow = '0 0 20px rgba(0, 0, 0, 0.2)';
    modal.style.zIndex = '1001'; 
    modal.style.width = '70%';
    modal.style.height = '80%'; 
    modal.style.display = 'flex';
    modal.style.flexDirection = 'column'; 

    const titleContainer = document.createElement('div');
    const title = document.createElement('h2');
    title.textContent = 'Problems';
    title.style.textAlign = 'center';
    title.style.marginBottom = '20px';
    titleContainer.appendChild(title);
    modal.appendChild(titleContainer);

    const problemsContainer = document.createElement('div');
    problemsContainer.style.display = 'flex';
    problemsContainer.style.flexDirection = 'column';
    problemsContainer.style.height = "80%";
    problemsContainer.style.overflowY = 'scroll'; 
    problemsContainer.style.border = '1px solid #ddd';
    problemsContainer.style.padding = '10px';
    problemsContainer.style.marginBottom = '20px'; 

    const table = document.createElement('table');
    table.style.width = '100%';
    table.style.borderCollapse = 'collapse';
    table.style.marginBottom = '20px';

    const headerRow = document.createElement('tr');
    ['ID', 'Requirement', 'Type', 'Points', 'Actions'].forEach(header => {
        const th = document.createElement('th');
        th.textContent = header;
        th.style.border = '1px solid #ddd';
        th.style.padding = '8px';
        th.style.backgroundColor = '#f2f2f2';
        th.style.textAlign = 'center';
        headerRow.appendChild(th);
    });
    table.appendChild(headerRow);

    const problems = await GetAllExamProblems(examId);
    problems.forEach(problem => {
        const row = document.createElement('tr');

        ['id', 'text', 'problemType', 'points'].forEach(key => {
            const td = document.createElement('td');
            td.textContent = problem[key];
            td.style.border = '1px solid #ddd';
            td.style.padding = '8px';
            td.style.textAlign = 'center';
            row.appendChild(td);
        });

        const actionsTd = document.createElement('td');
        actionsTd.style.border = '1px solid #ddd';
        actionsTd.style.padding = '8px';
        actionsTd.style.textAlign = 'center';

        const updateIcon = document.createElement('i');
        updateIcon.classList.add('fas', 'fa-edit');
        updateIcon.style.cursor = 'pointer';
        updateIcon.style.color = '#4CAF50';
        updateIcon.style.marginRight = '10px';
        updateIcon.addEventListener('click', () => {
            console.log(`Update problem with Id: ${problem.id}`);
        });

        updateIcon.addEventListener('mouseenter', () => {
            updateIcon.style.color = '#388E3C'; 
        });
        updateIcon.addEventListener('mouseleave', () => {
            updateIcon.style.color = '#4CAF50'; 
        });

        const deleteIcon = document.createElement('i');
        deleteIcon.classList.add('fas', 'fa-trash');
        deleteIcon.style.cursor = 'pointer';
        deleteIcon.style.color = '#f44336';
        deleteIcon.addEventListener('click', () => {
            console.log(`Delete problem with Id: ${problem.id}`);
        });

        deleteIcon.addEventListener('mouseenter', () => {
            deleteIcon.style.color = '#d32f2f'; 
        });
        deleteIcon.addEventListener('mouseleave', () => {
            deleteIcon.style.color = '#f44336'; 
        });

        actionsTd.appendChild(updateIcon);
        actionsTd.appendChild(deleteIcon);
        row.appendChild(actionsTd);

        table.appendChild(row);
    });

    problemsContainer.appendChild(table);

    const addButtonContainer = document.createElement('div');
    addButtonContainer.style.display = 'flex';
    addButtonContainer.style.justifyContent = 'center'; 
    addButtonContainer.style.marginTop = '20px';

    const addButton = document.createElement('button');
    addButton.textContent = 'Add New Problem';
    addButton.style.marginTop = '10px';
    addButton.style.padding = '10px 20px';
    addButton.style.width = '15%';
    addButton.style.textAlign = 'center';
    addButton.style.backgroundColor = '#3c2ebe';
    addButton.style.color = '#fff';
    addButton.style.border = 'none';
    addButton.style.cursor = 'pointer';
    addButton.style.borderRadius = '5px'; 
    addButton.addEventListener('click', () => {
        console.log('Add new problem clicked');
    });

    addButton.addEventListener('mouseenter', () => {
        addButton.style.backgroundColor = '#170a86de';
    });
    addButton.addEventListener('mouseleave', () => {
        addButton.style.backgroundColor = '#3c2ebe';
    });

    addButtonContainer.appendChild(addButton); 
    modal.appendChild(problemsContainer); 
    modal.appendChild(addButtonContainer); 

    const buttonsContainer = document.createElement('div');
    buttonsContainer.style.display = 'flex';
    buttonsContainer.style.justifyContent = 'center'; 
    buttonsContainer.style.marginTop = '20px'; 
    buttonsContainer.style.paddingTop = '20px'; 

    const saveButton = document.createElement('button');
    saveButton.textContent = 'Save';
    saveButton.style.padding = '10px 20px';
    saveButton.style.marginRight = '10px';
    saveButton.style.backgroundColor = '#3c2ebe';
    saveButton.style.color = '#fff';
    saveButton.style.border = 'none';
    saveButton.style.cursor = 'pointer';
    saveButton.style.borderRadius = '5px'; 
    saveButton.addEventListener('click', () => {
        console.log(`Saving changes for exam with Id: ${examId}`);
        closeModal(modal); 
    });

    saveButton.addEventListener('mouseenter', () => {
        addButton.style.backgroundColor = '#170a86de';
    });
    saveButton.addEventListener('mouseleave', () => {
        addButton.style.backgroundColor = '#3c2ebe';
    });

    const closeButton = document.createElement('button');
    closeButton.textContent = 'Close';
    closeButton.style.padding = '10px 20px';
    closeButton.style.marginLeft = '10px';
    closeButton.style.backgroundColor = '#3c2ebe';
    closeButton.style.color = '#fff';
    closeButton.style.border = 'none';
    closeButton.style.cursor = 'pointer';
    closeButton.style.borderRadius = '5px'; 
    closeButton.addEventListener('click', () => {
        closeModal(modal); 
    });

    closeButton.addEventListener('mouseenter', () => {
        addButton.style.backgroundColor = '#170a86de';
    });
    closeButton.addEventListener('mouseleave', () => {
        addButton.style.backgroundColor = '#3c2ebe';
    });

    buttonsContainer.appendChild(saveButton);
    buttonsContainer.appendChild(closeButton);

    modal.appendChild(buttonsContainer);

    const overlay = document.createElement('div');
    overlay.style.position = 'fixed';
    overlay.style.top = '0';
    overlay.style.left = '0';
    overlay.style.width = '100%';
    overlay.style.height = '100%';
    overlay.style.backgroundColor = 'rgba(0, 0, 0, 0.5)';
    overlay.style.zIndex = '1000';

    document.body.appendChild(overlay);
    document.body.appendChild(modal);

    function closeModal(modal) {
        modal.remove();
        overlay.remove();
    }
}
