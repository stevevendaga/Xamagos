

document.addEventListener('DOMContentLoaded', function () {
      // Select all links with the class 'external-link'
      var externalLinks = document.querySelectorAll('.external-link');

  // Attach a click event listener to each external link
  externalLinks.forEach(function (link) {
    link.addEventListener('click', function (event) {
      // Open the link externally
      window.open(link.href, '_system');

      // Prevent the default behavior of the link (opening in the WebView)
      event.preventDefault();
    });
      });
   });
