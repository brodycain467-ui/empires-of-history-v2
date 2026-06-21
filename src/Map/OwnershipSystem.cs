using System;
using System.Collections.Generic;

namespace EmpiresOfHistory.Map
{
    /// <summary>
    /// Tracks which nation owns each province and which provinces belong to each nation.
    /// Provides bidirectional lookup and fires an event on every ownership change.
    /// Extensible for future conquest, liberation, and core-claim mechanics.
    /// </summary>
    public class OwnershipSystem
    {
        // Province ID → Nation ID
        private readonly Dictionary<string, string> _provinceOwnership = new Dictionary<string, string>();

        // Nation ID → list of owned province IDs
        private readonly Dictionary<string, List<string>> _nationProvinces = new Dictionary<string, List<string>>();

        /// <summary>
        /// Fired whenever a province changes hands.
        /// Parameters: provinceId, oldNationId, newNationId
        /// </summary>
        public event Action<string, string, string> OnOwnershipChanged;

        /// <summary>
        /// Sets the owner of a province without firing the ownership-changed event.
        /// Use this for initial data loading; use TransferProvince for in-game changes.
        /// </summary>
        public void SetOwnership(string provinceId, string nationId)
        {
            // Remove from previous owner's list
            if (_provinceOwnership.TryGetValue(provinceId, out var previousOwner))
            {
                if (_nationProvinces.TryGetValue(previousOwner, out var prevList))
                    prevList.Remove(provinceId);
            }

            _provinceOwnership[provinceId] = nationId;

            if (!_nationProvinces.ContainsKey(nationId))
                _nationProvinces[nationId] = new List<string>();

            if (!_nationProvinces[nationId].Contains(provinceId))
                _nationProvinces[nationId].Add(provinceId);
        }

        /// <summary>Returns the nation ID that currently owns the given province, or null if unowned.</summary>
        public string GetOwner(string provinceId)
        {
            return _provinceOwnership.TryGetValue(provinceId, out var owner) ? owner : null;
        }

        /// <summary>Returns all province IDs owned by the given nation.</summary>
        public List<string> GetProvinces(string nationId)
        {
            return _nationProvinces.TryGetValue(nationId, out var provinces)
                ? provinces
                : new List<string>();
        }

        /// <summary>
        /// Transfers a province from one nation to another and fires OnOwnershipChanged.
        /// </summary>
        public void TransferProvince(string provinceId, string fromNationId, string toNationId)
        {
            SetOwnership(provinceId, toNationId);
            OnOwnershipChanged?.Invoke(provinceId, fromNationId, toNationId);
        }
    }
}
