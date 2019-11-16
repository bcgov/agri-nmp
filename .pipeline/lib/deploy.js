'use strict';
const {OpenShiftClientX} = require('pipeline-cli')
const path = require('path');

module.exports = (settings)=>{
  const phases = settings.phases
  const options= settings.options
  const phase=options.env
  const oc=new OpenShiftClientX(Object.assign({'namespace':phases[phase].namespace}, options));
  const templatesLocalBaseUrl =oc.toFileUrl(path.resolve(__dirname, '../../OpenShift'))
  var objects = []

  // The deployment of your cool app goes here ▼▼▼
  objects.push(...oc.processDeploymentTemplate(`${templatesLocalBaseUrl}/postgresql.dc.json`, {
    'param':{
      'NAME_SUFFIX': phases[phase].suffix
    }
  }));

  objects.push(...oc.processDeploymentTemplate(`${templatesLocalBaseUrl}/nmp.dc.json`, {
    'param':{
      'NAME_SUFFIX': phases[phase].suffix,
      'ENV_NAME': phases[phase].tag,
      'HOST': phases[phase].host || '',
      'NMP_REPLICAS': phases[phase].nmpreplicas,
      'PDF_REPLICAS': phases[phase].pdfreplicas
    }
  }));

  oc.applyRecommendedLabels(objects, phases[phase].name, phase, phases[phase].changeId, phases[phase].instance)
  oc.importImageStreams(objects, phases[phase].tag, phases.build.namespace, phases.build.tag)
  oc.applyAndDeploy(objects, phases[phase].instance)
}
