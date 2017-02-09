var File = File || {};
File.mvFile = function () {
    var self = this;
    self.Files = ko.observableArray();
    self.OneFile = ko.observable(new File.mFile);
    self.ListType = ko.observableArray([{ name: 'Tất cả', value: 0 }, { name: 'Banner Home', value: 1 }, { name: 'Banner left', value: 2 }]);
    self.SelectType = ko.observable(0);
    self.LoadData = function () {
        CommonUtils.showWait(true);
        $.ajax({
            type: "POST",
            url: '/Admin/File/GetList',
            dataType: 'json',
            success: function (data) {
                self.Files(CommonUtils.MapArray(data, File.mFile));
            }
        }).always(function () { CommonUtils.showWait(false) });
    };
    /*******************************************************************************************************/
    var filters = [
       {
           Type: "text",
           Name: "Files",
           Value: ko.observable(""),
           RecordValue: function (record) { return record.Files; }
       }

    ];
    var sortOptions = [
        {
            Name: "Id",
            Value: "Id",
            Sort: function (left, right) { return left.Id < right.Id; }
        }
    ];
    self.filter = new CommonUtils.FilterModel(filters, self.Files);
    self.sorter = new CommonUtils.SorterModel(sortOptions, self.filter.filteredRecords);
    self.pager = new CommonUtils.PagerModel(self.sorter.orderedRecords);
    /*******************************************************************************************************/


    self.AddFile = function () {
        $('#modal-file-add').modal('show');
    };
    self.EditFile = function (obj) {
        self.OneFile(obj);
        $('#modal-file-edit').modal('show');
    };
    self.RemoveFile = function (data) {
        $(".hide_delete").css("display", "block");
        bootbox.confirm("Bạn muốn xóa danh mục <strong>" + data.Files() + "</strong> ?", function (result) {
            if (result) {
                CommonUtils.showWait(true);
                $.ajax({
                    type: 'POST',
                    url: CommonUtils.url('/Admin/File/RemoveFile'),
                    cache: false,
                    data: { id: data.Id() },
                    success: function (r) {
                        if (r.success) {
                            $.gritter.add({
                                title: 'Thông báo',
                                text: r.messaging
                            });
                            self.LoadData();
                        }
                        else {
                            alert(r.messaging)
                        }

                    }
                }).always(function () { CommonUtils.showWait(false) });
            }
        });

    };
    self.uploadFile = function (data) {
        if (data) {
            $(data).fileupload({
                autoUpload: true,
                url: '/Admin/Common/UploadFile',
                dataType: 'json',
                // formData: { map: self.MapPathUpload() },
                add: function (e, data) {
                    var jqXHR = data.submit()
                        .success(function (data, textStatus, jqXHR) {
                            if (data.isUploaded) {
                                  toastr.success(data.message);
                                self.LoadData();
                            }
                            else {
                                alert(data.messaging);
                            }

                        })
                        .error(function (data, textStatus, errorThrown) {
                            if (typeof (data) != 'undefined' || typeof (textStatus) != 'undefined' || typeof (errorThrown) != 'undefined') {
                                alert(textStatus + errorThrown + data);
                            }
                        });
                },
                fail: function (event, data) {
                    if (data.files[0].error) {
                        alert(data.files[0].error);
                    }
                }
            });
        } else {
            //alert("Bạn chưa chọn thư mục");
        }

    };

    self.Start = function () {
        ko.applyBindings(self);
        self.LoadData();
    };
};