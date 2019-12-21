#!groovy

import groovy.json.JsonOutput
import bcgov.GitHubHelper

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

createCommitStatus ('DEV ', 'PENDING')

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
    environment {
        devSuffix = "dev"
        devHost = "nmp-pr-${CHANGE_ID}-agri-nmp-dev.pathfinder.gov.bc.ca"

        testSuffix = "test"
        testHost = "nmp-test-agri-nmp-test.pathfinder.gov.bc.ca"

        prodSuffix = "prod"
        prodHost = "nmp.apps.nrs.gov.bc.ca"
    }
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
            }
        }
        stage('Deploy (DEV)') {
            agent { label 'deploy' }
            steps {
                echo "Deploying ..."

                // Report status to GitHub
                createDeploymentStatus(devSuffix, 'PENDING', devHost)        
                createDeploymentStatus(testSuffix, 'PENDING', testHost)
                createDeploymentStatus(prodSuffix, 'PENDING', prodHost)
                sh "cd .pipeline && ./npmw ci && ./npmw run deploy -- --pr=${CHANGE_ID} --env=${devSuffix}"

                // Report status to GitHub
                createDeploymentStatus(devSuffix, 'SUCCESS', devHost)                
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
                createDeploymentStatus(testSuffix, 'PENDING', testHost)    

                sh "cd .pipeline && ./npmw ci && ./npmw run deploy -- --pr=${CHANGE_ID} --env=${testSuffix}"

                // Report status to GitHub
                createDeploymentStatus(testSuffix, 'SUCCESS', testHost)     
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
                createDeploymentStatus(prodSuffix, 'PENDING', prodHost)    

                sh "cd .pipeline && ./npmw ci && ./npmw run deploy -- --pr=${CHANGE_ID} --env=${prodSuffix}"

                // Report status to GitHub
                createDeploymentStatus(prodSuffix, 'SUCCESS', prodHost)    
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