
basicPipeline {
    name = 'nmp'
    env = [
        'dev':[:],
        'test':[:],
        'prod':['params':['host':'nmp.apps.nrs.gov.bc.ca']]
    ]
    templates = [
        'build':[
            ['file':'OpenShift/dotnet-20.bc.json'],
            ['file':'OpenShift/dotnet-20-node.bc.json'],
            ['file':'OpenShift/nmp.bc.json'],
        ],
        'deployment':[
            ['file':'OpenShift/nmp.dc.json', 'params':['HOST':'${env[DEPLOY_ENV_NAME]?.params?.host?:""}']]
        ]
    ]
}

