# Approved UI Visual References

> **Status: APPROVED** — These screens are the locked visual target for all future UI work.
> Do not deviate from the style, layout, or color language established here without explicit approval.

---

## Reference Files

| File | Screen | Status |
|------|--------|--------|
| `approved_main_menu.png` | Start / Main Menu Screen | ✅ Approved |
| `approved_world_map.png` | World Map Screen | ✅ Approved |
| `approved_war_screen.png` | War Screen | ✅ Approved |

---

## Style Guide — Extracted from Approved References

### 🎨 Color Palette

| Role | Color | Usage |
|------|-------|-------|
| Background | Deep dark brown / near-black `#1a1208` | All panel and screen backgrounds |
| Gold accent | Warm gold `#c9a84c` | Titles, headers, highlights, borders |
| Panel surface | Dark olive-brown `#2a1f0e` | UI panels, sidebars, card backgrounds |
| Text primary | Off-white / cream `#f0e6cc` | Body text, stats, labels |
| Text secondary | Muted tan `#a08060` | Subtitles, timestamps, secondary info |
| Attacker blue | `#4a7abf` | War score bars, attacker-side indicators |
| Defender red | `#bf4a4a` | War score bars, defender-side indicators |
| Ukrainian blue | `#3a6fbb` | Map territory — Ukrainian control |
| Russian red | `#bb3a3a` | Map territory — Russian control |
| Contested grey | `#7a7a7a` (hatched) | Map territory — contested zones |
| Button CTA | Dark gold / bronze `#8b6914` | Primary action buttons (Start Game, End War) |
| Button danger | Deep red `#8b1414` | Destructive actions (End War, Surrender) |

---

### 🖋 Typography

| Element | Style |
|---------|-------|
| Game title (`EMPIRES OF HISTORY`) | Large serif, all-caps, gold, widely tracked |
| Screen/section headers | Medium serif or small-caps, gold, tracked |
| Panel headers (`WAR OVERVIEW`, `RECENT SAVES`) | Small-caps or uppercase, gold, letter-spaced |
| Body / stats text | Clean sans-serif, cream/off-white, ~12–14px |
| Sub-labels | Sans-serif, muted tan, ~10–12px |
| Dates & timestamps | Monospaced or tabular, muted, small |

---

### 🗂 Layout Principles

#### Main Menu Screen
- Left sidebar: vertical nav menu with icon + label rows, dark panel, gold rule separators
- Center panel: **Recent Saves** list with flag thumbnails, nation name, date/time
- Right panel: **Quick Start** form — date picker, nation dropdown, difficulty dropdown, CTA button
- Background: dramatic painterly scene (throne room / columns), heavily darkened
- Version number bottom-left in small muted text

#### World Map Screen
- Top bar: selected nation flag + name, resource strip (treasury, population, GDP, military, tech, loyalty), date + turn indicator — full-width dark bar
- Center: full world map with colored nation territories, dark ocean `#0d1b2a`
- Right sidebar: nation detail card — flag, name, tier label (Great Power / etc.), leader photo, key stats table, top officials list, building/department icon row, mini strategic map
- Bottom bar: icon tab row — GOVERNMENT · ECONOMY · MILITARY · DIPLOMACY · TECHNOLOGY · RELIGION · INTELLIGENCE · GIA · MAP MODES
- Lower-left overlay: **World News** ticker panel, semi-transparent dark

#### War Screen
- Title bar: `[NATION A] - [NATION B] WAR` centered, subtitle with war start date, close ✕ button top-right
- Left column: **Attacker** card — flag, name, tier, leader photo + name, stats list (War Support, Exhaustion, Troops, Deployed, Casualties, Economy)
- Center column: tabbed panel (Overview / Battles / Military / Economy / Diplomacy) — war score bar (blue vs red), current objectives split left/right with star priority markers
- Right column: **Defender** card (mirror of attacker), **War Map** — geographic mini-map with territory coloring and legend
- Bottom action bar: full-width dark strip with icon+label buttons — NEGOTIATE PEACE · WAR GOALS · DEPLOY TROOPS · MILITARY STRATEGY · VIEW INTELLIGENCE · WORLD DIPLOMACY · END WAR (red)

---

### 🧩 Component Standards

#### Panels / Cards
- Background: dark translucent brown, thin gold or dark-rule border
- Header label: all-caps or small-caps gold, horizontally centered or left-aligned
- Corner radius: minimal (2–4px) or none — keep it sharp and period-appropriate

#### Buttons
- Default: dark fill, gold or cream text, subtle border
- Primary CTA (`START GAME`): slightly lighter fill, bold label
- Danger (`END WAR`, `SURRENDER`): deep red fill, cream text
- Hover: gold border glow or slight brightness lift — no cartoonish effects

#### Icons
- Line/outline style, cream or gold tint
- Used in tab bars, action bars, and nav menus — always paired with a label below

#### Progress / Score Bars
- Segmented or solid fill, blue (attacker) vs red (defender)
- Percentage labels on both ends, label centered above bar

#### Maps
- Dark navy ocean `#0d1b2a`
- Nation territories filled with muted, distinct colors — no bright or saturated fills
- Thin border lines between territories, slightly lighter than fill
- Strategic overlay maps use simplified coloring (blue/red/grey only)

#### Leader / Nation Portraits
- Real-world photo or illustrated portrait in a square or portrait crop
- Placed in card context beside name and stats
- Slightly desaturated or stylized to match the dark UI tone

---

### ✅ Do / ❌ Don't

| ✅ Do | ❌ Don't |
|-------|----------|
| Use dark backgrounds with gold accents | Use bright, saturated, or flat-color backgrounds |
| Keep typography serif for headers | Use rounded or playful fonts |
| Use muted, earthy map territory colors | Use neon or fully saturated nation colors |
| Maintain consistent panel structure across screens | Introduce new layout patterns without approval |
| Use real flag imagery for nations | Use placeholder or generic icons for nations |
| Keep the bottom action bar full-width on all major screens | Float action buttons randomly on screen |

---

*Last updated: 2026-06-20 — Approved by brodycain467-ui*
