import geb.spock.GebReportingSpec
import pages.app.ValidateStaticPage

class StaticErrorSpecs extends GebReportingSpec {

	def "Static Error Test"(){
	when:
		to ValidateStaticPage
	then:
		assert staticDataError
    }
}
