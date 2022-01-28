/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
