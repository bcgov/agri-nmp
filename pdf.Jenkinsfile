stage('build') {
	node('master') {  
		echo "Building..."
		openshiftBuild bldCfg: 'pdf', showBuildLogs: 'true'
		openshiftTag destStream: 'pdf', verbose: 'true', destTag: '$BUILD_ID', srcStream: 'pdf', srcTag: 'latest'
		openshiftTag destStream: 'pdf', verbose: 'true', destTag: 'dev', srcStream: 'pdf', srcTag: 'latest'
	}
}

stage('deploy-test') {
	timeout(time: 4, unit: 'HOURS') { input "Deploy to test?"}
	node('master') {  
		openshiftTag destStream: 'pdf', verbose: 'true', destTag: 'test', srcStream: 'pdf', srcTag: '$BUILD_ID'
	}
}

stage('deploy-prod') {
	timeout(time: 24, unit: 'HOURS') { input "Deploy to prod?"}
	node('master') {  
		openshiftTag destStream: 'pdf', verbose: 'true', destTag: 'prod', srcStream: 'pdf', srcTag: '$BUILD_ID'  
	}
}




