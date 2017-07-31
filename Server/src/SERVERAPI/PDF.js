module.exports = function (callback, html, pdf_options) {

    const DEFAULT_PDF_OPTIONS = {
        format: 'letter',
        orientation: 'landscape' // portrait or landscape
    };

    // https://www.npmjs.com/package/html-pdf
    var pdf = require('html-pdf');
    var config = {

        // Page options 
        border: {
            top: "0in",            // default is 0, units: mm, cm, in, px 
            right: "0in",
            bottom: "0in",
            left: "0in"
        },

        header:
        {
            height: "15mm",
            contents: '<div style="text-align: center;">Nutrient Mamangement Report</div>'
        },
        footer:
        {
            height: "15mm",
            contents: '<span style="color: #444;">{{page}}</span>/<span>{{pages}}</span>'
        },

        // File options 
        type: "pdf",             // allowed file types: png, jpeg, pdf 
        quality: "75",           // only used for types png & jpeg 
    }
    // PDF options
    var options = Object.assign({}, DEFAULT_PDF_OPTIONS, config);

    // export as PDF
    pdf.create(html, options).toBuffer(function (err, buffer) {
        if (err) {
            callback(err, null);
        }
        else {
            callback(null, buffer.toJSON());
        }
    });
};