# EMPIRES OF HISTORY V2 UI BIBLE
> **Authority:** This document is the permanent visual law of Empires of History V2. Every developer, designer, and AI agent must treat it as the single source of truth for UI appearance, hierarchy, spacing, interaction, and motion. If any future UI conflicts with this file, this file wins.

## Document Status
| Field | Value |
|---|---|
| Status | Locked visual reference |
| Scope | All screens, scenes, overlays, cards, controls, and map-adjacent UI |
| Source basis | `src/UI/Theme/EmpiresDarkTheme.cs`, current scenes, current UI scripts, approved references in `docs/ui/approved/` |
| Enforcement | Mandatory |

## Non-Negotiable Laws
1. Use the dark historical palette; no bright, playful, neon, or glossy UI.
2. Gold is for hierarchy, borders, active states, and ceremonial emphasis.
3. Cream text on dark surfaces is the default reading pattern.
4. Headers are serif; body and data text are sans-serif.
5. Corners stay sharp: square or 2px radius unless explicitly overridden here.
6. Depth comes from borders, layering, and scrims, not drop shadows.
7. The map screen geometry is stable: 48px top bar, 60px bottom bar, 280px right sidebar, 320×200 lower-left overlay.
8. Buttons are uppercase, flat, and restrained.
9. Map colors are muted and geopolitical, never arcade-bright.
10. Any new UI must map back to this bible before implementation.

---
# 1. COLOR PALETTE
## 1.1 Core Palette
| Token | Role | Hex | Usage |
|---|---|---|---|
| `color.bg.root` | Root background | `#1a1208` | Screen background, modal shell, major dark surfaces |
| `color.bg.panel` | Panel surface | `#2a1f0e` | Panels, cards, sidebars, secondary buttons |
| `color.bg.panel.hover` | Hover surface | `#3a2f1e` | Hovered rows, hovered cards, passive interactive lift |
| `color.border.gold` | Gold border | `#c9a84c` | Borders, active states, focus states, headers, rules |
| `color.border.gold.soft` | Soft separator | `#c9a84c4d` | Light separators, subtle borders, low-emphasis rules |
| `color.text.primary` | Cream text | `#f0e6cc` | Default body, values, button labels |
| `color.text.secondary` | Muted tan | `#a08060` | Metadata, timestamps, helper labels |
| `color.text.inverse` | Dark ink | `#1a1208` | Text on gold chips or filled emphasis badges |
| `color.action.primary` | CTA bronze | `#8b6914` | Primary CTAs |
| `color.action.danger` | Danger red | `#8b1414` | Delete, surrender, end war, destructive actions |
| `color.map.ocean` | Ocean navy | `#0d1b2a` | Water fill and strategic sea regions |
| `color.war.attacker` | Attacker blue | `#4a7abf` | War score, attacker bars, attacker identity |
| `color.war.defender` | Defender red | `#bf4a4a` | War score, defender bars, defender identity |
| `color.neutral.contested` | Contested gray | `#7a7a7a` | Contested regions, fallback territory state |

### Usage Rules
- Default to `#1a1208` whenever unsure.
- Use `#2a1f0e` for most reusable surfaces.
- Use gold for emphasis, never as a full-screen fill.
- Use cream for primary reading text.
- Use muted tan only for subordinate information.
- Reserve bronze and red for action roles.

## 1.2 Opacity and Alpha Variants
| Token | Hex | Opacity | Usage |
|---|---|---:|---|
| `color.gold.30` | `#c9a84c4d` | 30% | Separators, soft borders |
| `color.scrim.overlay` | `#00000080` | 50% | Overlay scrim |
| `color.scrim.modal` | `#00000099` | 60% | Modal scrim |
| `color.panel.feed` | `#1a1208d9` | 85% | Semi-transparent overlay panels |
| `color.disabled` | any token at 40% | 40% | Disabled controls |
| `color.read` | any event card at 70% | 70% | Read-state event cards |

### Alpha Rules
- Use alpha for scrims, separators, disabled states, overlay panels, and read-state dimming only.
- Do not use translucent gold as a large panel fill.
- Dim the container before dimming critical text.

## 1.3 Surface Hierarchy
| Layer | Color | Use |
|---|---|---|
| Layer 0 | `#1a1208` | App root, screen background |
| Layer 1 | `#2a1f0e` | Panels, cards, buttons, slots |
| Layer 2 | `#3a2f1e` | Hovered cards or lifted interactive states |
| Layer 3 | `#c9a84c` border only | Focus, selection, active emphasis |

### Surface Rules
- Hover changes surface before changing silhouette.
- Selection is communicated with gold border emphasis first.
- The root screen should usually remain darker than nested cards.

## 1.4 Text Roles
| Text Role | Hex | Use |
|---|---|---|
| Primary | `#f0e6cc` | Paragraphs, values, button text |
| Secondary | `#a08060` | Supporting labels, timestamps |
| Accent | `#c9a84c` | Headers, active labels, section titles |
| Inverse | `#1a1208` | Text on gold or cream chips |
| Disabled | `#f0e6cc` at 40% | Unavailable controls and tabs |

### Text Color Rules
- No stark white body copy.
- No blue-gray body text.
- Long reading passages should be cream on dark.
- Gold text is for hierarchy, not paragraph content.

## 1.5 Status Colors
| Status | Hex | Use |
|---|---|---|
| Positive | `#5a7a5a` | Gains, stability, favorable changes |
| Negative | `#8b3a3a` | Decline, hostile effects, losses |
| Warning | `#c97a3a` | Risk, caution, partial instability |
| Danger | `#bf4a4a` | Critical threat, severe failure |
| Informational | `#4a6a8b` | Neutral analytical or intelligence notes |

### Status Rules
- Keep status colors muted and institutional.
- Danger red is not a generic highlight.
- Use compact chips, bars, or markers instead of loud fills.

## 1.6 War Screen Colors
| Role | Hex | Use |
|---|---|---|
| War shell | `#1a1208` | Screen shell |
| War surface | `#2a1f0e` | Cards, panels, tabs |
| Attacker | `#4a7abf` | Attacker side identity |
| Defender | `#bf4a4a` | Defender side identity |
| Objective priority | `#c9a84c` | Stars, key objectives, active emphasis |
| Critical action | `#8b1414` | End War, Surrender |
| Secondary metadata | `#a08060` | Dates, helper stats |

### War Rules
- Blue and red may dominate only in war comparison contexts.
- Identity colors belong on bars, chips, and legends, not giant screen fills.
- War score bars stay flat and disciplined.

## 1.7 Nation Color Table
| Nation | Hex | Use |
|---|---|---|
| USA | `#3a5fa0` | Territory and nation identity |
| RUS | `#8b3a3a` | Territory and conflict identity |
| CHN | `#c9493a` | Territory and diplomatic identity |
| DEU | `#5a7a5a` | Territory and stable-state identity |
| BRA | `#4a7a4a` | Territory and regional identity |
| IND | `#c97a3a` | Territory and demographic identity |
| GBR | `#6a5a8b` | Territory and political identity |
| FRA | `#4a6a8b` | Territory and strategic identity |

### Nation Color Rules
- Nation colors primarily belong on the map.
- Nation colors do not replace the dark/gold global chrome.
- Capital provinces may be slightly lightened.
- Over text, nation fills must maintain readability or receive a dark overlay.

## 1.8 Map Color Rules
| Role | Hex | Use |
|---|---|---|
| Ocean | `#0d1b2a` | Water |
| Province border | `#00000040` | Internal border lines |
| Selected border | `#c9a84c` | Selected province or nation outline |
| Contested base | `#7a7a7a` | Contested fill |
| Capital marker | `#c9a84c` | Capital dot |
| Fallback territory | `#7a7a7a` | Missing owner color fallback |

### Map Rules
- Ocean must remain darker than all land colors.
- Province borders should be visible but restrained.
- Gold selection borders must read over every nation fill.
- Contested state must use patterning, not flat gray alone.

---
# 2. TYPOGRAPHY
## 2.1 Font Families
| Role | Family Direction | Tone |
|---|---|---|
| Display title | Serif | Monumental, historical, ceremonial |
| Screen header | Serif or small-caps serif | Formal, institutional |
| Panel header | Serif or serif-styled small caps | Archival, bureaucratic |
| Body | Sans-serif | Clean and readable |
| Data labels | Sans-serif | Compact and functional |
| Timestamps | Sans-serif with tabular numerals or monospaced numerals | Stable numeric rhythm |
| Tooltips | Sans-serif | Compact and clear |

### Family Rules
- Headers must feel historical.
- Body copy must feel neutral.
- Avoid playful, futuristic, bubbly, or handwritten fonts.
- Preserve hierarchy even if exact fonts vary by platform.

## 2.2 Type Scale
| Token | Role | Size | Weight | Tracking | Case | Color |
|---|---|---:|---|---|---|---|
| `type.display.game-title` | Game title | 40px | Bold | +8% | ALL CAPS | Gold |
| `type.display.screen-header` | Screen header | 28px | Semibold | +4% | ALL CAPS / small caps | Gold |
| `type.display.panel-header` | Panel header | 18px | Semibold | +6% | ALL CAPS | Gold |
| `type.body.default` | Body | 14px | Regular | 0% | Sentence case | Cream |
| `type.body.sub-label` | Sub-label | 12px | Regular | +2% | Uppercase or title case | Muted tan |
| `type.data.stat-value` | Stat value | 14px | Semibold | 0% | Numeric / title | Cream |
| `type.data.timestamp` | Timestamp | 11px | Regular | +2% | Numeric / uppercase month okay | Muted tan |
| `type.overlay.tooltip` | Tooltip | 12px | Regular | 0% | Sentence case | Cream |

### Scale Rules
- Only the game title behaves like true display type.
- Panel headers should read as labels, not hero headlines.
- Body copy should generally stay within 12–14px equivalent.
- Tooltip text must never exceed body text size.

## 2.3 Element Rules
| Element | Rules |
|---|---|
| Game title | Serif, gold, widely tracked, all caps, centered or stacked |
| Screen headers | Serif, gold, formal, all caps or small caps |
| Panel headers | Gold, tracked, compact, uppercase |
| Body text | Sans-serif, cream, sentence case |
| Sub-labels | Sans-serif, muted tan, small |
| Stat values | Sans-serif, cream, semibold when emphasis is needed |
| Dates/timestamps | Small, muted tan, numeric stability prioritized |
| Tooltips | Compact sans-serif on dark surface |

## 2.4 Capitalization Rules
| Element | Case |
|---|---|
| Game title | ALL CAPS |
| Screen headers | ALL CAPS or small caps |
| Panel headers | ALL CAPS |
| Primary and secondary buttons | ALL CAPS |
| Tabs | ALL CAPS |
| Body paragraphs | Sentence case |
| Stat labels | Title case or sentence case |
| Save metadata | Sentence case with numeric dates |
| Timestamps | Numeric or abbreviated uppercase month |

### Capitalization Law
- Command and navigation text defaults to uppercase.
- Reading text defaults to sentence case.

## 2.5 Alignment Rules
| Element | Alignment |
|---|---|
| Game title | Center |
| Major screen header | Center or left per layout |
| Panel headers | Left by default, center only when ceremonial |
| Stat labels | Left |
| Stat values | Right |
| Resource clusters | Center or right |
| Buttons | Center |
| Event metadata | Structured left-to-right grouping |

---
# 3. SPACING & LAYOUT
## 3.1 Base Unit and Scale
**Base unit: 4px**
| Token | Size |
|---|---:|
| `space.xs` | 4px |
| `space.sm` | 8px |
| `space.md` | 12px |
| `space.lg` | 16px |
| `space.xl` | 24px |
| `space.2xl` | 32px |
| `space.3xl` | 48px |
| `space.4xl` | 64px |

### Scale Rules
- Margins and paddings should resolve to this scale.
- `8`, `12`, `16`, and `24` are the default working values.
- Avoid odd values unless scene anchoring demands them.

## 3.2 Standard Spacing
| Component | Spacing |
|---|---|
| Screen panel padding | 16px |
| Card padding | 12px |
| Event card padding | 12px + 4px stripe |
| Header bar internal spacing | 8–16px |
| Button padding | 12px horizontal / 8px vertical |
| Tooltip padding | 8px |
| Save row spacing | 8–12px |
| Major region gap | 16px |
| Internal control gap | 8–12px |

## 3.3 Screen Zones
| Zone | Spec |
|---|---|
| TopBar | 48px height |
| BottomTabBar | 60px height |
| Right sidebar | 280px width |
| Lower-left overlay | 320×200px target block |
| Modal shell | Centered within scrim |

## 3.4 World Map Layout Diagram
```text
┌──────────────────────────────────────────────────────────────────────────────────────────────┐
│ TOP BAR · 48px                                                                             │
├───────────────────────────────────────────────────────────────┬──────────────────────────────┤
│                                                               │ RIGHT SIDEBAR · 280px       │
│ WORLD MAP                                                     │ Nation info / portrait /    │
│ full remaining canvas                                         │ stats / officials / mini-map│
│ ocean + provinces + overlays                                  │                              │
│                                                               │                              │
├───────────────────────────────────────────────────────────────┴──────────────────────────────┤
│ BOTTOM TAB BAR · 60px                                                                       │
└──────────────────────────────────────────────────────────────────────────────────────────────┘

Lower-left overlay target:
┌──────────────────────────────┐
│ WORLD NEWS / EVENT FEED      │
│ 320px × 200px                │
└──────────────────────────────┘
```

## 3.5 Main Menu Layout Diagram
```text
┌──────────────────────────────────────────────────────────────────────────────────────────────┐
│ Title block                                                                                 │
├───────────────┬───────────────────────────────────────────────┬──────────────────────────────┤
│ Left Nav      │ Recent Saves Panel                            │ Quick Start Panel            │
│ 240px         │ flexible                                      │ 320px                        │
├───────────────┴───────────────────────────────────────────────┴──────────────────────────────┤
│ Version label                                                                                │
└──────────────────────────────────────────────────────────────────────────────────────────────┘
```

## 3.6 Nation Select Layout Diagram
```text
┌──────────────────────────────────────────────────────────────────────────────────────────────┐
│ SELECT YOUR NATION                                                                          │
├──────────────────────────────────────────────────────────────────────────────────────────────┤
│ Scrollable nation list                                                                      │
│ flag / name / tier / government                                                             │
├──────────────────────────────────────────────────────────────────────────────────────────────┤
│ BACK                                                                              CONFIRM   │
└──────────────────────────────────────────────────────────────────────────────────────────────┘
```

## 3.7 Stats Grid Rules
| Column | Alignment | Role |
|---|---|---|
| Left | Left | Label |
| Right | Right | Value |

Example:
```text
Population .............. 331.9M
Treasury ................ $2.8T
Military ................ 1.4M
Technology .............. 88
```

### Stats Grid Rules
- Use one row per metric.
- Right-align values.
- Labels may be tan; values should remain cream.
- Do not create three-column stat grids unless the screen is explicitly analytical.

## 3.9 Scaling Rules
- Preserve bar heights unless accessibility scaling requires adjustment.
- The right sidebar should collapse to an overlay before becoming too narrow.
- The lower-left overlay pins to 16px from left and bottom edges.
- Responsive behavior must preserve hierarchy, not invent new patterns.

---
# 4. BUTTONS
## 4.1 Global Button Spec
| Property | Spec |
|---|---|
| Radius | 2px |
| Border width | 1px default, 2px on focus/strong selection |
| Padding | 12px horizontal / 8px vertical |
| Label case | Uppercase |
| Default label color | Cream |
| Interaction style | Flat with brightness shift |

### Shared State Rules
- Hover = brightness +15%.
- Pressed = brightness -10%.
- Disabled = 40% opacity.
- Focus = gold border emphasis or 2px border.
- No bounce, no scale-pop, no glossy gradients.

## 4.2 Variant State Matrix
| Variant | State | Fill | Text | Border | Notes |
|---|---|---|---|---|---|
| Primary | Normal | `#8b6914` | `#f0e6cc` | `#c9a84c` | CTA |
| Primary | Hover | `#8b6914` +15% | `#f0e6cc` | `#c9a84c` | Slight lift |
| Primary | Pressed | `#8b6914` -10% | `#f0e6cc` | `#c9a84c` | Firm response |
| Primary | Disabled | 40% opacity | 40% opacity | 40% opacity | Unavailable CTA |
| Primary | Focus | `#8b6914` | `#f0e6cc` | `#c9a84c` 2px | Keyboard/controller clarity |
| Secondary | Normal | `#2a1f0e` | `#f0e6cc` | `#c9a84c` | Default secondary action |
| Secondary | Hover | `#3a2f1e` | `#f0e6cc` | `#c9a84c` | Panel-surface lift |
| Secondary | Pressed | `#2a1f0e` -10% | `#f0e6cc` | `#c9a84c` | Flat press |
| Secondary | Disabled | 40% opacity | 40% opacity | 40% opacity | Unavailable secondary |
| Secondary | Focus | `#2a1f0e` | `#f0e6cc` | `#c9a84c` 2px | Strong outline |
| Ghost | Normal | Transparent | `#c9a84c` | `#c9a84c` | Outline-forward |
| Ghost | Hover | gold at 10–15% | `#f0e6cc` | `#c9a84c` | Soft emphasis |
| Ghost | Pressed | gold at ~20% | `#f0e6cc` | `#c9a84c` | Slight density |
| Ghost | Disabled | 40% opacity | 40% opacity | 40% opacity | Minimal unavailable action |
| Ghost | Focus | Transparent | `#f0e6cc` | `#c9a84c` 2px | Accessibility focus |
| Danger | Normal | `#8b1414` | `#f0e6cc` | `#c9a84c` or dark red | Destructive action |
| Danger | Hover | `#8b1414` +15% | `#f0e6cc` | `#c9a84c` | Strong but controlled |
| Danger | Pressed | `#8b1414` -10% | `#f0e6cc` | `#c9a84c` | Tactile response |
| Danger | Disabled | 40% opacity | 40% opacity | 40% opacity | Still clearly destructive |
| Danger | Focus | `#8b1414` | `#f0e6cc` | `#c9a84c` 2px | Required clarity |

## 4.3 Tab Button Spec
| Property | Spec |
|---|---|
| Layout | Icon above label |
| Height target | Bottom bar 60px or compatible compact rail |
| Label case | Uppercase |
| Inactive label/icon | Cream |
| Active label/icon | Gold |
| Disabled | 40% opacity |

### Tab Rules
- Active tabs use gold, not a giant filled pill.
- Inactive tabs remain readable.
- Disabled tabs should look unavailable, not hidden.
- Tab switching is immediate.

---
# 5. PANELS & CARDS
## 5.1 Shared Container Rules
| Property | Default |
|---|---|
| Background | `#2a1f0e` or `#1a1208` depending on elevation |
| Border | `#c9a84c` or `#c9a84c4d` depending on emphasis |
| Radius | 0–2px |
| Internal padding | 12–16px |
| Header style | Gold, uppercase, letter-spaced |

## 5.2 Panel and Card Specs
| Component | Background | Border | Radius | Padding | Header Style |
|---|---|---|---:|---|---|
| Screen Panel | `#1a1208` | `#c9a84c` or soft gold | 0–2px | 16px | Gold screen header |
| Content Card | `#2a1f0e` | `#c9a84c4d` | 2px | 12px | Gold panel header |
| Event Card | `#2a1f0e` | `#c9a84c4d` | 2px | 12px + 4px stripe | Category/date row + gold title |
| Stat Row | Transparent or `#21170b` | optional soft rule | 0–2px | 8px | Label left/value right |
| Nation Info Card | `#2a1f0e` | `#c9a84c4d` | 2px | 16px | Nation name, tier, details |
| Save Slot Card | `#2a1f0e` | `#7a5d2c` or soft gold | 2px | 12px | Compact slot metadata |
| War Card | `#2a1f0e` | `#c9a84c4d` | 2px | 16px | Side identity + gold header |
| Header Bar | `#1a1208` | bottom rule soft gold | 0px | 8–16px | Dense operational layout |

## 5.3 Specific Rules
| Component | Rules |
|---|---|
| Screen Panel | Use for modal shells and major overlays; darker than nested cards |
| Content Card | Use for reusable info blocks; keep border subtle |
| Event Card | Always include importance signaling; must stay readable at 320px width |
| Stat Row | Keep one metric per row; labels left, values right |
| Nation Info Card | Must support flag/color identity, leader portrait, stats, officials, mini-map |
| Save Slot Card | Empty slots preserve rhythm; delete control must be visually red |
| War Card | Left and right cards mirror each other and support fast comparison |
| Header Bar | Full-width, disciplined, low-decoration shell |

---
# 6. ICONS
## 6.1 Icon Style
| Property | Spec |
|---|---|
| Style | Outline |
| Stroke | 2px |
| Default color | Cream |
| Active color | Gold |
| Disabled color | 40% opacity |
| Tone | Period-appropriate, sober, strategic |

### Icon Rules
- Icons should feel governmental, military, archival, or strategic.
- No bubbly app-store iconography.
- No thick cartoon fills.

## 6.2 Icon Sizes
| Token | Size | Use |
|---|---:|---|
| `icon.inline` | 16px | Inline metadata and stat rows |
| `icon.tab` | 24px | Bottom tab bar |
| `icon.action` | 32px | Action bars |
| `icon.feature` | 48px | Feature highlights and empty states |

## 6.3 Tab Bar Icon List
| Tab | Icon Concept | Description |
|---|---|---|
| Government | Capitol / laurel crest | State structure |
| Economy | Coin stack / ledger | Treasury and markets |
| Military | Sword / shield | Armed forces |
| Diplomacy | Handshake / globe | Treaties and influence |
| Technology | Gear / spark | Research |
| Religion | Temple / flame | Faith and doctrine |
| Intelligence | Eye / cipher | Espionage |
| GIA | Advisor seal / speech emblem | Strategic advisor |
| Events | Scroll / bell | Event history and alerts |
| Map Modes | Layer stack / compass | Overlay selection |

## 6.4 Event Category Icons
| Category | Icon Concept |
|---|---|
| Politics | Gavel / capitol |
| Economy | Coin / chart |
| Military | Sword / standards |
| Diplomacy | Handshake / treaty quill |
| Technology | Gear / starburst |
| Religion | Temple / flame |
| Intelligence | Eye / mask |
| Culture | Lyre / column |
| Disaster | Cracked earth / storm |
| General | Scroll |

## 6.5 Importance Icons
| Importance | Symbol | Meaning |
|---|---|---|
| Minor | `•` | Routine change |
| Notable | `◆` | Worth attention |
| Major | `★` | Strategic significance |
| Critical | `⚡` | Immediate attention |

---
# 7. SHADOWS & DEPTH
## 7.1 Depth Philosophy
No drop shadows. Depth comes from contrast, borders, scrims, layering order, and spacing.

## 7.2 Approved Depth Cues
| Cue | Spec |
|---|---|
| Border-only separation | Primary depth method |
| Surface step | Root darker than panel, panel darker than hover |
| Overlay scrim | `#00000080` |
| Modal scrim | `#00000099` |
| Focus border | Gold 2px |
| Hover lift | Brightness only |

## 7.3 Tooltip Spec
| Property | Spec |
|---|---|
| Background | `#1a1208` |
| Border | `#c9a84c` 1px |
| Padding | 8px |
| Text | 12px cream sans-serif |
| Density | 2–4 short lines preferred |
| Shadow | None |

### Depth Rules
- Overlays use scrims, not blur fog.
- Focus is communicated with borders first.
- If a component needs more depth, add a stronger border or darker nesting layer rather than shadow.

---
# 8. ANIMATIONS
## 8.1 Motion Philosophy
Motion clarifies change; it does not entertain. All motion should be short, readable, restrained, and tactical.

## 8.2 Approved Animation Specs
| Element | Motion | Duration | Easing |
|---|---|---:|---|
| Scene transition | Fade to `#1a1208` | 0.3s | Standard fade |
| Panel open | Opacity `0→1` | 0.15s | Ease-out |
| Event popup | Opacity + scale `0.95→1.0` | 0.2s | Ease-out |
| Button hover | Brightness +15% | 0.1s | Quick ease |
| Button press | Brightness -10% | 0.05s | Immediate |
| Province hover | Brightness +15% | 0.1s | Quick ease |
| Province select | Gold border reveal | 0.1s | Quick ease |
| Tab switch | Immediate | 0s | None |
| Number counter | Count-up | 0.5s | Ease-out |
| Toast | Slide up, hold 2s, fade out 0.3s | 2.3s total | Ease-out + fade |
| Event feed item | Slide in from left | 0.2s | Ease-out |
| Map pan/zoom | Tweened move | 0.3s | Ease-in-out |

### Motion Rules
- Do not animate every component on load.
- Hover should feel almost immediate.
- Tabs should switch instantly.
- No bounce, jelly, or carnival motion language.

---
# 9. MAP COLORS
## 9.1 Base Map Treatment
| Role | Spec |
|---|---|
| Ocean | `#0d1b2a` |
| Province unselected | Nation base color |
| Province hover | Nation color brightened by 15% |
| Province selected | Nation color + gold border |
| Province borders | `#00000040` |
| Capital marker | Gold dot, 8px |
| Contested | `#7a7a7a` with hatch pattern |

## 9.2 Province State Rules
| State | Rules |
|---|---|
| Unselected | Nation color fill, subdued border, no glow |
| Hover | Brightness +15%, no halo |
| Selected | Gold outline, readable over all fills |
| Capital | Slightly lightened fill permitted + gold dot |
| Contested | Gray hatched treatment, not flat gray alone |

## 9.3 LOD Rules
| Zoom Level | Representation |
|---|---|
| `>= 3` | Full polygon detail |
| `>= 1` | Simplified polygon |
| `0` / strategic far view | Dot or highly reduced marker |

### LOD Rules
- Readability beats fidelity when zoomed out.
- Labels appear only when useful.
- Fine geometry should not overload distant views.

## 9.4 Placeholder Map Mode Schemes
| Map Mode | Placeholder Scheme |
|---|---|
| Population | Low = muted tan, high = deeper bronze-red |
| Religion | Distinct muted color families by religion group |
| Government | Structured muted palette by regime type |
| Historical Accuracy | Low = muted red, medium = tan, high = muted green-gold |

### Map Mode Rules
- Placeholder schemes must still harmonize with the dark shell.
- Legends belong in cards or overlay panels, not as naked free-floating color bars.

---
# 10. LEADER PORTRAITS
## 10.1 Portrait Spec
| Property | Spec |
|---|---|
| Shape | Rectangular 3:4 crop |
| Sizes | `48×64`, `96×128`, `192×256` |
| Container background | `#1a1208` |
| Container border | 1px gold |
| Fallback | Silhouette icon, gold tint |
| Saturation filter | 85% |

## 10.3 Era Style Guide
| Era | Direction |
|---|---|
| Ancient | Sculptural, painted, relief-like, parchment-toned |
| Medieval | Illuminated manuscript or painted portrait sensibility |
| Modern | Documentary or archival-photo treatment |
| Future | Disciplined state-portrait style, still grounded in the house palette |

## 10.4 Naming and Storage
| Property | Spec |
|---|---|
| File naming | `portrait_{leader_id}.png` |
| Directory | `assets/portraits/` |
| Crop rule | Maintain 3:4 ratio |
| Preferred framing | Head and upper torso |

---
# 11. EVENT CARDS
## 11.1 Base Spec
| Property | Spec |
|---|---|
| Width | 320px |
| Height | 80–160px |
| Background | `#2a1f0e` |
| Border | `#c9a84c4d` |
| Radius | 2px |
| Hover | Brightness +8% |
| Click | Opens `EventPopupWindow` |

## 11.2 Importance Stripe
| Importance | Stripe Width | Hex |
|---|---:|---|
| Minor | 4px | `#a08060` |
| Notable | 4px | `#c9a84c` |
| Major | 4px | `#c97a3a` |
| Critical | 4px | `#bf4a4a` |

## 11.3 Layout Rows
| Row | Content |
|---|---|
| 1 | Category + date + flag / nation marker |
| 2 | Title |
| 3 | Preview text |
| 4 | Action buttons when needed |

## 11.4 Secondary Signals
| Signal | Spec |
|---|---|
| Unread dot | Gold, 6px, top-right |
| Read state | 70% opacity |
| Hover state | +8% brightness |
| Selected/opened | Gold border emphasis |

---
# 12. COMPONENT INVENTORY
## 12.1 Status Legend
| Icon | Meaning |
|---|---|
| ✅ | Implemented in the repository |
| 🔒 | Approved/locked visual target, not fully implemented yet |
| ⚠️ | Planned, placeholder, or future component |

## 12.2 Existing Implemented Components
| Component | File Path | Status | Phase |
|---|---|---:|---:|
| Global theme builder | `src/UI/Theme/EmpiresDarkTheme.cs` | ✅ | 1 |
| Main menu screen root | `scenes/MainMenu.tscn` | ✅ | 1 |
| Main menu controller | `src/UI/MainMenuScreen.cs` | ✅ | 1 |
| Main menu left navigation | `scenes/MainMenu.tscn#RootMargin/RootVBox/ContentRow/LeftNav` | ✅ | 1 |
| Recent saves panel | `scenes/MainMenu.tscn#RootMargin/RootVBox/ContentRow/RecentSavesPanel` | ✅ | 1 |
| Quick start panel | `scenes/MainMenu.tscn#RootMargin/RootVBox/ContentRow/QuickStartPanel` | ✅ | 1 |
| Save/load dialog scene | `scenes/UI/SaveLoadDialog.tscn` | ✅ | 1 |
| Save/load dialog controller | `src/UI/SaveLoadDialog.cs` | ✅ | 1 |
| Nation select screen root | `scenes/NationSelect.tscn` | ✅ | 1 |
| Nation select controller | `src/UI/NationSelectScreen.cs` | ✅ | 1 |
| Nation selection row card | `src/UI/NationSelectScreen.cs#CreateNationRow` | ✅ | 1 |
| World map screen root | `scenes/Map/WorldMapScreen.tscn` | ✅ | 2 |
| World map controller | `src/UI/WorldMapScreen.cs` | ✅ | 2 |
| Top bar | `src/UI/TopBar.cs` | ✅ | 1 |
| Turn controls scene | `scenes/UI/TurnControls.tscn` | ✅ | 1 |
| Turn controls controller | `src/UI/TurnControls.cs` | ✅ | 1 |
| Right nation info panel | `src/UI/NationInfoPanel.cs` | ✅ | 2 |
| Bottom tab bar | `src/UI/BottomTabBar.cs` | ✅ | 1 |
| Event feed overlay scene | `scenes/Events/EventFeedPanel.tscn` | ✅ | 2 |
| Event feed overlay controller | `src/UI/Events/EventFeedPanel.cs` | ✅ | 2 |
| Event popup scene | `scenes/Events/EventPopupWindow.tscn` | ✅ | 2 |
| Event popup controller | `src/UI/Events/EventPopupWindow.cs` | ✅ | 2 |
| Event archive scene | `scenes/Events/EventArchiveScreen.tscn` | ✅ | 2 |
| Event archive controller | `src/UI/Events/EventArchiveScreen.cs` | ✅ | 2 |
| Province visual scene | `scenes/Map/ProvinceVisual.tscn` | ✅ | 2 |
| Province visual controller | `src/Map/Rendering/ProvinceVisual.cs` | ✅ | 2 |
| World map renderer / interaction shell | `src/Map/Rendering/WorldMapManager.cs` | ✅ | 2 |
| Map camera interaction | `src/Map/Rendering/MapCamera.cs` | ✅ | 2 |
| Map LOD manager | `src/Map/Rendering/LODManager.cs` | ✅ | 2 |
| Nation color registry | `src/Map/Systems/NationColorRegistry.cs` | ✅ | 2 |

## 12.3 Locked Approved Targets
| Component | File Path / Source | Status | Phase |
|---|---|---:|---:|
| Approved reference pack | `docs/ui/approved/README.md` | 🔒 | 1 |
| Approved main menu target | `docs/ui/approved/approved_main_menu.png.md` | 🔒 | 1 |
| Approved world map target | `docs/ui/approved/approved_world_map.png.md` | 🔒 | 2 |
| Approved war screen target | `docs/ui/approved/approved_war_screen.png.md` | 🔒 | 5 |
| Painterly main menu backdrop treatment | Approved main menu reference | 🔒 | 1 |
| Expanded top resource strip | Approved world map reference | 🔒 | 2 |
| Leader portrait block in right sidebar | Approved world map reference | 🔒 | 2 |
| Mini strategic map in right sidebar | Approved world map reference | 🔒 | 2 |
| Persistent world news overlay treatment | Approved world map reference | 🔒 | 2 |
| War screen root shell | Approved war screen reference | 🔒 | 5 |
| Attacker war card | Approved war screen reference | 🔒 | 5 |
| Defender war card | Approved war screen reference | 🔒 | 5 |
| War score bar | Approved war screen reference | 🔒 | 5 |
| War objectives panel | Approved war screen reference | 🔒 | 5 |
| War mini-map | Approved war screen reference | 🔒 | 5 |
| War bottom action bar | Approved war screen reference | 🔒 | 5 |

## 12.4 Planned and Placeholder Components
| Component | Planned Home | Status | Phase |
|---|---|---:|---:|
| Government detail screen | future `src/UI/` screen | ⚠️ | 4 |
| Economy detail screen | future `src/UI/` screen | ⚠️ | 4 |
| Military detail screen | future `src/UI/` screen | ⚠️ | 5 |
| Diplomacy detail screen | future `src/UI/` screen | ⚠️ | 7 |
| Technology detail screen | future `src/UI/` screen | ⚠️ | 6 |
| Religion detail screen | future `src/UI/` screen | ⚠️ | 4 |
| Intelligence detail screen | future `src/UI/` screen | ⚠️ | 7 |
| GIA advisor screen | future `src/UI/` screen | ⚠️ | 7 |
| Map modes panel | future `src/UI/` screen or overlay | ⚠️ | 2 |
| Population map mode legend | future map overlay UI | ⚠️ | 3 |
| Religion map mode legend | future map overlay UI | ⚠️ | 4 |
| Government map mode legend | future map overlay UI | ⚠️ | 4 |
| Historical accuracy map mode legend | future map overlay UI | ⚠️ | 3 |
| Dedicated tooltip system | shared overlay utility | ⚠️ | 1 |
| Notification toast system | shared overlay utility | ⚠️ | 2 |
| Negotiation peace dialog | future war UI | ⚠️ | 5 |
| War goals dialog | future war UI | ⚠️ | 5 |
| Deployment panel | future war UI | ⚠️ | 5 |
| Military strategy panel | future war UI | ⚠️ | 5 |
| World diplomacy panel | future diplomacy UI | ⚠️ | 7 |
| Scenario browser | future main menu panel | ⚠️ | 3 |
| Multiplayer screen | future main menu panel | ⚠️ | 8 |
| Settings screen | future main menu panel | ⚠️ | 1 |
| Credits screen | future main menu panel | ⚠️ | 1 |
| Tutorial overlay system | shared onboarding UI | ⚠️ | 10 |

### Inventory Rules
- Every new UI component must be added to this table.
- `✅` means implemented, `🔒` means approved target, `⚠️` means planned or placeholder.
- When a planned component is built, update status and phase as needed.

---
# 13. HOW TO ADD A NEW COMPONENT
## 13.1 Five-Step Recipe
1. **Choose tokens first.** Select the surface color, text roles, spacing tokens, card archetype, and motion spec from this bible before drawing or coding.
2. **Place it in hierarchy.** Decide whether it is a screen, sidebar card, modal, overlay, map attachment, or toolbar item, then lock it to the approved layout system.
3. **Apply house rules.** Use dark surfaces, gold emphasis, cream reading text, serif headers, sharp corners, border-only depth, restrained motion, and uppercase action labels.
4. **Compare against an archetype.** Match the component to the closest existing family: button, content card, event card, nation info card, save slot card, war card, tooltip, or overlay panel.
5. **Update the inventory.** Add the component to Section 12 with a path, status icon, and phase number.

## 13.2 Final Gate Checklist
- [ ] Uses approved palette tokens only
- [ ] Preserves serif-header / sans-body hierarchy
- [ ] Uses spacing tokens from the approved scale only
- [ ] Uses the correct button/card archetype
- [ ] Uses no drop shadows
- [ ] Uses approved motion timings only
- [ ] Uses correct war/map identity colors if applicable
- [ ] Updates the component inventory
- [ ] Does not drift from approved references

## Closing Mandate
This document is **LAW**. Every future UI component, screen, scene, overlay, and visual refactor in Empires of History V2 must conform to this bible unless an explicit replacement standard is approved.
