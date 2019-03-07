pipeline {
    agent none
    options {
        disableResume()
    }
    stages {
        stage('Build') {
            agent { label 'build' }
            steps {
                echo "Aborting all running jobs ..."
                script {
                    abortAllPreviousBuildInProgress(currentBuild)
                }
                echo "Building ..."
                sh "OpenShift/pipeline-cli build --pr=${CHANGE_ID}"
            }
        }
        stage('Deploy (DEV)') {
            agent { label 'deploy' }
            steps {
                echo "Deploying ..."
                sh "OpenShift/pipeline-cli deploy --pr=${CHANGE_ID} --env=dev"
            }
        }
        stage('Deploy (TEST)') {
            agent { label 'deploy' }
            input {
                message "Approve deployment to TEST?"
                ok "Yes!"
            }
            steps {
                echo "Deploying ..."
                sh "OpenShift/pipeline-cli deploy --pr=${CHANGE_ID} --env=test"
            }
        }
        stage('Deploy (PROD)') {
            agent { label 'deploy' }
            input {
                message "Approve deployment to PROD?"
                ok "Yes!"
            }
            steps {
                echo "Deploying ..."
                sh "OpenShift/pipeline-cli deploy --pr=${CHANGE_ID} --env=prod"
            }
        }
    }
}