import { DisplayInitialRegisterRequestTable } from "./RegisterRequestTableGenerator.js";

const pageContent = document.getElementById('pageContent');

//todo move this to another js file
export function DisplayRegisterHomeTableMenu(){
    pageContent.innerHTML = '';
    GenerateRegisterTableMenu();
};

function GenerateRegisterTableMenu(){
    const menu = document.createElement("div");
    menu.classList.add("table-generator-menu");
            
    const ul = document.createElement("ul");
    ul.classList.add("table-option");
            
    const entities = ["Student registrations", "Professor registrations"];
            
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
        
            DisplayInitialRegisterRequestTable();
        });
        li.appendChild(a);
        ul.appendChild(li);
    });
            
    const hr = document.createElement("hr");
    hr.classList.add("register-table-divider");

    menu.appendChild(ul);

    pageContent.appendChild(menu);
    pageContent.appendChild(hr);
    
    const tableDiv = document.createElement("div");
    tableDiv.classList.add('table-content');
    
    pageContent.append(tableDiv);
}