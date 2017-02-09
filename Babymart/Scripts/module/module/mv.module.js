var Module = Module || {};
Module.mvModule = function () {
    var self = this;

    self.ShowType = ko.observable();
    var routes = $.sammy(function () {
        this.get("#/cam-nang/list", function () {
            if (!self.mvlist)
                self.mvlist = new Module.mvlist();
            self.mvlist.LoadMenu(5);
            self.mvlist.TypeModule('Magazine');
            self.ShowType("List");
        });
        this.get("#/hoat-hinh/list", function () {
            if (!self.mvlist)
                self.mvlist = new Module.mvlist();
            self.mvlist.LoadMenu(3);
            self.mvlist.TypeModule('Animation');
            self.ShowType("List");
        });
        this.get("#/am-nhac/list", function () {
            if (!self.mvlist)
                self.mvlist = new Module.mvlist();
            self.mvlist.LoadMenu(2);
            self.mvlist.TypeModule('Music');
            self.ShowType("List");
        });
        this.get("#/game/list", function () {
            if (!self.mvlist)
                self.mvlist = new Module.mvlist();
            self.mvlist.LoadMenu(1);
            self.mvlist.TypeModule('Game');
            self.ShowType("List");
        });
        this.get("#/edit/:id", function () {
            var id = this.params['id'];
            //if (!self.mvedit)
            self.mvedit = new Module.mvedit();
            self.mvedit.LoadData(id);
            self.ShowType("Edit");
        });
        this.get("#/new", function () {
            if (!self.mvedit) {
                self.mvadd = new Module.mvadd();
                self.mvadd.InitFinish(true);
            }
            self.ShowType("Add");
        });
    });

    self.Start = function () {
        ko.applyBindings(self);
        if (window.location.hash == "")
            routes.run("#/list");
        else {
            routes.run(window.location.hash);
        }
    };
};