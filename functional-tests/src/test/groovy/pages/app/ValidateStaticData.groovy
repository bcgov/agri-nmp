package pages.app

import geb.Page

class ValidateStaticData extends Page {

    static at = { title=="Validate Static Data" }
    static url = "/Home/ValidateStaticData"
	static at = { staticDataErrors=="No Errors" }
}
