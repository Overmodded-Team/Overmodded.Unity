//
// Core Just Enough Methods Library Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using UnityEngine;

namespace JEM.UnityEngine.Attribute
{
    /// <inheritdoc />
    /// <summary>
    ///     UnityEditor read only attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class JEMReadOnlyAttribute : PropertyAttribute { }
}