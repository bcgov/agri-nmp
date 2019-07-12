pipeline {
    agent none
    options {
        disableResume()
    }
    environment {
        OCP_PIPELINE_VERSION = '0.0.7'
        OCP_PIPELINE_CLI_URL = 'https://raw.githubusercontent.com/BCDevOps/ocp-cd-pipeline/v0.0.7/src/main/resources/pipeline-cli'
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
                sh "cd jenkins; curl -sSL '${OCP_PIPELINE_CLI_URL}' | bash -s build --config=config.groovy --pr=${CHANGE_ID}"
            }
        }
        stage('Deploy (DEV)') {
            agent { label 'deploy' }
            steps {
                echo "Deploying ..."
                sh "cd jenkins; curl -sSL '${OCP_PIPELINE_CLI_URL}' | bash -s deploy --config=config.groovy --pr=${CHANGE_ID} --env=dev"
            }
        }
        /*
        stage('Deploy (TEST)') {
            agent { label 'deploy' }
            input {
                message "Should we continue with deployment to TEST?"
                ok "Yes!"
            }
            steps {
                echo "Deploying ..."
                sh "cd jenkins; curl -sSL '${OCP_PIPELINE_CLI_URL}' | bash -s deploy --config=config.groovy --pr=${CHANGE_ID} --env=test"
            }
        }
        */
        stage('Deploy (PROD)') {
            agent { label 'deploy' }
            input {
                message "Should we continue with deployment to PROD?"
                ok "Yes!"
            }
            steps {
                echo "Deploying ..."
                sh "cd jenkins; curl -sSL '${OCP_PIPELINE_CLI_URL}' | bash -s deploy --config=config.groovy --pr=${CHANGE_ID} --env=prod"
            }
        }
        stage('Verification/Cleanup') {
            agent { label 'deploy' }
            input {
                message "Should we continue with cleanup, merge, and close PR?"
                ok "Yes!"
            }
            steps {
                echo "Cleaning ..."
                sh "cd jenkins; curl -sSL '${OCP_PIPELINE_CLI_URL}' | bash -s cleanup --config=config.groovy --pr=${CHANGE_ID}"
                script {
                    String mergeMethod=("master".equalsIgnoreCase(env.CHANGE_TARGET))?'merge':'squash'
                    echo "Merging (using '${mergeMethod}' method) and closing PR"
                    bcgov.GitHubHelper.mergeAndClosePullRequest(this, mergeMethod)
                }
            }
        }
    }
}