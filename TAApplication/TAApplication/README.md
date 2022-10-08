```
Author:     Noah Carlson
Partner:    Sebastian Ramirez
Date:       8-October-2022
Course:     CS 4540, University of Utah, School of Computing
GitHub ID:  Smirqy, sebastianerz28
Repo:       https://github.com/uofu-cs4540-fall2022/taapplication-fullstring.git
Commit Tag: PS5
Project:    TA Application
Copyright:  CS 4540 and Noah Carlson, Sebastian Ramirez - This work may not be copied for use in Academic Coursework.
```
# Website Links:
Noah - https://ec2-44-203-198-132.compute-1.amazonaws.com
Sebastian - https://ec2-3-95-151-197.compute-1.amazonaws.com/

# Overview of the TA Application Functionality 

The TA Application program is currently capable of taking applicants' applications and storing them in a database a long with taking file uploads of resumes and profile pictures. The most recent work was adding applications to the database as well as adding file uploads
for the applicants. 

# Comments to Evaluators:

-na

# File/Image Uploads

We completed the file uploads.

# ModificationTracking

The ModificationTracking code just gives the admins and dev's more information about the items being added to the database. Knowing when something was and added and by who helps with debugging and security checking.
If something were to get messed up or somebody starts messing with the database being able to trace back to when and who was doing these changes is extremely useful. Also shows if applications are up to date so if 
an application has not been updated in a long time it may be removed in order to save space. The modificationtracking code happens in the applicationdbcontext. All the entries that were modified are grabbed and either the user,
dbseeder, or if not logged in sadness is used as the user. Then all the changed entries are looped over and the modification tracking properties are updated with the previous information.

# Improvements:

-Added functionality for replacing the resume and profile picture because it seemed like an essential feature.

# Consulted Peers:

We did not speak to any peers other than the TA's because we did not need the help

# Peers Helped:

None, No one asked for help

# Acknowledgements:

All images were free art commons.

# References:

    1. Bootstrap - https://getbootstrap.com
    2. Stack Overflow - https://stackoverflow.com/questions/12763344/removing-bold-styling-from-part-of-a-header
    3. All logos owned by the University of Utah - https://brand.utah.edu/secure/university-of-utah-logo-downloads/
    4. Colors used - https://utah.edu
    5. Stack Overflow - https://stackoverflow.com/questions/19689183/add-user-to-role-asp-net-identity/56737271#56737271
    6. University of Utah CS4540 slides - https://utah.instructure.com/courses/805543

# Time Expenditures:

    1. Assignment One: Predicted Hours: 8 Actual Hours: 9
    2. Assignment Two: Predicted Hours: 8 Actual Hours: 10
    3. Assignment Three: Predicted Hours: 8 Actual Hours: 7
    4. Assignment Four: Predicted Hours:  9 Actual Hours:  10 
    5. Assignment Five: Predicted Hours: 12 Actual Hours: 10/11
