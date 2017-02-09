var Brand = Brand || {};
Brand.mvlist = function () {
    var self = this;
    self.DataArray = ko.observableArray();
    self.mBrand = ko.observable(new Brand.mBrand());
    self.LoadData = function () {
        CommonUtils.showWait(true);
        $.ajax({
            type: "GET",
            url: '/Admin/Brand/List',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.DataArray(CommonUtils.MapArray(data, Brand.mBrand));
                CommonUtils.showWait(false);

            }
        });
    };
    /********************************************/
    var filters = [
       {
           Type: "text",
           Name: "tenhieu",
           Value: ko.observable(""),
           RecordValue: function (record) { return record.tenhieu; }
       }];
    var sortOptions = [
        //{
        //    Name: "Mã hiệu",
        //    Value: "mahieu",
        //    Sort: function (left, right) { return left.mahieu > right.mahieu; }
        //},
        {
            Name: "Tên hiệu",
            Value: "tenhieu",
            Sort: function (left, right) { return CommonUtils.CompareCaseInsensitive(left.tenhieu, right.tenhieu); }
        }
    ];
    self.filter = new CommonUtils.FilterModel(filters, self.DataArray);
    self.sorter = new CommonUtils.SorterModel(sortOptions, self.filter.filteredRecords);
    self.pager = new CommonUtils.PagerModel(self.sorter.orderedRecords);

    /********************************************/
    self.linkadd = ko.computed(function () {
        return CommonUtils.url('brand#/new');
    });
    self.setshow = function (data) {
        var isvisible = data.hide() ? false : true;
        var string = !data.hide() ? "ẩn" : "hiện";
        bootbox.confirm("Bạn muốn <strong>" + string + "</strong> thương hiệu <strong>" + data.tenhieu() + "</strong> ?", function (result) {
            if (result) {
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Brand/SetVisible'),
                    cache: false,
                    data: { id: data.mahieu(), visible: isvisible },
                    success: function (result) {
                        CommonUtils.showWait(true);

                        if (result.success) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: result.messaging
                            });
                            self.LoadData();
                        }
                        else {
                            alert(result.messaging)
                        }
                        CommonUtils.showWait(false);
                    }
                });

            }
        });
    };
    self.remove = function (data) {
        bootbox.confirm("Bạn muốn xóa sản phẩm <strong>" + data.tenhieu() + "</strong> ?", function (result) {
            if (result) {
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Brand/Remove'),
                    cache: false,
                    data: { id: data.mahieu() },
                    success: function (result) {
                        CommonUtils.showWait(true);

                        if (result.success) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: result.messaging
                            });
                            self.LoadData();
                        }
                        else {
                            alert(result.messaging)
                        }
                        CommonUtils.showWait(false);
                    }
                });

            }
        });
    };
    self.showhomemodal = function (data) {
        var showhome = data.showhome() ? false : true;
        var string = data.showhome() ? "ẩn" : "hiển thị";
        bootbox.confirm("Bạn muốn <strong>" + string + "</strong> ngoài trang chủ <strong>" + data.tenhieu() + "</strong> ?", function (result) {
            if (result) {
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Brand/SetShowHome'),
                    cache: false,
                    data: { id: data.mahieu(), showhome: showhome },
                    success: function (result) {
                        CommonUtils.showWait(true);

                        if (result.success) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: result.messaging
                            });
                            self.LoadData();
                        }
                        else {
                            alert(result.messaging)
                        }
                        CommonUtils.showWait(false);
                    }
                });
            }
        });
    };

}