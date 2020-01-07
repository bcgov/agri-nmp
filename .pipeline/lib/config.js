'use strict';
const options= require('pipeline-cli').Util.parseArguments()
const changeId = options.pr //aka pull-request
const version = '1.0.0'
const name = 'nmp'

const phases = {
  build: {namespace:'agri-nmp-tools', name: `${name}`, phase: 'build', changeId:changeId, suffix: `-build-${changeId}`, instance: `${name}-build-${changeId}`, version:`${version}-${changeId}`, tag:`build-${version}-${changeId}`, nmpreplicas:'1', pdfreplicas:'1' , persistentVolumeClass:'netapp-file-standard' , host:`${name}-pr-${changeId}-agri-nmp-tools.pathfinder.gov.bc.ca`},
    dev: {namespace:'agri-nmp-dev'  , name: `${name}`, phase: 'dev'  , changeId:changeId, suffix: `-dev-${changeId}`  , instance: `${name}-dev-${changeId}`  , version:`${version}-${changeId}`, tag:`dev-${version}-${changeId}`  , nmpreplicas:'1', pdfreplicas:'1' , persistentVolumeClass:'netapp-file-standard' , host:`${name}-pr-${changeId}-agri-nmp-dev.pathfinder.gov.bc.ca`},
   test: {namespace:'agri-nmp-test' , name: `${name}`, phase: 'test' , changeId:changeId, suffix: `-test`             , instance: `${name}-test`             , version:`${version}-${changeId}`, tag:`test-${version}`             , nmpreplicas:'2', pdfreplicas:'2' , persistentVolumeClass:'netapp-file-standard' , host:`${name}-test-agri-nmp-test.pathfinder.gov.bc.ca`},
   prod: {namespace:'agri-nmp-prod' , name: `${name}`, phase: 'prod' , changeId:changeId, suffix: `-prod`             , instance: `${name}-prod`             , version:`${version}-${changeId}`, tag:`prod-${version}`             , nmpreplicas:'3', pdfreplicas:'3' , persistentVolumeClass:'netapp-file-standard' , host:'nmp.apps.nrs.gov.bc.ca'}
};

// This callback forces the node process to exit as failure.
process.on('unhandledRejection', (reason) => {
  console.log(reason);
  process.exit(1);
});

module.exports = exports = {phases, options};