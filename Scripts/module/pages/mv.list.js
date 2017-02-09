var Pages = Pages || {};
Pages.mvlist = function () {
    var self = this;
    self.listpage = ko.observableArray();
    self.InitFinish = ko.observable(false);
    self.LoadData = function () {
        CommonUtils.showWait(true);
        $.ajax({
            type: "GET",
            url: '/Admin/Page/Getlist',
            success: function (data) {
                self.listpage(CommonUtils.MapArray(data, Pages.mPages));
            }
        }).always(
                function () {
                    CommonUtils.showWait(false);
                });
    };
    /*******************************************************************************************************/
    var filters = [
       {
           Type: "text",
           Name: "title",
           Value: ko.observable(""),
           RecordValue: function (record) { return record.tieude; }
       }

    ];
    var sortOptions = [
        {
            Name: "id",
            Value: "id",
            Sort: function (left, right) { return left.id < right.id; }
        }
    ];
    self.filter = new CommonUtils.FilterModel(filters, self.listpage);
    self.sorter = new CommonUtils.SorterModel(sortOptions, self.filter.filteredRecords);
    self.pager = new CommonUtils.PagerModel(self.sorter.orderedRecords);
    /*******************************************************************************************************/
    self.RemovePage = function (value) {
        bootbox.confirm("Bạn muốn xóa <strong>" + value.tieude() + "</strong> ?", function (result) {
            if (result) {
                CommonUtils.showWait(true);
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/Page/DeleteArticle'),
                    cache: false,
                    data: { id: value.id() },
                    success: function (r) {
                        if (r.success) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: r.messaging
                            });
                            self.LoadData();
                        } else { alert(r.messaging) }
                    }
                }).always(function () {
                    CommonUtils.showWait(false);
                });
            }
        })
    };
    self.linkadd = ko.computed(function () {
        return CommonUtils.url('#/new');
    });

    /******************************************************/
}