﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AKSoftware.Localization.MultiLanguages.CodeGeneration
{
    internal static class CodeGenerationExtensions
    {

        /// <summary>
        /// Append the generated by a tool comment to the string builder
        /// </summary>
        /// <param name="stringBuilder"></param>
        internal static void AppendGeneratedByToolComment(this StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine(GetGeneratedByToolComment());
        }

        internal static string GetGeneratedByToolComment()
        {
            return @"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a AKSoftware.Localization.MultiLanguages.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
//      
//     For more information see: https://github.com/aksoftware98/multilanguages
// </auto-generated>
//------------------------------------------------------------------------------";
        }
    }
}
