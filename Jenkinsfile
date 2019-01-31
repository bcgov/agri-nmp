basicPipeline {
    name = 'nmp'
    env = [
        'dev':[:],
        'test':[:],
        'prod':['params':['host':'nmp.apps.nrs.gov.bc.ca']]
    ]
    templates = [
        'build':[
            ['file':'OpenShift/dotnet-21.bc.json'],
            ['file':'OpenShift/dotnet-21-node.bc.json'],
            ['file':'OpenShift/nmp.bc.json'],
			['file':'OpenShift/backup-build.json']
        ],
        'deployment':[
            ['file':'OpenShift/postgresql.dc.json'],
            ['file':'OpenShift/nmp.dc.json', 'params':['HOST':'${env[DEPLOY_ENV_NAME]?.params?.host?:""}']],
			['file':'OpenShift/backup-deploy.json']
        ]
    ]
}
