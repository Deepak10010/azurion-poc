Feature: Collision safety
    The C-arc wraps around the patient, so a steep angulation combined with a raised,
    laterally-offset table can drive the detector into the table. The system must detect
    this danger envelope, raise a collision fault, and halt all motion.

    Background:
        Given the system is reset

    Scenario: Driving into the danger envelope halts the system
        When I drive into the collision envelope
        Then a Collision fault is raised
        And the system is not moving

    Scenario: A steep angulation alone is safe while the table stays low
        # Angulation past the threshold, but a low, centred table keeps us clear.
        When I command StandAngulation to 70 degrees
        And I wait for the system to settle
        Then StandAngulation is at 70 within 0.05
        And no fault is raised

    Scenario: A raised table alone is safe while the stand stays shallow
        When I command TableHeight to 118 centimetres
        And I wait for the system to settle
        Then TableHeight is at 118 within 0.05
        And no fault is raised
