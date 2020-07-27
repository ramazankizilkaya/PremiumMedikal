
$(document).ready(function () {
    
    //$('#searchProName').autocomplete({
    //    source: '/Admin/SearchProductName'
    //});

    // popup tablarının hepsini valide etmek için
    $.validator.setDefaults({
        ignore: ""
    }); 

     
        $('.i-checks').iCheck({
            checkboxClass: 'icheckbox_square-green',
        radioClass: 'iradio_square-green',
    });
   

    //$.validator.unobtrusive.parse("#updateProductPopupForm");

    

    //Product Update Popup
    $("body").on("click", ".editPro", function () {
        var url = $(this).data("target");
        $.get(url, function (data) { })
            .done(function (data) {
                $("#updateProductPopup .modal-body").html(data);
                $.validator.unobtrusive.parse("#updateProductPopupForm");
                $("#updateProductPopup").modal("show");
            })
            .fail(function () {
                //modalımızın bodysine Error! yaz
                $("#updateProductPopup .modal-body").text("Hata!!");
                //sonra da modalimizi göster
                $("#updateProductPopup").modal("show");
            })
    });


    //Ürün sil butonu tıklama
    $('.dataTables-example tbody').on('click', 'button.deletePro', function () {
        debugger;

        var result = confirm("Ürün silinecek. Onaylıyor musunuz?");
        if (result) {
            var url = $(this).data("target");
            $.get(url, function (data) { })
                .done(function (data) {
                    if (data.success == true) {
                        toastr.success(data.message, 'Başarılı');

                        $(".dataTables-example tr").each(function () {
                            var rowSelector = $(this);

                            if (rowSelector.find("button.deletePro").attr('data-rowId') == data.proId) {
                                rowSelector.remove();
                            }
                        });
                    }

                    if (data.success == false) {
                        toastr.error(data.message, 'Hata');
                    }

                })
                .fail(function () {
                    toastr.error(data.message, 'Hata');
                })
        }

    });

    //Ürün silmeyi geri al butonu tıklama
    $('.dataTables-example tbody').on('click', 'button.undeletePro', function () {
        //debugger;

        var result = confirm("Ürünün silindi etiketi kaldırılacak. Onaylıyor musunuz?");
        if (result) {
            var url = $(this).data("target");
            $.get(url, function (data) { })
                .done(function (data) {
                    if (data.success == true) {
                        toastr.success(data.message, 'Başarılı');

                        $(".dataTables-example tr").each(function () {
                            var rowSelector = $(this);

                            if (rowSelector.find("button.undeletePro").attr('data-rowId') == data.proId) {
                                rowSelector.remove();
                            }
                        });
                    }

                    if (data.success == false) {
                        toastr.error(data.message, 'Hata');
                    }

                })
                .fail(function () {
                    toastr.error(data.message, 'Hata');
                })
        }

    });

    //Ürün kalıcı sil butonu tıklama
    $('.dataTables-example tbody').on('click', 'button.hardDeletePro', function () {
        debugger;

        var result = confirm("Ürünün kalıcı olarak db'den silinecek. Onaylıyor musunuz?");
        if (result) {
            var url = $(this).data("target");
            $.get(url, function (data) { })
                .done(function (data) {
                    if (data.success == true) {
                        toastr.success(data.message, 'Başarılı');

                        $(".dataTables-example tr").each(function () {
                            var rowSelector = $(this);

                            if (rowSelector.find("button.hardDeletePro").attr('data-rowId') == data.proId) {
                                rowSelector.remove();
                            }
                        });
                    }

                    if (data.success == false) {
                        toastr.error(data.message, 'Hata');
                    }

                })
                .fail(function () {
                    toastr.error(data.message, 'Hata');
                })
        }

    });


    

    //Product Delete on ProductList
    //$("body").on("click", ".deactivatePro", function () {
    //    debugger;
    //    var url = $(this).data("target");
    //    $.get(url, function (data) { })
    //        .done(function (data) {
    //            if (data.success==true) {
    //                toastr.success(data.message, 'Başarılı');

    //                var delayInMilliseconds = 1000; 
    //                setTimeout(function () {
    //                    window.location.href('/adminproduct/productlist/ActivateDeactivateProductPopup');
    //                }, delayInMilliseconds);
    //            }

    //            if (data.success == false) {
    //                toastr.error(data.message, 'Hata');
    //            }
                 
    //        })
    //        .fail(function () {
    //            toastr.error(data.message, 'Hata');
    //        })
    //});

    

    //i-check
    $('.i-checks').iCheck({
        checkboxClass: 'icheckbox_square-green',
        radioClass: 'iradio_square-green',
    });


    //Product popup Ana kategori değiştiğinde orta kategorileri doldurur
    //$("#SelectedTopCategoryId").change(function () {
    //    debugger;
    //    $.ajax({
    //        url: "/Admin/Product/GetMiddleCategoryList",
    //        dataType: 'json',
    //        type: "get",
    //        data: { 'id': $('#SelectedTopCategoryId').val() },
    //        success: function (data) {
    //            var items = '';
    //            $("#SelectedMiddleCategoryId").empty();
    //            $.each(data, function (i, row) {
    //                items += "<option value='" + row.value + "'>" + row.text + "</option>";
    //            });
    //            $("#SelectedMiddleCategoryId").html(items);
    //        }
    //    });
    //});


    //Orta kategori değiştiğinde alt kategorileri doldurur
    //$("#SelectedMiddleCategoryId").change(function () {
    //    debugger;
    //    $.ajax({
    //        url: "/Admin/Product/GetSubCategoryList",
    //        dataType: 'json',
    //        type: "get",
    //        data: { 'id': $('#SelectedMiddleCategoryId').val() },
    //        success: function (data) {
    //            var items = '';
    //            $("#SelectedSubCategoryId").empty();
    //            $.each(data, function (i, row) {
    //                items += "<option value='" + row.value + "'>" + row.text + "</option>";
    //            });
    //            $("#SelectedSubCategoryId").html(items);
    //        }
    //    });
    //});

    //Üst kategori değiştiğinde orta kategorileri doldurur
    $("body").on("change", ".topCat", function () {
        //debugger;
        //alert($('#SelectedTopCategoryId').val());
        $.ajax({
            url: "/Admin/Product/GetMiddleCategoryList",
            dataType: 'json',
            type: "get",
            data: { 'id': $('#SelectedTopCategoryId').val() },
            success: function (data) {
                var items = "<option style='color:red;' value=''>Lütfen Orta Kategori Seçiniz</option>";
                $("#SelectedMiddleCategoryId").empty();
                $.each(data, function (i, row) {
                    items += "<option value='" + row.value + "'>" + row.text + "</option>";
                });
                if (data.length != 0) {
                    $("#SelectedMiddleCategoryId").html(items);
                }
                $("#SelectedSubCategoryId").empty();
                $(".midCat").trigger("change");
            }
        });
    });

    //Orta kategori değiştiğinde alt kategorileri doldurur
    $("body").on("change", ".midCat", function () {
        //debugger;
        //alert($('#SelectedTopCategoryId').val());
        $.ajax({
            url: "/Admin/Product/GetSubCategoryList",
            dataType: 'json',
            type: "get",
            data: { 'id': $('#SelectedMiddleCategoryId').val() },
            success: function (data) {
                var items = "<option style='color:red;' value=''>Lütfen Alt Kategori Seçiniz</option>";
                $("#SelectedSubCategoryId").empty();
                $.each(data, function (i, row) {
                    items += "<option value='" + row.value + "'>" + row.text + "</option>";
                });
                if (data.length != 0) {
                    $("#SelectedSubCategoryId").html(items);
                }
            }
        });
    });

    //Ürün vitrin fotoğrafını siler
    $("body").on("click", ".delMainPhoto", function () {

        var result = confirm("Fotoğraf silinecek. Onaylıyor musunuz?");
        if (result) {
            var fotoId = $(this).attr('data-photoId');
            $.ajax({
                url: "/Admin/Product/DeleteMainPhoto",
                dataType: 'json',
                type: "get",
                data: { 'photoId': fotoId, proId: $(this).attr('data-proId') },
                success: function (response) {
                    if (response.success) {
                        toastr.success(response.message);
                        var myDivmyDiv = $('#updateProductPopupForm').find('[data-divId="' + fotoId + '"]')
                        myDivmyDiv.css("display", "none");
                    }
                    else {
                        toastr.warning(response.responseText);
                    }
                }
            });
        }
        
    });


    //Ürün ilave fotoğrafını siler
    $("body").on("click", ".delAdditionalPhoto", function () {
        var result = confirm("Fotoğraf silinecek. Onaylıyor musunuz?");
        if (result) {
            var fotoId = $(this).attr('data-photoId');
            $.ajax({
                url: "/Admin/Product/DeleteAdditionalPhoto",
                dataType: 'json',
                type: "get",
                data: { 'photoId': fotoId },
                success: function (response) {
                    if (response.success) {
                        toastr.success(response.message);
                        var myDivmyDiv = $('#updateProductPopupForm').find('[data-divId="' + fotoId + '"]')
                        myDivmyDiv.css("display", "none");
                    }
                    else {
                        toastr.warning(response.responseText);
                    }
                }
            });
        }
    });



    //Dosya input boxuna seçili dosya adedini yazar
    $(document).ready(function () {

        $("body").on("change", "#mainUpload", function () {

            var sdsd = document.getElementById("mainUpload");
            var fileNamee = sdsd.files.item(0).name;
            var fileLabell = document.getElementById('mainUploadLabel');
            fileLabell.innerHTML = fileNamee;
        });

        $("body").on("change", "#additionalUpload", function () {

            var sdsd = document.getElementById("additionalUpload");
            if (sdsd.files.length==1) {
                var fileNamee = sdsd.files.item(0).name;
                var fileLabell = document.getElementById('additionalUploadLabel');
                fileLabell.innerHTML = fileNamee;
            }
            else {
                var fileCount = sdsd.files.length;
                var fileLabell = document.getElementById('additionalUploadLabel');
                fileLabell.innerHTML = fileCount + ' dosya seçildi';
            }
            
        });



          

        //var uploadMainPhotoField = document.getElementById("mainUpload");

        //uploadMainPhotoField.onchange = function () {

        //    var fileNamee = this.files.item(0).name;
        //    var fileLabell = document.getElementById('mainUploadLabel');
        //    fileLabell.innerHTML = fileNamee;
        //}

        //document.getElementById("mainUpload").addEventListener("change", myFunction, false);

        //function myFunction() {
        //    var fileNamee = this.files.item(0).name;
        //    var fileLabell = document.getElementById('mainUploadLabel');
        //    fileLabell.innerHTML = fileNamee;
        //}

        






        //$('.custom-file-input').on("change", function () {
        //    debugger;
        //    var fileLabel = $(this).next('.custom-file-label');
        //    var files = $(this)[0].files;
        //    if (files.length > 1) {
        //        fileLabel.html(files.length + ' dosya seçildi');
        //    }
        //    else if (files.length == 1) {
        //        fileLabel.html(files[0].name);
        //    }
        //});
    });


    
    //$("body").on("click", "#uploadMainBtn", function () {
    //    debugger;

    //    var input = document.getElementById('mainUpload');
    //    var files = input.files;
    //    alert(files.length);
    //    alert(input.files[0].fileName);
        
    //    var formData = new FormData();
    //    formData.append("files", files[0]);


    //    $.ajax({
    //        processData: false,
    //        contentType: false,
    //        url: "/Admin/Product/SaveMainPhoto",
    //            dataType: 'json',
    //            type: "post",
    //            data: { 'file': formData, 'productId': input.getAttribute('data-urunId') },
    //        //data: { 'file': new FormData(document.getElementById("updateProductPopupForm")), 'productId': input.getAttribute('data-urunId') },
    //            success: function (response) {
    //                if (response.success) {
    //                    toastr.success(response.message);
    //                    $('#warnResultMainPhoto').html("<div class='alert alert-info' role='alert'>'" + response.message + "'</div>");
    //                }
    //                else {
    //                    toastr.error(response.message);
    //                    $('#warnResultMainPhoto').html("<div class='alert alert-info' role='alert'>'" + response.message + "'</div>");
    //                }
    //            }
    //        });
        
    //});

});




//$(document).ready(function () {

//    $('button[data-toggle="ajax-modal"]').click(function (event) {
//        debugger;
//        var url = $(this).data('url');
//        alert(url);
//        var popUpPlace = $('#modalPopupHere');
//        $.get(url).done(function (data) {

//            popUpPlace.html(data);
//            popUpPlace.find('modal').modal('show');
//        });
//    });


//});



