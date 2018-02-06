// https://www.npmjs.com/package/html-pdf
//var pdf = require('html-pdf');

module.exports = function (callback, rawdata, pdfOptions) {

	// https://www.npmjs.com/package/html-pdf
    var pdf = require('html-pdf');
    var err;
    var buffer;
	
	// export as PDF
    pdf.create(rawdata, pdfOptions).toBuffer(function (err, buffer) {
        if (err) {
            callback(err, null);
        }
        else {
            callback(null, buffer.toJSON());
        }
    });
    //pdf.create(rawdata, pdfOptions).toBuffer(callback);

    pdf = null;
};
