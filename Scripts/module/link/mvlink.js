var Link = Link || {};
Link.mvlink = function () {
    var self = this;
    self.mvlist = new Link.mvlist_link();
    self.Start = function () {
        self.mvlist.LoadData();
        ko.applyBindings(self);
    };
};