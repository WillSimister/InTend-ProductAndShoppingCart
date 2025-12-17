# InTend-ProductAndShoppingCart

Thank you for the opportunity to complete the technical test. In this document, you will find instructions on how to run the application, a brief overview of design pattern decisions, and an overview of assumptions made about the broader requirements.

## Running the Application

You can either clone the repo from this repository or, alternatively, I will have sent over a Zip file. This should be opened in Visual Studio. All NuGet packages should install automatically on build.

1. In Visual Studio, ensure that the project `InTend-ProductAndShoppingCart` is set as the startup project.
2. Ensure you are about to run the application with HTTPS.
3. Run the application. This will launch the Swagger UI, where you can interact with the endpoints.  
   (Alternatively, the endpoints can be called from a browser, Postman, ThunderClient, or other similar apps.)

## System Overview

This is a closed system with an in-memory repository for both a collection of products and a shopping cart.

### Products
- Functionality is provided to view a collection of all products or to get a product by its ID.

### Shopping Cart
- Functionality is provided to view your shopping cart, add a product to your shopping cart, remove a product from your cart, increase and decrease a product’s quantity, and finally clear your cart.

## Endpoints and Their Usage

There are two controllers created: `ProductController` and `ShoppingCartController`.

### Product Controller
- **[GET]** `/Product/AllProducts`  
  Will return a list of all products.
  
- **[GET]** `/Product/ProductById/{ProductID}`  
  Will return a single product (if the ID is valid, otherwise it will return an error).

### Shopping Cart Controller
- **[GET]** `/ShoppingCart/ShoppingCart`  
  Will return the current state of the shopping cart.
  
- **[POST]** `/ShoppingCart/AddItemToCart?productId={ProductID}&quantity={Quantity}`  
  Will add the specified quantity of an item to the shopping cart. If the quantity is not specified, it defaults to 1. If the item already exists in the cart, the quantity will be increased accordingly.

- **[POST]** `/ShoppingCart/RemoveItemQuantityFromCart/{ProductID}?quantity={Quantity}`  
  Will remove the specified quantity of an item from the shopping cart.

- **[DELETE]** `/ShoppingCart/RemoveItemFromCart/{ProductID}`  
  Will remove all of an item from the cart.

- **[DELETE]** `/ShoppingCart/ClearCart` 
  Will remove all items from the cart.

## List of Endpoints with Queries to Test in Order

1. **Get All Products**  
   `GET` [https://localhost:7101/Product/GetAll](https://localhost:7101/Product/GetAll)
   
2. **Get a Single Product by ID**  
   `GET` [https://localhost:7101/Product/GetById/48d57da5-ec8d-463c-a25a-12f989d68c8c](https://localhost:7101/Product/GetById/48d57da5-ec8d-463c-a25a-12f989d68c8c)
   
3. **Get Shopping Cart**  
   `GET` [https://localhost:7101/ShoppingCart/GetShoppingCart](https://localhost:7101/ShoppingCart/GetShoppingCart)
   
4. **Add Product to Cart (5 times)**  
   `POST` [https://localhost:7101/ShoppingCart/AddItemToCart?productId=48d57da5-ec8d-463c-a25a-12f989d68c8c&quantity=5](https://localhost:7101/ShoppingCart/AddItemToCart?productId=48d57da5-ec8d-463c-a25a-12f989d68c8c&quantity=5)
   
5. **Get Shopping Cart**  
   `GET` [https://localhost:7101/ShoppingCart/GetShoppingCart](https://localhost:7101/ShoppingCart/GetShoppingCart)

6. **Add 1 Product to Cart**  
   `POST` [https://localhost:7101/ShoppingCart/AddItemToCart?productId=48d57da5-ec8d-463c-a25a-12f989d68c8c](https://localhost:7101/ShoppingCart/AddItemToCart?productId=48d57da5-ec8d-463c-a25a-12f989d68c8c)
   
7. **Get Shopping Cart**  
   `GET` [https://localhost:7101/ShoppingCart/GetShoppingCart](https://localhost:7101/ShoppingCart/GetShoppingCart)

8. **Remove 2 of the Product from Cart**  
   `PATCH` [https://localhost:7101/ShoppingCart/RemoveItemQuantityFromCart/48d57da5-ec8d-463c-a25a-12f989d68c8c?quantity=2](https://localhost:7101/ShoppingCart/RemoveItemQuantityFromCart/48d57da5-ec8d-463c-a25a-12f989d68c8c?quantity=2)
   
9. **Get Shopping Cart**  
   `GET` [https://localhost:7101/ShoppingCart/GetShoppingCart](https://localhost:7101/ShoppingCart/GetShoppingCart)
   
10. **Remove Product Completely from Cart**  
    `DELETE` [https://localhost:7101/ShoppingCart/RemoveItemFromCart/48d57da5-ec8d-463c-a25a-12f989d68c8c](https://localhost:7101/ShoppingCart/RemoveItemFromCart/48d57da5-ec8d-463c-a25a-12f989d68c8c)
    
11. **Get Shopping Cart**  
    `GET` [https://localhost:7101/ShoppingCart/GetShoppingCart](https://localhost:7101/ShoppingCart/GetShoppingCart)

## Assumptions Made About Broader Requirements

1. It is assumed that the front-end is available to hold collections of products, which can be selected, with an associated UI for these actions. These components are assumed to know their item GUID.
2. It is assumed that a database will eventually become available and the top of the repository classes can be cleaned up accordingly.
3. It is assumed that product images are not required for this system.
4. It is assumed that products do not have categories or any other special types, and all products in the requirements are treated equally.
5. It is assumed that the system will be used only in the UK, so currency and currency calculation are not concerns.
6. It is assumed that there are no special offers or special offer systems.

## Design Pattern Justification

For this project, I have opted for the **Repository Pattern** to handle data access. This pattern lends itself well to CRUD systems and is easy to maintain. It also facilitates Dependency Injection (DI) for testing purposes.

Another reason to use the Repository Pattern is that the system, though simple now, has the potential to grow and become more complex. The Repository Pattern helps keep the codebase clean and maintainable, which is crucial for scalability.

This design pattern also accommodates future feature requirements. While developing, I considered adding discount and currency handlers. These could be added as modules/services, and the `GetTotal` method on the repository could operate via these services to handle such functionality, all without significantly altering the current implementation.

I have chosen to completely decouple the application, as I usually do in my current approach, even though this might not have been necessary for such a small system.

### Application Layers

1. **Controllers**  
   My philosophy for writing simple code is that a controller should be "stupid", essentially a thin wrapper around an API. The controllers here request repositories via DI, create APIs with the repositories, and then ask the API for what they need.

2. **Business Layer**  
   The API in the business layer should only orchestrate services. Some logic is acceptable, but I like to keep this layer as a thin wrapper for services. APIs orchestrate controller requests to necessary handlers and retrievers, preventing them from directly interacting with the data layer. This loose coupling allows us to modify the API without affecting the repository, and vice versa.

   The handler and retriever code for `Product` and `ShoppingCart` could be combined into one file per concept as a larger service, but I prefer separating concerns into small, readable, and maintainable chunks.

3. **Data Layer**  
   The data layer contains only repositories. These would interface with an ORM if a real database were implemented. If I had more time, I would redesign this layer to use a query handler pattern.

4. **Common Project**  
   I've also created a small project called `.common`, where I added a custom exception. This isn't strictly necessary, but it does provide more context to exceptions compared to the generic ones.

## Testing

I have set up two unit test files, one for each API. However, I have not spent as much time on them as I would have liked, as I ended up working on this project longer than I had anticipated.

If I had more time, I would set up testing context builders and methods in the repositories to allow for more control over the product store. Additionally, I would create common testing data builders for frequently used scenarios.

Apologies if the tests are slightly messy. They are not exhaustive, and not all files that should have tests have them at this stage due to time constraints.
