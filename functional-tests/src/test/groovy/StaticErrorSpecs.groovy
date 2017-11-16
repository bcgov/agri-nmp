<<<<<<< HEAD
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
=======
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
>>>>>>> 8fba86f6ad578b2dacd7a3c1d7d72507103e933e
