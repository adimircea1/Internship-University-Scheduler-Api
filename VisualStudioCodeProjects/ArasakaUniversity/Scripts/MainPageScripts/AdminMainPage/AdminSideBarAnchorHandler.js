import { DisplayHomeTableMenu } from './HomePageContentGenerator.js';
import { RemoveAddEntityMenu } from './RemoveEntityMenu.js';
import { RemoveTokenAsync } from '../../Logout.js';
import { DisplayInitialRegisterRequestTable } from './RegisterRequestTableGenerator.js';
import { DisplayRegisterHomeTableMenu } from './RegisterRequestTableMenuGenerator.js';

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
        console.log(directoryText);

        switch(directoryText){
            case "Home" : 
                DisplayHomeTableMenu();
                RemoveAddEntityMenu();
                break;
            case "Registration" :              
                DisplayRegisterHomeTableMenu();

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