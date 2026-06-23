"use strict";

// ---- API helpers ----------------------------------------------------------
const api = {
  state: () => fetch("/api/system/state").then(r => r.json()),
  post: (path, body) =>
    fetch("/api/system/" + path, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: body ? JSON.stringify(body) : undefined,
    }).then(r => r.json()),
};

// ---- Axis metadata for the dropdown ---------------------------------------
const AXES = [
  "StandRotation", "StandAngulation", "StandRoll", "Sid",
  "TableLongitudinal", "TableLateral", "TableHeight", "TableTilt", "TableCradle",
];

const $ = id => document.getElementById(id);
const get = (snap, name) => snap.axes.find(a => a.axis === name) || { position: 0, target: 0, min: 0, max: 1 };

// ---- SVG drawing -----------------------------------------------------------
const SVGNS = "http://www.w3.org/2000/svg";
function el(name, attrs, children) {
  const e = document.createElementNS(SVGNS, name);
  for (const k in attrs) e.setAttribute(k, attrs[k]);
  (children || []).forEach(c => e.appendChild(c));
  return e;
}

// A C-arm: arc + X-ray source (top) and detector panel (bottom), drawn around (0,0),
// then rotated/offset by the caller. `sid` scales the source<->detector distance.
function cArm(rotationDeg, sid) {
  const R = 86;
  const det = 28 + (sid - 90) * 0.6; // detector pushes out as SID grows
  const g = el("g", { transform: `rotate(${rotationDeg} 150 150)` });
  // The arc (a thick C around the isocenter)
  g.appendChild(el("path", {
    d: `M ${150 - R} 150 A ${R} ${R} 0 1 1 ${150 + R} 150`,
    fill: "none", stroke: "#4b5b6e", "stroke-width": 12, "stroke-linecap": "round",
  }));
  // X-ray source (tube) at top
  g.appendChild(el("circle", { cx: 150, cy: 150 - R, r: 12, fill: "#cfe3ff", stroke: "#7fb2ff", "stroke-width": 2 }));
  g.appendChild(el("text", { x: 150, y: 150 - R + 4, "text-anchor": "middle", "font-size": 9, fill: "#0e1116" }, [txt("S")]));
  // Beam
  g.appendChild(el("line", { x1: 150, y1: 150 - R + 12, x2: 150, y2: 150 + det, stroke: "#7fb2ff55", "stroke-width": 24 }));
  // Detector panel at bottom
  g.appendChild(el("rect", { x: 150 - 22, y: 150 + det, width: 44, height: 12, rx: 3, fill: "#3fb95033", stroke: "#3fb950", "stroke-width": 2 }));
  return g;
}

function txt(s) { return document.createTextNode(s); }

function table(yOffset, xOffset, tiltDeg) {
  const g = el("g", { transform: `translate(${xOffset} ${yOffset}) rotate(${tiltDeg} 150 150)` });
  // Table top
  g.appendChild(el("rect", { x: 60, y: 146, width: 180, height: 8, rx: 4, fill: "#5a6b7d" }));
  // Patient
  g.appendChild(el("rect", { x: 95, y: 138, width: 110, height: 9, rx: 4, fill: "#9fb0c2" }));
  g.appendChild(el("circle", { cx: 205, cy: 142, r: 7, fill: "#9fb0c2" }));
  // Pedestal
  g.appendChild(el("rect", { x: 140, y: 154, width: 20, height: 60, fill: "#3a4654" }));
  return g;
}

function isocenter() {
  return el("circle", { cx: 150, cy: 150, r: 3, fill: "#d29922" });
}

function drawSide(snap) {
  const svg = $("sideView");
  svg.replaceChildren();
  const rot = get(snap, "StandRotation").position;
  const sid = get(snap, "Sid").position;
  const height = get(snap, "TableHeight").position;
  const tableY = (95 - height) * 1.4; // higher table -> moves up on screen
  svg.appendChild(table(tableY, 0, 0));
  svg.appendChild(cArm(rot, sid));
  svg.appendChild(isocenter());
}

function drawHead(snap) {
  const svg = $("headView");
  svg.replaceChildren();
  const ang = get(snap, "StandAngulation").position;
  const sid = get(snap, "Sid").position;
  const lateral = get(snap, "TableLateral").position;
  const tilt = get(snap, "TableTilt").position;
  svg.appendChild(table(0, lateral * 1.6, tilt));
  svg.appendChild(cArm(ang, sid));
  svg.appendChild(isocenter());
}

// ---- Readout table & badges ------------------------------------------------
function drawAxisTable(snap) {
  const tbody = $("axisRows");
  tbody.replaceChildren();
  for (const a of snap.axes) {
    const tr = document.createElement("tr");
    if (a.isMoving) tr.className = "moving";
    const pct = (a.position - a.min) / (a.max - a.min || 1);
    tr.innerHTML =
      `<td>${a.axis}</td>` +
      `<td>${a.position.toFixed(1)}<div class="bar"><i style="left:${(pct * 100).toFixed(1)}%"></i></div></td>` +
      `<td>${a.target.toFixed(1)}</td>` +
      `<td>${a.min}…${a.max} ${a.unit}</td>`;
    tbody.appendChild(tr);
  }
}

function drawBadges(snap) {
  const bMove = $("bMove");
  bMove.textContent = snap.isMoving ? "Moving" : "Idle";
  bMove.className = "badge" + (snap.isMoving ? " on-move" : "");

  const bSettled = $("bSettled");
  bSettled.textContent = snap.isSettled ? "Settled" : "Not settled";
  bSettled.className = "badge" + (snap.isSettled ? " on-settled" : "");

  $("bFlaky").textContent = "Flakiness: " + snap.flakinessProfile;

  const bEstop = $("bEstop");
  bEstop.textContent = snap.emergencyStopped ? "E-STOP ACTIVE" : "E-stop clear";
  bEstop.className = "badge" + (snap.emergencyStopped ? " on-estop" : "");
}

function drawFaults(snap) {
  const box = $("faults");
  box.replaceChildren();
  // Show most recent first, cap at 8.
  const faults = snap.faults.slice(-8).reverse();
  for (const f of faults) {
    const d = document.createElement("div");
    d.className = "fault";
    d.innerHTML = `<span class="k">${f.kind}</span>${f.axis ? " · " + f.axis : ""} — ${f.message}`;
    box.appendChild(d);
  }
}

// ---- Render loop -----------------------------------------------------------
let connected = false;
function setConn(live) {
  if (live === connected) return;
  connected = live;
  $("dot").className = "dot" + (live ? " live" : "");
  $("connText").textContent = live ? "live" : "disconnected";
}

function render(snap) {
  drawSide(snap);
  drawHead(snap);
  drawAxisTable(snap);
  drawBadges(snap);
  drawFaults(snap);
  // Keep the flakiness dropdown in sync with server state.
  const sel = $("flakySel");
  if (sel.value !== snap.flakinessProfile.toLowerCase()) sel.value = snap.flakinessProfile.toLowerCase();
}

async function poll() {
  try {
    const snap = await api.state();
    setConn(true);
    render(snap);
  } catch {
    setConn(false);
  }
}

// ---- Wire up controls ------------------------------------------------------
function populateAxisSelect() {
  const sel = $("axisSel");
  for (const a of AXES) {
    const o = document.createElement("option");
    o.value = a; o.textContent = a;
    sel.appendChild(o);
  }
}

function wire() {
  $("btnMove").onclick = () => api.post("move", { axis: $("axisSel").value, value: Number($("moveVal").value) }).then(render);
  $("btnHome").onclick = () => api.post("home").then(render);
  $("btnLao30").onclick = () => api.post("move", { axis: "StandRotation", value: 30 }).then(render);
  // Force collision: command the system into the danger envelope.
  $("btnCollide").onclick = async () => {
    await api.post("move", { axis: "StandAngulation", value: 70 });
    await api.post("move", { axis: "TableHeight", value: 118 });
    await api.post("move", { axis: "TableLateral", value: 24 });
  };
  $("btnStop").onclick = () => api.post("stop").then(render);
  $("btnEstop").onclick = () => api.post("emergency-stop").then(render);
  $("btnReset").onclick = () => api.post("reset").then(render);
  $("flakySel").onchange = e => api.post("flakiness", { profile: e.target.value }).then(render);
}

populateAxisSelect();
wire();
poll();
setInterval(poll, 80); // ~12 Hz refresh
