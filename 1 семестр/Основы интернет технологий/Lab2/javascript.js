let button = document.querySelectorAll('.addTaskButton');
let newTask = document.querySelectorAll('.taskForm');
let emptyList = document.querySelectorAll('.emptyList');
const addForm = document.querySelector('.addForm');
let taskBloks = document.querySelectorAll('.taskBlog');
let current;
let idTaskForm = 0;
let names = ['Новые задачи', 'В работе', 'Завершены'];
let idNewTask = 0;
const form = document.querySelector('#formMain');

form.addEventListener('dragstart', function(event) {
    if (event.target.getAttribute('data-action') == 'button')
    {
        current = event.target;
    }
})

form.addEventListener('dragover', function(event) {
    if (event.target.getAttribute('data-action') == 'body')
    {
        event.preventDefault();
    }
})

form.addEventListener('drop', function(event) {
    if (event.target.getAttribute('data-action') == 'body' || event.target.getAttribute('data-action') == 'button')
    {
        event.target.appendChild(current);
        newTask = document.querySelectorAll('.taskForm');
        emptyList = document.querySelectorAll('.emptyList');
        EmptyList();
        localStorage.clear();                        
        localStorage.setItem('all', form.innerHTML); 
    }
})

addForm.addEventListener('click', function(event) 
{
    event.preventDefault();
    let nameForm = prompt('Введите название колонки');
    CreateColumn(nameForm);
    localStorage.clear();                        
    localStorage.setItem('all', form.innerHTML);              //SAVE
})

function CreateColumn(nameForm) {
    const htmlForm = 
    `<form class="form">

        <div class="headerForm">
            <p class="header"> ${nameForm} </p>
            <button onclick="return false" data-action="delete-column" class="close">
                <img src="картинки/x.png" alt="Удалить" data-action="delete-column" class="closePicture">
            </button>
                            
        </div>
                        
        <div class="bodyForm">

            <div class="taskForm" data-action="body" id="${idTaskForm}">

                <div class="emptyList" id="emptyList">
                    <p class="emptyMarc"> Список пуст </p>
                </div>

            </div>

            <div class="addTask">
                <button onclick="return false" data-action="add-task" class="addTaskButton">
                    <p data-action="add-task" class="addTaskMark"> Добавить задачу </p>
                </button>
            </div>
                                    
        </div>
    </form>`
    idTaskForm++;
    form.insertAdjacentHTML('beforeend', htmlForm);
    button = document.querySelectorAll('.addTaskButton');
    newTask = document.querySelectorAll('.taskForm');
    localStorage.clear();                             //СОЗДАТЬ СНАЧАЛА
    localStorage.setItem('all', form.innerHTML);      //SAVE
}

form.innerHTML = localStorage.getItem('all');

function ShowAll() {
    localStorage.clear();
    localStorage.setItem('all', form.innerHTML);
    form.innerHTML = localStorage.getItem('all');
}

ShowAll();

form.addEventListener('click', function(event)
{
    if (event.target.getAttribute('data-action') == 'delete-task')
    {
        if (confirm("Вы действительнно хотите удалить задачу?"))
        {
            event.target.closest('div').remove();
            //event.currentTarget.remove();
            EmptyList();
            localStorage.clear();
            localStorage.setItem('all', form.innerHTML);   
        }
    }

    else if (event.target.getAttribute('data-action') == 'add-task')
    {
        let nameTask = prompt('Введите название задачи:');
        let htmlTask = `<div class="taskBlog" draggable="true" id="${idNewTask}" data-action="button">
        <p class="task"> ${nameTask} </p>
            <button onclick="return false" data-action="delete-task" class="deleteTask">
                 <img src="картинки/x.png" alt="Удалить" data-action="delete-task" class="closePicture">
            </button>
        </div>`;
        event.target.closest('.bodyForm').querySelector('.taskForm').insertAdjacentHTML('beforeend', htmlTask);
        taskBloks = document.querySelectorAll('.taskBlog');
        newTask = document.querySelectorAll('.taskForm');
        idNewTask++;
        EmptyList();
        localStorage.clear();
        localStorage.setItem('all', form.innerHTML); 
    }

    else if (event.target.getAttribute('data-action') == 'delete-column')
    {
        if (confirm("Вы действительнно хотите удалить колонку?"))
        {
            event.target.closest('form').querySelector('.taskForm').remove();
            event.target.closest('form').querySelector('.addTaskButton').remove();
            event.target.closest('form').remove();
            button = document.querySelectorAll('.addTaskButton');
            newTask = document.querySelectorAll('.taskForm');
            localStorage.clear();
            localStorage.setItem('all', form.innerHTML);
        }
    }
})

function EmptyList() {
    let form = document.querySelectorAll('.form');
    emptyList = document.querySelectorAll('.emptyList');
    newTask = document.querySelectorAll('.taskForm');
    for (i = 0; i < form.length; i++)
    {
        if (newTask[i].children.length > 1)
            emptyList[i].style.display = "none";
        else
            emptyList[i].style.display = "block";
    }   
}