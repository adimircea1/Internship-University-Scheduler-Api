import { InitialisePage } from "./ExamsPageGenerator.js";
import { RemoveTokenAsync } from "../../../Scripts/Logout.js";
import { GenerateInitialGradesTableAsync } from "./StudentGradesPage.js";
import { GenerateProfileAsync } from "./StudentProfileSection.js";
import { DisplayInitialAttendanceTable } from "./AttendancesTableGenerator.js";

const sideBar = document.getElementById('sidebar');
const menu = sideBar.querySelector('#menu');
const menuContent = menu.querySelectorAll('.menu-element');
const upperBarCurrentDirectory = document.querySelector('.upperBarCurrentDirectory');

menuContent.forEach(element => {
    element.addEventListener('click', async (event) => {

        const sidebarActiveElement = menu.querySelector('.sidebar-active');
        sidebarActiveElement.classList.remove('sidebar-active');
    
        const anchorElement = element.querySelector('a');
        anchorElement.classList.add('sidebar-active');
        
        const iconName = element.querySelector('i').classList.value;
        const directoryText = element.textContent.trim();

        upperBarCurrentDirectory.innerHTML = `<i class="${iconName}"></i> ${directoryText} <span class="subdirectory"></span>`;

        const pageContent = document.querySelector('.page-content');

        switch(directoryText){
            case "Exams" : 
                pageContent.innerHTML = '';
                await InitialisePage();
                break;
            case "Grades" :
                pageContent.innerHTML = '';
                await GenerateInitialGradesTableAsync();
                break;
            case "Profile" :
                pageContent.innerHTML = '';
                await GenerateProfileAsync();
                break;
            case "Attendance" :
                pageContent.innerHTML = '';
                const tableDiv = document.createElement("div");
                tableDiv.classList.add('table-content');
                pageContent.append(tableDiv);
                await DisplayInitialAttendanceTable();
                break;
            case "Logout" :
                pageContent.innerHTML = '';
                await RemoveTokenAsync();
                break;

            default :
                pageContent.innerHTML = '';
                break;
        }
    });

});