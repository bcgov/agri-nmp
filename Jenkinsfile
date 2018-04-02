
basicPipeline {
    name = 'nmp'
    env = [
        'dev':['project':'csnr-devops-lab-deploy'],
        'test':['project':'csnr-devops-lab-deploy'],
        'prod':['project':'csnr-devops-lab-deploy']
    ]
    templates = [
        'build':[
            ['file':'OpenShift/dotnet-20.bc.json'],
            ['file':'OpenShift/dotnet-20-node.bc.json'],
            ['file':'OpenShift/nmp.bc.json'],
        ],
        'deployment':[
            ['file':'OpenShift/nmp.dc.json']
        ]
    ]
}



