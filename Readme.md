# Cricket Statistics Database

## Overview

This application provides the ability to store cricket scorecards for a team, and then calculate statistics
based upon these stored matches.

## Setup

If one wishes to setup and build the solution:

- First clone the repo into a new folder
- Run the command `git submodule init`
- Run the command `git submodule update --recursive`

Now building the solution using dotnet will work. Alternatively, executing the script at
`BuildProcess/build.ps1` will build the solution, ensure all tests pass, and output a published
 version into a folder at the same level as the root of the repo. This folder can be changed in the build.config file.