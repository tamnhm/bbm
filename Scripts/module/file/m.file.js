var File = File || {};
File.mFile = function () {
    var self = this;
    self.Id = ko.observable();
    self.Files = ko.observable();
    self.Files_us = ko.observable();
    self.link = ko.observable();
    self.hide = ko.observable(false);
    self.type = ko.observable(0);
    self.sort = ko.observable(0);
};