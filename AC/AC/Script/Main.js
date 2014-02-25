$(function () {
    'use strict';

    //Remove the success message
    $('#successbutton').click(function () {
        $(this).parent().remove();
    });

    var anchor = $('#imageswitcher a');

    //Count the amount of images so we get the offset for the scrollLeft
    var images = 0;
    anchor.each(function () {
        var href = $(this).attr('href');
        images += 1;

        //Ugly solution, but it does what it should
        //decodeURIComponent(location.search) gives us the decoded queryString
        //split the string a & if there are more values in it
        //filter the string based on if there is a file= in it
        //since there can be several of them we use the first one from the sorted result
        var listA = decodeURIComponent(location.search).split('&'),
            list = listA.filter(function (s) { return s.indexOf('file=') !== -1 }).sort()[0];
        if (href === list) {
            $(this).addClass('currentimage');
            //console.log the result, which isn't really necessary
            //Since animate() isnt sideeffect free we can do this and still get a result
            //we use animate since it doesn't just jump, it scrolls instead
            console.log(".liimageswitcher.scrollLeft ", $('.liimageswitcher').animate({ scrollLeft: (images-1) * 30 }, "slow"));
        }
    });
});