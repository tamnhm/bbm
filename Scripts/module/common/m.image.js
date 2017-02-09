var Common = Common || {};
Common.mImage = function () {
    var self = this;
    self.id = ko.observable();
    self.TypeId = ko.observable();
    self.RefId = ko.observable();
    self.url = ko.observable();
    self.alt = ko.observable();
    self.hide = ko.observable();
    self.ImgPrimary = ko.observable();
};
Common.mTypeImage = function () {
    var self = this;
    self.id = ko.observable();
    self.refid = ko.observable();
    self.alt = ko.observable();
    self.hide = ko.observable();

};