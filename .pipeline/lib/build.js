'use strict';
const {OpenShiftClientX} = require('pipeline-cli')
const path = require('path');

module.exports = (settings)=>{
  const phases = settings.phases
  const options = settings.options
  const phase='build'
  const oc=new OpenShiftClientX(Object.assign({'namespace':phases.build.namespace}, options));
  const objects = []
  const templatesLocalBaseUrl =oc.toFileUrl(path.resolve(__dirname, '../../OpenShift'))

  // The building of your cool app goes here ▼▼▼
  objects.push(...oc.processDeploymentTemplate(`${templatesLocalBaseUrl}/dotnet-21.bc.json`, {
    'param':{
    'NAME': phases[phase].name,
    'SUFFIX': phases[phase].suffix,
    'VERSION': phases[phase].tag,
    }
  }));

  objects.push(...oc.processDeploymentTemplate(`${templatesLocalBaseUrl}/dotnet-21-node.bc.json`, {
    'param':{
      'NAME': phases[phase].name,
      'SUFFIX': phases[phase].suffix,
      'VERSION': phases[phase].tag,
      'SOURCE_REPOSITORY_URL': oc.git.http_url,
      'GIT_REF': oc.git.ref
    }
  }));

  objects.push(...oc.processDeploymentTemplate(`${templatesLocalBaseUrl}/nmp.bc.json`, {
    'param':{
      'NAME': phases[phase].name,
      'SUFFIX': phases[phase].suffix,
      'VERSION': phases[phase].tag,
      'SOURCE_REPOSITORY_URL': oc.git.http_url,
      'GIT_REF': oc.git.ref
    }
  }));
  oc.applyRecommendedLabels(objects, phases[phase].name, phase, phases[phase].changeId, phases[phase].instance)
  oc.applyAndBuild(objects)
}