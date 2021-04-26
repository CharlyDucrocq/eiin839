# Let's Go Biking Project - CharlyDucrocq

[Link to the drive](https://unice-my.sharepoint.com/:f:/g/personal/charly_ducrocq_etu_unice_fr/Er8T6UqdIe5PsZ9qTgPjKngBaHLF3LdS_dgSz5d8jsCmaA?e=wC7hF3)

Here can find every part of my project.
Folders info 
- MyBicycleRedirectionService  the WCF of my redirection service. It's here that the redirection will be estimated.
- WebProxyService  the WCF of the proxy. A simple proxy who will make simple REST request, save the result in a cache for a given (or default = 60s) time and return the result of the request into string.
- HeavyClient  the heavy client of my project. It's a simple console who will print all step of the trajectory.
- WebFront  Here is the SoftClient. A simple html interface which will allow you to select the start and end of the traject on a map and to display the resulting trajectory on the map.
- RunnablesServices  This contain two visuals studio project made to create runnable wcf.

## How to run it 

You could find a .bat file named run_all.bat wich will run all tou need.
3 console will be open for the proxy, the service and the heavy client and 1 page will be open in your default browser for the soft client.