var Module = Module || {};
Module.mModule = function () {
    var self = this;
    self.id = ko.observable();
    self.title = ko.observable().extend({
        required: { params: true, message: "Tên bài viết không được bỏ trống" }
    });
    self.title_us = ko.observable();
    self.groupid = ko.observable();
    self.typlemodule = ko.observable();
    self.menuid = ko.observable();
    self.extract = ko.observable();
    self.contents = ko.observable();
    self.extract_us = ko.observable();
    self.contents_us = ko.observable();
    self.img = ko.observable();
    self.description = ko.observable();
    self.keyword = ko.observable();
    self.url = ko.observable();
    self.createdate = ko.observable();
    self.hide = ko.observable();
    self.linkdetail = ko.computed(function () {
        return CommonUtils.url('#/edit/' + self.id());
    });

    self.ListTags = ko.observableArray();
    self.Tags = ko.observableArray();
    self.ListTagsRefProduct = ko.observableArray();
    self.TagsProduct = ko.observableArray();
};
Module.mModuleGroup = function () {
    var self = this;
    self.id = ko.observable();
    self.title = ko.observable();

    self.title_us = ko.observable();
    self.url = ko.observable();
    self.idmenu = ko.observable();
    self.showhome = ko.observable(0);
    self.des=ko.observable();
    self.keyword=ko.observable();
    self.title.subscribe(function (value) {
        if (value !== 'default')
            self.url(CommonUtils.NonUnicode(value));
    });
};
Module.mModuleMenu = function () {
    var self = this;
    self.id = ko.observable();
    self.tenloai = ko.observable();

    self.tenloai_us = ko.observable();
    self.typemodule = ko.observable();
    self.url = ko.observable();
    self.des = ko.observable();
    self.keyword = ko.observable();
    self.tenloai.subscribe(function (value) {
        if (value !== 'default')
            self.url(CommonUtils.NonUnicode(value));
    });
    self.ListGroup = ko.observableArray();
    self.module_group = ko.observableArray();
};
Module.msys_tags_Ref= function () {
    var self = this;
    self.Id = ko.observable();
    self.RefId = ko.observable();
    self.RefTag = ko.observable();
    self.Tags = ko.observable();
};
Module.msys_tags_Summary = function () {
    var self = this;
    self.Id = ko.observable();
    self.Tags = ko.observable();
    self.IsActive = ko.observable(false);
    self.RefType = ko.observable();
};