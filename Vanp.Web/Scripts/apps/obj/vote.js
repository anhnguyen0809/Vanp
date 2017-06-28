function Vote($placeholder) {
    var _this = this;
    _this.$element = null,
    _this.$template = $(".comment-template>.comment"),
    _this.$placeholder = $placeholder,
    _this.Id = null;
}
Vote.prototype.init = function (vote) {
    var _this = this;
    _this.Id = vote.Id;
    _this.$element = _this.$template.clone();
    _this.$element.find(".comment-author .vote").text(vote.Vote);
    _this.$element.find(".comment-author .author").text(vote.CreatedByFullName);
    _this.$element.find(".comment-body").text(vote.Content);
    _this.$element.find(".entry-date").text(vote.CreatedByName + " - " + moment(vote.CreatedWhen).format("DD/MM/YYYY HH:mm:ss"));
    _this.$element.appendTo(_this.$placeholder).show('slow');

}
