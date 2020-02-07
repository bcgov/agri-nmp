#!groovy

import groovy.json.JsonOutput
import bcgov.GitHubHelper

buildEnvironment = "build"

devEnvironment = "dev"
devHost = "nmp-pr-${CHANGE_ID}-agri-nmp-dev.pathfinder.gov.bc.ca"

testEnvironment = "test"
testHost = "nmp-test-agri-nmp-test.pathfinder.gov.bc.ca"

prodEnvironment = "prod"
prodHost = "nmp.apps.nrs.gov.bc.ca"

// Notify stage status and pass to Jenkins-GitHub library
void createCommitStatus (String name, String status) {
    GitHubHelper.createCommitStatus(
        this,
        GitHubHelper.getPullRequestLastCommitId(this),
        status,
        "${env.BUILD_URL}",
        "Stage '${name}'",
        "Stage: ${name}"
    )
}

// These Status Checks appear on Github under - 'Settings > Branches > Branch Protection Rules'
// Add/Edit rules
//      provide a 'branch name pattern' e.g. master, Sprint*
//      under 'Protect matching branches'
//          Check 'Require status checks to pass before merging'
//          Check 'Require branches to be up to date before merging'
//          Check the appropriate checks under 'Status checks found in the last week for this repository'
//
// You can create multiple rules here - e.g. 
//      for the master branch you want it deployed in DEV, TEST, PROD before a PR can be merged
//      for your sprint branch you want it deployed in DEV only
//
// Note: The below few lines of code really don't need to be run everytime. Once you have entered the status checks
//       then they will stay there. If you ever turn off the branch protection
//       rules for more than a week, then you won't be able to select them back. Uncomment the code below
//       to add the status checks back. This is why the title of the section is 'Status checks found in the last week for this repository'
// createCommitStatus (buildEnvironment, 'PENDING')
// createCommitStatus (devEnvironment, 'PENDING')
// createCommitStatus (testEnvironment, 'PENDING')
// createCommitStatus (prodEnvironment, 'PENDING')

// Create deployment status and pass to Jenkins-GitHub library
void createDeploymentStatus (String suffix, String status, String stageUrl) {
    def ghDeploymentId = new GitHubHelper().createDeployment(
        this,
        "pull/${CHANGE_ID}/head",
        [
            'environment':"${suffix}",
            'task':"deploy:pull:${CHANGE_ID}"
        ]
    )

    new GitHubHelper().createDeploymentStatus(
        this,
        ghDeploymentId,
        "${status}",
        ['targetUrl':"https://${stageUrl}"]
    )

    if ('SUCCESS'.equalsIgnoreCase("${status}")) {
        echo "${suffix} deployment successful!"
    } else if ('PENDING'.equalsIgnoreCase("${status}")){
        echo "${suffix} deployment pending."
    }
}

pipeline {
    agent none
    options {
        disableResume()
    }
    stages {
        stage('Build') {
            agent { label 'build' }
            steps {
                script {
                    def filesInThisCommitAsString = sh(script:"git diff --name-only HEAD~1..HEAD | grep -v '^.jenkins/' || echo -n ''", returnStatus: false, returnStdout: true).trim()
                    def hasChangesInPath = (filesInThisCommitAsString.length() > 0)
                    echo "${filesInThisCommitAsString}"
                    if (!currentBuild.rawBuild.getCauses()[0].toString().contains('UserIdCause') && !hasChangesInPath){
                        currentBuild.rawBuild.delete()
                        error("No changes detected in the path ('^.jenkins/')")
                    }
                }
                echo "Aborting all running jobs ..."
                script {
                    abortAllPreviousBuildInProgress(currentBuild)
                }
                echo "Building ..."
                sh "cd .pipeline && ./npmw ci && ./npmw run build -- --pr=${CHANGE_ID}"
                
                // Report status to GitHub
                createCommitStatus (buildEnvironment, 'SUCCESS')
                // The corresponding Pending status for the below is posted by Jenkins image.
                createCommitStatus ('continuous-integration/jenkins/pr-head', 'SUCCESS')           
            }
        }
        stage('Deploy (DEV)') {
            agent { label 'deploy' }
            steps {
                echo "Deploying ..."

                // Report status to GitHub
                createDeploymentStatus(devEnvironment, 'PENDING', devHost)   

                sh "cd .pipeline && ./npmw ci && ./npmw run deploy -- --pr=${CHANGE_ID} --env=${devEnvironment}"

                // Report status to GitHub
                createCommitStatus (devEnvironment, 'SUCCESS')
                createDeploymentStatus(devEnvironment, 'SUCCESS', devHost)                
            }
        }
        stage('Deploy (TEST)') {
            agent { label 'deploy' }
            when {
                expression { return env.CHANGE_TARGET == 'master';}
                beforeInput true
            }
            input {
                message "Should we continue with deployment to TEST?"
                ok "Yes!"
            }
            steps {
                echo "Deploying ..."

                // Report status to GitHub
                createDeploymentStatus(testEnvironment, 'PENDING', testHost)    

                sh "cd .pipeline && ./npmw ci && ./npmw run deploy -- --pr=${CHANGE_ID} --env=${testEnvironment}"

                // Report status to GitHub
                createCommitStatus (testEnvironment, 'SUCCESS')
                createDeploymentStatus(testEnvironment, 'SUCCESS', testHost)     
            }
        }
        stage('Deploy (PROD)') {
            agent { label 'deploy' }
            when {
                expression { return env.CHANGE_TARGET == 'master';}
                beforeInput true
            }
            input {
                message "Should we continue with deployment to PROD?"
                ok "Yes!"
            }
            steps {
                echo "Deploying ..."

                // Report status to GitHub
                createDeploymentStatus(prodEnvironment, 'PENDING', prodHost)    

                sh "cd .pipeline && ./npmw ci && ./npmw run deploy -- --pr=${CHANGE_ID} --env=${prodEnvironment}"

                // Report status to GitHub
                createCommitStatus (prodEnvironment, 'SUCCESS')
                createDeploymentStatus(prodEnvironment, 'SUCCESS', prodHost)    
            }
        }
        stage('Accept Pull Request?') {
            agent { label 'deploy' }
            input {
                message "Ready to Accept/Merge, and Close pull-request?"
                ok "Yes!"
                submitter 'authenticated'
                submitterParameter "APPROVED_BY"
                parameters {
                    choice(name: 'MERGE_METHOD', choices: ((env.CHANGE_TARGET == 'master')?['merge', 'squash']:['squash', 'merge']), description: '')
                }
            }
            steps {
                script{
                    bcgov.GitHubHelper.mergeAndClosePullRequest(this, "${MERGE_METHOD}")
                }
            }
        }   
        stage('Cleanup PR artifacts') {
            agent { label 'deploy' }
            input {
                message "Should we continue with removing PR based artifacts from build and dev namespaces?"
                ok "Yes!"
            }
            steps {
                echo "Removing PR based artifacts from DEV namespace ..."
                sh "cd .pipeline && ./npmw ci && ./npmw run clean -- --pr=${CHANGE_ID} --env=build"
                sh "cd .pipeline && ./npmw ci && ./npmw run clean -- --pr=${CHANGE_ID} --env=dev"
            }
        }
    }
}