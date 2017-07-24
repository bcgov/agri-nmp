import geb.spock.GebReportingSpec
import pages.app.HomePage
import pages.app.Disclaimer
import spock.lang.Unroll


class FlowSpecs extends GebReportingSpec {

	def "Simple Test"{
		given:
			to HomePage
	when:
		$("p").click()
	then:
			at homePage    
    }
}
