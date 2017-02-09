debugger
var Banner = Banner || {};
Banner.mBanner = function () {
    var self = this;
    self.Id = ko.observable();
    self.Banner = ko.observable();
    self.Banner_us = ko.observable();
    self.Type = ko.observable();
    self.Hide = ko.observable(false);
};