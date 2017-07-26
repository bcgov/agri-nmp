package pages.app

import geb.Page

class HomePage extends Page {

    static at = { title=="Swagger UI" }
    static url = "/swagger/ui/index.html"
}
