## Digital Call Center Platform (DCCP)

An online platform for outsourcing the call center part of a company.

Designed used ASP.NET MVC

[Documentation Link](https://docs.google.com/document/d/1zsmhNhklg1jMbu1z225hh3qGyRFdR7PDWTK3WuiTjpE)

```markdown

# Current Models

1. IdentityModels
2. AccountViewModels
3. ManageViewModels

**04-11-2018**

4. WorkPlatformModels (Customer Account General Information)
5. PhoneModels (Customer Account Phone Information)
6. NotesModels (Notes left on a specific customer account)
7. InvoiceModels (Invoices on a specific customer account)
8. AddressModels (Addresses on a specific customer account)
9. WorkPlatformAccountViewModels (View Model for the WorkPlatform page)
10. ActionsModels (List with all available action codes)
11. StatusesModels (List with all available statuses)
12. PaymentsModels (Keep track of all payments made in system)

```

```markdown

# Current Controllers

1. AccountController
2. HomeController
3. ManageController

**04-11-2018**

4. WorkPlatformController (Controller that will handle any action performed on a specific customer account)
5. BackofficeController (Controller that will handle all backoffice operation like Add accounts, post trust and post payment requests)

**05-11-2018**

6. ClientPortalController (Controller designated for client actions like Reports Audit and Payments)

```


```markdown

# Current Views

1. Account
2. Home
3. Manage
4. Shared

**04-11-2018**

5. WorkPlatform (Customer Account View)
6. Backoffice (Views for Backoffice actions)

**05-11-2018**

7. ClientPortal(Views for ClientPortal actions)

```
