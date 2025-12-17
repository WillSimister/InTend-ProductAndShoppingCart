# InTend-ProductAndShoppingCart
Thankyou for the opportunity to complete the technical test
In this document there will be instructions on how to run the application and a brief overview of design patterns decisions and overview of assumptions made about the broader requirements.

# Running the application
Either clone the Repo from this repository, or alternatively I will have sent over a Zip file. This should be opened in Visual Studio. All Nuget packages should install on build.

1. In Visual Studio ensure that the project 'InTend-ProductAndShoppingCart' is set as the startup project.
2. Ensure you are about to run in https.
3. Run the applicaiton. This will launch the swagger UI. Here you will be able to poke at the end points. (Alternatively the emdpoints can be called from a browser, postman/ThunderClient or other similar apps.)

# System Overview
This is a closed system with an in memory repository for both a collection of products and a shopping cart.
For Products - There is functionality to view a collection of all products or to get a product by its ID.
For the Shopping Cart - There is functionality to view your shopping cart, add a product to your shopping cart, remove a product from your cart, increase and decrease a products quantity and finally to clear the your cart.

# Endpoints and their usage
There are two controllers that have been created ProductController and ShoppingCartController

# Product Controller
https://localhost:7101/Product/GetAll - Will return a list of Products
https://localhost:7101/Product/GetById/{ProductID} - Will return a single product (If the ID is valid, else will complain).

# Shopping Cart Controller
Get Cart
https://localhost:7101/ShoppingCart/GetShoppingCart - Will return the current state of the shopping cart

Add to Cart
https://localhost:7101/ShoppingCart/AddItemToCart?productId={ProductID}&quantity={Quantity} - Will add the item with specified quantity. Quantity does not need to be specified, if it is not, then 1 Stock Item will be added to the ShoppingCart. If the item already exists in the Shopping Cart, the given quantity (or 1 if no quantity specified) will be increased to the cart

Remove Quantity From Cart
https://localhost:7101/ShoppingCart/RemoveItemQuantityFromCart/{ProductID}?quantity={quantity}

Remove All of an Item from Cart
https://localhost:7101/ShoppingCart/RemoveItemFromCart/{ProductID}

# List of endpoints with queries to hit in this order for a quick test.
https://localhost:7101/Product/GetAll - Get all products 
https://localhost:7101/Product/GetById/48d57da5-ec8d-463c-a25a-12f989d68c8c - Get a single product
https://localhost:7101/ShoppingCart/GetShoppingCart 
https://localhost:7101/ShoppingCart/AddItemToCart?productId=48d57da5-ec8d-463c-a25a-12f989d68c8c&quantity=5 - Add a product to the cart (5 Times)
https://localhost:7101/ShoppingCart/GetShoppingCart
https://localhost:7101/ShoppingCart/AddItemToCart?productId=48d57da5-ec8d-463c-a25a-12f989d68c8c - Add just one of the items to the cart
https://localhost:7101/ShoppingCart/GetShoppingCart
https://localhost:7101/ShoppingCart/RemoveItemQuantityFromCart/48d57da5-ec8d-463c-a25a-12f989d68c8c?quantity=2 - Remove two of that item from the cart
https://localhost:7101/ShoppingCart/GetShoppingCart
https://localhost:7101/ShoppingCart/RemoveItemFromCart/48d57da5-ec8d-463c-a25a-12f989d68c8c - Remove the item from the cart completely
https://localhost:7101/ShoppingCart/GetShoppingCart

# Assumptions made about broader requirements
One broader requirement that is assumed in this project is that the front end is available to hold collections of products which can be selected and have associated UI for these actions, these components are assumed to know their item guid.

I have assumed that a database will at somepoint become available and the top of the repository classed can be cleaned up. 

I have assumed that in these requirements, product images are not needed.

I have assumed that the products do not have categories or any other special types and that all products in the requirements are treated equally.

I have assumed that the system is only used in the UK, so currency and currency calculation is not an issue.

I have assumed that there are no special offers or special offer systems. 

# Design pattern justification
For the overarching project I have opted for the Repository Pattern to fulfill data access. This pattern lends its self well to CRUD systems and is easy to maintain. It also makes it easy to DI the Repositories in for Testing purposes. 

Another reason to use the Repository pattern here is that it is already a nuanced and slightly complex system, which in the real world would likely grow legs and balloon. The repository pattern helps keep the code base clean and easily maintanable, which is needed for scalability.

This design pattern lends its self well to future feature requirements. Whilst developing I had ideas for both discount and currency handlers. Theses could be added as modules/services and the getTotal method on the repository could operate via these services to handle this functionality, most importantly this could be done without having to change the current implementation (Much at least).

I have opted to completely decouple the application just as it is how I always write code at the moment, it probably wasn't completely necessary for a system this small. 
- We have a Web Layer which contains Controllers and the method of DI I have opted for is controller injection and feed down.
- A business layer which contains APIs, Models, Handler Services, Retriever Services, repository interfaces and Validation Logic.
- A Data layer with concrete implementations of the Repositories.

# Application Layers - Controllers
My philosphy I try to stick to to help write simple code is that a controller should be stupid. A thin wrapper around an API. I have put this into practice here.

The controllers ask for the repositories via DI, create APIs with the repositories and then ask the API for what they need.

# Application Layers - Business

Again, an API, shouldn't do much more than orchestrate services, some logic is okay, but I like to keep this layer as thin wrappers for services.

The APIs orchestrate controller requests to necessary Handlers and Retrievers. This stops APIs from directly being in contact with the data layer, allowing for a loose decoupling and the benefits that gives us (A change in the API doesn't affect the repo and vice versa).

The Handler and Retriever code for Product and the Handler and retriever for ShoppingCart could be combined into one file per concept as a larger service, I just like seperating concerns into small readable and maintainable chunks.

The repository interfaces are required here so that proper DI can be used, as well as making the Repositories accessible to tests.

# Application Layers - Data

The data layer contains nothing much other than repositories. These are what would interface with an ORM if a real database was implemented through Queries and would slightly be redesigned to use a query handler pattern if I had more time.

# I have also created a small project called .common, in here I have added a Custom Exception as I got carried away, not necessary, but it does add a lot of context to an exception over the generic ones.

# Testing

I have set up two unit test files, one testing each api. I have opted not to go further or to put as much thought as I would have liked to into the tests as at this point I have been working on this project for a little longer than I had anticipated. If I had more time, I would like to set up testing context builders and methods in the repositories to allow me to have control over what is in the product store. And some common testing data builders for commonly used scenarios. Apologies if they are slightly messy.

The tests are not exhaustive and not all files that should have tests have tests at this stage as time is limited.







