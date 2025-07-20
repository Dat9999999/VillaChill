$(document).ready(function () {
    loadNumberOfVilla();
});

function loadNumberOfVilla() {
    const card = document.querySelector('[data-owner-id]');
    if (!card) return;

    const ownerEmail = card.getAttribute('data-owner-id');
    
    $(".chart-spinner").show();

    $.ajax({
        url: `/Dashboard/GetNumberOfVilla/?ownerEmail=${ownerEmail}`,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            document.querySelector("#spanTotalVillaCount").innerHTML = data.count;

            $(".chart-spinner").hide();
        }
    });
}

