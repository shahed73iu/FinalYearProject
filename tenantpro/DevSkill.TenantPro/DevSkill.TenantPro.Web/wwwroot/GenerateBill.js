

$('#btnAddTripExpt').on('click', function () {

    $(".chosen_select_L").chosen('destroy');

    var num = $('.clonedInputTripExpt').length,

        newNum = new Number(num + 1),
        newRow = new Number(num),
        newElem = $('#entryTripExpt' + num).clone().attr('id', 'entryTripExpt' + newNum).fadeIn('slow');

    var newIda = $('#entryTripExpt' + num).find('.Sl').html(),
        newId = parseInt(newIda) + 1;
    newElem.find('.Name').parent().attr('for', 'ID' + newId + 'Name');//Printings[0].Format
    newElem.find('.Name').attr('id', 'ID' + newId + 'Name').val('').attr('name', 'Printings[' + newIda + '].BillFormat').val('');

    $('#entryTripExpt' + num).after(newElem);
    $('#entryTripExpt' + newNum).focus();

    $('#btnDelTripExpt').attr('disabled', false);

    newElem.find('.Sl').html(newNum);
    $(".chosen_select_L").chosen();
});


$(document).on('click', '.btnDelTripExpt', function () {

    event.preventDefault();

    var num = $('.clonedInputTripExpt').length,

        newNum = new Number(num);

    if (newNum == 1) {
        $('.btnDelTripExpt').attr('disabledt', true);
        alert("Minimum fill one row!!");
    }
    else {
        var tr = $(this).closest('tr');
        tr.addClass("bg-danger");
        tr.fadeOut(500, function () {
            var table = tr.closest('table');
            tr.remove();
            updateIndexesExp(table);
        });
    }

});
        // Update Clond Input fields Indexes

function updateIndexesExp(table) {

    table.find("tr:gt(0)").each(function (i, row) {
        $(row).find("td:first").html(i + 1);
        var id = i + 1;

        $(row).find('.Name').attr('name', 'Printings[' + id + '].BillFormat');
        $(row).attr('id', 'entryTripExpt' + id);

        $(row).find('input, select').each(function (j, input) {

            var id = input.name.match(/\d+/);
            if (id != null && id.length && id.length == 1) {
                var attr = $(input).attr("name");
                var newName = attr.replace(attr.match(/\d+/), i);
                $(input).attr("name", newName);
            }
        });


    });
}


$('#tenantTable').on('click', '.bill', function (event) {
    var id = $(this).data("id");

    var modal = $("#modal-default2");
    var e, r, s;
    modal.find('.modal-body p').text('Please select the bill type you want to generate.');
    var obj = {
        tenantId: id
    };
    $.ajax({
        type: "GET",
        url: '/Admin/Tenant/GetPrintingsByTenant',
        dataType: "json",
        data: obj,
        success: function (data) {
            if (data == "0") {

            } else {

                var arr = JSON.parse(data);


                for (var i = 1; i < arr.length; i++) {

                    $(".chosen_select_L").chosen('destroy');

                    var num = $('.clonedInputTripExpt').length,

                        newNum = new Number(num + 1),
                        newRow = new Number(num),
                        newElem = $('#entryTripExpt' + num).clone().attr('id', 'entryTripExpt' + newNum).fadeIn('slow');

                    var newIda = $('#entryTripExpt' + num).find('.Sl').html(),
                        newId = parseInt(newIda) + 1;
                    newElem.find('.Name').parent().attr('for', 'ID' + newId + 'Name');//Printings[0].Format
                    newElem.find('.Name').attr('id', 'ID' + newId + 'Name').val('').attr('name', 'Printings[' + newIda + '].BillFormat').val('');
                    $('#entryTripExpt' + num).after(newElem);
                    $('#entryTripExpt' + newNum).focus();

                    $('#btnDelTripExpt').attr('disabled', false);

                    newElem.find('.Sl').html(newNum);
                    $(".chosen_select_L").chosen();


                }
                for (var i = 0; i < arr.length; i++) {
                    var id = 'ID'.concat((i + 1)).concat('Name');
                    var element = document.getElementById(id);

                    for (var j = 0; j < arr[i].length; j++) {
                        for (var k = 1; k < element.options.length; k++) {

                            if (element.options[k].value == arr[i][j]) {
                                element.children[k].setAttribute("selected", "selected");
                            }
                        }


                    }
                }

                $(".chosen_select_L").chosen('destroy');

                var num = $('.clonedInputTripExpt').length,

                    newNum = new Number(num + 1),
                    newRow = new Number(num),
                    newElem = $('#entryTripExpt' + num).clone().attr('id', 'entryTripExpt' + newNum).fadeIn('slow');

                var newIda = $('#entryTripExpt' + num).find('.Sl').html(),
                    newId = parseInt(newIda) + 1;
                newElem.find('.Name').parent().attr('for', 'ID' + newId + 'Name');//Printings[0].Format
                newElem.find('.Name').attr('id', 'ID' + newId + 'Name').val('').attr('name', 'Printings[' + newIda + '].BillFormat').val('');

                $('#entryTripExpt' + num).after(newElem);
                $('#entryTripExpt' + newNum).focus();

                $('#btnDelTripExpt').attr('disabled', false);

                newElem.find('.Sl').html(newNum);
                $(".chosen_select_L").chosen();


                event.preventDefault();

                var num = $('.clonedInputTripExpt').length,

                    newNum = new Number(num);
                $('#entryTripExpt' + newNum).remove();


            }
        },

        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert('internal server error'); }
        },
        error: function (req, status, errorObj) {

        }
    });

    $("#billId").val(id);
    $('#billButton').prop('disabled', true);
    modal.modal('show');

});



