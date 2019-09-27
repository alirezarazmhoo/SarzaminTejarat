function numberWithCommas(x) {
    var tmp = x.value;
    while (tmp.indexOf(',') != -1)
        tmp = tmp.replace(',', '');
    var parts = tmp.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    x.value = parts.join(".");
}