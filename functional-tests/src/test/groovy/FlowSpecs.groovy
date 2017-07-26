import geb.spock.GebReportingSpec
import pages.app.HomePage
import pages.app.Disclaimer
import spock.lang.Unroll


class FlowSpecs extends GebReportingSpec {

	@Unroll
	def "Simple test"(){
	given:
			to HomePage
	when:
		$("a","id":"Explore").click()
	then:
			at HomePage    
    }
}
