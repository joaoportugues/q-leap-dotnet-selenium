Feature: Q-Leap Landing2

@run
Scenario: Q-Leap Test3
	Given I navigate to 'https://q-leap.eu/'
	And I click 'training' on 'Login'
	Then I read 'Our Training Catalogue' on element 'trainingCatalog' on 'Training'
	And I click 'contact' on 'Login'
	And I type 'Test' on element 'firstName' on 'Contact'

@run
Scenario: Q-Leap Test4
	Given I navigate to 'https://q-leap.eu/'
	And I click 'training' on 'Login'
	Then I read 'Our Training Catalogue' on element 'trainingCatalog' on 'Training'
	And I click 'contact' on 'Login'
	And I type 'Test' on element 'firstName' on 'Contact'