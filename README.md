# Package-Management-App 
## Designed and Implemented a Package Management Application using object-oriented analysis and design knowledge for an apartment with 5 buildings and 10 units on each. 
### In this apartment, all residents’ packages are delivered to the apartment's leasing office and residents are required to pick up their packages from the leasing office. 
### Currently,
      ●	For residents, it is hard to know when the packages are delivered to the leasing office unless they manually check online with the delivery service provider.
      ●	For the leasing office, stacks of unclaimed packages occupy extra office spaces (especially after Thanksgiving) and make a package hard to find.
### I had to develop a package management software to help the apartment improve their package management service so that current residents would be notified when their packages are delivered to the leasing office and ready for pickup.

### Software Requirements for the project:
          ●	Leasing office staff should be able to log in to the system using a username and password 
          ●	When a package is delivered to the leasing office:
                  1.	The office staff will search for the resident listed on the package labels in the application. 
                  2.	Then, the office staff selects the correct resident and agency of posting service (FedEx, USPS, UPS, etc). 
                  3.	After confirming the selected information, the software will send an email notification to this resident. Meanwhile, the package and resident information will be added to Pending Area on the software. 
                  4.	Once the resident has picked up the package, the office staff will remove the record of this picked package from the Pending Area
                  5.	If there is a package whose owner is NOT a current resident in the apartment, input the package information, including the package owner, the agency of posting service, and delivery date to the UNKNOW area on the software. And this unknown package will return to the post office later.
                  6.	The office staff can retrieve package records history for a resident based on the unit number and resident name.
      
