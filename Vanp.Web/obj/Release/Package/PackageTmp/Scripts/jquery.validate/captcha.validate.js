(function () {
	$.validator.setDefaults({
		onkeyup: function (element, event) {
			if ($(element).hasClass("captchaVal")) {
				event.preventDefault();
				return;
			}
			if (event.which === 9 && this.elementValue(element) === "") {
				return;
			} else if (element.name in this.submitted || element === this.lastElement) {
				this.element(element);
			}
		},
		onfocusout: function (element, event) {
			if ($(element).hasClass("captchaVal")) {
				event.preventDefault();
				return;
			}
			if (!this.checkable(element) && (element.name in this.submitted || !this.optional(element))) {
				this.element(element);
			}
		},
		showErrors: function (errorMap, errorList) {
			this.defaultShowErrors();
			for (var i = 0; i < errorList.length; i++) {
				var element = errorList[i].element;
				var message = errorList[i].message;
				if (element.className.match(/captchaVal/) &&
					message === this.settings.messages[element.id].remote) {
					element.Captcha.ReloadImage();
					$("form").valid();
				}
			}
		}
	});
})();


$(document).ready(function () {
	// add validation rules by CSS class, so we don't have to know the
	// exact client id of the Captcha code textbox
	$(".captchaVal").rules('add', {
		required: true,
		remote: $(".captchaVal").get(0).Captcha.ValidationUrl,
		messages: {
			required: "Mã Captcha không được bỏ trống.",
			remote: "Mã Captcha không chính xác."
		}
	});
});