# Project Ideas — Complex-Machine Simulators for Test-Automation Practice

A catalog of follow-on projects modelled on **Azurion** (this repo). Every idea reuses the same
recipe, so your skills compound rather than reset each time:

> **a software simulator of a complex real-world machine (the *system under test*) + a REST API + a
> live dashboard to *watch* it move + a BDD test framework (Reqnroll / Gherkin) that drives it** —
> with each project teaching one **distinct test-automation lesson**.

Azurion's signature lesson is *flakiness → wait-until-settled + tolerance*. The value of a second
project isn't a different machine — it's a different lesson. **Pick by the italic lesson tag, not by
the machine.** Each entry below is formatted:

`**Machine — what's interesting** *(the distinct testing lesson it teaches)*`

---

## 🏥 Healthcare

1. **Infusion pump / syringe driver** — flow rate over time, occlusion & air-in-line alarms *(rate-over-time + alarm latency)*
2. **Ventilator** — breath cycles, PEEP, trigger sensitivity *(cyclic waveform verification)*
3. **Dialysis machine** — fluid balance over a multi-hour session *(long-horizon / accelerated clock)*
4. **MRI scanner** — gradient coils, patient table, quench safety *(interlock + sequence)*
5. **CT scanner** — gantry rotation synced to table indexing, dose modulation *(rotational sync)*
6. **Linear accelerator (LINAC radiotherapy)** — beam, MLC leaves, gantry angle, dose interlocks (the Therac-25 lineage) *(safety interlocks)*
7. **Robotic surgical arm (da Vinci-style)** — multi-joint, tremor filtering, motion scaling *(filtering + scaling)*
8. **Anesthesia delivery machine** — gas blending, vaporizer, fresh-gas flow *(mixture-ratio control)*
9. **Heart-lung / cardiopulmonary bypass** — pump flow, oxygenation *(continuous-perfusion safety)*
10. **Multi-parameter patient monitor** — ECG/SpO₂/NIBP, alarm prioritization *(alarm prioritization)*
11. **Defibrillator / AED** — rhythm detection, charge, shock advisory *(decision logic + charge timing)*
12. **Closed-loop insulin pump (artificial pancreas)** — CGM feedback, basal/bolus *(closed-loop control safety)*
13. **Pharmacy dispensing robot** — pick correct drug, count, audit log *(pick accuracy + audit trail)*
14. **Clinical lab analyzer / liquid handler** — pipetting, carousel, barcode tracking *(throughput + sample tracking)*
15. **Centrifuge** — ramp to speed, imbalance detection, lid interlock *(imbalance abort)*
16. **PCR thermocycler** — temperature ramp cycles *(thermal-cycling timing)*
17. **Autoclave / sterilizer** — temp/pressure cycle validation *(cycle validation)*
18. **Neonatal incubator** — temp/humidity/O₂ regulation *(environment regulation)*
19. **Automated NIBP cuff** — oscillometric inflate/deflate *(pressure ramp + measurement window)*
20. **Powered exoskeleton / prosthetic joint** — gait phase, fall detection *(gait-phase control)*
21. **Endoscope robot** — articulation, insertion limits *(articulated-motion limits)*
22. **Hospital delivery robot (TUG) / pneumatic tube** — routing, obstacle stop *(routing + obstacle)*
23. **Hyperbaric chamber** — pressurization profile *(pressure-ramp safety)*

## 🎓 Education / training & lab machines

1. **Flight-sim motion platform (Stewart hexapod, 6-DOF)** — six actuators → one cockpit pose *(coordinated multi-actuator / inverse kinematics)*
2. **Driving-simulator motion rig** — motion cueing within travel *(washout/cueing verification)*
3. **Observatory telescope mount (equatorial)** — sidereal tracking, meridian flip *(continuous tracking + interlock event)*
4. **Planetarium dome projector** — star-field positioning *(coordinate transforms)*
5. **Educational 6-DOF robot arm** — teaching kinematics *(forward/inverse kinematics)*
6. **3D printer (makerspace FDM)** — G-code, extruder temp, bed leveling *(additive motion + thermal)*
7. **Laser cutter/engraver** — gantry, power, focus, door interlock *(power-vs-speed + safety door)*
8. **CNC mill (school workshop)** — multi-axis G-code, tool change *(tool-path + tool-change sequence)*
9. **Wind tunnel (engineering lab)** — fan speed, airflow setpoint *(setpoint + measurement)*
10. **Inverted pendulum / cart-pole rig** — classic control demo *(stabilization control)*
11. **Scanning electron microscope stage** — nano-positioning, vacuum, beam *(nano-positioning + vacuum interlock)*
12. **University research reactor (TRIGA-style)** — control rods, scram *(reactivity control + emergency shutdown)*
13. **Smart-campus building automation** — multi-zone HVAC *(multi-zone control)*
14. **Library RFID book-sorting robot** — route + inventory *(routing + inventory reconciliation)*
15. **Autonomous campus shuttle / golf cart** — path follow, safety stop *(path-following + e-stop)*
16. **Patient-simulation manikin (SimMan)** — vitals model, drug response, scenario scripting *(physiological model + scenario scripting)*
17. **Instrumented gait treadmill + force plates** — belt speed, measurement sync *(belt control + data sync)*
18. **Resistance ergometer (sports-science lab)** — resistance setpoint *(setpoint + measurement)*
19. **Haptic / force-feedback glove** — force rendering *(force-feedback control)*
20. **Robotic observatory dome** — shutter + mount coordination *(multi-subsystem coordination)*
21. **Pen plotter / vinyl cutter** — 2-axis path *(path accuracy)*
22. **Vacuum-tube particle-accelerator teaching model** — beam steering magnets *(beam control)*

## 🛰️ Space tech

1. **Mars rover drivetrain** — terrain slip, hazard stop, Earth–rover light-delay *(commanding under comms latency)*
2. **Satellite attitude control (reaction wheels)** — 3D momentum, desaturation *(momentum/quaternion control)*
3. **Rocket-engine test stand** — valve sequencing, auto-abort *(sequence + abort triggers)*
4. **Thrust-vector-control gimbal** — engine steering loop *(gimbal control loop)*
5. **Solar-array deployment** — hinges, latches, one-shot deploy *(deployment-sequence verification)*
6. **Docking / berthing approach** — relative nav, closing rate, abort *(relative-position tolerance + abort)*
7. **Robotic arm (Canadarm-style)** — long-reach capture *(long-reach kinematics)*
8. **Reaction control thrusters (RCS)** — pulse firing, propellant budget *(pulse control)*
9. **Star tracker / sun-sensor attitude determination** — sensor fusion *(estimation/sensor fusion)*
10. **Ground-station tracking dish** — track across sky, keyhole *(tracking + keyhole handling)*
11. **Cryogenic propellant loading** — chill-down, fill, boil-off *(thermal/fluid sequence)*
12. **Entry-descent-landing sequencer** — parachute, retro, leg deploy *(event-triggered timing chain)*
13. **Spacecraft thermal control** — radiators, louvers, heaters *(thermal regulation)*
14. **Ion / electric thruster** — slow continuous low-thrust *(long-duration control)*
15. **CubeSat detumble (B-dot) + magnetorquers** — kill tumble after deploy *(detumbling control)*
16. **Sample-collection drill / arm (Perseverance)** — contact, force *(contact/force control)*
17. **Spacecraft power system** — solar + battery + load shedding *(power budget + load shed)*
18. **Reentry guidance (bank-angle control)** — trajectory shaping *(trajectory control)*
19. **Time-tagged command scheduler (uplink)** — onboard command queue *(time-tagged execution)*
20. **Fine-pointing telescope (JWST-style)** — jitter, stability *(fine-pointing stability)*
21. **Stage-separation mechanism** — pyro/event timing *(separation event verification)*
22. **Mars helicopter rotor** — flight in thin atmosphere *(flight control in low density)*

## 🌾 Agriculture & farming

1. **Autonomous tractor** — GPS A–B lines, headland turns *(path tracking in a tolerance corridor)*
2. **Combine harvester** — header, threshing, grain tank, unload-on-the-go *(coordinated subsystems + throughput)*
3. **Greenhouse climate controller** — temp/humidity/CO₂, slow thermal *(accelerated-clock / long-horizon control)*
4. **Center-pivot irrigation** — rotation, variable-rate water *(rotational coverage + VR)*
5. **Precision planter / seed drill** — seed spacing, downforce, section control *(spacing accuracy + section control)*
6. **Self-propelled crop sprayer** — nozzle rate vs ground speed, section shutoff *(rate-vs-speed + coverage)*
7. **Robotic milking machine** — teat detection, vacuum/pulsation *(per-cow concurrent state machines)*
8. **Grain dryer** — moisture + temperature over hours *(long-horizon thermal)*
9. **Silo / auger grain handling** — level, flow, jam detection *(level + jam recovery)*
10. **Fruit/veg harvesting robot** — vision pick, ripeness, gripper *(vision-guided picking)*
11. **Weeding robot (laser/mechanical)** — plant detect → actuate *(detect-and-act timing)*
12. **Poultry-house controller** — ventilation, feed, lighting *(multi-zone environment)*
13. **TMR feed mixer wagon** — weigh, mix ratio *(batch weighing ratio)*
14. **Fertigation / nutrient dosing** — pH/EC dosing loop *(dosing control)*
15. **Crop-spraying / scouting drone** — flight + spray pattern *(flight + coverage map)*
16. **Egg grading & sorting** — weigh, candle, sort into grades *(sorting + reconciliation)*
17. **Round baler** — pickup → form → wrap → eject cycle *(cyclic process sequence)*
18. **Vertical farm / hydroponic stack** — per-layer light, flow, climate *(multi-layer control)*
19. **Aquaculture feeder + water-quality** — feed + O₂/temperature *(environment + dosing)*
20. **Auto cattle drafting gate** — RFID weigh + draft *(ID + actuation)*
21. **Truck weighbridge + ticketing** — weigh, tare, record *(weigh + reconciliation/audit)*
22. **Soil-sampling robot** — sampling path + georef *(sampling-path coverage)*

## 🏦 Banking & finance

*Physical cash machines:*
1. **ATM cash recycler** — note transport, jams, cassette balance *(reconciliation invariants — every note accounted for)*
2. **Dispense-only ATM** — pick, present, retract on no-take *(dispense state machine)*
3. **Banknote sorting machine (central bank)** — count, fitness sort, shred *(classification + reconciliation)*
4. **Coin counter / sorter / wrapper** — count, sort, roll *(counting accuracy)*
5. **Check / document sorter** — MICR read, high-speed pockets, jams *(throughput + jam recovery)*
6. **Smart safe / cash deposit machine** — validate, log, provisional credit *(deposit validation + audit)*
7. **Teller cash recycler (TCR)** — behind-counter cassettes *(cassette reconciliation)*
8. **Vault door / time-lock controller** — dual-control, time windows *(time-lock interlock)*
9. **Card personalization / embossing line** — issue cards, audit *(production line + audit)*
10. **FX / bureau-de-change kiosk** — rate quote → dispense *(rate + dispense)*
11. **Self-checkout cash module** — accept/dispense change *(cash-handling state machine)*

*Financial "machines" (stateful engines with timing + invariants):*
12. **Payment settlement / clearing engine** — batch netting, T+n *(conservation-of-funds invariant)*
13. **Order-matching engine (exchange)** — price-time priority order book *(matching correctness + fairness invariants)*
14. **Authorization switch (card/POS)** — auth → hold → capture → reversal, timeouts *(transaction lifecycle + idempotency + timeouts)*
15. **Fraud / transaction-monitoring pipeline** — streaming rules, alerts *(alert latency + rule property testing)*
16. **Loan / interest-accrual engine** — daily accrual, compounding *(long-horizon financial accuracy)*
17. **ACH / wire batch processor** — cutoffs, returns, retries *(cutoff timing + idempotency)*
18. **Double-entry ledger / reconciliation system** — debits = credits *(invariant property testing)*
19. **Real-time margin / risk engine** — margin calls, liquidation cascade *(threshold + cascade behavior)*
20. **Cash-in-transit chain-of-custody** — smart bags, handoffs *(chain-of-custody integrity)*
21. **ATM-network cash forecasting & replenishment** — predict + schedule refills *(forecasting + scheduling)*

## ⚡ Energy

1. **Wind turbine** — yaw-to-wind, blade pitch, RPM, high-wind shutdown, grid sync *(protective shutdown under environment)*
2. **Solar tracker (1/2-axis PV)** — sun tracking, high-wind stow *(tracking + stow safety)*
3. **Solar inverter / MPPT** — max-power-point, anti-islanding *(control + anti-islanding protection)*
4. **Battery energy storage (BESS)** — charge/discharge, SoC, cell balancing, thermal *(SoC management + thermal protection)*
5. **EV DC fast charger** — handshake, charge curve, thermal derate *(protocol handshake + state machine)*
6. **Nuclear reactor control** — control rods, reactivity, SCRAM *(reactivity control + emergency shutdown)*
7. **Steam turbine + generator** — startup ramp, grid sync, overspeed trip *(startup sequence + overspeed trip)*
8. **Gas turbine (combined cycle)** — ignition, ramp, emissions limits *(startup sequence)*
9. **Hydro turbine + governor** — flow, frequency droop regulation *(frequency-droop control)*
10. **Grid AGC / frequency regulation** — balance generation to load *(real-time frequency balancing)*
11. **Substation protection relay / breaker** — fault detect, trip, auto-reclose *(protection logic + trip timing)*
12. **Transformer on-load tap changer** — stepwise voltage regulation *(stepwise regulation)*
13. **Pumped-hydro storage** — pump vs generate modes *(mode switching)*
14. **Microgrid controller** — island ⇄ grid-connect transition, source dispatch *(mode transition + dispatch)*
15. **Demand-response / smart-meter controller** — load shedding on signal *(load shed + scheduling)*
16. **Pipeline pump + valve SCADA** — pressure control, leak detection *(pressure control + leak alarm)*
17. **Boiler combustion control** — air/fuel ratio, drum level *(ratio + level control)*
18. **Electrolyzer (green hydrogen)** — current, temp, gas purity *(process control + purity)*
19. **Fuel-cell stack** — flow, temperature, power *(stack control)*
20. **LNG liquefaction / regas** — cryogenic process sequence *(thermal-process sequence)*
21. **District heating network** — flow/temperature balancing across nodes *(network balancing)*
22. **Diesel backup genset + transfer switch / UPS** — auto-start on outage, failover *(failover timing)*
23. **Wind-farm SCADA / curtailment** — coordinate a fleet, curtail on grid signal *(fleet coordination + curtailment)*
24. **HVDC converter station** — AC↔DC conversion control *(power-conversion control)*

## 💧 Water management

1. **Drinking-water treatment plant** — coagulation → flocculation → sedimentation → filtration → chlorination *(multi-stage process pipeline + dosing)*
2. **Wastewater treatment plant (activated sludge)** — aeration with dissolved-oxygen control, clarifiers, return sludge *(slow biological-process control / accelerated clock)*
3. **Reverse-osmosis desalination plant** — high-pressure pumps, membrane flux, permeate salinity, fouling *(pressure + flux control + fouling degradation)*
4. **Water distribution network / booster pumps** — pressure zones, demand swings *(network pressure balancing)*
5. **Dam / spillway gate controller** — reservoir level, controlled flood release *(level control + flood-safety release)*
6. **Canal lock / sluice gate** — chamber fill/drain, level equalization before doors open *(level-equalization interlock sequence)*
7. **Stormwater pump station** — wet-well level, lead/lag pumps, anti-short-cycle *(level-based pump cycling + anti-cycle guard)*
8. **Sewage lift station** — duty/standby pumps with alternation, dry-run protection *(duty/standby alternation + failover)*
9. **Chemical dosing / chlorination system** — residual setpoint, slow feedback *(closed-loop dosing with deadtime)*
10. **UV disinfection reactor** — dose as a function of flow and lamp intensity *(dose-vs-flow verification)*
11. **Water-main leak detection (SCADA)** — pressure transients, night-flow analysis *(transient / anomaly detection)*
12. **Elevated tank / water tower level control** — pump fill, hysteresis band *(setpoint + hysteresis control)*
13. **Storm-surge / flood barrier (Thames Barrier / MOSE-style)** — deploy on surge forecast *(forecast-triggered deployment)*
14. **Rapid sand filter backwash sequencer** — filter run → backwash → settle cycle *(cyclic process sequence)*
15. **Membrane bioreactor (MBR)** — permeate flux, periodic backwash/relax *(cyclic backwash scheduling)*
16. **Anaerobic digester (biogas)** — temperature, pH, gas production over days *(slow bioprocess control + long-horizon)*
17. **Sludge dewatering (belt press / centrifuge)** — feed, polymer dosing, cake solids *(ratio control + process cycle)*
18. **Aeration blower controller** — energy-optimized DO across basins *(control optimization across zones)*
19. **Cooling tower** — fan speed, makeup water, blowdown, biocide dosing *(thermal + water-chemistry coupling)*
20. **Groundwater well field + aquifer recharge** — drawdown limits, recharge balance *(drawdown/recharge constraint control)*
21. **Smart water metering / district metering area (DMA)** — consumption, minimum-night-flow leak detection *(usage-anomaly detection)*
22. **Pressure-management valve (PRV) / pump-as-turbine** — reduce/recover pressure in the network *(pressure regulation)*
23. **Flood-forecasting + reservoir pre-release controller** — draw down ahead of forecast rain *(forecast-driven scheduling)*
24. **Ship ballast-water treatment** — filtration + UV, IMO compliance logging *(treatment + compliance/audit logging)*
25. **Fish ladder / turbine-bypass flow split at a dam** — divert attraction flow while generating *(flow-split allocation)*

## 🔬 Semiconductor manufacturing

*Lithography & patterning:*
1. **EUV / DUV lithography scanner (stepper)** — wafer + reticle stage nano-positioning, scan sync, overlay, focus *(nanometre positioning + overlay alignment tolerance)*
2. **Photoresist coat-and-develop track** — spin speed profile, dispense timing, soft/hard bake *(spin profile + dispense timing)*
3. **EUV light source (LPP, tin droplets)** — droplet generation synced to a high-rate laser *(high-rate timing synchronization)*
4. **Mask / reticle inspection & metrology** — defect scanning, classification *(defect detection + classification)*
5. **CD-SEM / overlay metrology tool** — critical-dimension measurement *(measurement repeatability + tolerance)*

*Deposition & etch (process recipes):*
6. **Plasma etcher (RIE)** — gas flow, RF power, chamber pressure, optical endpoint *(process recipe + endpoint detection)*
7. **Chemical vapor deposition (CVD / PECVD)** — gas, temperature, film thickness *(thickness control)*
8. **Atomic layer deposition (ALD)** — cyclic precursor/purge pulse sequence *(cyclic pulse-sequence verification)*
9. **PVD / sputtering** — target erosion, vacuum, deposition rate *(rate control + target life)*
10. **Epitaxy reactor (epi)** — temperature, gas, growth rate *(growth-rate control)*
11. **Electrochemical deposition (Cu electroplating)** — current, bath chemistry *(plating-current control)*

*Thermal & implant:*
12. **Ion implanter** — beam current, energy, dose, wafer scan *(dose uniformity across the wafer)*
13. **Rapid thermal processing (RTP) / spike anneal** — fast ramp, soak, cooldown *(thermal ramp/soak timing)*
14. **Diffusion / oxidation furnace (batch tube)** — multi-zone temperature, long cycles *(multi-zone temp + long-horizon cycle)*

*Planarization & wet:*
15. **Chemical-mechanical planarization (CMP)** — pad, slurry flow, polish endpoint *(endpoint detection)*
16. **Wet bench / wet-etch station** — chemical baths, immersion timing, rinse & dry *(immersion-time control)*
17. **Chemical / slurry delivery system (CDS)** — mixing, dosing, flow to tools *(chemical-delivery dosing)*
18. **Gas cabinet + mass-flow controllers (MFC)** — flow regulation, leak detect, abatement *(flow regulation + safety interlock)*

*Handling, scheduling & test:*
19. **Cluster-tool vacuum robot** — multi-chamber wafer routing under throughput targets *(cluster-tool scheduling)*
20. **EFEM / atmospheric wafer-handling robot** — FOUP load, pick/place, slot mapping *(pick/place + slot mapping)*
21. **Load lock pump-down / vent** — pressure cycling between atmosphere and vacuum *(pump-down/vent sequence)*
22. **Wafer prober + ATE** — needle contact, X-Y indexing, test program, bin/yield *(test sequencing + binning/yield reconciliation)*
23. **Dicing saw** — blade feed, kerf, wafer-frame indexing *(feed control)*
24. **Die bonder (pick-and-place packaging)** — pick die, place to accuracy, throughput *(placement accuracy + throughput)*
25. **Wire bonder** — bond loop, force, ultrasonic energy, timing *(bond force/energy timing)*
26. **Wafer sorter** — re-arrange wafers between FOUPs, slot map *(sorting + slot-map reconciliation)*
27. **Fab AMHS (OHT overhead hoist transport)** — FOUP transport across the fab *(fab-wide transport scheduling)*

## 🚗 Automotive

*ADAS & active safety:*
1. **Anti-lock braking system (ABS)** — wheel-slip sensing, pump modulation *(closed-loop slip control + actuation timing)*
2. **Electronic stability control (ESC/ESP)** — yaw estimate, per-wheel braking *(multi-actuator stabilization)*
3. **Automatic emergency braking (AEB)** — detect → warn → brake *(decision + braking-latency verification)*
4. **Adaptive cruise control (ACC)** — radar follow, gap & speed regulation *(safe-following distance control)*
5. **Lane-keeping assist (LKA)** — lane detection, steering-torque correction *(path-correction control)*
6. **Automated parking / park assist** — ultrasonic sensing, maneuver planning *(maneuver-path execution)*
7. **Airbag / restraint controller** — crash sensing, deploy decision *(crash decision + deploy-timing window)*
8. **TPMS (tire-pressure monitoring)** — threshold alarms *(threshold-alarm verification)*
9. **Adaptive matrix-LED headlights** — beam steering, glare masking *(beam-pattern control)*
10. **Driver-monitoring system (DMS)** — drowsiness/attention detection *(detection logic)*
11. **Autonomous-driving stack (L3/L4)** — perception → planning → control *(scenario-based testing + sensor fusion)*

*Powertrain & chassis (by-wire):*
12. **Engine ECU (fuel injection / ignition)** — air-fuel ratio, spark timing maps *(ratio + timing-map control)*
13. **Automatic transmission controller** — shift schedule, torque-converter lockup *(shift sequencing)*
14. **Electric power steering (EPS)** — torque assist, return-to-center *(torque control)*
15. **Throttle / drive-by-wire** — pedal map, redundancy/limp-home *(pedal mapping + fault redundancy)*
16. **Brake-by-wire + regen blending** — friction vs regenerative blend *(brake blending continuity)*
17. **Active suspension / adaptive damping** — ride height, damping rates *(ride-control regulation)*
18. **Torque vectoring (AWD)** — distribute torque across wheels *(torque-distribution control)*
19. **Hill-hold / hill-descent control** — hold on grade, controlled release *(hold + release timing)*

*EV powertrain:*
20. **EV battery management system (BMS)** — SoC, cell balancing, thermal limits *(SoC management + thermal protection)*
21. **Traction inverter / motor control** — torque command, regenerative braking *(torque + regen control)*
22. **Onboard charger / DC-fast-charge handshake** — plug, negotiate, charge curve *(protocol handshake + state machine)*
23. **EV thermal management (heat pump / battery loop)** — cabin + pack cooling/heating *(coupled thermal control)*

*In-vehicle networks & manufacturing/test:*
24. **CAN / gateway ECU** — message routing, arbitration, OBD diagnostics *(bus arbitration + message-timing)*
25. **Body-shop welding / paint robot cell** — coordinated robots, cycle time *(coordinated multi-robot + cycle-time)*
26. **End-of-line (EOL) test stand** — actuate, measure, pass/fail, log *(test sequencing + result reconciliation)*
27. **Chassis / engine dynamometer** — load + speed control to a drive cycle (WLTP) *(load control + drive-cycle tracking)*
28. **Crash-test sled** — programmed acceleration/deceleration pulse *(acceleration-profile reproduction)*

---

## How to use this list

- **Pick by the lesson, not the machine.** A second project is only worth building if it teaches
  something Azurion can't — e.g. *commanding under comms latency* (Mars rover), *reconciliation
  invariants* (ATM cash recycler), *property/invariant testing* (traffic signal, double-entry
  ledger), or *forecast-triggered deployment* (flood barrier).
- **Reuse the Azurion structure.** The layout ports almost directly:
  `src/<Name>.Simulator` (the system under test), `src/<Name>.Web` (REST API + live dashboard),
  `tests/<Name>.Tests` (Reqnroll features + a typed `Client` "page object" + poll-until-settled
  `Wait` helpers). The data-driven axis model and the `Wait` helpers are the parts worth copying
  first.
- **Cross-domain overlaps** are fine — hydroelectric reservoir, pumped-hydro, and tidal-barrage
  sluices sit in both **Energy** and **Water management**; frame each by whichever portfolio story
  fits.
- Any single idea here can be expanded into a full buildable plan (simulator design, axes/state,
  REST API, dashboard, and the Gherkin features that teach its lesson) — just say which one.
