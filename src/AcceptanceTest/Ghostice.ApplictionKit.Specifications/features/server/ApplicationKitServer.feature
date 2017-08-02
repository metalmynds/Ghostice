Feature: ApplicationKitServer
	To test a .Net Gui application
	As an automation tester
	I need the testing to be cost efficient

@Draft @Integration @Server @
Scenario Outline: Loading the Server Takes Very Little Time
	Given I have the <UI> server installed
	And the endpoint <EndPoint> is clear
	When I start the <UI> server
	Then the server should start in under <Timeout> seconds

| UI      | Timeout | EndPoint                |
| GUI     | 5       | http://localhost:21300/ |
| Console | 2       | http://localhost:21300/ |


