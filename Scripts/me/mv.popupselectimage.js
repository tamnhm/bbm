var Shared = Shared || {};

Shared.mv_popupselectimage = function (popupName, isSelectMulti, selectedCallback) {
    var self = this;
    self.mpopupimage = new Shared.m_popupselectimage(popupName, isSelectMulti, selectedCallback);
};
Shared.m_popupselectimage = function (popupName, isSelectMulti, selectedCallback) {
    var self = this;

    self.IsInit = ko.observable(false);
    self.popupName = ko.observable(popupName);
    self.IsSelectMulti = ko.observable(isSelectMulti);

    self.UploadImage = ko.observableArray();
    self.ProductImage = ko.observableArray();

    self.SimplePageUploadImage = ko.observable(new Shared.m_filtersimple());
    self.SimplePageUploadImage().Page = ko.observable(new Shared.m_paged(0));
    self.SimplePageUploadImage().SetCurrentPageTo(1);
    self.SimplePageUploadImage().Page().PageSize(10);
    self.SimplePageUploadImage().FilterChange.subscribe(function () {
        self.LoadUploadImage();
    });
    self.SimplePageUploadImage().Page().CurrentPage.subscribe(function () {
        self.LoadUploadImage();
    });

    self.SimplePageProductImage = ko.observable(new Shared.m_filtersimple());
    self.SimplePageProductImage().Page = ko.observable(new Shared.m_paged(0));
    self.SimplePageProductImage().SetCurrentPageTo(1);
    self.SimplePageProductImage().Page().PageSize(10);
    self.SimplePageProductImage().FilterChange.subscribe(function () {
        self.LoadProductImage();
    });
    self.SimplePageProductImage().Page().CurrentPage.subscribe(function () {
        self.LoadProductImage();
    });

    self.LoadUploadImage = function () {
        CommonUtils.showWait(true);
        $.ajax({
            type: 'POST',
            url: CommonUtils.url('Media/GetUploadImageList'),
            cache: false,
            data: ko.toJSON(self.SimplePageUploadImage()),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                self.UploadImage(CommonUtils.MapArray(result.Data.Data, Shared.mfile));
                self.SimplePageUploadImage().SetTotalItemCount(result.Data.TotalRecord);
            }
        }).always(function () {
            CommonUtils.showWait(false);
        });
    };
    self.LoadProductImage = function () {
        CommonUtils.showWait(true);
        $.ajax({
            type: 'POST',
            url: CommonUtils.url('Media/GetProductImageList'),
            cache: false,
            data: ko.toJSON(self.SimplePageProductImage()),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                self.ProductImage(CommonUtils.MapArray(result.Data.Data, Shared.mfile));
                self.SimplePageProductImage().SetTotalItemCount(result.Data.TotalRecord);
            }
        }).always(function () {
            CommonUtils.showWait(false);
        });
    };
    self.LoadFileList = function () {
        self.LoadUploadImage();
        self.LoadProductImage();
    };

    self.CurrentSize = ko.observable();
    self.SizeList = (new Shared.mfile()).SizeList();

    self.CurrentImg = ko.observable();
    self.CurrentImgList = ko.observableArray();
    self.InputUrl = ko.observable();
    self.ClickImage = function (item) {
        if (self.IsSelectMulti()) {
            var tmpImage = ko.utils.arrayFirst(self.CurrentImgList(), function (t) {
                return t.Id() == item.Id();
            });
            if (tmpImage == null)
                self.CurrentImgList.push(item);
            else
                self.CurrentImgList.remove(tmpImage);
        } else
            self.CurrentImg(item);
    };

    self.SelectImage = function () {
        if (selectedCallback)
            if (self.IsUrlTab())
                selectedCallback(self.InputUrl());
            else
                if (self.IsSelectMulti()) {
                    var tmpResult = ko.observableArray();
                    ko.utils.arrayForEach(self.CurrentImgList(), function (item) {
                        tmpResult.push(item.getSize(self.CurrentSize()));
                    });
                    selectedCallback(tmpResult());
                }
                else
                    selectedCallback(self.CurrentImg().getSize(self.CurrentSize()));
        CommonUtils.closePopupModal(self.popupName());
    };

    self.IsDisplayPaging = ko.observable(true);
    self.IsDisplayUploadFile = ko.observable(true);
    self.IsUrlTab = ko.observable(false);
    self.CurrentSimplePage = ko.observable(self.SimplePageUploadImage());
    self.ChangeTab = function (isDisplayPaging, isDisplayUploadFile, isUrlTab, currentSimplePage) {
        self.IsDisplayPaging(isDisplayPaging);
        self.IsDisplayUploadFile(isDisplayUploadFile);

        if (currentSimplePage) self.CurrentSimplePage(currentSimplePage);
        self.CurrentSimplePage().SetCurrentPageTo(1);

        self.IsUrlTab(isUrlTab);
        self.CurrentImg(null);
    };
    self.ClickTabUploadImage = function () {
        self.ChangeTab(true, true, false, self.SimplePageUploadImage());
    };
    self.ClickTabProductImage = function () {
        self.ChangeTab(true, false, false, self.SimplePageProductImage());
    };
    self.ClickTabUrl = function () {
        self.ChangeTab(false, false, true);
    };
    self.IsEnableSelect = ko.computed(function () {
        if (self.IsUrlTab())
            if (self.InputUrl())
                return true;
            else
                return false;
        else
            if (self.IsSelectMulti())
                if (self.CurrentImgList())
                    return true;
                else return false;
            else
                if (self.CurrentImg())
                    return true;
                else return false;
    });

    self.ButtonImageTitle = ko.observable("Upload file");
    self.IsUploading = ko.observable();
    self.IsUploading.subscribe(function (value) {
        if (value)
            CommonUtils.showWait(true);
        else
            CommonUtils.showWait(false);
    });
    self.IsError = ko.observable();
    self.IsError.subscribe(function (value) {
        CommonUtils.showWait(false);
    });
    self.IsFinish = ko.observable(false);
    self.IsFinish.subscribe(function (val) {
        if (val && !self.IsError()) {
            toastr.info("Hình ảnh của bạn đã được tải lên thành công");
            self.LoadUploadImage();
            self.IsFinish(false);
        }
        self.ButtonImageTitle("Upload file");
    });
};