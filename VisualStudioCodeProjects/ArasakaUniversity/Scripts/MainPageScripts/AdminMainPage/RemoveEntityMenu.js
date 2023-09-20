export function RemoveAddEntityMenu(){
    const addEntitySidebar = document.querySelector('.add-entity-sidebar');
    const mainPageContent = document.querySelector('.main-page-content');
    if(addEntitySidebar !== null){
        mainPageContent.removeChild(addEntitySidebar);
    }
}