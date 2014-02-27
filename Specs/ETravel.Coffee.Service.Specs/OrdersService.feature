Feature: Orders Service
	As an orders service consumer
	I want to be able to perform list, create and delete operations with it

Scenario: List orders
	When I place a GET request to the orders service 
	Then I should get back all the orders currently in the database

Scenario: Create an order
	When I place a POST request to the orders service for a new order
	Then The orders service should respond with the created order
	When I place a GET request to the orders service
	Then I should get back all the orders currently in the database
	And The new order should be among them

Scenario: Delete an order with one order item
	When I place a DELETE request to the orders service for an existing order with one order item
	Then The orders service should respond with status Ok
	When I place a GET request to the orders service
	Then I should get back all the orders currently in the database
	And The deleted order should not be among them

Scenario: Delete an order with nore than one order items
	When I place a DELETE request to the orders service for an existing order with multiple order items
	Then The orders service should respond with status Ok
	When I place a GET request to the orders service
	Then I should get back all the orders currently in the database
	And The deleted order should not be among them