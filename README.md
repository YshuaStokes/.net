## Change Notes

I've followed instructions as outlined in the project. Additionally, I've seperated data access logic from the controller logic in the API, I added a request.http file to manually test endpoints, I added an enum class for time signature and added logic to the Artist details page to display the proper time signature, and finally I added some unit tests with XUnit.

### Details on How To Test Endpoints Manually(Visual Studio)
* Select the Multitracks_Api project and set as startup project.
* Press Start. There is no default page for the API so it'll just open a browser page with a 403 error. Minimize the tab.
* navigate to the HttpRequests folder in the Multitracks_Api project, open the file Request.http.
* Click "Send Request" over any of the requests and check the response in the side window that opens up.
