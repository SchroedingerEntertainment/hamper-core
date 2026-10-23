// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace System
{
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    static partial class TypeExtension
    {
        private static readonly Type IntPtrType = typeof(IntPtr);
        private static readonly Type UIntPtrType = typeof(UIntPtr);

        /// <summary>
        /// Determines if the type is a struct
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsStruct(this Type type)
        {
            return 
            (
                // ReSharper disable MergeIntoPattern
                
                type.IsValueType && 
               !type.IsPrimitive && 
               !type.IsEnum && 
                type != IntPtrType && 
                type != UIntPtrType
                
                // ReSharper restore MergeIntoPattern
            );
        }
    }
}
