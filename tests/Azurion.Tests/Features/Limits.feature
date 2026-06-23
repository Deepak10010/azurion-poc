Feature: Range limits and emergency stop
    Each axis has hard travel limits, and an emergency stop must block all motion.
    These negative-path scenarios prove the system refuses unsafe commands instead of
    silently clamping or ignoring them.

    Background:
        Given the system is reset

    Scenario: Commanding past the upper limit is refused
        # StandRotation travels -120..120 degrees
        When I command StandRotation to 200 degrees
        Then an OutOfRange fault is raised
        And StandRotation is at 0 within 0.05

    Scenario: Commanding past the lower limit is refused
        When I command TableHeight to 10 centimetres
        Then an OutOfRange fault is raised
        And TableHeight is at 95 within 0.05

    Scenario: A valid command at the very edge of travel is accepted
        When I command StandRotation to 120 degrees
        And I wait for the system to settle
        Then StandRotation is at 120 within 0.05
        And no fault is raised

    Scenario: Emergency stop blocks subsequent commands until reset
        When I trigger an emergency stop
        And I command StandRotation to 30 degrees
        Then an EmergencyStop fault is raised
        And StandRotation is at 0 within 0.05
