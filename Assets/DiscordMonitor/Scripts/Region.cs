using System;
using System.Collections.Generic;
using UnityEngine;

namespace DiscordMonitor {
    using RegionList = Dictionary<Region.Name, Transform>;

    public class Region : MonoBehaviour {
      public RegionList list {
        get {
          if(this._list == null) {
            this._list = new RegionList();

            this._BuildRegionList();
          }

          return this._list;
        }
      }

      public enum Name {
        AVATAR_SPAWN,
        PENTHOUSE_VOICE,
      }

      private RegionList _list = null;

      private void _BuildRegionList() {
        for(int i = 0; i < this.transform.childCount; i++) {
          var child     = this.transform.GetChild(i);
          var childName = child.name.ToUpper().Replace(" ", "_");

          if(Enum.TryParse(childName, out Name regionName)) {
            this._list.Add(regionName, child.transform);
          } else {
          }
        }

        foreach(
          var regionName in
            (Name[])Enum.GetValues(typeof(Name))
        ) {
          if(!this._list.ContainsKey(regionName)) {
            Debug.LogError(
              $"No key for '{regionName}'. Using this transform."
            );

            this._list.Add(regionName, this.transform);
          }
        }
      }
    }
}
