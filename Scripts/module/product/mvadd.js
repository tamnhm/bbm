var Products = Products || {};
Products.mvadd_product = function () {
    var self = this;
    self.danhmucsp = ko.observableArray();
    self.autourl = ko.observable(true);
    self.mProduct = ko.observable(new Products.mProduct());
    self.ischeckshow = ko.observable();
    self.ischecksaleoff = ko.observable();
    self.HasError = ko.observable(false);
    self.ErorAray = ko.observableArray();
    self.ischecksaleoff.subscribe(function (val) {
        if (val) { self.mProduct().ischecksaleoff(true); }
        else { self.mProduct().ischecksaleoff(false); }
        self.mProduct().giakm("");
    });
    self.ischeckshow.subscribe(function (val) {
        if (val) {
            self.mProduct().ischeckshow(false);
        }
        else {
            self.mProduct().ischeckshow(true);
        }
    });
    self.mProduct().tensp.subscribe(function (value) {
        debugger
        if (self.autourl()) {
            var str = value;
            str = str.toLowerCase();
            str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ  |ặ|ẳ|ẵ/g, "a");
            str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
            str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
            str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ  |ợ|ở|ỡ/g, "o");
            str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
            str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
            str = str.replace(/đ/g, "d");
            str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g, "-");
            /* tìm và thay thế các kí tự đặc biệt trong chuỗi sang kí tự - */
            str = str.replace(/-+-/g, "-"); //thay thế 2- thành 1-
            str = str.replace(/^\-+|\-+$/g, "");
            //cắt bỏ ký tự - ở đầu và cuối chuỗi 
            self.mProduct().spurl(str);
        }
    });
    self.autourl.subscribe(function (value) {
        self.autourl(value);
    });
    self.submit = function (data) {
        debugger
        self.Validator = ko.validatedObservable(data);
        self.ErorAray.removeAll();
        ko.utils.arrayForEach(self.Validator().errors(), function (it) {
            self.ErorAray.push(it);
        });
        if (self.Validator().isValid()) {

            $.ajax({
                type: 'POST',
                url: CommonUtils.url('/Admin/Product/AddNew'),
                cache: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: ko.toJSON(data),

            });
            toastr.success("Thành công");
        } else {

            self.HasError(true);

        }
    };
    self.loaddanhmuc = function (){
    };
    self.Start = function () {

        ko.applyBindings(self);
    };
};