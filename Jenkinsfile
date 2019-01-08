
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
        ],
        'deployment':[
            ['file':'OpenShift/postgresql.dc.json']
        ]
    ]
}

