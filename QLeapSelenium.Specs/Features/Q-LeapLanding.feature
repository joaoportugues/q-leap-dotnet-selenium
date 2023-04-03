
Feature: Q-Leap Landing

#Scenario: Add two numbers
	#Given the first number is 50
	#And the second number is 70
	#When the two numbers are added
	#Then the result should be 120


#Scenario Outline: Add two numbers permutations
	#Given the first number is <First number>
	#And the second number is <Second number>
	#When the two numbers are added
	#Then the result should be <Expected result>

#Examples:
	#| First number | Second number | Expected result |
	#| 0            | 0             | 0               |
	#| -1           | 10            | 9               |
	#| 6            | 9             | 15              |

@run
Scenario: Q-Leap Test
	Given I navigate to 'https://q-leap.eu/'
	And I click 'training' on 'Login'
	Then I read 'Our Training Catalogue' on element 'trainingCatalog' on 'Training'
	And I click 'contact' on 'Login'
	And I type 'Test' on element 'firstName' on 'Contact'

@run
Scenario: Q-Leap Test2
	Given I navigate to 'https://q-leap.eu/'
	And I click 'training' on 'Login'
	Then I read 'Our Training Catalogue' on element 'trainingCatalog' on 'Training'
	And I click 'contact' on 'Login'
	And I type 'Test' on element 'firstName' on 'Contact'