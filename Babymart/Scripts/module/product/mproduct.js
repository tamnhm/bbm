var Products = Products || {};
Products.mProduct = function () {
    var self = this;
    self.id = ko.observable();
    self.masp = ko.observable();
    self.maloai = ko.observable();
    self.madanhmuc = ko.observable();
    self.madanhmuccon = ko.observable();
    self.mahieu = ko.observable();
    self.hide = ko.observable(false);
    self.ischecksaleoff = ko.observable(false);
    self.ischeckout = ko.observable(false);
    self.ischeckgift = ko.observable(false);
    self.tensp = ko.observable().extend({
        required: { params: true, message: "Tên sản phẩm không được bỏ trống" }
    });
    self.tensp_us = ko.observable();
    
    self.chitiet = ko.observable();
    self.thongtin = ko.observable();
    self.chitiet_us = ko.observable();
    self.thongtin_us = ko.observable();

    self.spurl = ko.observable();
    self.spurl_us = ko.observable();
    self.tensp_us.subscribe(function (value) {
        if (value) {
            self.spurl_us(CommonUtils.NonUnicode(value));
            self.sptitle_us(value);
        }
    });
    self.tensp.subscribe(function (value) {
        if (value) {
            self.spurl(CommonUtils.NonUnicode(value));
            self.sptitle(value);
        }
    });
    self.spkeywords = ko.observable();
    self.spkeywords_us = ko.observable();
    self.spdescription = ko.observable();
    self.spdescription_us = ko.observable();
    self.sptitle = ko.observable();
    self.sptitle_us = ko.observable();

    self.gift = ko.observable();
    //    .extend({

    //    required: {
    //        params: true, message: "Quà tặng không được bỏ trống",
    //        onlyIf: function () { return (self.ischecksaleoff() && self.ischeckgift()); }
    //    }
    //});

    self.timeend = ko.observable();

    //    .extend({
    //    required: {
    //        params: true, message: "Ngày kết thúc không được bỏ trống",
    //        onlyIf: function () { return (self.ischecksaleoff()); }
    //    },
    //});
    self.img = ko.observable();
    self.GetLinkPageDetail = ko.computed(function () {
        return CommonUtils.url('product#/edit/' + encodeURIComponent(self.id()));

    });
    self.shop_bienthe = ko.observableArray();
    self.shop_image = ko.observableArray();
    self.kg = ko.observable();
    //self.PublishDatePicker = ko.observable(new Date());
    //self.PublishDate = ko.observable();
    //self.timestart.subscribe(function (value) {
    //    if (value && value.constructor != Date)
    //        self.PublishDatePicker(new Date(parseInt(value.substr(6))));
    //    else
    //        self.PublishDatePicker(new Date());
    //});
    self.chieudai = ko.observable();
    self.chieucao = ko.observable();
    self.chieurong = ko.observable();
    self.chieudaisd = ko.observable();
    self.chieucaosd = ko.observable();
    self.chieurongsd = ko.observable();
    self.showkg = ko.observable();
    self.showcm = ko.observable();
    self.showhome = ko.observable(false);
    self.showbanner = ko.observable(false);
    self.showsptangkemvaomuckm = ko.observable(false);
    self.plantype = ko.observable();
    self.ListTags = ko.observableArray();
    self.ListTagsArticle = ko.observableArray();
    self.Tags = ko.observableArray();
};

Products.mbienthe = function () {
    var self = this;
    self.id = ko.observable();
    self.idsp = ko.observable();
    self.title = ko.observable('default').extend({
        required: { params: true, message: "Tên biến thể không được bỏ trống" }
    });
    self.title_us = ko.observable();
    self.tmpgia = ko.observable();
    self.gia = ko.observable(0).extend({
        required: { params: true, message: "Giá sản phẩm không được bỏ trống" },
        number: { params: true, message: "Giá sản phẩm chỉ được nhập số." }
    });
    self.giasosanh = ko.observable(0).extend({
        required: { params: true, message: "Giá sản phẩm không được bỏ trống" },
        number: { params: true, message: "Giá sản phẩm khuyễn mãi chỉ được nhập số." }
    });
    self.giasosanh.subscribe(function (val) {
        if (val.length <= 0)
            self.giasosanh(0);
    });
    self.gia.subscribe(function (val) {
        if (val.length <= 0)
            self.gia(0);
        else {
            if (val != 0 && val < self.tmpgia()) {
                self.giasosanh(self.tmpgia());
            }
        }

    });
    self.title.subscribe(function (val) {
        if (val.length <= 0)
            self.title('default');
    });
    self.tensp = ko.observable();

    self.fullten = ko.computed(function () {
        if (self.title() != 'default')
            return self.tensp() + ' - ' + self.title();
        else
            return self.tensp();

    });
    self.imgsp = ko.observable();
    self.masp = ko.observable();
    self.isdelete = ko.observable();
};
Products.mdanhmuc = function () {
    var self = this;
    self.madanhmuc = ko.observable();
    self.tendanhmuc = ko.observable();
};
Products.mdanhmuccon = function () {
    var self = this;
    self.madanhmuccon = ko.observable();
    self.tendanhmuccon = ko.observable();

};
Products.mBrand = function () {
    var self = this;
    self.mahieu = ko.observable();
    self.tenhieu = ko.observable();
};
Products.mcollection = function () {
    var self = this;
    self.id = ko.observable();
    self.idloai = ko.observable();
    self.tenloai = ko.observable();
    self.iddm = ko.observable();
    self.danhmuc = ko.observable();
    self.danhmuccon = ko.observable();
    self.iddmc = ko.observable();
    self.idsp = ko.observable();
    self.lable = ko.observable();
};
Products.mPlan_type = function () {
    var self = this;
    self.Id = ko.observable();
    self.Type = ko.observable();
};

Products.msys_tags_Ref = function () {
    var self = this;
    self.Id = ko.observable();
    self.RefId = ko.observable();
    self.RefTag = ko.observable();
    self.Tags = ko.observable();
};
Products.msys_tags_Summary = function () {
    var self = this;
    self.Id = ko.observable();
    self.Tags = ko.observable();
    self.IsActive = ko.observable(false);
    self.RefType = ko.observable();
};