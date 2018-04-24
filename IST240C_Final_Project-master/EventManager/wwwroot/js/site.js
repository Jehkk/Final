$(document).ready(function ()
{
    // Wire up the Add button to send the new item to the server

    //Edit View Save Button
    $('#save-edit-button').on('click', function (e) {editItem(e.target);});

    //Create View Save Button
    $('#save-create-button').on('click',createEvent);});


//Handles the Create View Post Request
function createEvent()
{
    //Grabs Inputs
    var eventName = $('#event-name').val();
    var eventDescription = $('#event-description').val();
    var eventDate = $('#event-date').val();

    //Calls the CreateEvent ActionResult and Passes in required params to create model NewEventItem
    $.post('/Event/CreateEvent', { Name: eventName, Description: eventDescription, EventTime: eventDate })
        // uses the returned guid to post the GET the details view of that event
        .done(function (data) {
            window.location = '/event/' +data +'/details/';
        })
        //Updates the Error Label on the Create View
        .fail(function (data) {
            if (data && data.responseJSON) {
                var firstError = data.responseJSON[Object.keys(data.responseJSON)[0]];
                $('#add-item-error').text(firstError);
                $('#add-item-error').show();
            }
        });

}

//Handles the Edit View Post Request
function editItem(input) {

    //Grabs Inputs
    var eventName = $('#event-name').val();
    var eventDescription = $('#event-description').val();
    var eventDate = $('#event-date').val();

    //calles the EditItem ActionResult and Passes in the guid and the required params to create model NewEventItem
    $.post('/Event/EditItem', { id: input.name, Name: eventName, Description: eventDescription, EventTime: eventDate }, function () {
        window.location = '/event/' + input.name + '/details';
    })
        //Updates the Error Label on the Create View
        .fail(function (data) {
            if (data && data.responseJSON) {
                var firstError = data.responseJSON[Object.keys(data.responseJSON)[0]];
                $('#add-item-error').text(firstError);
                $('#add-item-error').show();
            }
        });
}