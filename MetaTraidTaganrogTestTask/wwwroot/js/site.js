var userTable; 
var bookTable;
$(document).ready(function () {
    loadBooksData();
    loadUsersData();
});

//добавить пользователя
$('#addClientButton').on('click', function () {
    $('#addClientFormContainer').fadeIn();
});

$('#submitClientButton').on('click', function () {
    var name = $('#clientName').val();
    var surname = $('#surname').val();
    $.ajax({
        url: '/Home/CreateClient',
        method: 'POST',
        data: {
            name: name,
            surname: surname
        },
        success: function (response) {
            $('#addClientFormContainer').fadeOut();
            loadUsersData();
        },
        error: function (error) {
            console.log('Произошла ошибка при добавлении клиента: ' + error);
        }
    });

});

//добавление книги
$('#addBookButton').on('click', function () {
    $('#addBookFormContainer').fadeIn();
});

$('#submitBookButton').on('click', function () {
    var name = $('#name').val();
    var description = $('#description').val();
    $.ajax({
        url: '/Home/CreateBook',
        method: 'POST',
        data: {
            name: name,
            description: description,
        },
        success: function (response) {
            $('#addBookFormContainer').fadeOut();
            loadBooksData();
        },
        error: function (error) {
            console.log('Произошла ошибка при добавлении книги: ' + error);
        }
    });
});

//переход к выбору пользователя для выдачи
$('#bookTable tbody').on('click', 'button.issue-btn', function () {
    var data1 = bookTable.row($(this).parents('tr')).data();
    var bookIdToIssue = data1[0];
    var bookNameToIssue = data1[1];
    $.ajax({
        url: '/Home/IssueBook?bookId=' + bookIdToIssue + '&bookName=' + bookNameToIssue,
        method: 'POST',
        dataType: 'html',
        success: function (html) {
            loadBooksData();
            window.location.href = '/Home/IssueBook?bookId=' + bookIdToIssue + '&bookName=' + bookNameToIssue; 
        },
    });
    
});
//выдача книги 
$('#userTable tbody').on('click', 'button.take-btn', function () {
    var dataToIssue2 = $('#userTable').DataTable().row($(this).parents('tr')).data();
    var userIdToIssue = dataToIssue2[0];
    var bookIdToIssue = $('#bookIdHiddenField').val();

    $.ajax({
        url: '/Home/IssueBookToUser?bookId=' + bookIdToIssue + '&userId=' + userIdToIssue,
        method: 'POST',
        dataType: 'html',
        success: function (html) {
            window.location.href = '/Home/Index';
        },
    });

});
//вернуть книгу
$('#bookTable tbody').on('click', 'button.return-btn', function () { 
    var dataToreturn = bookTable.row($(this).parents('tr')).data();
    var bookIdToReturn = dataToreturn[0];
    $.ajax({
        url: '/Home/ReturnBook?bookId=' + bookIdToReturn,
        method: 'POST',
        dataType: 'html',
        success: function (html) {
            window.location.href = '/Home/Index';
        },
    });
});

$('#bookTable tbody').on('click', 'button.edit-book-btn', function () {
        var dataToEdit = bookTable.row($(this).parents('tr')).data();
        var bookIdToEdit = dataToEdit[0];
        $.ajax({
            url: '/Home/EditBookView?bookId=' + bookIdToEdit,
            method: 'GET',
            dataType: 'html', // Corrected dataType
            success: function (html) {
                window.location.href = '/Home/EditBookView?bookId=' + bookIdToEdit;
            },
            error: function (error) {
                
                console.log('Error: ' + error);
            }
        });

});


//высвечивание списка книг
function loadBooksData() {
    if ($.fn.DataTable.isDataTable('#bookTable')) {
        $('#bookTable').DataTable().destroy();
    }
    $.ajax({
        url: '/Home/GetBooks',
        method: 'GET',
        dataType: 'json',

        success: function (data) {
            bookTable = $('#bookTable').DataTable({
                language: {
                    url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/ru.json',
                }
            });
            bookTable.clear().draw();
            data.data.forEach(function (book) {
                var status = book.IsTaken ? 'Выдана' : 'Свободна';
                var buttonText = book.IsTaken ? 'Вернуть' : 'Выдать';
                var buttonClass = book.IsTaken ? 'return-btn' : 'issue-btn';
                bookTable.row.add([
                    book.Id,
                    book.Name,
                    book.Description,
                    status,
                    '<button class="' + buttonClass + '">' + buttonText + '</button>',
                    '<button class="edit-book-btn">Редатировать описание</button>'
                ]).draw();
            });
        },
        error: function (error) {
            console.log('Произошла ошибка при получении данных с сервера: ' + error);
        }
    });
}

//список пользователей
function loadUsersData() {
    if ($.fn.DataTable.isDataTable('#userTable')) {
        $('#userTable').DataTable().destroy();
    }
    $.ajax({
        url: '/Home/GetUsers',
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            table = $('#userTable').DataTable({
                language: {
                    url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/ru.json',
                }
            });

            table.clear().draw();
            data.data.forEach(function (client) {
                table.row.add([
                    client.Id,
                    client.Name,
                    client.Surname,
                    '<button class="take-btn" data-bookid="@ViewBag.BookId">Выдать книгу</button>'
                ]).draw();
            });
        },
        error: function (error) {
            console.log('Произошла ошибка при получении данных с сервера: ' + error);
        }
    });
}
