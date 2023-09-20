import { DisplayInitialTable } from "./HomeEntityTableGenerator.js";
import { GenerateAddEntitySidebar } from "./AddEntityMenuGenerator.js";
import { RemoveAddEntityMenu } from "./RemoveEntityMenu.js";

const pageContent = document.getElementById('pageContent');

//todo move this to another js file
export function DisplayHomeTableMenu(){
    pageContent.innerHTML = '';
    GenerateAddEntityButton();
    GenerateHomeTableMenu();
};


function GenerateHomeTableMenu(){
    const menu = document.createElement("div");
    menu.classList.add("table-generator-menu");
            
    const ul = document.createElement("ul");
    ul.classList.add("table-option");
            
    const entities = ["Students", "Professors", "Admins", "Groups"];
            
    entities.forEach(entity => {
        const li = document.createElement("li");
        li.classList.add("option-element");

        const a = document.createElement("a");
        a.href = "#";
        a.textContent = entity;
                    
        li.addEventListener('click', async () =>{
            ul.querySelectorAll('.option-element a')
                .forEach(element => {
                    element.classList.remove('option-active'); 
                });

            a.classList.add('option-active');

            const subdirectorySpan = document.querySelector('.subdirectory');
            const subdirectoryText = li.textContent.trim();
    
            subdirectorySpan.textContent = `> ${subdirectoryText}`;
        
            DisplayInitialTable();

            let tableContent = document.querySelector('.table-content');

            tableContent.innerHTML = '';

            RemoveAddEntityMenu();
        });
        li.appendChild(a);
        ul.appendChild(li);
    });
            
    const hr = document.createElement("hr");
    hr.classList.add("table-divider");

    menu.appendChild(ul);

    pageContent.appendChild(menu);
    pageContent.appendChild(hr);
    
    const tableDiv = document.createElement("div");
    tableDiv.classList.add('table-content');
    
    pageContent.append(tableDiv);
}

function GenerateAddEntityButton() {
    const addButton = document.createElement("button");
    addButton.classList.add("add-entity-button");
    
    const plusIcon = document.createElement("i");
    plusIcon.classList.add("fas", "fa-plus");
    addButton.appendChild(plusIcon);

    const buttonTextSpan = document.createElement("span");
    buttonTextSpan.classList.add('add-button-text');
    
    addButton.appendChild(buttonTextSpan);

    addButton.addEventListener('click', () => {
        let tableData, requirements;
        switch(addButton.textContent){
            case ' Add a new student':
                tableData =  ["StudyYear", "FirstName", "LastName", "Email", "PhoneNumber", "BirthdayDate", "PersonalEmail"];
                requirements = ["required", "required", "required", "required", "not-required", "required", "not-required","required"];
                break;
            
            case ' Add a new professor':
                tableData =  ["FirstName", "LastName", "Email", "PhoneNumber", "Speciality", "BirthdayDate"];
                requirements = ["required", "required", "required", "not-required", "required", "not-required"];
                break;

            case ' Add a new admin':
                tableData =  ["UserName", "Email", "Password"];
                requirements = ["required", "required", "required"];
                break;

            case ' Add a new university group':
                tableData =  ["MaxSize", "Name", "Specialization"];
                requirements = ["required", "required", "required"];
                break;

            default:
                return;
        }

        GenerateAddEntitySidebar(addButton.textContent, tableData, requirements);    
    });

    pageContent.appendChild(addButton);
}






