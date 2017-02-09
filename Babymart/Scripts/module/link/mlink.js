var Link = Link || {};
Link.mlinkcha = function () {
    var self = this;
    self.madanhmuc = ko.observable();
    self.tendanhmuc = ko.observable();
    self.tendanhmuc_us = ko.observable();
    self.hide = ko.observable();
    self.url = ko.observable();
    self.url_us = ko.observable();
    self.meta_title = ko.observable();
    self.meta_keyword = ko.observable();
    self.meta_description = ko.observable();
    self.meta_title_us = ko.observable();
    self.meta_keyword_us = ko.observable();
    self.meta_description_us = ko.observable();
    self.isaddlink = ko.observable(false);
    self.tendanhmuc.subscribe(function (value) {
        if (self.isaddlink()) {
            self.url(CommonUtils.NonUnicode(value));
            self.meta_title(value);
        }
    });
    self.noidung = ko.observable();
    self.noidung_us = ko.observable();
    self.shop_danhmuccon = ko.observableArray();
    self.background = ko.observable();
    //self.LoadData = function (data) {
    //    ko.mapping.fromJS(data, {}, self);
    //    self.danhmuccons(CommonUtils.MapArray(data.danhmuccons, Link.mlinkcon));

    //};
    self.sort = ko.observable(1);
    self.icon = ko.observable();
    self.css = ko.observable();
    self.ListTagsArticle = ko.observableArray();
};

Link.mlinkcon = function () {
    var self = this;
    self.madanhmuccon = ko.observable();
    self.madanhmuc = ko.observable();
    self.tendanhmuccon = ko.observable();
    self.tendanhmuccon_us = ko.observable();
    self.hide = ko.observable();
    self.isaddlinkcon = ko.observable(false);
    self.tendanhmuccon.subscribe(function (value) {
        if (self.isaddlinkcon()) {
            self.url(CommonUtils.NonUnicode(value));
            self.metatitle(value);
        }
    });
    self.url = ko.observable();
    self.url_us = ko.observable();
    self.metakeywords = ko.observable();
    self.metadescription = ko.observable();
    self.metatitle = ko.observable(); 
    self.metakeywords_us = ko.observable();
    self.metadescription_us = ko.observable();
    self.metatitle_us = ko.observable();
    self.noidung = ko.observable();
    self.noidung_us = ko.observable();
    self.sort = ko.observable(1);
    self.css = ko.observable();
    self.grouplink = ko.observable('---');

    self.grouplink_us = ko.observable('---');
    self.ListTagsArticle = ko.observableArray();
};

Link.msys_tags_Ref = function () {
    var self = this;
    self.Id = ko.observable();
    self.RefId = ko.observable();
    self.RefTag = ko.observable();
    self.Tags = ko.observable();
};
Link.msys_tags_Summary = function () {
    var self = this;
    self.Id = ko.observable();
    self.Tags = ko.observable();
    self.IsActive = ko.observable(false);
    self.RefType = ko.observable();
};