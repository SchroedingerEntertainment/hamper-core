// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Storage
{
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    static partial class SpanExtension
    {
        // ReSharper disable InvalidXmlDocComment
        
        /// <summary>
        /// Combines two target components into a single target
        /// </summary>
        /// <param name="target2">The second component to combine</param>
        /// <returns>An n-value tuple representing the combined target</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanTuple<char, char> Combine(this ReadOnlySpan<char> target1, in ResourceIdentifier.TargetComponent target2)
        {
            return ResourceIdentifier.Combine(target1, target2);
        }
        /// <summary>
        /// Appends a target component to the provided n-value tuple
        /// </summary>
        /// <param name="target">The target component to append</param>
        /// <returns>An n-value tuple representing the combined target</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanTuple<char, char, char> Combine(this ReadOnlySpanTuple<char, char> tuple, in ResourceIdentifier.TargetComponent target)
        {
            return ResourceIdentifier.Combine(tuple, target);
        }
        /// <summary>
        /// Appends a target component to the provided n-value tuple
        /// </summary>
        /// <param name="target">The target component to append</param>
        /// <returns>An n-value tuple representing the combined target</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanTuple<char, char, char, char> Combine(this ReadOnlySpanTuple<char, char, char> tuple, in ResourceIdentifier.TargetComponent target)
        {
            return ResourceIdentifier.Combine(tuple, target);
        }
        /// <summary>
        /// Appends a target component to the provided n-value tuple
        /// </summary>
        /// <param name="target">The target component to append</param>
        /// <returns>An n-value tuple representing the combined target</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanTuple<char, char, char, char, char> Combine(this ReadOnlySpanTuple<char, char, char, char> tuple, in ResourceIdentifier.TargetComponent target)
        {
            return ResourceIdentifier.Combine(tuple, target);
        }
        /// <summary>
        /// Appends a target component to the provided n-value tuple
        /// </summary>
        /// <param name="target">The target component to append</param>
        /// <returns>An n-value tuple representing the combined target</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanTuple<char, char, char, char, char, char> Combine(this ReadOnlySpanTuple<char, char, char, char, char> tuple, in ResourceIdentifier.TargetComponent target)
        {
            return ResourceIdentifier.Combine(tuple, target);
        }
        /// <summary>
        /// Appends a target component to the provided n-value tuple
        /// </summary>
        /// <param name="target">The target component to append</param>
        /// <returns>An n-value tuple representing the combined target</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanTuple<char, char, char, char, char, char, char> Combine(this ReadOnlySpanTuple<char, char, char, char, char, char> tuple, in ResourceIdentifier.TargetComponent target)
        {
            return ResourceIdentifier.Combine(tuple, target);
        }
        /// <summary>
        /// Combines three target components into a single target
        /// </summary>
        /// <param name="target2">The second component to combine</param>
        /// <param name="target3">The third component to combine</param>
        /// <returns>An n-value tuple representing the combined target</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanTuple<char, char, char> Combine(this ReadOnlySpan<char> target1, in ResourceIdentifier.TargetComponent target2, in ResourceIdentifier.TargetComponent target3)
        {
            return ResourceIdentifier.Combine(target1, target2, target3);
        }
        
        // ReSharper restore InvalidXmlDocComment
    }
}