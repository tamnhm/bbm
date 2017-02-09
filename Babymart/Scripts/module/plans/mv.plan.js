var Plan = Plan || {};
Plan.mvPlan = function () {
    var self = this;
    self.ListplanType = ko.observableArray();
    self.mtypePlan = ko.observable(new Plan.mPlan_type());
    self.ListplanTypebyAdmin = ko.observableArray();
    self.ListSalleoffbyAdmin = ko.observableArray();
    self.Load_plan_saleoffbyAdmin = function () {
        $.ajax({
            type: 'GET',
            url: CommonUtils.url('/Admin/Plans/GetlistShop_plan_saleoff'),
            cache: false,
            success: function (data) {
                self.ListSalleoffbyAdmin(CommonUtils.MapArray(data, Plan.mplan_saleoff));
                debugger
            }
        }).always(function () {
            self.waitloader(false);
        });
    };
    self.LoadDatabyAdmin = function () {
        CommonUtils.showWait(true);
        debugger
        $.ajax({
            type: "GET",
            url: '/Admin/Plans/GetlistShop_plan_type',
            cache: false,
            success: function (data) {
                debugger
                self.ListplanTypebyAdmin(CommonUtils.MapArray(data, Plan.mPlan_type));
                self.Load_plan_saleoffbyAdmin();
                debugger
            }
        }).always(function () {
            CommonUtils.showWait(false);
        });
    };
    self.linkaddType = function () {
        $('#modal-add-type').modal('show');
        self.mtypePlan().Id(undefined);
        self.mtypePlan().Type(undefined);
        self.mtypePlan().Type_us(undefined);
    };
    self.linkeditType = function (obj) {
        $('#modal-edit-type').modal('show');
        self.mtypePlan().Id(obj.Id());
        self.mtypePlan().Type(obj.Type());
        self.mtypePlan().Type_us(obj.Type_us());

    };
    self.submitAddType = function (obj) {
        CommonUtils.showWait(true);
        var input = ko.toJSON({ model: obj });
        $.ajax({
            type: 'POST',
            url: CommonUtils.url('/Admin/Plans/InsertPlanType'),
            cache: false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: input,
            success: function (r) {
                if (r.success) {
                    $.gritter.add({
                        title: 'Thông báo',
                        text: r.messaging
                    });
                    $('#modal-add-type').modal('hide');
                    self.LoadDatabyAdmin();
                }
                else { alert(r.messaging) }
            }
        }).always(function () {
            CommonUtils.showWait(false);
        });
    };
    self.submitEditType = function (obj) {
        CommonUtils.showWait(true);
        var input = ko.toJSON({ model: obj });
        $.ajax({
            type: 'POST',
            url: CommonUtils.url('/Admin/Plans/UpdatePlanType'),
            cache: false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: input,
            success: function (r) {
                if (r.success) {
                    $.gritter.add({
                        title: 'Thông báo',
                        text: r.messaging
                    });
                    $('#modal-edit-type').modal('hide');
                    self.LoadDatabyAdmin();
                }
                else { alert(r.messaging) }
            }
        }).always(function () {
            CommonUtils.showWait(false);

        });
    };
    self.delete_type = function (obj) {
        bootbox.confirm("Bạn muốn xóa <strong>" + obj.Type() + "</strong> ?", function (result) {
            CommonUtils.showWait(true);
            if (result) {
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Plans/DeletePlanType'),
                    cache: false,
                    data: { id: obj.Id() },
                    success: function (r) {
                        if (r.success) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: r.messaging
                            });
                            $('#modal-edit-type').modal('hide');
                            self.LoadDatabyAdmin();
                        }
                        else { alert(r.messaging) }
                    }
                }).always(function () {
                    CommonUtils.showWait(false);
                });
            }
        });
    };
    self.Start = function () {
        ko.applyBindings(self);
        self.LoadDatabyAdmin();
    };
};