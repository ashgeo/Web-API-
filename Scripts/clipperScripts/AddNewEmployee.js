//<script src="Scripts/jquery-1.10.2.min.js"></script>
//    <script src="Scripts/bootstrap.min.js"></script>
//    <script type="text/javascript">
//        var selectedRole = "";
//        $(document).ready(function () {

//        $('#linkClose').click(function () {
//            $('#divError').hide('fade');
//        });
//        $(".dropdown-menu li a").click(function () {

//            selectedRole = $(this).text();    
//        $(this).parents(".dropdown").find('.btn').html($(this).text() + ' <span class="caret"></span>');
//                $(this).parents(".dropdown").find('.btn').val($(this).data('value'));
//            });
//            $('#btnRegister').click(function () {
//            $.ajax({
//                url: '/api/Employees/Register',
//                method: 'POST',
//                data: {
//                    EmpID: $('#empID').val(),
//                    FirstName: $('#firstName').val(),
//                    LastName: $('#lastName').val(),
//                    EmpRole: selectedRole,
//                    Phone: $('#phone').val(),
//                    Email: $('#email').val()
//                },
//                success: function () {
//                    $('#successModal').modal('show');
//                },
//                error: function (jqXHR) {
//                    jsonValue = jQuery.parseJSON(jqXHR.status);
//                    $('#divErrorText').text(jqXHR.responseText);
//                    $('#divError').show('fade');
//                }
//            });                
//        });
//        });
//    </script>