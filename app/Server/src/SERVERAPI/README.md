# Updating Static Code table lookup values

Static Codes, or 'Book Values' can be updated by authorized Product Owners, in addition to any of the developer team.  The steps to do so are as follows:

1. Ensure you have the appropriate permissions, by requesting 'Write' access from one of the repository admins
    - Backup admins include [Clecio Varjao, DevOps Chapter Lead for NRM](mailto:Clecio.Varjao@gov.bc.ca?subject=AGRI-NMP%20GitHub%20Access) or [Gary Wong, Agile Architect Owner for NRM](mailto:Gary.T.Wong@gov.bc.ca?subject=AGRI-NMP%20GitHub%20Access)

2. From the branch dropdown box (top left of the main repo [page](https://github.com/bcgov/agri-nmp)), select the "Development" branch

![Set branch to Development](https://gist.githubusercontent.com/garywong-bc/73db443ac647548ff26b2dded48e74f0/raw/315e1dccbcedf78f954242af91c86eb320c461d9/agri-nmp.static-code.1.png)

3. Edit the [static.json](https://github.com/bcgov/agri-nmp/blob/Development/app/Server/src/SERVERAPI/Data/static.json)  file in this branch

    - There are two options to edit the static data (pencil icon at right)
        - Click “edit this file” then copy and paste to json editor (recommended), or
        - Click “edit this file” and make changes directly to the Github file
    - If there is a corresponding Excel spreadsheet for the static data, ensure the changes have been done there *first*, and use the spreadsheet as the source for the data

4. After making the changes, under “staticDataVersion” on [line 5](https://github.com/bcgov/agri-nmp/blob/Development/app/Server/src/SERVERAPI/Data/static.json#L5), update the version number to Year.Commit#

    - as an example: `2017.591` -> `2017.595`
    - the Commit# should be the total number of ‘commits’ for the entire app, including changes to the static data file and other files: see top left of main repo [page](https://github.com/bcgov/agri-nmp) (e.g. **952** Commits)

5. Click **Preview changes** (tab next to the `<> Edit File` tab) to review your changes
6. Scroll down and enter a logical title to what you did (and if more details are warranted, add a more in-depth description in the “add an optional extended description”)
7. Click 'Commit Changes' which will save your changes to the “Development” branch, and then initiate a pull request into the “master” branch
    - if there is a pull request from the Development branch already in the queue, the changes made will not require a new pull request; the changes will automatically occur after committing

![New PR](https://gist.githubusercontent.com/garywong-bc/73db443ac647548ff26b2dded48e74f0/raw/315e1dccbcedf78f954242af91c86eb320c461d9/agri-nmp.static-code.2.png)

8. Wait for the pull request or the new commit (if a pull request is already in progress) to go to the “dev” stage in the Jenkins pipeline
    - once it has been pushed into “Dev” on Jenkins, to test your changes on the DEV environment, go to the unique dev URL (e.g. https://nmp-dev-pr-193-agri-nmp-dev.pathfinder.gov.bc.ca/) that has been automatically created
    - you can find this URL by clicking on the pull requests tab at the top then clicking “deployed”; Note that the `193` is an example and this number will match *your* Pull Request number

![Navigate to Pipeline](https://gist.githubusercontent.com/garywong-bc/73db443ac647548ff26b2dded48e74f0/raw/315e1dccbcedf78f954242af91c86eb320c461d9/agri-nmp.static-code.3.png)

9. Test the updated file by opening your web browser and navigate to the test URL page, which as `/Home/ValidateStaticData` at the end (e.g. https://nmp-dev-pr-193-agri-nmp-dev.pathfinder.gov.bc.ca/Home/ValidateStaticData)

    - If there are errors with the parent-child relationship in the static data, the errors will be listed
10.	 Once tested by the Product Owner in Dev (i.e. “done-done”), deploy from dev to test in Jenkins 
    - https://jenkins-agri-nmp-tools.pathfinder.gov.bc.ca/
    - Find the pull request in the bcgov-agri-nmp pipeline
11.	This is the standard TEST URL: https://nmp-test-agri-nmp-test.pathfinder.gov.bc.ca/
    - You may do some exploratory testing by navigating the application, being sure to at least visit the page that uses the updated static code values. 
12. If the changes are ready for prod, accept the pull request into prod from the Jenkins site. 

![Accept PR](https://gist.githubusercontent.com/garywong-bc/73db443ac647548ff26b2dded48e74f0/raw/315e1dccbcedf78f954242af91c86eb320c461d9/agri-nmp.static-code.4.png)

13. Approve merge of changes in GitHub by going to the “pull request” tab and approving the merge
