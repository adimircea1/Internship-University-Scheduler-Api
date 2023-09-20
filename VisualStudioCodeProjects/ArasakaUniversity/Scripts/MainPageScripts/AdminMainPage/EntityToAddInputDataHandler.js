
export function GetEntityInputData(){
    const addEntitySidebar = document.querySelector('.add-entity-sidebar');
    const addEntitySidebarText = addEntitySidebar.querySelector('.add-entity-sidebar-message');
    console.log(addEntitySidebarText.textContent);
    switch(addEntitySidebarText.textContent){
        case ' Add a new student':
            let studentToAdd = [];
            const studentProperties = ["StudyYear", "FirstName", "LastName", "Email", "PhoneNumber", "BirthdayDate", "PersonalEmail"];
            studentProperties.forEach(property => {
                const inputTextBox = addEntitySidebar.querySelector(`.${property}`);
                studentToAdd[property] = inputTextBox.value;
            });
            return studentToAdd;
            
        case ' Add a new professor':
            let professorToAdd = [];
            const professorProperties = ["Speciality", "FirstName", "LastName", "Email", "PhoneNumber", "BirthdayDate"];
            professorProperties.forEach(property => {
                const inputTextBox = addEntitySidebar.querySelector(`.${property}`);
                professorToAdd[property] = inputTextBox.value;
            });
            return professorToAdd;

        case ' Add a new admin':
            let adminToAdd = [];
            const adminProperties = ["UserName", "Email", "Password"];
            adminProperties.forEach(property => {
                const inputTextBox = addEntitySidebar.querySelector(`.${property}`);
                adminToAdd[property] = inputTextBox.value;
            });
            return adminToAdd;
        
        case ' Add a new university group':
            let groupToAdd = [];
            const groupProperties = ["MaxSize", "Name", "Specialization"];
            groupProperties.forEach(property => {
                const inputTextBox = addEntitySidebar.querySelector(`.${property}`);
                groupToAdd[property] = inputTextBox.value;
            });
            return groupToAdd;
        default:
            return;
    }
    
}


