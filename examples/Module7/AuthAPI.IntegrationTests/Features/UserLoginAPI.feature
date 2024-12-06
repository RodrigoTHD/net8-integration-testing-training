Feature: User Login API

Scenario: Successful Login
    Given a user with username "validUser" and password "validPass"
    When the user requests the "LoginUser" endpoint
    Then the response should be 200 OK
    And the response should contain a token

Scenario: Invalid Credentials
    Given a user with username "wrongUser" and password "wrongPass"
    When the user requests the "LoginUser" endpoint
    Then the response should be 401 Unauthorized
    And the response message should be "Invalid credentials"
