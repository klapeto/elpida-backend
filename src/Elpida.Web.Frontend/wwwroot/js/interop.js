window.interopFunctions = {
    scrollTo: function (element, top, left, behavior) {
        element.scrollTo({
            top: top,
            left: left,
            behavior: behavior
        });
    }
}