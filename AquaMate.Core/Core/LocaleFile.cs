/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

namespace AquaMate.Core
{
    public sealed class LocaleFile
    {
        public readonly int Code;
        public readonly string Sign;
        public readonly string Name;
        public readonly string FileName;

        public LocaleFile(int code, string sign, string name, string fileName)
        {
            Code = code;
            Sign = sign;
            Name = name;
            FileName = fileName;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
