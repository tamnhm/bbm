var Contents = Contents || {};
Contents.mContents = function () {
    var self = this;
    self.Id = ko.observable();
    self.Type = ko.observable();  
    self.Name = ko.observable();
    self.Contents = ko.observable();
    self.Name_us = ko.observable();
    self.Contents_us = ko.observable();
    self.hide = ko.observable(false);
};