var Pages = Pages || {};
Pages.mvPages = function () {
    var self = this;
    self.ShowType = ko.observable();
    var routes = $.sammy(function () {
        this.get("#/list", function () {
            self.mvlist = new Pages.mvlist();
            self.mvlist.LoadData();
            self.ShowType("List");
        });
        this.get("#/edit/:id", function () {
            var id = this.params['id'];
            //if (!self.mvedit)
            self.mvedit = new Pages.mvedit();
            self.mvedit.LoadData(id);
            self.ShowType("Edit");
        });
        this.get("#/new", function () {
            self.mvedit = new Pages.mvedit();
            self.mvedit.InitFinish(true);
            self.ShowType("Edit");
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