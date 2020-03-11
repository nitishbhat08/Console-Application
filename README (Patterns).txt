1] Facade Pattern:

*Using Facade:

We've used this pattern because it hides the details of fetching data from the JSON. If someone needs to use these methods, variables (Insert Login/Customer/Transaction) then they cannot do it directly using the instance. So, we have added the reference of this assembly file in Program.cs. Using that, we can call the methods (Library.Facade.GetAndSaveCustomers, Library.Facade.GetAndSaveLogins). This ensures data hiding to limit the content availability.

*Advantages:

Facade created multiple entry points to the system. 
Reduced coupling as subsystems can communicate only using facades.
It offered code isolation from the complexity.

*If Facade wasn't used:

All the controllers were looking bloated before implementing facade. It provided Code elegence. Because of the keyword "internal", we could clearly see, that only classes within the same namespace would be able to use the functions.

Dependency of external code would've been much higher without it.

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

2]MVC Pattern:

*Using MVC:

We've used MVC pattern to separate the input, processing and output of the program. The Controllers present in our Program (Account/Customer/Transaction/Login) handles the requests, retrieves the fields it needs from the Model classes to display the end result to the user via View (MainMenu, CustomerMenu).

As we started off by directly seperating MVC, the updated code was much easy to understand even if some changes were done from the partner's end.

*Advantages:

Development of the application was faster because of parallel development. We divided the work, where one was working on the brain function (controller) while other was just focused on handling errors and displaying the information to the front end.

Modification of the the View was made easier. We made updates to the View much later as per our controllers' logic. 

*If MVC wasn't used:

The entire program would have looked unclustered. 

Migration towards Assignment 2 would be a lot harder without the unclassified MVC.

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


