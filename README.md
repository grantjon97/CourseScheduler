# CourseScheduler
A single-page web app that helps prospective students plan a 4-year university schedule.

## Getting Started

Follow these instructions to get the project running on your machine:

### Prerequisites

1. A computer with Windows (strongly suggested)
2. [Microsoft Visual Studio](https://visualstudio.microsoft.com/vs/) (Community Edition 2017 or higher)
3. A basic understanding of object-oriented programming

### Installing Visual Studio

Follow the instructions given by Microsoft to install VS.<br/>
When you get to a window called "Workloads" you will want to checkmark 2 packages:

![NET-pic-1](https://user-images.githubusercontent.com/17295231/56679574-04e34000-668b-11e9-980d-e6db4c7d0e7b.PNG)
![NET-pic-2](https://user-images.githubusercontent.com/17295231/56679612-188ea680-668b-11e9-8d6c-0d623c8a12fb.PNG)

These packages include libraries that are used within the controllers of the project.

### Cloning the project

Copy this URL to your clipboard: ```https://github.com/grantjon97/CourseScheduler.git``` <br/>
Open Visual Studio. Go to ```View -> Team Explorer```. <br/>
On the right-hand side of your window, you should see a box that looks like this:

![test-repo-before](https://user-images.githubusercontent.com/17295231/56680228-7bcd0880-668c-11e9-8742-a7a3a84a4d58.PNG)

The number of Local Git Repositories might be different for you, and that's okay.<br/>
Click clone, and paste the URL that you copied before into the input box.<br/>
A local repository file location will automatically be generated for you. You can rename the repository whatever you want.

![test-repo](https://user-images.githubusercontent.com/17295231/56680154-5213e180-668c-11e9-9e30-97c438252b19.PNG)

After cloning, you will have access to all project files in the Solution Explorer.<br/>
Double click CourseSchedulingTool.sln, found near the bottom of the Solution Explorer.

### Installing Entity Framework

Inside the project, go to Tools -> NuGet Package Manager -> Package Manager Console.<br/>
The console will appear at the bottom of the page. Make sure that the Default Project is CourseSchedulingTool.<br/>
Inside the console, type the following lines:

1. ```Install-Package EntityFramework```
2. ```Enable-Migrations```
3. ```Update-Database Initial-Migration```

The tables for the project will automatically be created.  They will need to be populated with some sample data<br/>
in order for the web application to make any sense. To do that, go to ```View -> SQL Server Object Explorer```.</br>
In the window, go to ```(localdb)\MSSQLLOCALDB -> Databases -> CourseSchedulingTool -> Tables```.<br/>
You will see a list of the tables created. You can right click and view data on each of them, and then fill in sample data.<br/>

### Testing the project

Navigate to ```index.html```.<br/>
Press ```F5``` to run the project in debug mode, or ```CTRL + F5``` to run the project without debug.
