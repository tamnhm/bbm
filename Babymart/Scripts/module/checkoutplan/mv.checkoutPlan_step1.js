
var Checkoutplan = Checkoutplan || {};
Checkoutplan.mvCheckoutPlan_step1 = function () {
	var self = this;
	self.mCheckout = ko.observable(new Checkoutplan.mCheckout());
	self.Isloadercart = ko.observable(false);

	self.LoadCart = function () {
		self.Isloadercart(true);
		$.ajax({
			type: "POST",
			url: '/CheckoutPlan/LoadCart',
			cache: false,
			success: function (data) {
				ko.mapping.fromJS(data, {}, self.mCheckout);
				var tmpPlanModel = ko.observableArray();
				var tmpPlanGiftModel = ko.observableArray();
				if (self.mCheckout().PlanModel()) {
					ko.utils.arrayForEach(self.mCheckout().PlanModel(), function (obj) {
						tmpPlanModel.push(ko.mapping.fromJS(obj, {}, new Checkoutplan.mPlanModel));
					});
				}
				if (self.mCheckout().Gift()) {
					tmpPlanGiftModel(CommonUtils.MapArray(self.mCheckout().Gift(), Checkoutplan.mGift));
				}
				self.mCheckout().PlanModel(tmpPlanModel());
				self.mCheckout().Gift(tmpPlanGiftModel());
			}
		}).always(function () {
			self.Isloadercart(false);
		});
	};
	self.Start = function () {
		ko.applyBindings(self);
		self.LoadCart();
	};
};