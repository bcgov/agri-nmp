'use strict';
const {OpenShiftClientX} = require('pipeline-cli')
const path = require('path');

module.exports = (settings)=>{
  const phases = settings.phases
  const options= settings.options
  const phase=options.env
  const oc=new OpenShiftClientX(Object.assign({'namespace':phases[phase].namespace}, options));
  const templatesLocalBaseUrl =oc.toFileUrl(path.resolve(__dirname, '../../OpenShift'))
  const msTeamsWebhookSecret = new OpenShiftClientX({namespace:phases.build.namespace}).get('secret/ms-teams-webhook')[0];
  const msTeamsWebhookURL = Buffer.from(msTeamsWebhookSecret.data.webhookURL, 'base64').toString('utf-8');

  var objects = []

  // The deployment of your cool app goes here ▼▼▼
  objects.push(...oc.processDeploymentTemplate(`${templatesLocalBaseUrl}/weasyprint-dc.json`, {
    'param':{
      'NAME': phases[phase].name,
      'SUFFIX': phases[phase].suffix,      
      'VERSION': phases[phase].tag,
      'WEASYPRINT_REPLICAS': phases[phase].weasyprintreplicas
    }
  }));

  objects.push(...oc.processDeploymentTemplate(`${templatesLocalBaseUrl}/postgresql.dc.json`, {
    'param':{
      'NAME': phases[phase].name,
      'SUFFIX': phases[phase].suffix,
      'PERSISTENT_VOLUME_CLASS': phases[phase].persistentVolumeClass
    }
  }));

  objects.push(...oc.processDeploymentTemplate(`${templatesLocalBaseUrl}/nmp.dc.json`, {
    'param':{
      'NAME': phases[phase].name,
      'SUFFIX': phases[phase].suffix,
      'VERSION': phases[phase].tag,
      'HOST': phases[phase].host,
      'NMP_REPLICAS': phases[phase].nmpreplicas,
      'PDF_REPLICAS': phases[phase].pdfreplicas,
      'MS_TEAMS_WEBHOOK_URL': msTeamsWebhookURL,
      'CERTBOT_MANAGED_AUTO_CERT_RENEWAL': phases[phase].certbotManaged
    }
  }));

  oc.applyRecommendedLabels(objects, phases[phase].name, phase, phases[phase].changeId, phases[phase].instance)
  oc.importImageStreams(objects, phases[phase].tag, phases.build.namespace, phases.build.tag)
  oc.applyAndDeploy(objects, phases[phase].instance)
}
