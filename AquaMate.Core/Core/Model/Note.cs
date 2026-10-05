/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using AquaMate.Core.Types;

namespace AquaMate.Core.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Note : AquariumDetails, IEventEntity
    {
        public DateTime Timestamp { get; set; }
        public string Event { get; set; }
        public string Content { get; set; }


        public override EntityType EntityType
        {
            get {
                return EntityType.Note;
            }
        }


        public Note()
        {
        }

        public override string ToString()
        {
            return Event;
        }
    }
}
