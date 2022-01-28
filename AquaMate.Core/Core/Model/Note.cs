/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
