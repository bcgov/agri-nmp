<<<<<<< HEAD
package pages.app

import geb.Page

class ValidateStaticPage extends Page {

    static at = { title == "Validate Static Data" }
    static url = "Home/validatestaticdata"
    static content = {
        staticDataError { $("label", text: contains("No Errors")) }
    }
}
=======
package pages.app

import geb.Page

class ValidateStaticPage extends Page {

    static at = { title == "Validate Static Data" }
    static url = "/Home/validatestaticdata"
    static content = {
        staticDataError { $("label", text: contains("No Errors")) }
    }
}
>>>>>>> 8fba86f6ad578b2dacd7a3c1d7d72507103e933e
