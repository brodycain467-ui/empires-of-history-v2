using Godot;
using System;
using System.Collections.Generic;
using EmpiresOfHistory.Map.Data;

namespace EmpiresOfHistory.Map
{
    /// <summary>
    /// Main map coordinator node (attached to MapRoot inside the SubViewport).
    /// Responsibilities:
    ///   • Loads seed nations and provinces on _Ready()
    ///   • Populates OwnershipSystem and NationColorRegistry
    ///   • Spawns ProvinceVisual nodes inside ProvinceLayer
    ///   • Handles province click → fires ProvinceSelected / NationSelected events
    ///   • Exposes SelectNation() and SelectProvince() for external callers
    /// </summary>
    [GlobalClass]
    public partial class WorldMapManager : Node2D
    {
        // ── Signals ────────────────────────────────────────────────────────────────────

        /// <summary>Fired when the player clicks a province.</summary>
        [Signal]
        public delegate void ProvinceSelectedEventHandler(string provinceId);

        /// <summary>Fired when a nation's territory is clicked (derived from province owner).</summary>
        [Signal]
        public delegate void NationSelectedEventHandler(string nationId);

        // ── Public state ───────────────────────────────────────────────────────────────

        public string SelectedProvinceId { get; private set; }
        public string SelectedNationId { get; private set; }

        // ── Data ───────────────────────────────────────────────────────────────────────

        public OwnershipSystem Ownership { get; private set; }
        public NationColorRegistry ColorRegistry { get; private set; }

        private readonly Dictionary<string, Province> _provinces = new Dictionary<string, Province>();
        private readonly Dictionary<string, Nation> _nations = new Dictionary<string, Nation>();
        private readonly Dictionary<string, ProvinceVisual> _provinceVisuals = new Dictionary<string, ProvinceVisual>();

        // Visual dimensions for prototype province rectangles (px).
        // Real geographic shapes will replace these in a later phase.
        private const float ProvinceWidth = 28f;
        private const float ProvinceHeight = 20f;

        // ── Scene references ───────────────────────────────────────────────────────────

        private Node2D _provinceLayer;
        private ColorRect _oceanBackground;

        // ── Lifecycle ──────────────────────────────────────────────────────────────────

        public override void _Ready()
        {
            Ownership = new OwnershipSystem();
            ColorRegistry = new NationColorRegistry();

            _provinceLayer = GetNodeOrNull<Node2D>("ProvinceLayer");
            _oceanBackground = GetNodeOrNull<ColorRect>("OceanBackground");

            if (_oceanBackground != null)
                _oceanBackground.Color = new Color("#0d1b2a");

            LoadNations();
            LoadProvinces();
            SpawnProvinceVisuals();
        }

        // ── Data loading ───────────────────────────────────────────────────────────────

        private void LoadNations()
        {
            foreach (var nation in SeedNations.GetNations())
            {
                _nations[nation.Id] = nation;
                ColorRegistry.Register(nation.Id, nation.MapColor);
            }
        }

        private void LoadProvinces()
        {
            foreach (var province in SeedProvinces.GetProvinces())
            {
                _provinces[province.Id] = province;
                Ownership.SetOwnership(province.Id, province.OwnerId);
            }
        }

        // ── Province visual spawning ───────────────────────────────────────────────────

        private void SpawnProvinceVisuals()
        {
            if (_provinceLayer == null)
            {
                GD.PushWarning("WorldMapManager: ProvinceLayer node not found — province visuals will not be spawned.");
                return;
            }

            foreach (var province in _provinces.Values)
            {
                var color = ColorRegistry.GetColor(province.OwnerId);

                var visual = new ProvinceVisual();
                visual.Name = $"Province_{province.Id}";

                // Center the rectangle on the province's map position.
                var topLeft = province.MapPosition - new Vector2(ProvinceWidth / 2f, ProvinceHeight / 2f);
                visual.Initialize(province.Id, color, topLeft, new Vector2(ProvinceWidth, ProvinceHeight));

                visual.Clicked += OnProvinceClicked;
                _provinceLayer.AddChild(visual);
                _provinceVisuals[province.Id] = visual;
            }
        }

        // ── Input handlers ─────────────────────────────────────────────────────────────

        private void OnProvinceClicked(string provinceId)
        {
            SelectProvince(provinceId);
        }

        // ── Public selection API ───────────────────────────────────────────────────────

        /// <summary>
        /// Selects a province by ID, highlights it, and fires both ProvinceSelected and
        /// NationSelected (for the province's current owner).
        /// </summary>
        public void SelectProvince(string provinceId)
        {
            // Deselect previous province
            if (SelectedProvinceId != null && _provinceVisuals.TryGetValue(SelectedProvinceId, out var prev))
                prev.SetSelected(false);

            SelectedProvinceId = provinceId;

            if (provinceId != null && _provinceVisuals.TryGetValue(provinceId, out var visual))
                visual.SetSelected(true);

            EmitSignal(SignalName.ProvinceSelected, provinceId ?? string.Empty);

            var ownerId = provinceId != null ? Ownership.GetOwner(provinceId) : null;
            SelectNation(ownerId);
        }

        /// <summary>
        /// Selects a nation directly (without selecting a specific province).
        /// Fires NationSelected.
        /// </summary>
        public void SelectNation(string nationId)
        {
            SelectedNationId = nationId;
            EmitSignal(SignalName.NationSelected, nationId ?? string.Empty);
        }

        // ── Data accessors ─────────────────────────────────────────────────────────────

        /// <summary>Returns the Nation object for the given ID, or null if not found.</summary>
        public Nation GetNation(string nationId)
        {
            return _nations.TryGetValue(nationId, out var nation) ? nation : null;
        }

        /// <summary>Returns the Province object for the given ID, or null if not found.</summary>
        public Province GetProvince(string provinceId)
        {
            return _provinces.TryGetValue(provinceId, out var province) ? province : null;
        }

        /// <summary>Returns all loaded nations.</summary>
        public IEnumerable<Nation> GetAllNations() => _nations.Values;

        /// <summary>Returns all loaded provinces.</summary>
        public IEnumerable<Province> GetAllProvinces() => _provinces.Values;

        // ── Ownership change hook ──────────────────────────────────────────────────────

        /// <summary>
        /// Updates the visual color of a province after an ownership transfer.
        /// Called externally when OwnershipSystem.OnOwnershipChanged fires.
        /// </summary>
        public void RefreshProvinceColor(string provinceId)
        {
            var ownerId = Ownership.GetOwner(provinceId);
            if (ownerId == null) return;

            var color = ColorRegistry.GetColor(ownerId);
            if (_provinceVisuals.TryGetValue(provinceId, out var visual))
                visual.UpdateColor(color);
        }
    }
}
