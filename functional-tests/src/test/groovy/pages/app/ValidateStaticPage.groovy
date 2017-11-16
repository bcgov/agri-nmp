package pages.app

import geb.Page

class ValidateStaticPage extends Page {

    static at = { title == "Validate Static Data" }
    static url = "Home/validatestaticdata"
    static content = {
        staticDataError { $("label", text: contains("No Errors")) }
    }
}
