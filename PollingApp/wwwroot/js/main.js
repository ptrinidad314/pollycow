$(document).ready(() => {

    $('#createAddOption').on('click', () => {
        $('#createOptionUl').append("<li><input type='text' name='options' placeholder='Enter option...'></li>");
    });
});

