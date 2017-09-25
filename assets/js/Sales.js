var uri = 'api/Sales';
$(document).ready(function () {
    $.getJSON(uri)
        .done(function (data) {
            $.each(data, function (key, item) {
                $('#salestable').append("<tr><td align='center'>" + item.SaleID + "</td>" + "<td align='center'>" + item.Date + "</td>" + "<td align='center'>" + item.FtposAmount + "</td>" + "<td align='center'>" + item.CashAmount + "</td>" + "<td align='center'>" + item.TotalSale + "</td>" + "<td align='center'>" + item.BlueBoxAmount + "</td>" + "<td align='center'>" + item.CashLeft + "</td ></tr>");
            });
        });
});

function formatItem(item) {
    return item.FirstName + '-' + item.EmpRole;
}
//Not using this function
//function find() {
//    var id = $('#empIDText').val();
//    $.getJSON(uri + '/' + id)
//        .done(function (data) {
//            $('#st').text(formatItem(data));
//        })
//        .fail(function (jqXHR, textStatus, err) {
//            $('#employees').text('error' + err);
//        })
//}

//Search
$("#search").on("keyup", function () {
    var value = $(this).val();

    $("table tr").each(function (index) {
        if (index !== 0) {

            $row = $(this);

            var id = $row.find("td:first").text();

            if (id.indexOf(value) !== 0) {
                $row.hide();
            }
            else {
                $row.show();
            }
        }
    });
});