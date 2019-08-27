//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core.Text;
using System.Collections.Generic;

namespace Overmodded
{
    public static class GameVar
    {
        public const string DefaultLocale = "eng";
        public const string LocaleFileStatistic = "STATISTIC";
        public const string LocaleFileSystem = "SYSTEM";
        public const string LocaleFileItem = "ITEM";
        public const string LocaleFileCharacter = "CHARACTER";
        public const string LocaleFileLong = "LONG";
        public const string LocaleFileAbility = "ABILITY";

        public static Dictionary<string, string> LocaleFilesArray = new Dictionary<string, string>()
        {
            {LocaleFileSystem, JEMLocale.DefaultLocaleDirectory},
            {LocaleFileStatistic, JEMLocale.DefaultLocaleDirectory},
            {LocaleFileItem, JEMLocale.DefaultLocaleDirectory},
            {LocaleFileCharacter, JEMLocale.DefaultLocaleDirectory},
            {LocaleFileLong, JEMLocale.DefaultLocaleDirectory},
            {LocaleFileAbility, JEMLocale.DefaultLocaleDirectory}
        };
    }
}
