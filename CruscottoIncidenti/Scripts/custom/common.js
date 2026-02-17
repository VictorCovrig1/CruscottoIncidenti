function clearValidationMessageEvent() {
    $("input, textarea, select.selectpicker").on("input change", function () {
        var $this = $(this);
        var fieldName = $this.attr("name");
        $("span[data-valmsg-for='" + fieldName + "']").text("");

        if ($this.prop('tagName') == "SELECT")
            $this = $this.parent();

        $this.removeClass("input-validation-error");
    });
}
