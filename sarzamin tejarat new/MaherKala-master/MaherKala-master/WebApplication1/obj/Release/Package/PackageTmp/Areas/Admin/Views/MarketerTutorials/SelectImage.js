
function setFile() {

    var myURL = window.URL || window.webkitURL;
    var fileURL = myURL.createObjectURL(Main_Image_Upload.files[0]);
    Main_Image_Show.src = fileURL;
}
function setImage2() {

    var myURL = window.URL || window.webkitURL;
    var result = "";
    for (var i = 0; i < product_images_upload.files.length; i++) {
        var fileURL = myURL.createObjectURL(product_images_upload.files[i]);
        var tag = "<img src='" + fileURL + "' style='width:150px;height:150px;margin:10px'>";
        result += tag;
    }
    $('#product_images').html(result);



}