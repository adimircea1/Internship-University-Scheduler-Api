import { GetAllExamsAsync, AddExamAsync, DeleteExamByIdAsync } from "../StudentMainPage/ExamEntity.js";
import { AddProblemAsync, DeleteProblemById, GetAllExamProblems } from "../ProfessorMainPage/ProblemEntity.js"; 


const tableHeaders = ["Id", "Course Id", "Available From", "Available Until", "Exam Duration", "Partial Grading", "Actions"];
const tableData = ["id", "courseId", "availableFrom", "availableUntil", "examDuration", "partialGradingAllowed"];

export async function FetchAndDisplayExams(numberOfEntitiesToShow, currentPage) {
    let examData = await GetAllExamsAsync(numberOfEntitiesToShow, currentPage, 1);
    await GenerateEntityTable(tableHeaders, tableData, examData);
}

async function GenerateEntityTable(headers, dataKeys, entities) {
    const tableContent = document.querySelector('.table-content');
    const tableContainer = document.createElement('div');
    tableContainer.className = 'tableContainer';

    tableContainer.style.padding = '10px';
    tableContainer.style.height = '18rem';
    tableContainer.style.maxHeight = '18rem';
    tableContainer.style.overflowY = 'auto'; 

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

    tableContainer.appendChild(table);

    tableContent.innerHTML = '';

    tableContent.appendChild(tableContainer);

    const buttonContainer = document.createElement('div');
    buttonContainer.style.display = 'flex';
    buttonContainer.style.justifyContent = 'center';
    buttonContainer.style.marginTop = '20px';

    const addButton = document.createElement('button');
    addButton.textContent = 'Add New Problem';
    addButton.style.padding = '10px 20px';
    addButton.style.width = '15%';
    addButton.style.textAlign = 'center';
    addButton.style.backgroundColor = '#3c2ebe';
    addButton.style.color = '#fff';
    addButton.style.border = 'none';
    addButton.style.cursor = 'pointer';
    addButton.style.borderRadius = '5px';

    addButton.addEventListener('mouseenter', () => {
        addButton.style.backgroundColor = '#170a86de';
    });
    addButton.addEventListener('mouseleave', () => {
        addButton.style.backgroundColor = '#3c2ebe';
    });

    addButton.addEventListener('click', async () => {
        await createExamModal();
    });

    buttonContainer.appendChild(addButton);
    tableContent.appendChild(buttonContainer);
}

function createExamModal() {
    const overlay = document.createElement('div');
    overlay.style.position = 'fixed';
    overlay.style.top = '0';
    overlay.style.left = '0';
    overlay.style.width = '100%';
    overlay.style.height = '100%';
    overlay.style.backgroundColor = 'rgba(0, 0, 0, 0.5)';
    overlay.style.zIndex = '1001';
    document.body.appendChild(overlay);

    const modal = document.createElement('div');
    modal.id = 'examModal';
    modal.className = 'modal';

    modal.style.position = 'fixed';
    modal.style.top = '50%';
    modal.style.left = '50%';
    modal.style.transform = 'translate(-50%, -50%)';
    modal.style.backgroundColor = '#fff';
    modal.style.padding = '20px';
    modal.style.boxShadow = '0 0 20px rgba(0, 0, 0, 0.3)';
    modal.style.zIndex = '1002';
    modal.style.width = '40%';
    modal.style.borderRadius = '8px';
    modal.style.display = 'none';

    const modalContent = document.createElement('div');
    modalContent.className = 'modal-content';

    const closeBtn = document.createElement('span');
    closeBtn.className = 'close-btn';
    closeBtn.innerHTML = '&times;';
    closeBtn.style.cursor = 'pointer';
    closeBtn.style.float = 'right';
    closeBtn.style.fontSize = '24px';
    closeBtn.addEventListener('click', () => {
        modal.style.display = 'none';
        overlay.remove();
    });

    const title = document.createElement('h2');
    title.innerText = 'Add Exam';
    title.style.marginBottom = '20px';
    title.style.textAlign = 'center';

    const form = document.createElement('form');
    form.id = 'examForm';

    const courseIdLabel = document.createElement('label');
    courseIdLabel.innerHTML = 'Course ID:';
    courseIdLabel.style.display = 'block';
    const courseIdInput = document.createElement('input');
    courseIdInput.type = 'number';
    courseIdInput.id = 'courseId';
    courseIdInput.name = 'courseId';
    courseIdInput.required = true;
    courseIdInput.style.width = '100%';
    courseIdInput.style.padding = '8px';
    courseIdInput.style.marginBottom = '10px';
    courseIdInput.style.border = '1px solid #ccc';
    courseIdInput.style.borderRadius = '4px';

    const durationLabel = document.createElement('label');
    durationLabel.innerHTML = 'Exam Duration (minutes):';
    durationLabel.style.display = 'block';
    const durationInput = document.createElement('input');
    durationInput.type = 'number';
    durationInput.id = 'examDuration';
    durationInput.name = 'examDuration';
    durationInput.required = true;
    durationInput.min = '0';
    durationInput.style.width = '100%';
    durationInput.style.padding = '8px';
    durationInput.style.marginBottom = '10px';
    durationInput.style.border = '1px solid #ccc';
    durationInput.style.borderRadius = '4px';

    const availableFromLabel = document.createElement('label');
    availableFromLabel.innerHTML = 'Available From:';
    availableFromLabel.style.display = 'block';
    const availableFromInput = document.createElement('input');
    availableFromInput.type = 'datetime-local';
    availableFromInput.id = 'availableFrom';
    availableFromInput.name = 'availableFrom';
    availableFromInput.required = true;
    availableFromInput.style.width = '100%';
    availableFromInput.style.padding = '8px';
    availableFromInput.style.marginBottom = '10px';
    availableFromInput.style.border = '1px solid #ccc';
    availableFromInput.style.borderRadius = '4px';

    const availableUntilLabel = document.createElement('label');
    availableUntilLabel.innerHTML = 'Available Until:';
    availableUntilLabel.style.display = 'block';
    const availableUntilInput = document.createElement('input');
    availableUntilInput.type = 'datetime-local';
    availableUntilInput.id = 'availableUntil';
    availableUntilInput.name = 'availableUntil';
    availableUntilInput.required = true;
    availableUntilInput.style.width = '100%';
    availableUntilInput.style.padding = '8px';
    availableUntilInput.style.marginBottom = '10px';
    availableUntilInput.style.border = '1px solid #ccc';
    availableUntilInput.style.borderRadius = '4px';

    const partialGradingContainer = document.createElement('div');
    partialGradingContainer.style.marginBottom = '15px';
    const partialGradingLabel = document.createElement('label');
    partialGradingLabel.style.display = 'flex'; 
    partialGradingLabel.style.alignItems = 'center'; 
    const partialGradingInput = document.createElement('input');
    partialGradingInput.type = 'checkbox';
    partialGradingInput.id = 'partialGrading';
    partialGradingInput.name = 'partialGrading';
    partialGradingInput.style.marginRight = '10px';
    const labelText = document.createTextNode('Allow partial grading');
    partialGradingLabel.appendChild(partialGradingInput);
    partialGradingLabel.appendChild(labelText);
    partialGradingContainer.appendChild(partialGradingLabel);

    const buttonsContainer = document.createElement('div');
    buttonsContainer.style.display = 'flex';
    buttonsContainer.style.justifyContent = 'center';
    buttonsContainer.style.marginTop = '20px';

    const addButton = document.createElement('button');
    addButton.type = 'submit';
    addButton.id = 'addExamBtn';
    addButton.innerText = 'Add Exam';
    addButton.style.padding = '10px 20px';
    addButton.style.marginRight = '10px';
    addButton.style.backgroundColor = '#3c2ebe';
    addButton.style.color = '#fff';
    addButton.style.border = 'none';
    addButton.style.borderRadius = '4px';
    addButton.style.cursor = 'pointer';

    addButton.addEventListener('click', async (event) => {
        event.preventDefault();

        let courseId = courseIdInput.value.trim();
        let examDuration = durationInput.value.trim();
        let availableFrom = availableFromInput.value;
        let availableUntil = availableUntilInput.value;
        let partialGrading = partialGradingInput.checked;

        if (!courseId || !examDuration || !availableFrom || !availableUntil) {
            alert('Please fill out all fields correctly.');
            return;
        }

        const newExam = {
            courseId: parseInt(courseId, 10),
            examDuration: parseInt(examDuration, 10),
            availableFrom: new Date(availableFrom).toISOString(),
            availableUntil: new Date(availableUntil).toISOString(),
            partialGradingAllowed: partialGrading
        };

        console.log(newExam);

        courseIdInput.value = '';
        durationInput.value = '';
        availableFromInput.value = '';
        availableUntilInput.value = '';
        partialGradingInput.checked = false;

        await AddExamAsync(newExam);

        modal.remove();
        overlay.remove();

        let tableContent = document.getElementsByClassName('table-content')[0];
        tableContent.innerHTML = '';
        await FetchAndDisplayExams();
    });

    addButton.addEventListener('mouseenter', () => {
        addButton.style.backgroundColor = '#170a86de';
    });
    addButton.addEventListener('mouseleave', () => {
        addButton.style.backgroundColor = '#3c2ebe';
    });

    const closeModalButton = document.createElement('button');
    closeModalButton.type = 'button';
    closeModalButton.id = 'closeModalBtn';
    closeModalButton.innerText = 'Close';
    closeModalButton.style.padding = '10px 20px';
    closeModalButton.style.backgroundColor = '#3c2ebe';
    closeModalButton.style.color = '#fff';
    closeModalButton.style.border = 'none';
    closeModalButton.style.borderRadius = '4px';
    closeModalButton.style.cursor = 'pointer';
    closeModalButton.addEventListener('click', () => {
        modal.style.display = 'none';
        overlay.remove();
    });

    closeModalButton.addEventListener('mouseenter', () => {
        closeModalButton.style.backgroundColor = '#170a86de';
    });
    closeModalButton.addEventListener('mouseleave', () => {
        closeModalButton.style.backgroundColor = '#3c2ebe';
    });

    buttonsContainer.appendChild(addButton);
    buttonsContainer.appendChild(closeModalButton);

    form.appendChild(courseIdLabel);
    form.appendChild(courseIdInput);
    form.appendChild(durationLabel);
    form.appendChild(durationInput);
    form.appendChild(availableFromLabel);
    form.appendChild(availableFromInput);
    form.appendChild(availableUntilLabel);
    form.appendChild(availableUntilInput);
    form.appendChild(partialGradingContainer);
    form.appendChild(buttonsContainer); 

    modalContent.appendChild(closeBtn);
    modalContent.appendChild(title);
    modalContent.appendChild(form);
    modal.appendChild(modalContent);

    document.body.appendChild(modal);

    modal.style.display = 'block';

    overlay.addEventListener('click', () => {
        modal.style.display = 'none';
        overlay.remove();
    });
}

async function handleDelete(examId) {
    try
    {
        await DeleteExamByIdAsync(examId);
        let tableContent = document.getElementsByClassName('table-content')[0];
        tableContent.innerHTML = '';
        await FetchAndDisplayExams();
    }
    catch
    {
        alert("Failed to delete exam!");
    }
}

async function handleUpdate(examId) {
    const overlay = document.createElement('div');
    overlay.style.position = 'fixed';
    overlay.style.top = '0';
    overlay.style.left = '0';
    overlay.style.width = '100%';
    overlay.style.height = '100%';
    overlay.style.backgroundColor = 'rgba(0, 0, 0, 0.5)'; 
    overlay.style.zIndex = '1001'; 

    overlay.addEventListener('click', () => {
        closeModal(modal);
    });

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

    const closeIcon = document.createElement('span');
    closeIcon.innerHTML = '&times;';
    closeIcon.style.cursor = 'pointer';
    closeIcon.style.fontSize = '24px';
    closeIcon.style.position = 'absolute';
    closeIcon.style.top = '10px';
    closeIcon.style.right = '20px';
    closeIcon.addEventListener('click', () => {
        closeModal(modal);
    });

    modal.appendChild(closeIcon);

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

    const displayProblems = async () => {
        const problems = await GetAllExamProblems(examId);
        table.innerHTML = ''; 
        table.appendChild(headerRow); 

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
            deleteIcon.addEventListener('click', async () => {
                await DeleteProblemById(problem.id); 
                await displayProblems(); 
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
    };

    await displayProblems();

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
        createProblemModal(examId, displayProblems);
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
        closeModal(modal); 
    });

    saveButton.addEventListener('mouseenter', () => {
        saveButton.style.backgroundColor = '#170a86de';
    });
    saveButton.addEventListener('mouseleave', () => {
        saveButton.style.backgroundColor = '#3c2ebe';
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
        closeButton.style.backgroundColor = '#170a86de';
    });
    closeButton.addEventListener('mouseleave', () => {
        closeButton.style.backgroundColor = '#3c2ebe';
    });

    buttonsContainer.appendChild(saveButton);
    buttonsContainer.appendChild(closeButton);

    modal.appendChild(buttonsContainer);

    document.body.appendChild(overlay); 
    document.body.appendChild(modal); 

    function closeModal(modal) {
        modal.remove();
        overlay.remove(); 
    }
}


function createProblemModal(examId, refreshProblemsCallback) {
    const overlay = document.createElement('div');
    overlay.style.position = 'fixed';
    overlay.style.top = '0';
    overlay.style.left = '0';
    overlay.style.width = '100%';
    overlay.style.height = '100%';
    overlay.style.backgroundColor = 'rgba(0, 0, 0, 0.5)'; 
    overlay.style.zIndex = '1001'; 
    document.body.appendChild(overlay);

    const modal = document.createElement('div');
    modal.id = 'problemModal';
    modal.className = 'modal';

    modal.style.position = 'fixed';
    modal.style.top = '50%';
    modal.style.left = '50%';
    modal.style.transform = 'translate(-50%, -50%)';
    modal.style.backgroundColor = '#fff';
    modal.style.padding = '20px'; 
    modal.style.boxShadow = '0 0 20px rgba(0, 0, 0, 0.3)';
    modal.style.zIndex = '1001'; 
    modal.style.width = '40%';
    modal.style.borderRadius = '8px';
    modal.style.display = 'none';

    const modalContent = document.createElement('div');
    modalContent.className = 'modal-content';

    const closeBtn = document.createElement('span');
    closeBtn.className = 'close-btn';
    closeBtn.innerHTML = '&times;';
    closeBtn.style.cursor = 'pointer';
    closeBtn.style.float = 'right';
    closeBtn.style.fontSize = '24px';
    closeBtn.addEventListener('click', () => {
        modal.style.display = 'none';
        overlay.remove(); 
    });

    const title = document.createElement('h2');
    title.innerText = 'Add Problem';
    title.style.marginBottom = '20px';
    title.style.textAlign = 'center';

    const form = document.createElement('form');
    form.id = 'problemForm';

    const textLabel = document.createElement('label');
    textLabel.innerHTML = 'Problem Text:';
    textLabel.style.display = 'block';
    const textInput = document.createElement('input');
    textInput.type = 'text';
    textInput.id = 'text';
    textInput.name = 'text';
    textInput.required = true;
    textInput.style.width = '100%';
    textInput.style.padding = '8px';
    textInput.style.marginBottom = '10px';
    textInput.style.border = '1px solid #ccc';
    textInput.style.borderRadius = '4px';

    const pointsLabel = document.createElement('label');
    pointsLabel.innerHTML = 'Points:';
    pointsLabel.style.display = 'block';
    const pointsInput = document.createElement('input');
    pointsInput.type = 'number';
    pointsInput.id = 'points';
    pointsInput.name = 'points';
    pointsInput.required = true;
    pointsInput.min = '0'; 
    pointsInput.style.width = '100%';
    pointsInput.style.padding = '8px';
    pointsInput.style.marginBottom = '10px';
    pointsInput.style.border = '1px solid #ccc';
    pointsInput.style.borderRadius = '4px';

    const problemTypeLabel = document.createElement('label');
    problemTypeLabel.innerHTML = 'Problem Type:';
    problemTypeLabel.style.display = 'block';
    const problemTypeSelect = document.createElement('select');
    problemTypeSelect.id = 'problemType';
    problemTypeSelect.name = 'problemType';
    problemTypeSelect.style.width = '100%';
    problemTypeSelect.style.padding = '8px';
    problemTypeSelect.style.marginBottom = '10px';
    problemTypeSelect.style.border = '1px solid #ccc';
    problemTypeSelect.style.borderRadius = '4px';

    const option1 = document.createElement('option');
    option1.value = 'MultipleAnswer';
    option1.innerText = 'Multiple Answer';
    const option2 = document.createElement('option');
    option2.value = 'SingleAnswer';
    option2.innerText = 'Single Answer';
    const option3 = document.createElement('option');
    option3.value = 'TextAnswer';
    option3.innerText = 'Text Answer';

    problemTypeSelect.appendChild(option1);
    problemTypeSelect.appendChild(option2);
    problemTypeSelect.appendChild(option3);

    const addButton = document.createElement('button');
    addButton.type = 'submit';
    addButton.id = 'addProblemBtn';
    addButton.innerText = 'Add Problem';
    addButton.style.padding = '10px 20px';
    addButton.style.marginRight = '10px';
    addButton.style.backgroundColor = '#3c2ebe';
    addButton.style.color = '#fff';
    addButton.style.border = 'none';
    addButton.style.borderRadius = '4px';
    addButton.style.cursor = 'pointer';

    addButton.addEventListener('click', async () => {
        event.preventDefault();

        let textInput = document.getElementById('text').value.trim();
        let pointsInput = document.getElementById('points').value;
        let typeInput = document.getElementById('problemType').value;

        if (!textInput || pointsInput === '' || parseInt(pointsInput, 10) < 0 || !typeInput) {
            alert('Please fill out all fields correctly.');
            return;
        }

        let newProblem = {
            text: textInput,
            points: parseInt(pointsInput, 10), 
            problemType: typeInput,
            examId: examId
        };

        console.log(newProblem);

        document.getElementById('text').value = '';
        document.getElementById('points').value = '';
        document.getElementById('problemType').value = 'MultipleAnswer'; 

        await AddProblemAsync(newProblem);

        modal.remove();
        overlay.remove(); 
        await refreshProblemsCallback();
    })

    addButton.addEventListener('mouseenter', () => {
        addButton.style.backgroundColor = '#170a86de';
    });
    addButton.addEventListener('mouseleave', () => {
        addButton.style.backgroundColor = '#3c2ebe';
    });

    const closeModalButton = document.createElement('button');
    closeModalButton.type = 'button';
    closeModalButton.id = 'closeModalBtn';
    closeModalButton.innerText = 'Close';
    closeModalButton.style.padding = '10px 20px';
    closeModalButton.style.backgroundColor = '#3c2ebe';
    closeModalButton.style.color = '#fff';
    closeModalButton.style.border = 'none';
    closeModalButton.style.borderRadius = '4px';
    closeModalButton.style.cursor = 'pointer';
    closeModalButton.addEventListener('click', () => {
        modal.style.display = 'none';
        overlay.remove(); 
    });

    closeModalButton.addEventListener('mouseenter', () => {
        closeModalButton.style.backgroundColor = '#170a86de';
    });
    closeModalButton.addEventListener('mouseleave', () => {
        closeModalButton.style.backgroundColor = '#3c2ebe';
    });

    form.appendChild(textLabel);
    form.appendChild(textInput);
    form.appendChild(pointsLabel);
    form.appendChild(pointsInput);
    form.appendChild(problemTypeLabel);
    form.appendChild(problemTypeSelect);
    form.appendChild(addButton);
    form.appendChild(closeModalButton); 

    form.addEventListener('submit', async (event) => {
        event.preventDefault();

        const problemData = {
            text: textInput.value,
            points: pointsInput.value,
            examId: examIdInput.value,
            problemType: problemTypeSelect.value
        };

        try {
            await AddProblemsAsync([problemData]);
            alert("Problem added successfully!");
            modal.style.display = 'none'; 
            overlay.remove(); 
        } catch (error) {
            console.error("Error adding problem:", error);
            alert("Failed to add problem.");
        }
    });

    modalContent.appendChild(closeBtn);
    modalContent.appendChild(title);
    modalContent.appendChild(form);
    modal.appendChild(modalContent);

    document.body.appendChild(modal);

    modal.style.display = 'block'; 
    overlay.addEventListener('click', () => {
        modal.style.display = 'none';
        overlay.remove();
    });
}

