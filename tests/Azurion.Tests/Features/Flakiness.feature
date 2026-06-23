Feature: Surviving flaky hardware
    Real motorised axes take time to move, settle a little late, and rest a hair off the
    exact target. A script that reads state the instant it sends a command is reading the
    system mid-flight — the number-one cause of "flaky" hardware tests. The cure is always
    the same: wait until the system settles, then assert with a sensible tolerance.

    These scenarios all pass — including under the harshest flakiness — precisely because
    they follow that discipline. Flip the dashboard's flakiness toggle to "Harsh" while they
    run and watch the arm overshoot and drift, yet still land within tolerance.

    Background:
        Given the system is reset

    Scenario: Why a script must wait for the hardware
        When I command StandRotation to 30 degrees
        And I read the state immediately
        # Asserting the target right here would be flaky — the arm is still travelling.
        Then StandRotation has not arrived at 30 yet
        When I wait for the system to settle
        Then StandRotation is at 30 within 0.05

    Scenario: A robust assertion survives harsh flakiness
        Given the flakiness profile is "harsh"
        When I command StandRotation to 30 degrees
        And I wait for the system to settle
        # Generous tolerance absorbs the jitter and overshoot the harsh profile injects.
        Then StandRotation is at 30 within 2.0
        And the system is settled

    Scenario: The wait-then-assert pattern holds across several axes at once
        Given the flakiness profile is "harsh"
        When I command StandRotation to -45 degrees
        And I command StandAngulation to 20 degrees
        And I command TableHeight to 105 centimetres
        And I wait for the system to settle
        Then StandRotation is at -45 within 2.0
        And StandAngulation is at 20 within 2.0
        And TableHeight is at 105 within 2.0

    Scenario Outline: Robust positioning holds for many oblique angles under harsh flakiness
        Given the flakiness profile is "harsh"
        When I command StandRotation to <angle> degrees
        And I wait for the system to settle
        Then StandRotation is at <angle> within 2.0

        Examples:
            | angle |
            | 15    |
            | -30   |
            | 60    |
            | -90   |
