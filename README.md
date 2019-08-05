This is an orphan branch and it is *NOT* intended (or possible) to merge with master.

# Build and Deployment Tools
Currently, we use ocp-cd-pipeline (customized by bcgov) and jenkins-basic:v2-42 (customized by bcgov).

## Setting up for the first time
When you setup Jenkins for the first time in your tools namespace, follow the process below.

### Create these secrets
1. template.jenkins-agri-nmp-github 
2. template.jenkins-agri-nmp-slave-user

### Have a client Linux machine with following tools installed
1. oc
2. git
3. tar
4. bash
5. groovy
6. java

### Use oc to login to your tools namespace

1. Run jenkins/build.sh 
2. Run jenkins/deploy.sh

## Upgrading Jenkins when it is already setup in your namespace
1. Create a new branch off of the tools branch, call it jenkins-upgrade
2. Check bcgov docker repo for the updated jenkins image and update jenkin.bc.yaml with the new image
3. Do a PR from jenkins-upgrade to the tools branch
4. This will start a PR based build to create a new Jenkins image in your tools namespace
5. Verify if this image looks ok. Check the UI etc. Doing full end-to-end testing from Github for this PR based image is not possible, as Github points to the main jenkins URL.
6. Approve the build for PROD
7. This will replace your PROD jenkins and use the same standar URL
8. Now you can create a new branch in the main repo, do an empty commit, create PR and see a PR based build of your app
9. Test a deployment from a commit to PROD
10. Merge jenkins-upgrade PR to the tools branch
11. Delete the jenkins-upgrade branch
12. TBD - delete the PR based build of Jenkins using Openshift console