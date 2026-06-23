Feature: C-arm and table positioning
    As a test engineer for the Azurion positioner
    I want to command the stand and table to defined poses
    So that the system reaches the requested geometry and reports when it has settled

    Background:
        Given the system is reset

    Scenario: Rotate the stand to a single oblique angle
        When I command StandRotation to 30 degrees
        And I wait for the system to settle
        Then StandRotation is at 30 within 0.05
        And no fault is raised

    Scenario: A relative move from the current position
        When I command StandRotation to 20 degrees
        And I wait for the system to settle
        And I command StandRotation by 15 degrees
        And I wait for the system to settle
        Then StandRotation is at 35 within 0.05

    Scenario: Position both stand axes and the table together for a working projection
        When I command StandRotation to -30 degrees
        And I command StandAngulation to 25 degrees
        And I command TableHeight to 105 centimetres
        And I command Sid to 110 centimetres
        And I wait for the system to settle
        Then StandRotation is at -30 within 0.05
        And StandAngulation is at 25 within 0.05
        And TableHeight is at 105 within 0.05
        And Sid is at 110 within 0.05

    Scenario: Returning to the home pose
        When I command StandRotation to 45 degrees
        And I command TableHeight to 115 centimetres
        And I wait for the system to settle
        And I send the system home
        And I wait for the system to settle
        Then StandRotation is at 0 within 0.05
        And TableHeight is at 95 within 0.05
