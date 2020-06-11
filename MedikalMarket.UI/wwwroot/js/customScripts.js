//topCategory SelectList
$(document).ready(function () {

    $("#OrderBySelectt").change(function () {
        debugger;

        var selectedSortVal = $(this).children("option:selected").attr("data-val");
        var pathVal = $('#pageRouteVal').attr('data-url');

        //alert(selectedSortVal);
        //alert(pathVal);

        $.ajax({
            type: 'POST',
            url: "/Product/ChangeSort",
            data: { sortingen: selectedSortVal},
            success: function ( ) {
                window.location.href = pathVal;
            },
            fail: function ( ) {
                 
            }
        });


    });
});

//autocomplete search box
$(document).ready(function () {

    $('#AramaFormu').attr('autocomplete', 'on');

    $('#headerSearchText').autocomplete({
        source: '/Home/SearchGivenInput',
        appendTo: '.cacikkk',
    });

});

//search box submit form
$(document).ready(function () {

    $('#AramaFormu').submit(function (e) {
        debugger;
        e.preventDefault();
        var searchVal = $('#headerSearchText').val();
        if (searchVal == '' || searchVal == null || searchVal==undefined) {
            return false;
        }
        else {
            $.ajax({
                url: "/Home/FindSearchItem",
                type: 'POST',
                data: { 'searchText': searchVal },

                success: function (response) {

                    if (response.success) {
                        window.location.href = response.result;
                    }
                    else {
                        toastr.error(response.message);
                    }
                }

            }).fail(function () {
                toastr.error(response.message);
            });
        }

    });

});


//send password
//$(document).ready(function () {
//    $('#btnSendMyPwd').click(function (e) {
//        debugger;
//        var emailVal = $('#inputForgotEmail').val();
//        var warnWrongEmail = document.getElementById('warnwrongEmail').value;
//        var warnEmptyEmail = document.getElementById('warnEmptyEmail').value;
//        var warnemailVal = document.getElementById('warnemailVal').value;

//        var pattern = /^(([^<>()\[\]\.,;:\s@\"]+(\.[^<>()\[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;

//        var emailValidation = pattern.test(emailVal);

//        if (emailVal.length == 0) {
//            $('#warnResult').html("<div class='alert alert-danger' role='alert'>" + warnEmptyEmail + "</div>");
//            $('#Mail').focus();
//            return false;
//        }
//        if (!emailValidation) {
//            $('#warnResult').html("<div class='alert alert-danger' role='alert'>" + warnWrongEmail + "</div>");
//            $('#Mail').focus();
//            return false;
//        }
//        if (emailVal.length < 2 || emailVal.length > 100) {
//            $('#warnResult').html("<div class='alert alert-danger' role='alert'>" + warnemailVal + "</div>");
//            $('#Mail').focus();
//            return false;
//        }

//        $.ajax({
//            type: 'POST',
//            url: "/Account/SendMyPassword",
//            data: { userEmail: emailVal },
//            success: function (response) {
//                if (response.success) {
//                    $('#warnResult').html(response.responseText);
//                }
//                if (!response.success) {
//                    $('#warnResult').html(response.responseText);
//                }
//            }
//        }).fail(function () {
//            $('#warnResult').html(response.responseText);
//        });
//    });
//});

//delete favorite product
//$(document).ready(function () {
//    $('a.deleteFav').click(function (e) {
//        debugger;
//        var myData = $(this).attr("data-id");

//        $('tr#' + myData).css("background-color", "wheat");
//        var eleman = $('tr#' + myData);

//        $.ajax({
//            type: 'POST',
//            url: "/Product/DeleteFavoriteProduct",
//            data: { fpId: $(this).attr("data-id") },
//            success: function (response) {
//                if (response.success) {
//                    toastr.success(response.responseText);
//                    $(eleman).fadeOut('slow', function () {
//                        $(eleman).remove();
//                    });
//                }
//                if (!response.success) {
//                    toastr.error(response.responseText);
//                }
//            }
//        });





//        //$.ajax({
//        //    type: 'POST',
//        //    url: "/Product/DeleteFavoriteProduct",
//        //    data: { fpId: $(this).attr("data-id") },
//        //    dataType: 'json',
//        //    contentType: false,
//        //    processData: false,      
//        //    success: function (response) {

//        //        if (response.success) {
//        //            toastr.error(response.responseText);
//        //            $(eleman).fadeOut('slow', function () {
//        //                $(eleman).remove();
//        //            });
//        //        }
//        //        else {
//        //            toastr.error(response.responseText);
//        //        }
//        //    },
//        //    error: function (response) {
//        //        toastr.error("Hata oluştu. Error occured.");
//        //    }
//        //});
//    });
//});

//Update User Ajax
//$(document).ready(function () {
//    $('#userUpdateSubmit').click(function (e) {
//        debugger;
//        e.preventDefault();

//        var form = $('#UpdateUserForm');
//        $.validator.unobtrusive.parse(form);
//        form.validate();

//        var nameSurname = document.getElementById('AdSoyad').value;
//        var emailVal = document.getElementById('Mail').value;
//        var phoneVal = document.getElementById('Telefon').value;
//        var adresVal = document.getElementById('Adres').value;
//        var pwdVal = document.getElementById('Sifre').value;
//        var pwdVal2 = document.getElementById('SifreTekrar').value;
//        var kampanyaMail = document.getElementById('KampanyaMail').checked;
//        var kampanyaSMS = document.getElementById('KampanyaSms').checked;

//        var warnWrongEmail = document.getElementById('warnwrongEmail').value;
//        var warnEmptyEmail = document.getElementById('warnEmptyEmail').value;
//        var warnemailVal = document.getElementById('warnemailVal').value;
//        var warnnameSurname = document.getElementById('warnnameSurname').value;
//        var warnnameSurnameEmpty = document.getElementById('warnnameSurnameEmpty').value;
//        var warnpwdVal = document.getElementById('warnpwdVal').value;
//        var warnpwdValEmpty = document.getElementById('warnpwdValEmpty').value;
//        var warnpwdVal2 = document.getElementById('warnpwdVal2').value;

//        var pattern = /^(([^<>()\[\]\.,;:\s@\"]+(\.[^<>()\[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
//        var emailValidation = pattern.test(emailVal);

//        if (nameSurname.length == 0) {
//            toastr.error(warnnameSurnameEmpty);
//            $('#AdSoyad').focus();
//            return false;
//        }
//        if (nameSurname.length < 2 || nameSurname.length > 100) {
//            toastr.error(warnnameSurname);
//            $('#AdSoyad').focus();
//            return false;
//        }
//        if (emailVal.length == 0) {
//            toastr.error(warnEmptyEmail);
//            $('#Mail').focus();
//            return false;
//        }
//        if (!emailValidation) {
//            toastr.error(warnWrongEmail);
//            $('#Mail').focus();
//            return false;
//        }
//        if (emailVal.length < 2 || emailVal.length > 100) {
//            toastr.error(warnemailVal);
//            $('#Mail').focus();
//            return false;
//        }
//        if (pwdVal.length == 0) {
//            toastr.error(warnpwdValEmpty);
//            $('#Sifre2').focus();
//            return false;
//        }
//        if (pwdVal.length < 5 || pwdVal.length > 20) {
//            toastr.error(warnpwdVal);
//            $('#Sifre').focus();
//            return false;
//        }
//        if (pwdVal2 != pwdVal) {
//            toastr.error(warnpwdVal2);
//            $('#SifreTekrar').val('');
//            $('#SifreTekrar').focus();
//            return false;
//        }
//        if (form.valid()) {
//            $.ajax({
//                type: 'POST',
//                url: "/Account/UpdateUserInfo",
//                data: { email: emailVal, name: nameSurname, password: pwdVal, sms: kampanyaSMS, mail: kampanyaMail, address: adresVal, phoneNumber: phoneVal },
//                success: function (response) {
//                    if (response.success == false && response.info == "emailCrash") {

//                        toastr.error(response.responseText, response.title);
//                        $('#Mail').focus();
//                    }
//                    if (response.success == false && response.info == "updateFail") {
//                        toastr.error(response.responseText, response.title);
//                    }
//                    if (response.success == true && response.info == "updateSuccess") {
//                        toastr.success(response.responseText, response.title);
//                    }
//                }
//            });
//        }
//    });
//});

//favoriye ekleme
//$(document).ready(function () {
//    //debugger;
//    if ($('#ifFav').val()) {

//        $('#addToFav').html($('#remFav').val());
//    }
//    else {
//        $('#addToFav').html($('#addFav').val());
//    }

//    $("#addToFav").click(function () {
//        if ($('#userSessionInfo').val() == "no") {
//            toastr.warning($('#plzLoginWarn').val());
//        }
//        else {

//            $.ajax({
//                type: 'POST',
//                url: "/Product/AddRemoveToFavorites",
//                data: { productId: $('#singleProId').val() },
//                success: function (response) {
//                    if (response.success) {
//                        toastr.success(response.responseText, response.responseTitle);
//                        if (response.typO == "create") {
//                            $('#addToFav').html($('#remFav').val());
//                            $('#ifFav').val(true);
//                        }
//                        if (response.typO == "delete") {
//                            $('#addToFav').html($('#addFav').val());
//                            $('#ifFav').val(false)
//                        }
//                    }
//                    else {
//                        toastr.error(response.responseText, response.responseTitle);
//                    }
//                }
//            });
//        }
//    });
//});

//Eposta aboneliği
function validateNewsletter() {
    //debugger;

    var inputVal = document.getElementById("widget-subscribe-form-email").value;
    var pattern = /^(([^<>()\[\]\.,;:\s@\"]+(\.[^<>()\[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;

    var validation = pattern.test(inputVal);
    if (inputVal == '') {
        toastr.warning(document.getElementById("enterMail").value);
        return false;
    }

    if (!validation) {
        toastr.error(document.getElementById("enterValidMail").value);
        return false;
    }

    if (validation) {
        $.ajax({
            url: "/Home/AddToNewsletter",
            type: 'POST',
            data: { 'email': inputVal },

            success: function (response) {

                if (response.success) {
                    document.getElementById("widget-subscribe-form-email").value = "";
                    toastr.success(response.responseText);
                }
                else {
                    document.getElementById("widget-subscribe-form-email").value = "";
                    toastr.warning(response.responseText);
                }
            }

        }).fail(function () {
            toastr.error(document.getElementById("errorMail").value);
            document.getElementById("widget-subscribe-form-email").value = "";
        });
    }
    event.preventDefault();
}

//LoginCheck Ajax
//$(document).ready(function () {
//    $('#loginFormSubmit').click(function (e) {
//        //debugger;
//        e.preventDefault();

//        var form = $('#loginForm');
//        $.validator.unobtrusive.parse(form);
//        form.validate();

//        var emailVal = document.getElementById('inputEmail').value;
//        var pwdVal = document.getElementById('inputPassword').value;
//        var remember = document.getElementById('rememberMe').checked;

//        if (form.valid()) {
//            $.ajax({
//                type: 'POST',
//                url: "/Account/LoginCheck",
//                data: { email: emailVal, password: pwdVal, rememberMe: remember },
//                success: function (response) {
//                    if (response.success) {
//                        toastr.success(response.responseText, response.title);
//                        var delayInMilliseconds = 1000; 
                        
//                        setTimeout(function () {
//                            window.location.href = "/";
//                        }, delayInMilliseconds);
//                    }
//                    else {
//                        toastr.warning(response.responseText, response.title);
//                    }
//                }
//            });
//        }
//    });
//});

//Register Ajax
//$(document).ready(function () {
//    $('#formRegister').click(function (e) {
//        //debugger;
//        e.preventDefault();

//        var form = $('#RegisterForm');
//        $.validator.unobtrusive.parse(form);
//        form.validate();

//        var nameSurname = document.getElementById('AdSoyad').value;
//        var emailVal = document.getElementById('Mail2').value;
//        var pwdVal = document.getElementById('Sifre2').value;
//        var pwdVal2 = document.getElementById('SifreTekrar').value;
//        var kampanyaMail = document.getElementById('KampanyaMail').checked;
//        var kampanyaSMS = document.getElementById('KampanyaSms').checked;
//        var sozlesmeChecked = document.getElementById('Sozlesme').checked;

//        var warnWrongEmail = document.getElementById('warnwrongEmail').value;
//        var warnEmptyEmail = document.getElementById('warnEmptyEmail').value;
//        var warnsozlesmeChecked = document.getElementById('warnsozlesmeChecked').value;
//        var warnnameSurname = document.getElementById('warnnameSurname').value;
//        var warnnameSurnameEmpty = document.getElementById('warnnameSurnameEmpty').value;
//        var warnemailVal = document.getElementById('warnemailVal').value;
//        var warnpwdVal = document.getElementById('warnpwdVal').value;
//        var warnpwdValEmpty = document.getElementById('warnpwdValEmpty').value;
//        var warnpwdVal2 = document.getElementById('warnpwdVal2').value;

//        var pattern = /^(([^<>()\[\]\.,;:\s@\"]+(\.[^<>()\[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
//        var emailValidation = pattern.test(emailVal);

//        if (nameSurname.length == 0) {
//            toastr.error(warnnameSurnameEmpty);
//            $('#AdSoyad').focus();
//            return false;
//        }
//        if (nameSurname.length < 2 || nameSurname.length > 100) {
//            toastr.error(warnnameSurname);
//            $('#AdSoyad').focus();
//            return false;
//        }
//        if (emailVal.length == 0) {
//            toastr.error(warnEmptyEmail);
//            $('#Mail2').focus();
//            return false;
//        }
//        if (!emailValidation) {
//            toastr.error(warnWrongEmail);
//            $('#Mail2').focus();
//            return false;
//        }
//        if (emailVal.length < 2 || emailVal.length > 100) {
//            toastr.error(warnemailVal);
//            $('#Mail2').focus();
//            return false;
//        }
//        if (pwdVal.length == 0) {
//            toastr.error(warnpwdValEmpty);
//            $('#Sifre2').focus();
//            return false;
//        }
//        if (pwdVal.length < 5 || pwdVal.length > 20) {
//            toastr.error(warnpwdVal);
//            $('#Sifre2').focus();
//            return false;
//        }
//        if (pwdVal2 != pwdVal) {
//            toastr.error(warnpwdVal2);
//            $('#SifreTekrar').focus();
//            return false;
//        }
//        if (!sozlesmeChecked) {
//            toastr.error(warnsozlesmeChecked);
//            $('#Sozlesme').focus();
//            return false;
//        }
//        if (form.valid()) {
//            $.ajax({
//                type: 'POST',
//                url: "/Account/Register",
//                data: { email: emailVal, name: nameSurname, password: pwdVal, sms: kampanyaSMS, mail: kampanyaMail },
//                success: function (response) {
//                    if (response.success) {
//                        //toastr.success(response.responseText, response.title);
//                        window.location.href = response.togo;
//                    }
//                    else {
//                        toastr.warning(response.responseText, response.title);
//                    }
//                }
//            });
//        }
//    });
//});

//Contact Ajax
//$(document).ready(function () {
//    $('#contactSubmit').click(function (e) {
//        debugger;
//        e.preventDefault();

//        var form = $('#ContactForm');
//        $.validator.unobtrusive.parse(form);
//        form.validate();

//        var nameSurname = document.getElementById('AdSoyad').value;
//        var emailVal = document.getElementById('Mail').value;
//        var subject = document.getElementById('Konu').value;
//        var message = document.getElementById('Mesaj').value;
//        var kod = document.getElementById('guvenlikKodu').value;

//        //alert($('#myCodeKey').val());

//        var warnnameSurnameEmpty = document.getElementById('warnnameSurnameEmptyC').value;
//        var warnWrongEmail = document.getElementById('warnwrongEmailC').value;
//        var warnEmptyEmail = document.getElementById('warnEmptyEmailC').value;
//        var warnnameSurname = document.getElementById('warnnameSurnameC').value;
//        var warnemailVal = document.getElementById('warnemailValC').value;
//        var warnSubjectVal = document.getElementById('warnSubjectC').value;
//        var warnEmptySubject = document.getElementById('warnSubjectEmptyC').value;
//        var warnMessageVal = document.getElementById('warnMessageC').value;
//        var warnEmptyMessage = document.getElementById('warnMessageEmptyC').value;
//        var warnEmptySecurityCode = document.getElementById('warnCodeEmptyC').value;


//        var pattern = /^(([^<>()\[\]\.,;:\s@\"]+(\.[^<>()\[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
//        var emailValidation = pattern.test(emailVal);

//        if (nameSurname.length == 0) {
//            toastr.error(warnnameSurnameEmpty);
//            $('#AdSoyad').focus();
//            return false;
//        }
//        if (nameSurname.length < 2 || nameSurname.length > 100) {
//            toastr.error(warnnameSurname);
//            $('#AdSoyad').focus();
//            return false;
//        }
//        if (emailVal.length == 0) {
//            toastr.error(warnEmptyEmail);
//            $('#Mail').focus();
//            return false;
//        }
//        if (!emailValidation) {
//            toastr.error(warnWrongEmail);
//            $('#Mail').focus();
//            return false;
//        }
//        if (emailVal.length < 2 || emailVal.length > 100) {
//            toastr.error(warnemailVal);
//            $('#Mail').focus();
//            return false;
//        }
//        if (subject.length == 0) {
//            toastr.error(warnEmptySubject);
//            $('#Konu').focus();
//            return false;
//        }
//        if (subject.length < 2 || subject.length > 100) {
//            toastr.error(warnSubjectVal);
//            $('#Konu').focus();
//            return false;
//        }
//        if (message.length == 0) {
//            toastr.error(warnEmptyMessage);
//            $('#Mesaj').focus();
//            return false;
//        }
//        if (message.length < 20 || message.length > 4000) {
//            toastr.error(warnMessageVal);
//            $('#Mesaj').focus();
//            return false;
//        }

//        if (form.valid()) {
//            $.ajax({
//                type: 'POST',
//                url: "/Home/DecodeText",
//                data: { imageFileName: $('#myCodeKey').attr('data-Nam') },
//                success: function (response) {
//                    if (response.success) {
//                        if (response.result != kod) {
//                            toastr.error($('#warnCodeErrorC').val());
//                            kod.value = "";
//                        }
//                        else {
//                            $.ajax({
//                                type: 'POST',
//                                url: "/Home/SendContactUsMessage",
//                                data: { nameSurname: nameSurname, email: emailVal, subject: subject, message: message },
//                                success: function (response) {
//                                    if (response.success) {
//                                        window.location.href = "/";
//                                    }
//                                    else {
//                                        toastr.warning(response.responseText, response.title);
//                                    }
//                                }
//                            });
//                        }
//                    }
//                }
//            });
//        }
//    });
//});






/**
 * Shuffles array in place.
 * @param {Array} a items An array containing the items.
 */
function shuffle(a) {
    var j, x, i;
    for (i = a.length - 1; i > 0; i--) {
        j = Math.floor(Math.random() * (i + 1));
        x = a[i];
        a[i] = a[j];
        a[j] = x;
    }
    return a;
}

function getRandomInt(min, max) {
    min = Math.ceil(min);
    max = Math.floor(max);
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

$(document).ready(function () {
    $('.i-checks').iCheck({
        checkboxClass: 'icheckbox_square-green',
        radioClass: 'iradio_square-green',
    });
});



//kayıttan login ekranına geçiş
//$(document).ready(function () {
//    var succesElement = document.getElementById("registerSuccess");
//    if (succesElement != null) {
//        if (succesElement.value != "") {
//            toastr.success(registerSuccessTitle, succesElement.value);
//            succesElement.value == "";
//        }
//    }
//});






//$(document).ready(function () {


//    toastr.options = {
//        "closeButton": true,
//        "debug": false,
//        "progressBar": true,
//        "positionClass": "toast-top-right",
//        "onclick": null,
//        "showDuration": "400",
//        "hideDuration": "1000",
//        "timeOut": "10000",
//        "extendedTimeOut": "1000",
//        "showEasing": "swing",
//        "hideEasing": "linear",
//        "showMethod": "fadeIn",
//        "hideMethod": "fadeOut"
//    }
//});






